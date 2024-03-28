using System.Collections.Generic;
using com.ethnicthv.Outer.Behaviour.Piece;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Inner.Object.Piece.Action
{
    public class RookAction : PieceAction
    {
        private static ActionFunction Move = DefaultMove;
        private static ActionFunction Attack = (p, d) => { };
        private static ActionFunction Defend = (p, d) => { };
        private static ActionFunction Dead = (p, d) => { };
        
        public RookAction() : base(Piece.Type.Rook ,new Dictionary<ActionType, ActionFunction>
        {
            {ActionType.Move, Move},
        })
        {
        }
    }
}