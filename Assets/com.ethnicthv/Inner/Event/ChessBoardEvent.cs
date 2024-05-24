using com.ethnicthv.Other;
using com.ethnicthv.Other.Network;
using com.ethnicthv.Other.Network.Client;
using com.ethnicthv.Other.Network.Client.P;

namespace com.ethnicthv.Inner.Event
{
    [Network(eventNetworkName: "chessboard")]
    public class ChessBoardEvent : NetworkEvent
    {
        public EventType Type { get; private set; }
        public object[] Data { get; private set; }
        
        public ChessBoardEvent()
        {
            Type = 0;
        }
        
        public ChessBoardEvent(EventType type, params object[] data)
        {
            Type = type;
            Data = data;
        }
     
        public enum EventType
        {
            Init,
            Move,
            Capture,
            Check,
            Checkmate,
            Stalemate,
            Draw,
            Resign,
            OfferDraw,
            AcceptDraw,
            DeclineDraw,
            OfferRematch,
            AcceptRematch,
            DeclineRematch,
            ResignRematch,
            GameOver
        }
        
        public Packet ToPacket()
        {
            var writer = PacketWriter.Create();
            writer.Write((byte)Type);
            return writer.GetPacket();
        }

        public override string ToString()
        {
            return $"Event Type: {Type}, Data: {Data}";
        }

        public override void ToPacket(PacketWriter writer)
        {
            writer.Write((byte)Type);
        }

        public override Other.Ev.Event FromPacket(PacketReader reader)
        {
            var eventType = (EventType)reader.ReadByte();
            return new ChessBoardEvent(eventType);
        }
    }
}