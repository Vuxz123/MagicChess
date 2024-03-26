using WebSocketSharp;

namespace com.ethnicthv.Util.Networking
{
    public class HeaderPacket: Packet
    {
        public static readonly byte PacketType = 0;
        public static readonly byte PacketTypeLength = 1;
        public HeaderPacket(byte headerID) : base(PacketType,headerID.ToByteArray(ByteOrder.Big))
        {
            
        }
    }
}