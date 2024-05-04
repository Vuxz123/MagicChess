using System;
using com.ethnicthv.Inner.Object.Piece;

namespace com.ethnicthv.Inner.Event
{
    public class PieceEvent : Other.Event.Event
    {
        public Type type { get; private set; }

        public Piece piece { get; private set; }
        
        public object[] data { get; private set; }
        
        public PieceEvent(Type type, Piece piece, params object[] data)
        {
            this.piece = piece;
            this.type = type;
            this.data = data;
        }
        
        public enum Type
        {
            Move,
            Attack,
            Defend,
            Die,
            Damage,
            Heal
        }
    }
}