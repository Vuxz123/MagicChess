using System.Collections.Generic;

namespace com.ethnicthv.Inner.Object.Piece.Action.DefaultImpl
{
    public class QueenAction : PieceAction
    {
        private static ActionFunction Move = DefaultMove;
        private static ActionFunction Attack = DefaultAttack;
        private static ActionFunction Defend = DefaultDefend;
        private static ActionFunction Dead = (p, d) => { };
        
        public QueenAction() : base(new Dictionary<ActionType, ActionFunction>
        {
            {ActionType.Move, Move},
            {ActionType.Attack, Attack},
            {ActionType.Defend, Defend},
            {ActionType.Dead, Dead},
        })
        {
        }
    }
}