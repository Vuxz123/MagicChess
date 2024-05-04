using com.ethnicthv.Other.Networking.Packet;

namespace com.ethnicthv.Other
{
    public abstract class NetworkEvent: Event.Event, IPacketConverter
    {
        public abstract Packet ToPacket(PacketWriter writer);
    }
}