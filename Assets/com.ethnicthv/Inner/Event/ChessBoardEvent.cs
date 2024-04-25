using com.ethnicthv.Util;
using com.ethnicthv.Util.Networking.Packet;

namespace com.ethnicthv.Inner.Event
{
    public class ChessBoardEvent : NetworkEvent
    {
        public static ChessBoardEvent Resolver(Packet packet)
        {
            var reader = PacketReader.Create(packet);
            var eventType = (EventType)reader.ReadByte();
            return new ChessBoardEvent(eventType);
        }
        
        public EventType eventType { get; private set; }
        public object[] data { get; private set; }
        
        public ChessBoardEvent(EventType eventType, params object[] data) : base(typeof(ChessBoardEvent), Resolver)
        {
            this.eventType = eventType;
            this.data = data;
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

        public override Packet ToPacket()
        {
            throw new System.NotImplementedException();
        }
    }
}