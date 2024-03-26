using System.Collections.Generic;
using com.ethnicthv.Outer.Chess.Square;
using com.ethnicthv.Util.Event;
using UnityEngine;

namespace com.ethnicthv.Outer.Event.Listener
{
    [EventListener(typeof(OnSquareSelectingEvent))]
    public class OnSquareSelectedListener
    {
        private List<ISquare> _selectedSquares = new();
        
        [LocalListener]
        public bool HandleEventLocal(OnSquareSelectingEvent e)
        {
            _selectedSquares.Add(e.Square);
            e.Square.Highlight(_selectedSquares.Count == 0 ? Color.green : Color.yellow);
            if (_selectedSquares.Count <= 2) return true;
            foreach (var square in _selectedSquares)
            {
                square.UnHighlight();
            }
            _selectedSquares.Clear();
            return true;
        }
    }
}