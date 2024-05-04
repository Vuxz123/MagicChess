namespace com.ethnicthv.Other.Networking.Packet
{
    public interface IPacketConverter
    {
        public Packet ToPacket(PacketWriter writer);
    }
}