using System;
using System.Diagnostics.CodeAnalysis;
using com.ethnicthv.Util.Networking.Packet;

namespace com.ethnicthv.Util
{
    public abstract class NetworkEvent: Event.Event, IPacketSerializable
    {
        private static byte _counter = 0;
        
        public byte Id { get; }
        
        public NetworkEvent([NotNull] Type eventType,[NotNull] Resolver resolver)
        {
            Id = _counter;
            _counter++;
            PacketResolver.RegisterPacketType(Id, eventType, resolver);
        }

        public abstract Packet ToPacket();
    }
}