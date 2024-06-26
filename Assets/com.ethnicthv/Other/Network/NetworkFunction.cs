﻿using System;
using System.Net.Sockets;
using System.Threading;
using com.ethnicthv.Other.Network.Client;
using com.ethnicthv.Other.Network.P;

namespace com.ethnicthv.Other.Network
{
    public static class NetworkFunction
    {
        public static void ReceiveLoop(int connectionId, TcpClient client, int maxMessageSize, ReceiveStack receivePipe, int QueueLimit)
        {
            // get NetworkStream from client
            NetworkStream stream = client.GetStream();

            // every receive loop needs it's own receive buffer of
            // HeaderSize + MaxMessageSize
            // to avoid runtime allocations.
            //
            // IMPORTANT: DO NOT make this a member, otherwise every connection
            //            on the server would use the same buffer simulatenously
            byte[] receiveBuffer = new byte[4 + maxMessageSize];

            // avoid header[4] allocations
            //
            // IMPORTANT: DO NOT make this a member, otherwise every connection
            //            on the server would use the same buffer simulatenously
            byte[] headerBuffer = new byte[4];

            // absolutely must wrap with try/catch, otherwise thread exceptions
            // are silent
            try
            {
                // add connected event to pipe
                receivePipe.Enqueue(connectionId, NetworkEventType.Connected, default);

                // let's talk about reading data.
                // -> normally we would read as much as possible and then
                //    extract as many <size,content>,<size,content> messages
                //    as we received this time. this is really complicated
                //    and expensive to do though
                // -> instead we use a trick:
                //      Read(2) -> size
                //        Read(size) -> content
                //      repeat
                //    Read is blocking, but it doesn't matter since the
                //    best thing to do until the full message arrives,
                //    is to wait.
                // => this is the most elegant AND fast solution.
                //    + no resizing
                //    + no extra allocations, just one for the content
                //    + no crazy extraction logic
                while (true)
                {
                    // read the next message (blocking) or stop if stream closed
                    if (!ReadMessageBlocking(stream, maxMessageSize, headerBuffer, receiveBuffer, out int size))
                        // break instead of return so stream close still happens!
                        break;

                    // create arraysegment for the read message
                    ArraySegment<byte> message = new ArraySegment<byte>(receiveBuffer, 0, size);

                    // send to main thread via pipe
                    // -> it'll copy the message internally so we can reuse the
                    //    receive buffer for next read!
                    receivePipe.Enqueue(connectionId, NetworkEventType.Data, message);

                    // disconnect if receive pipe gets too big for this connectionId.
                    // -> avoids ever growing queue memory if network is slower
                    //    than input
                    // -> disconnecting is great for load balancing. better to
                    //    disconnect one connection than risking every
                    //    connection / the whole server
                    if (receivePipe.Count(connectionId) >= QueueLimit)
                    {
                        // log the reason
                        Debug.LogWarning($"[Telepathy] receivePipe reached limit of {QueueLimit} for connectionId {connectionId}. This can happen if network messages come in way faster than we manage to process them. Disconnecting this connection for load balancing.");

                        // IMPORTANT: do NOT clear the whole queue. we use one
                        // queue for all connections.
                        //receivePipe.Clear();

                        // just break. the finally{} will close everything.
                        break;
                    }
                }
            }
            catch (Exception exception)
            {
                // something went wrong. the thread was interrupted or the
                // connection closed or we closed our own connection or ...
                // -> either way we should stop gracefully
                Debug.Log("[Telepathy] ReceiveLoop: finished receive function for connectionId=" + connectionId + " reason: " + exception);
            }
            finally
            {
                // clean up no matter what
                stream.Close();
                client.Close();

                // add 'Disconnected' message after disconnecting properly.
                // -> always AFTER closing the streams to avoid a race condition
                //    where Disconnected -> Reconnect wouldn't work because
                //    Connected is still true for a short moment before the stream
                //    would be closed.
                receivePipe.Enqueue(connectionId, NetworkEventType.Disconnected, default);
            }
        }

        public static void SendLoop(int connectionId, TcpClient client, SendStack sendPipe,
            ManualResetEvent sendPending)
        {
            var stream = client.GetStream();

            byte[] payload = null;

            try
            {
                while (client.Connected)
                {
                    sendPending.Reset();

                    // send payload;
                    if (sendPipe.DequeueAndSerializeAll(ref payload, out var packetSize))
                    {
                        if(!SendMessagesBlocking(stream, payload, packetSize))
                        {
                            break;
                        }
                    }
                
                    sendPending.WaitOne();
                }
            }
            catch (ThreadAbortException)
            {
                // happens on stop. don't log anything.
            }
            catch (ThreadInterruptedException)
            {
                // happens if receive thread interrupts send thread.
            }
            catch (Exception exception)
            {
                // something went wrong. the thread was interrupted or the
                // connection closed or we closed our own connection or ...
                // -> either way we should stop gracefully
                Debug.Log("[Telepathy] SendLoop Exception: connectionId=" + connectionId + " reason: " + exception);
            }
            finally
            {
                // clean up no matter what
                // we might get SocketExceptions when sending if the 'host has
                // failed to respond' - in which case we should close the connection
                // which causes the ReceiveLoop to end and fire the Disconnected
                // message. otherwise the connection would stay alive forever even
                // though we can't send anymore.
                stream.Close();
                client.Close();
            }
        }

        private static bool SendMessagesBlocking(NetworkStream stream, byte[] payload, int packetSize)
        {
            // stream.Write throws exceptions if client sends with high
            // frequency and the server stops
            try
            {
                // write the whole thing
                stream.Write(payload, 0, packetSize);
                return true;
            }
            catch (Exception exception)
            {
                // log as regular message because servers do shut down sometimes
                Debug.Log("[Telepathy] Send: stream.Write exception: " + exception);
                return false;
            }
        }

        private static bool ReadMessageBlocking(NetworkStream stream, int maxMessageSize, byte[] headerBuffer, byte[] payloadBuffer, out int size)
        {
            size = 0;

            // buffer needs to be of Header + MaxMessageSize
            if (payloadBuffer.Length != 4 + maxMessageSize)
            {
                Debug.Log($"[Telepathy] ReadMessageBlocking: payloadBuffer needs to be of size 4 + MaxMessageSize = {4 + maxMessageSize} instead of {payloadBuffer.Length}");
                return false;
            }

            // read exactly 4 bytes for header (blocking)
            if (!stream.ReadExactly(headerBuffer, 4))
                return false;

            // convert to int
            size = BytesUtil.BytesToInt(headerBuffer);

            // protect against allocation attacks. an attacker might send
            // multiple fake '2GB header' packets in a row, causing the server
            // to allocate multiple 2GB byte arrays and run out of memory.
            //
            // also protect against size <= 0 which would cause issues
            if (size > 0 && size <= maxMessageSize)
            {
                // read exactly 'size' bytes for content (blocking)
                return stream.ReadExactly(payloadBuffer, size);
            }
            Debug.LogWarning("[Telepathy] ReadMessageBlocking: possible header attack with a header of: " + size + " bytes.");
            return false;
        }
    }
}