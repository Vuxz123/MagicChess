using System.Collections.Generic;
using com.ethnicthv.Outer.Behaviour.Piece;
using Unity.VisualScripting;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public class KingAction :PieceAction
    {
        private static ActionFunction Move = (p, d) =>
        {
            if (!p.IsMovable()) return;
            
            var board = GameManagerInner.Instance.Board;
            
            Debug.Assert(d[0] is IPiece, "d[0] is IPiece");
            Debug.Assert(d[1] is (int, int), "d[1] is (int, int)");
            var controller = (IPiece) d[0];
            var targetPosition = ((int, int)) d[1];
            var currentPosition = board.GetPiecePosition(p);

            
            var target = board[targetPosition];

            if (target == null) return;
            
            if(target == p) return;
            
            if (target.GetPieceSide() == p.GetPieceSide()) return;
            
            board.MovePiece( controller , currentPosition.Item1, currentPosition.Item2, targetPosition.Item1, targetPosition.Item2);
        };
        private static ActionFunction Attack = (p, d) => { };
        private static ActionFunction Defend = (p, d) => { };
        private static ActionFunction Dead = (p, d) => { };
        
        public KingAction() : base(Piece.Type.King ,new Dictionary<ActionType, ActionFunction>
        {
            {ActionType.Move, Move},
        })
        {
        }
    }
}