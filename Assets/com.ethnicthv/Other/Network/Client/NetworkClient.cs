using System;
using System.Net.Sockets;
using System.Threading;

namespace com.ethnicthv.Other.Network.Client
{
    public class NetworkClient : Common, NetworkSender, NetworkTicker
    {
        public Action OnConnected;
        public Action OnDisconnected;
        public Action<ArraySegment<byte>> OnDataReceived;
        
        public int SendQueueLimit = 10000;
        public int ReceiveQueueLimit = 10000;

        private ClientConnectionState _state;
        
        public bool Connected => _state is { Connected: true };
        public bool Connecting => _state is { Connecting: true };

        public NetworkClient(int maxMessageSize) : base(maxMessageSize)
        {
        }
        
        public void Connect(string ip, int port)
        {
            // not if already started
            if (Connecting || Connected)
            {
                Debug.LogWarning("[Telepathy] Client can not create connection because an existing connection is connecting or connected");
                return;
            }

            _state = new ClientConnectionState(MaxMessageSize)
            {
                Connecting = true,
                client =
                {
                    Client = null
                }
            };

            _state.receiveThread = new Thread(() => {
                ThreadFunction(_state, ip, port, MaxMessageSize, NoDelay, SendTimeout, ReceiveTimeout, ReceiveQueueLimit);
            })
            {
                IsBackground = true
            };
            _state.receiveThread.Start();
        }
        
        public void Disconnect()
        {
            // only if started
            if (Connecting || Connected)
            {
                _state.Dispose();

                // IMPORTANT: DO NOT set state = null!
                // we still want to process the pipe's disconnect message etc.!
            }
        }

        public bool Send(ArraySegment<byte> message)
        {
            if (Connected)
            {
                // respect max message size to avoid allocation attacks.
                if (message.Count <= MaxMessageSize)
                {
                    // check send pipe limit
                    if (_state.sendPipe.Count < SendQueueLimit)
                    {
                        // add to thread safe send pipe and return immediately.
                        // calling Send here would be blocking (sometimes for long
                        // times if other side lags or wire was disconnected)
                        _state.sendPipe.Enqueue(message);
                        _state.sendPending.Set(); // interrupt SendThread WaitOne()
                        return true;
                    }
                    else
                    {
                        // log the reason
                        Debug.LogWarning($"[Telepathy] Client.Send: sendPipe reached limit of {SendQueueLimit}. This can happen if we call send faster than the network can process messages. Disconnecting to avoid ever growing memory & latency.");

                        // just close it. send thread will take care of the rest.
                        _state.client.Close();
                        return false;
                    }
                }
                Debug.LogError("[Telepathy] Client.Send: message too big: " + message.Count + ". Limit: " + MaxMessageSize);
                return false;
            }
            Debug.LogWarning("[Telepathy] Client.Send: not connected!");
            return false;
        }

        public int Tick(int processLimit, Func<bool> checkEnabled = null)
        {
            if (_state == null)
                return 0;

            for (var i = 0; i < processLimit; ++i)
            {
                if (checkEnabled != null && !checkEnabled())
                    break;

                if (_state.receivePipe.TryPeek(out int _, out NetworkEventType eventType, out ArraySegment<byte> message))
                {
                    switch (eventType)
                    {
                        case NetworkEventType.Connected:
                            OnConnected?.Invoke();
                            break;
                        case NetworkEventType.Data:
                            OnDataReceived?.Invoke(message);
                            break;
                        case NetworkEventType.Disconnected:
                            OnDisconnected?.Invoke();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    _state.receivePipe.TryDequeue();
                }
                else break;
            }

            // return what's left to process for next time
            return _state.receivePipe.TotalCount;
        }


        private static void ThreadFunction(ClientConnectionState state, string ip, int port, int maxMessageSize, bool noDelay,
            int sendTimeout, int receiveTimeout, int receiveQueueLimit)
        {
            Thread sendThread = null;

            try
            {
                state.client.Connect(ip, port);
                state.Connecting = false;

                state.client.NoDelay = noDelay;
                state.client.SendTimeout = sendTimeout;
                state.client.ReceiveTimeout = receiveTimeout;

                sendThread = new Thread(() => { NetworkFunction.SendLoop(0, state.client, state.sendPipe, state.sendPending); })
                    {
                        IsBackground = true
                    };
                sendThread.Start();

                NetworkFunction.ReceiveLoop(0, state.client, maxMessageSize, state.receivePipe, receiveQueueLimit);
            }
            catch (SocketException exception)
            {
                // this happens if (for example) the ip address is correct
                // but there is no server running on that ip/port
                Debug.Log("[Telepathy] Client Receive: failed to connect to ip=" + ip + " port=" + port + " reason=" + exception);
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
                Debug.LogError("[Telepathy] Client Receive Exception: " + exception);
            }
            
            sendThread?.Interrupt();
            
            state.Connecting = false;
            
            state.client?.Close();
        }
    }
}