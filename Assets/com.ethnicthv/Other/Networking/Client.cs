using System;
using System.Net.Sockets;
using System.Threading;

namespace com.ethnicthv.Other.Networking
{
    public class Client
    {
        public Action OnConnected;
        public Action OnDisconnected;
        public Action<ArraySegment<byte>> OnDataReceived;
        
        public bool NoDelay = true;
        
        public readonly int MaxMessageSize;
        
        public int SendTimeout = 5000;
        public int ReceiveTimeout = 5000;
        
        public int SendQueueLimit = 10000;
        public int ReceiveQueueLimit = 10000;

        ConnectionState state;
        
        public bool Connected => state is { Connected: true };
        public bool Connecting => state is { Connecting: true };

        public Client(int maxMessageSize)
        {
            MaxMessageSize = maxMessageSize;
        }

        static void ThreadFunction(ConnectionState state, string ip, int port, int MaxMessageSize, bool NoDelay,
            int SendTimeout, int ReceiveTimeout, int ReceiveQueueLimit)
        {
            Thread sendThread = null;

            try
            {
                state.client.Connect(ip, port);
                state.Connecting = false;

                state.client.NoDelay = NoDelay;
                state.client.SendTimeout = SendTimeout;
                state.client.ReceiveTimeout = ReceiveTimeout;

                sendThread = new Thread(() => { SendLoop(0, state.client, state.sendPipe, state.sendPending); });
                sendThread.IsBackground = true;
                sendThread.Start();

            }
            catch (SocketException exception)
            {
                // this happens if (for example) the ip address is correct
                // but there is no server running on that ip/port
                Debug.Log("[Telepathy] Client Recv: failed to connect to ip=" + ip + " port=" + port + " reason=" + exception);
            }
            catch (ThreadInterruptedException)
            {
                // expected if Disconnect() aborts it
            }
            catch (ThreadAbortException)
            {
                // expected if Disconnect() aborts it
            }
            catch (ObjectDisposedException)
            {
                // expected if Disconnect() aborts it and disposed the client
                // while ReceiveThread is in a blocking Connect() call
            }
            catch (Exception exception)
            {
                // something went wrong. probably important.
                Debug.LogError("[Telepathy] Client Recv Exception: " + exception);
            }
            
            sendThread?.Interrupt();
            
            state.Connecting = false;
            
            state.client?.Close();
        }

        static void SendLoop(int connectionId, TcpClient client, SendStack sendPipe,
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
    }
}