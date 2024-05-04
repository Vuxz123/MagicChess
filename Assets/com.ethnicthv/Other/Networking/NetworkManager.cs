using System;
using System.Collections.Generic;
using System.Reflection;
using com.ethnicthv.Other.Networking.Packet;

namespace com.ethnicthv.Other.Networking
{
    public class NetworkManager
    {
        public readonly PacketResolver Resolver;
        public readonly NetworkID NetID;
        
        private static NetworkManager _instance;
        
        private readonly Dictionary<Type, byte> PacketTypes = new();
        
        public static NetworkManager Instance => _instance ??= new NetworkManager();
        
        private NetworkManager()
        {
            Resolver = new PacketResolver();
            NetID = new NetworkID();
        }

        public void Init()
        {
            var networkEvents = ReflectionHelper.GetClassesWithAttribute<NetworkAttribute>();
            var i = 0;
            foreach (var networkEvent in networkEvents)
            {
                var attribute = networkEvent.GetCustomAttribute<NetworkAttribute>();
                var fromPacketMethod = attribute.GetFromPacketMethod(networkEvent);
                var id = NetID.GetID(attribute.EventNetworkName);
                PacketTypes[networkEvent] = id;
                Resolver.RegisterPacketType(id, networkEvent, packet => fromPacketMethod.Invoke(null, new object[] {packet}));
                i++;
            }
            
            Debug.Log($"Registered {i} network events resolver.");
        }
        
        public Event.Event ResolvePacket(Packet.Packet packet)
        {
            var (_, obj) = Resolver.ResolvePacketType(packet);
            return obj as Event.Event;
        }
        
        public Packet.Packet PacketizeEvent(Event.Event e)
        {
            var writer = PacketWriter.Create();
            writer.Write(PacketTypes[e.GetType()]);
            var converter = e as IPacketConverter;
            return converter?.ToPacket(writer);
        }
    }
}