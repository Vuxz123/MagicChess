using System;
using System.Collections.Generic;
using com.ethnicthv.Other.Network.Client;

namespace com.ethnicthv.Other.Network
{
    public class ReceiveStack
    {
        // queue entry message. only used in here.
        // -> byte arrays are always of 4 + MaxMessageSize
        // -> ArraySegment indicates the actual message content
        private struct Entry
        {
            public int connectionId;
            public NetworkEventType eventType;
            public ArraySegment<byte> data;
            public Entry(int connectionId, NetworkEventType eventType, ArraySegment<byte> data)
            {
                this.connectionId = connectionId;
                this.eventType = eventType;
                this.data = data;
            }
        }

        // message queue
        // ConcurrentQueue allocates. lock{} instead.
        //
        // IMPORTANT: lock{} all usages!
        readonly Queue<Entry> queue = new Queue<Entry>();

        // byte[] pool to avoid allocations
        // Take & Return is beautifully encapsulated in the pipe.
        // the outside does not need to worry about anything.
        // and it can be tested easily.
        //
        // IMPORTANT: lock{} all usages!
        Pool<byte[]> pool;

        // unfortunately having one receive pipe per connetionId is way slower
        // in CCU tests. right now we have one pipe for all connections.
        // => we still need to limit queued messages per connection to avoid one
        //    spamming connection being able to slow down everyone else since
        //    the queue would be full of just this connection's messages forever
        // => let's use a simpler per-connectionId counter for now
        readonly Dictionary<int, int> queueCounter = new();

        // constructor
        public ReceiveStack(int MaxMessageSize)
        {
            // initialize pool to create max message sized byte[]s each time
            pool = new Pool<byte[]>(() => new byte[MaxMessageSize]);
        }

        // return amount of queued messages for this connectionId.
        // for statistics. don't call Count and assume that it's the same after
        // the call.
        public int Count(int connectionId)
        {
            lock (this)
            {
                return queueCounter.TryGetValue(connectionId, out int count)
                       ? count
                       : 0;
            }
        }

        // total count
        public int TotalCount
        {
            get { lock (this) { return queue.Count; } }
        }

        // pool count for testing
        public int PoolCount
        {
            get { lock (this) { return pool.Count(); } }
        }

        // enqueue a message
        // -> ArraySegment to avoid allocations later
        // -> parameters passed directly so it's more obvious that we don't just
        //    queue a passed 'Message', instead we copy the ArraySegment into
        //    a byte[] and store it internally, etc.)
        public void Enqueue(int connectionId, NetworkEventType eventType, ArraySegment<byte> message)
        {
            // pool & queue usage always needs to be locked
            lock (this)
            {
                // does this message have a data array content?
                ArraySegment<byte> segment = default;
                if (message != default && message.Array != null)
                {
                    // ArraySegment is only valid until returning.
                    // copy it into a byte[] that we can store.
                    // ArraySegment array is only valid until returning, so copy
                    // it into a byte[] that we can queue safely.

                    // get one from the pool first to avoid allocations
                    var bytes = pool.Take();

                    // copy into it
                    Buffer.BlockCopy(message.Array, message.Offset, bytes, 0, message.Count);

                    // indicate which part is the message
                    segment = new ArraySegment<byte>(bytes, 0, message.Count);
                }

                // enqueue it
                // IMPORTANT: pass the segment around pool byte[],
                //            NOT the 'message' that is only valid until returning!
                var entry = new Entry(connectionId, eventType, segment);
                queue.Enqueue(entry);

                // increase counter for this connectionId
                var oldCount = Count(connectionId);
                queueCounter[connectionId] = oldCount + 1;
            }
        }

        // peek the next message
        // -> allows the caller to process it while pipe still holds on to the
        //    byte[]
        // -> TryDequeue should be called after processing, so that the message
        //    is actually dequeued and the byte[] is returned to pool!
        // => see TryDequeue comments!
        //
        // IMPORTANT: TryPeek & Dequeue need to be called from the SAME THREAD!
        public bool TryPeek(out int connectionId, out NetworkEventType eventType, out ArraySegment<byte> data)
        {
            connectionId = 0;
            eventType = NetworkEventType.Disconnected;
            data = default;

            lock (this)
            {
                if (queue.Count > 0)
                {
                    Entry entry = queue.Peek();
                    connectionId = entry.connectionId;
                    eventType = entry.eventType;
                    data = entry.data;
                    return true;
                }
                return false;
            }
        }
        
        public bool TryDequeue()
        {
            lock (this)
            {
                if (queue.Count > 0)
                {
                    Entry entry = queue.Dequeue();

                    if (entry.data != default)
                    {
                        pool.Return(entry.data.Array);
                    }

                    queueCounter[entry.connectionId]--;

                    if (queueCounter[entry.connectionId] == 0)
                        queueCounter.Remove(entry.connectionId);

                    return true;
                }
                return false;
            }
        }

        public void Clear()
        {
            lock (this)
            {
                while (queue.Count > 0)
                {
                    var entry = queue.Dequeue();
                    
                    if (entry.data != default)
                    {
                        pool.Return(entry.data.Array);
                    }
                }

                queueCounter.Clear();
            }
        }
    }
}