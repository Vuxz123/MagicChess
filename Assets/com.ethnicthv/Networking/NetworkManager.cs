using System;
using System.Collections.Generic;
using System.Reflection;
using com.ethnicthv.Other;
using com.ethnicthv.Other.Config;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Other.Network;
using com.ethnicthv.Other.Network.Client;
using com.ethnicthv.Other.Network.Client.P;
using com.ethnicthv.Other.Network.Server;

namespace com.ethnicthv.Networking
{
    public class NetworkManager
    {
        private readonly NetworkID _netID;

        private static NetworkManager _instance;
        
        private readonly Dictionary<Type, byte> _packetTypes = new();
        private readonly Dictionary<byte, NetworkEvent> _eventBuilders = new();
        
        private int _maxMessageSize;
        
        private int _processPerTick;

        public static NetworkManager Instance => _instance ??= new NetworkManager();
        
        private NetworkClient _networkClient;
        private NetworkServer _networkServer;

        //state of the network
        private NetworkState _state = NetworkState.None;
        //storing the sender and ticker for the state
        private NetworkSender _sender;
        private NetworkTicker _ticker;
        
        private NetworkManager()
        {
            _netID = new NetworkID();
        }

        public void Init()
        {
            var networkEvents = ReflectionHelper.GetClassesWithAttribute<NetworkAttribute>();
            var i = 0;
            foreach (var networkEvent in networkEvents)
            {
                var attribute = networkEvent.GetCustomAttribute<NetworkAttribute>();
                var id = _netID.GetID(attribute.EventNetworkName);
                _packetTypes[networkEvent] = id;
                _eventBuilders[id] = (NetworkEvent) Activator.CreateInstance(networkEvent);
                i++;
            }
            
            Debug.Log($"Registered {i} network events resolver.");
            
            _maxMessageSize = ConfigProvider.GetConfig().NetworkConfig.MaxMessageSize;
            _processPerTick = ConfigProvider.GetConfig().NetworkConfig.ProcessPerTick;
        }  
        
        public Event ResolvePacket(Packet packet)
        {
            var reader = PacketReader.Create(packet);
            var id = reader.ReadByte();
            var obj = _eventBuilders[id].FromPacket(reader);
            reader.Close();
            return obj;
        }
        
        public Packet PacketizeEvent(NetworkEvent e)
        {
            var writer = PacketWriter.Create();
            writer.Write(_packetTypes[e.GetType()]);
            e.ToPacket(writer);
            return writer.GetPacket();
        }
        
        /// <summary>
        /// Open a server on the given port
        /// </summary>
        /// <param name="port"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public void StartServer(int port)
        {
            if (_state == NetworkState.Client)
                throw new InvalidOperationException("Cannot start server while client is connected.");
            
            _networkServer ??= new NetworkServer(_maxMessageSize);
            
            _networkServer.Start(port);
            
            _state = NetworkState.Server;
            _sender = _networkServer;
            _ticker = _networkServer;
        }
        
        /// <summary>
        /// Connect to the server
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Connect(string ip, int port)
        {
            if (_state == NetworkState.Server)
                throw new InvalidOperationException("Cannot connect to server while server is running.");
            // if _client is null then create a new client
            if(_networkClient == null) CreateClient();

            _networkClient.Connect(ip, port);
            
            _state = NetworkState.Client;
            _sender = _networkClient;
            _ticker = _networkClient;
        }
        
        /// <summary>
        /// Disconnect to the server if the state is client
        /// Else stop the server
        /// </summary>
        public void Disconnect()
        {
            switch (_state)
            {
                case NetworkState.Client:
                    _networkClient.Disconnect();
                    break;
                case NetworkState.Server:
                    _networkServer.Stop();
                    break;
                case NetworkState.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// Send a network event to the server
        /// </summary>
        /// <param name="e"> event to be sent </param>
        public void Send(NetworkEvent e)
        {
            var packet = PacketizeEvent(e);
            Send(packet);
        }
        
        /// <summary>
        /// Send a packet to the server
        /// </summary>
        /// <param name="packet"> packet to be sent </param>
        public void Send(Packet packet)
        {
            SendUnClear(packet);
            packet.Clear();
        }
        
        /// <summary>
        /// Send a packet with out clearing it
        /// (Warning: This will cause memory leak if not cleared)
        /// </summary>
        /// <param name="packet"> packet to be sent </param>
        public void SendUnClear(Packet packet)
        {
            _sender?.Send(packet.GetBytes());
        }
        
        public void Tick()
        {
            _ticker?.Tick(_processPerTick);
        }

        public void Dispose()
        {
        }

        public enum NetworkState
        {
            Server,
            Client,
            None
        }
        
        /// <summary>
        /// Util method to create a client
        /// </summary>
        private void CreateClient()
        {
            _networkClient = new NetworkClient(_maxMessageSize);
            _networkClient.OnConnected += () => Debug.Log("Connected to server.");
        }
    }
}