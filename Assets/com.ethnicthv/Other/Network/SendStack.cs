using System;
using System.Collections.Generic;
using com.ethnicthv.Other.Network.P;

namespace com.ethnicthv.Other.Network
{
    public class SendStack
    {
        readonly Queue<ArraySegment<byte>> Queue = new Queue<ArraySegment<byte>>();

        Pool<byte[]> _pool;

        public int Count {
            get {
                lock (this)
                {
                    return Queue.Count;
                }
            }
        }
        
        public SendStack(int MaxMessageSize)
        {
            _pool = new Pool<byte[]>(() => new byte[MaxMessageSize]);
        }
        
        public void Enqueue(ArraySegment<byte> segment)
        {
            lock (this)
            {
                byte[] bytes = _pool.Take();
                
                Buffer.BlockCopy(segment.Array, segment.Offset, bytes, 0, segment.Count);
                
                ArraySegment<byte> newSegment = new ArraySegment<byte>(bytes, 0, segment.Count);
                
                Queue.Enqueue(newSegment);
            }
        }
        
        public bool DequeueAndSerializeAll(ref byte[] payload, out int packetSize)
        {
            // pool & queue usage always needs to be locked
            lock (this)
            {
                // do nothing if empty
                packetSize = 0;
                if (Queue.Count == 0)
                    return false;

                // we might have multiple pending messages. merge into one
                // packet to avoid TCP overheads and improve performance.
                //
                // IMPORTANT: Mirror & DOTSNET already batch into MaxMessageSize
                //            chunks, but we STILL pack all pending messages
                //            into one large payload so we only give it to TCP
                //            ONCE. This is HUGE for performance so we keep it!
                packetSize = 0;
                foreach (ArraySegment<byte> message in Queue)
                    packetSize += 4 + message.Count; // header + content

                // create payload buffer if not created yet or previous one is
                // too small
                // IMPORTANT: payload.Length might be > packetSize! don't use it!
                if (payload == null || payload.Length < packetSize)
                    payload = new byte[packetSize];

                // dequeue all byte[] messages and serialize into the packet
                var position = 0;
                while (Queue.Count > 0)
                {
                    // dequeue
                    ArraySegment<byte> message = Queue.Dequeue();

                    // write header (size) into buffer at position
                    BytesUtil.IntToBytes(message.Count, payload, position);
                    position += 4;

                    // copy message into payload at position
                    Buffer.BlockCopy(message.Array, message.Offset, payload, position, message.Count);
                    position += message.Count;

                    // return to pool so it can be reused (avoids allocations!)
                    _pool.Return(message.Array);
                }

                // we did serialize something
                return true;
            }
        }
        
        public void Clear()
        {
            // pool & queue usage always needs to be locked
            lock (this)
            {
                // clear queue, but via dequeue to return each byte[] to pool
                while (Queue.Count > 0)
                {
                    _pool.Return(Queue.Dequeue().Array);
                }
            }
        }
    }
}