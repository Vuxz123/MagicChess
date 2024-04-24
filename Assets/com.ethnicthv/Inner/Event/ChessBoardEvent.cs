using com.ethnicthv.Util.Networking.Packet;

namespace com.ethnicthv.Inner.Event
{
    public class ChessBoardEvent : Util.Event.Event
    {
        public Type type { get; private set; }
        public object[] data { get; private set; }
        
        public ChessBoardEvent(Type type, params object[] data)
        {
            this.type = type;
            this.data = data;
        }
     
        public enum Type
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

        public override Packet PackEvent()
        {
            return null;
        }

        public override void UnpackEvent(Packet packet)
        {
            return;
        }
    }
}