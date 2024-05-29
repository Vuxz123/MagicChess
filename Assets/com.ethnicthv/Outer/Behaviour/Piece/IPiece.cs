using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Behaviour.Movement;

namespace com.ethnicthv.Outer.Behaviour.Piece
{
    public interface IPiece
    {
        public ModifiablePositionMovementBehaviour GetMovementBehaviour();
        public void SetPosToSquare(int x, int y, bool animated = true);
        
        public void SetPosToSquare((int, int) pos, bool animated = true)
        {
            SetPosToSquare(pos.Item1, pos.Item2, animated);
        }
        
        public Inner.Object.Piece.Piece Inner { get; }

        void DoAction(ActionType move, params object[] data);
    }
}