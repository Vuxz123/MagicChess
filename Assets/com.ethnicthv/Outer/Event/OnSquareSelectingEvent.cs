using com.ethnicthv.Outer.Behaviour.Chess.Square;
using com.ethnicthv.Util.Networking.Packet;

namespace com.ethnicthv.Outer.Event
{
    public class OnSquareSelectingEvent : ethnicthv.Util.Event.Event
    {
        public ISquare Square { get; }

        public OnSquareSelectingEvent(ISquare square)
        {
            Square = square;
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