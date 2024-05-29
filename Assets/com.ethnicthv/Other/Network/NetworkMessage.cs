using com.ethnicthv.Other.Network.P;

namespace com.ethnicthv.Other.Network
{
    public abstract class NetworkMessage: NetworkObject
    {
        public abstract void ToPacket(PacketWriter writer);
        public abstract NetworkMessage FromPacket(PacketReader packet);
    }
}