﻿using System.Collections.Generic;
using com.ethnicthv.Outer.Behaviour.Piece;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public class BishopAction : PieceAction
    {
        private static ActionFunction Move = DefaultMove;
        private static ActionFunction Attack = DefaultAttack;
        private static ActionFunction Defend = DefaultDefend;
        private static ActionFunction Dead = (p, d) => { };
        
        public BishopAction() : base(Piece.Type.Bishop ,new Dictionary<ActionType, ActionFunction>
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