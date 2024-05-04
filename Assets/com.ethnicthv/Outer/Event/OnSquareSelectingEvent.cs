using com.ethnicthv.Outer.Behaviour.Chess.Square;

namespace com.ethnicthv.Outer.Event
{
    public class OnSquareSelectingEvent : Other.Event.Event
    {
        public ISquare Square { get; }

        public OnSquareSelectingEvent(ISquare square)
        {
            Square = square;
        }
    }
}