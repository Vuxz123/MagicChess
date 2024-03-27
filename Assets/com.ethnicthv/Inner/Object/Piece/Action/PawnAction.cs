using System.Collections.Generic;
using com.ethnicthv.Outer.Behaviour.Piece;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public class PawnAction : PieceAction
    {
        private static ActionFunction Move = (p, d) =>
        {
            Debug.Log("PawnAction: Move PawnAction");
            if (!p.IsMovable()) return;
            Debug.Log("PawnAction: Pawn isMovable");
            var board = GameManagerInner.Instance.Board;
            
            Debug.Assert(d.Length == 2);
            Debug.Assert(d[0] is IPiece);
            Debug.Assert(d[1] is (int, int));
            var controller = (IPiece) d[0];
            var targetPosition = ((int, int)) d[1];
            var currentPosition = board.GetPiecePosition(p);
            Debug.Log("PawnAction: Getting Data Complete");
            
            var target = board[targetPosition];
            Debug.Log("PawnAction: Target: " + target);
            if (target != null) return;

            Debug.Log("PawnAction: Calling MovePiece");
            board.MovePiece(controller, currentPosition.Item1, currentPosition.Item2, targetPosition.Item1, targetPosition.Item2);
        };
        private static ActionFunction Attack = (p, d) => { };
        private static ActionFunction Defend = (p, d) => { };
        private static ActionFunction Dead = (p, d) => { };
        
        public PawnAction() : base(Piece.Type.Pawn ,new Dictionary<ActionType, ActionFunction>
        {
            {ActionType.Move, Move},
        })
        {
        }
    }
}