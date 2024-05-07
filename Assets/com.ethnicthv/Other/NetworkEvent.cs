using com.ethnicthv.Other.Ev;
using com.ethnicthv.Other.Networking.P;

namespace com.ethnicthv.Other
{
    public abstract class NetworkEvent: Event, IPacketConverter
    {
        public abstract void ToPacket(PacketWriter writer);
        public abstract Event FromPacket(PacketReader packet);
    }
}