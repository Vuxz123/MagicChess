using System;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Movement;
using com.ethnicthv.Outer.Util;

namespace com.ethnicthv.Outer.Piece
{
    public interface IPiece
    {
        public ModifiablePositionMovementBehaviour GetMovementBehaviour();
        public void SetPosToSquare(int x, int y);
    }
}