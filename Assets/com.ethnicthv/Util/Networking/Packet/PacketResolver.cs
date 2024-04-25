using System;
using System.Collections.Generic;

namespace com.ethnicthv.Util.Networking.Packet
{
    
    public delegate object Resolver(Packet packet);
    
    public static class PacketResolver
    {
        private static readonly Dictionary<byte, (Type, Resolver)> PacketTypes = new Dictionary<byte, (Type, Resolver)>();
        public static void RegisterPacketType(byte id, Type type, Resolver resolver)
        {
            if (PacketTypes.ContainsKey(id))
            {
                throw new ArgumentException($"Packet type with id {id} is already registered.");
            }
            PacketTypes[id] = (type, resolver);
        }
        
        public static (Type, object) ResolvePacketType(Packet packet)
        {
            var temp = PacketReader.Create(packet);
            var id = temp.ReadByte();
            temp.Close();
            if (!PacketTypes.ContainsKey(id))
            {
                throw new ArgumentException($"Packet type with id {id} is not registered.");
            }
            var obj = PacketTypes[id].Item2(packet);
            return (PacketTypes[id].Item1, obj);
        }
    }
}