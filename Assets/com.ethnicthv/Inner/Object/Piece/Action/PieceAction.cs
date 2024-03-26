using System;
using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece.Exception;
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
    }
}