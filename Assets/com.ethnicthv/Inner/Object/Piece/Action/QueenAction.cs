using System.Collections.Generic;
using com.ethnicthv.Outer.Behaviour.Piece;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public class QueenAction : PieceAction
    {
        private static ActionFunction Move = DefaultMove;
        private static ActionFunction Attack = DefaultAttack;
        private static ActionFunction Defend = DefaultDefend;
        private static ActionFunction Dead = (p, d) => { };
        
        public QueenAction() : base(Piece.Type.Queen ,new Dictionary<ActionType, ActionFunction>
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