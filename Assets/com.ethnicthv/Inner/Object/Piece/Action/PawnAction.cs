using System.Collections.Generic;
using System.Diagnostics;
using com.ethnicthv.Outer.Piece;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public class PawnAction : PieceAction
    {
        private static ActionFunction Move = (p, d) =>
        {
            if (!p.IsMovable()) return;
            
            var board = GameManagerInner.Instance.Board;
            
            Debug.Assert(d.Length == 2);
            Debug.Assert(d[0] is IPiece);
            Debug.Assert(d[1] is (int, int));
            var controller = (IPiece) d[0];
            var targetPosition = ((int, int)) d[1];
            var currentPosition = board.GetPiecePosition(p);

            
            var target = board[targetPosition];

            if (target == null) return;
            
            if(target == p) return;
            
            if (target.GetPieceSide() == p.GetPieceSide()) return;
            
            board.MovePiece(currentPosition.Item1, currentPosition.Item2, targetPosition.Item1, targetPosition.Item2);
        };
        private static ActionFunction Attack = (p, d) => { };
        private static ActionFunction Defend = (p, d) => { };
        private static ActionFunction Dead = (p, d) => { };
        
        public PawnAction() : base(Piece.Type.Pawn ,new Dictionary<ActionType, ActionFunction>
        {
            {ActionType.Move, Move},
            {ActionType.Attack, Attack},
            {ActionType.Defend, Defend},
            {ActionType.Dead, Dead}
        })
        {
        }
    }
}