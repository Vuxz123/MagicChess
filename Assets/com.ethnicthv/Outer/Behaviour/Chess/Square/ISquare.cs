using com.ethnicthv.Outer.Util;
using UnityEngine;

namespace com.ethnicthv.Outer.Behaviour.Chess.Square
{
    public interface ISquare
    {
        public CbPos Pos { get; }
        public CbType Type { get; }
        public Transform transform { get; }
        
        public void Highlight(Color color);
        public void UnHighlight();
        public void MarkDirty();
        
        public bool HasPiece();
    }
}