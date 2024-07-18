using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece.Exception;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public class PieceActionManager
    {
        public readonly Dictionary<int, PieceAction> TypeMap = new();
        
        public void RegisterAction(int actionID, PieceAction action)
        {
            if(TypeMap.TryGetValue(actionID, out var currentAction))
            {
                if (currentAction.GetType() == action.GetType())
                {
                    return;
                }
            }
            TypeMap.Add(actionID, action);
        }
        
        public PieceAction GetPieceAction(int actionID)
        {
            if (TypeMap.TryGetValue(actionID, out var pieceAction))
            {
                return pieceAction;
            }
            throw new ActionTypeNotFoundException($"action id {actionID} not found in piece action map");
        }
    }
}