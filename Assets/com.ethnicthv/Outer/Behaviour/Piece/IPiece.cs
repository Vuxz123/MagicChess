using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Behaviour.Movement;

namespace com.ethnicthv.Outer.Behaviour.Piece
{
    public interface IPiece
    {
        public ModifiablePositionMovementBehaviour GetMovementBehaviour();
        public void SetPosToSquare(int x, int y);
        
        public void SetPosToSquare((int, int) pos)
        {
            SetPosToSquare(pos.Item1, pos.Item2);
        }
        
        public Inner.Object.Piece.Piece Inner { get; }

        void DoAction(ActionType move, params object[] data);
    }
}