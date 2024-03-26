using com.ethnicthv.Outer.Chess.Square;
using com.ethnicthv.Util.Networking;

namespace com.ethnicthv.Outer.Event
{
    public class OnSquareSelectingEvent : ethnicthv.Util.Event.Event
    {
        private ISquare _square;
        
        public ISquare Square => _square;
        
        public OnSquareSelectingEvent(ISquare square)
        {
            _square = square;
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