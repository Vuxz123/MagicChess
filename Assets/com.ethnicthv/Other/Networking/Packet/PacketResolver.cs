using System;
using System.Collections.Generic;

namespace com.ethnicthv.Other.Networking.Packet
{
    
    public delegate object Resolver(Other.Networking.Packet.Packet packet);
    
    public class PacketResolver
    {
        private readonly Dictionary<byte, (Type, Resolver)> _packetTypes = new();
        public void RegisterPacketType(byte id, Type type, Resolver resolver)
        {
            if (_packetTypes.ContainsKey(id))
            {
                throw new ArgumentException($"Packet type with id {id} is already registered.");
            }
            _packetTypes[id] = (type, resolver);
        }
        
        public (Type, object) ResolvePacketType(Packet packet)
        {
            var temp = PacketReader.Create(packet);
            var id = temp.ReadByte();
            temp.Close();
            if (!_packetTypes.ContainsKey(id))
            {
                throw new ArgumentException($"Packet type with id {id} is not registered.");
            }
            var obj = _packetTypes[id].Item2(packet);
            return (_packetTypes[id].Item1, obj);
        }
    }
}