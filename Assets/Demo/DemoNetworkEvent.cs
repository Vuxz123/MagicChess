using com.ethnicthv.Other;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Other.Network.Client.P;

namespace Demo
{
    // This attribute is needed to specify the method that will be used to convert the packet to the event.
    // [Network(fromPacketMethodName: nameof(FromPacket), eventNetworkName: "DemoNetworkEvent")]
    
    // NetworkEvent is a framework class that defines the basic structure of a network event.
    public class DemoNetworkEvent: NetworkEvent
    {
        private int a;
        private int b;
        private bool c;
        private float d;
        private byte e;
        
        public DemoNetworkEvent(int a, int b, bool c, float d, byte e)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
            this.e = e;
        }
        
        public override void ToPacket(PacketWriter writer)
        {
            writer.Write(a);
            writer.Write(b);
            writer.Write(c);
            writer.Write(d);
            writer.Write(e);
        }

        public override Event FromPacket(PacketReader reader)
        {
            return new DemoNetworkEvent(
                reader.ReadInt(),
                reader.ReadInt(),
                reader.ReadBool(),
                reader.ReadFloat(),
                reader.ReadByte()
            );
        }
    }
}