using System;
using System.Collections.Generic;
using System.Reflection;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Other.Networking.P;

namespace com.ethnicthv.Other.Networking
{
    public delegate Event FromPacketDelegate(Packet packet);
    public class NetworkManager
    {
        public readonly NetworkID NetID;
        
        private static NetworkManager _instance;
        
        private readonly Dictionary<Type, byte> _packetTypes = new();
        private readonly Dictionary<byte, NetworkEvent> _eventBuilders = new();

        public static NetworkManager Instance => _instance ??= new NetworkManager();
        
        private NetworkManager()
        {
            NetID = new NetworkID();
        }

        public void Init()
        {
            var networkEvents = ReflectionHelper.GetClassesWithAttribute<NetworkAttribute>();
            var i = 0;
            foreach (var networkEvent in networkEvents)
            {
                var attribute = networkEvent.GetCustomAttribute<NetworkAttribute>();
                var id = NetID.GetID(attribute.EventNetworkName);
                _packetTypes[networkEvent] = id;
                _eventBuilders[id] = (NetworkEvent) Activator.CreateInstance(networkEvent);
                i++;
            }
            
            Debug.Log($"Registered {i} network events resolver.");
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
    }
}