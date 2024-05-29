using com.ethnicthv.Other.Ev;
using com.ethnicthv.Other.Network;
using com.ethnicthv.Other.Network.P;

namespace com.ethnicthv.Other
{
    public abstract class NetworkEvent: Event, NetworkObject
    {
        public abstract void ToPacket(PacketWriter writer);
        public abstract Event FromPacket(PacketReader packet);
    }
}