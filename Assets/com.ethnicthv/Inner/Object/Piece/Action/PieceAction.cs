using System;
using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece.Exception;
using com.ethnicthv.Outer.Behaviour.Piece;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public delegate void ActionFunction(Piece piece, params object[] data);
    public abstract class PieceAction
    {
        private static Dictionary<Inner.Object.Piece.Piece.Type, PieceAction> TypeMap = new();
        
        public static PieceAction GetPieceAction(Inner.Object.Piece.Piece.Type type)
        {
            if (TypeMap.TryGetValue(type, out var pieceAction))
            {
                return pieceAction;
            }
            throw new ActionTypeNotFoundException($"piece type {type} not found in piece action map");
        }
        
        public static void Setup()
        {
            new KingAction();
            new QueenAction();
            new BishopAction();
            new KnightAction();
            new RookAction();
            new PawnAction();
        }

        protected Dictionary<ActionType, ActionFunction> ActionMap;

        protected PieceAction(Inner.Object.Piece.Piece.Type type , Dictionary<ActionType, ActionFunction> actionMap)
        {
            TypeMap[type] = this;
            ActionMap = actionMap;
        }

        public void DoAction(ActionType type, Piece piece, params object[] data)
        {
            if (ActionMap.TryGetValue(type, out var actionFunction))
            {
                if (data.Length != (int)type)
                    throw new ActionParamNotMatchException(
                        $"Action {type} requires {(int)type} parameters, but {data.Length} were given");
                actionFunction(piece, data);
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public static ActionFunction DefaultMove = (p, d) =>
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
    }
}