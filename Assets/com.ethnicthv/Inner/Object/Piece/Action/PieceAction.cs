using System;
using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece.Exception;
using com.ethnicthv.Outer.Piece;

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
            throw new ArgumentOutOfRangeException();
        }

        protected Dictionary<ActionType, ActionFunction> ActionMap = new Dictionary<ActionType, ActionFunction>();

        protected PieceAction(Inner.Object.Piece.Piece.Type type , Dictionary<ActionType, ActionFunction> actionMap)
        {
            TypeMap[type] = this;
            ActionMap = actionMap;
        }

        public void DoAction(ActionType type, Piece piece, params object[] data)
        {
            if (ActionMap.TryGetValue(type, out var actionFunction))
            {
                if (data.Length == (int)type)
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