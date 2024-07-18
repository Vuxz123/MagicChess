using System;
using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece.Exception;
using com.ethnicthv.Outer.Behaviour.Piece;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public delegate void ActionFunction(Piece piece, params object[] data);
    public abstract class PieceAction
    {
        protected Dictionary<ActionType, ActionFunction> ActionMap;

        protected PieceAction(Dictionary<ActionType, ActionFunction> actionMap)
        {
            ActionMap = actionMap;
        }

        public void DoAction(ActionType type, Piece piece, params object[] data)
        {
            if (ActionMap.TryGetValue(type, out var actionFunction))
            {
                if (data.Length != type.Np)
                    throw new ActionParamNotMatchException(
                        $"Action {type} requires {type.Np} parameters, but {data.Length} were given");
                actionFunction(piece, data);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        protected static readonly ActionFunction DefaultMove = (p, d) =>
        {
            Debug.Log("DefaultMove: Called");
            if (!p.IsMovable()) return;
            Debug.Log("DefaultMove: Piece isMovable");
            var board = GameManagerInner.Instance.Board;

            Debug.Assert(d.Length == 2);
            Debug.Assert(d[0] is IPiece);
            Debug.Assert(d[1] is (int, int));
            var controller = (IPiece)d[0];
            var targetPosition = ((int, int))d[1];
            var currentPosition = board.GetPiecePosition(p);
            Debug.Log("DefaultMove: Getting Data Complete");

            var target = board[targetPosition];
            Debug.Log("DefaultMove: Target: " + target);
            if (target != null) return;

            Debug.Log("DefaultMove: Calling MovePiece");
            board.MovePiece(controller, currentPosition.Item1, currentPosition.Item2, targetPosition.Item1,
                targetPosition.Item2);
        };

        protected static readonly ActionFunction DefaultAttack = (p, d) =>
        {
            Debug.Log("DefaultAttack: Called");
            if (!p.IsAttackable()) return;
            Debug.Log("DefaultAttack: Piece isAttackable");
            var board = GameManagerInner.Instance.Board;

            Debug.Assert(d.Length == 2);
            Debug.Assert(d[0] is IPiece);
            Debug.Assert(d[1] is (int, int));
            var controller = (IPiece)d[0];
            var targetPosition = ((int, int))d[1];
            var currentPosition = board.GetPiecePosition(p);
            Debug.Log("DefaultAttack: Getting Data Complete");

            var target = board[targetPosition];
            Debug.Log("DefaultAttack: Target: " + target);
            if (target == null) return;

            Debug.Log("DefaultAttack: Perform Attack");
            //TODO: Attack
        };

        protected static readonly ActionFunction DefaultDefend = (p, d) =>
        {
            Debug.Log("DefaultDefend: Called");
            if (!p.IsDefendable()) return;
            Debug.Log(("DefaultDefend: Piece isDefendable"));
            var board = GameManagerInner.Instance.Board;

            Debug.Assert(d.Length == 2);
            Debug.Assert(d[0] is IPiece);
            Debug.Assert(d[1] is (int, int));
            var controller = (IPiece)d[0];
            var targetPosition = ((int, int))d[1];
            var currentPosition = board.GetPiecePosition(p);
            Debug.Log("DefaultDefend: Getting Data Complete");

            var target = board[targetPosition];
            Debug.Log("DefaultDefend: Target: " + target);
            if (target == null) return;

            Debug.Log("DefaultDefend: Perform Defend");
            //TODO: Defend
        };
    }
}