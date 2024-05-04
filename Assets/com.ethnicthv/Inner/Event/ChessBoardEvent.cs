using com.ethnicthv.Other;
using com.ethnicthv.Other.Networking;
using com.ethnicthv.Other.Networking.Packet;

namespace com.ethnicthv.Inner.Event
{
    [Network(fromPacketMethodName: nameof(FromPacket), eventNetworkName: "chessboard")]
    public class ChessBoardEvent : NetworkEvent
    {
        public static ChessBoardEvent Resolver(Packet packet)
        {
            var reader = PacketReader.Create(packet);
            var eventType = (EventType)reader.ReadByte();
            return new ChessBoardEvent(eventType);
        }
        
        public EventType Type { get; private set; }
        public object[] Data { get; private set; }
        
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
        
        public static ChessBoardEvent FromPacket(Packet packet)
        {
            var reader = PacketReader.Create(packet);
            var eventType = (EventType)reader.ReadByte();
            return new ChessBoardEvent(eventType);
        }

        public override string ToString()
        {
            return $"Event Type: {Type}, Data: {Data}";
        }

        public override Packet ToPacket(PacketWriter writer)
        {
            writer.Write((byte)Type);
            return writer.GetPacket();
        }
    }
}