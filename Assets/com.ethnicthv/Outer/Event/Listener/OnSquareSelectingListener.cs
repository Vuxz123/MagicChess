using System;
using System.Collections.Generic;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Behaviour.Chess.Square;
using com.ethnicthv.Util.Event;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Outer.Event.Listener
{
    [EventListener(typeof(OnSquareSelectingEvent))]
    public class OnSquareSelectedListener
    {
        private readonly List<ISquare> _selectedSquares = new();

        [LocalListener]
        public bool HandleEventLocal(OnSquareSelectingEvent e)
        {
            Debug.Log("OnSquareSelectedListener: " + e);
            if (_selectedSquares.Count == 0 && !e.Square.HasPiece()) return false;
            Debug.Log("HasPiece: " + e.Square.HasPiece());
            _selectedSquares.Add(e.Square);
            e.Square.Highlight(_selectedSquares.Count == 0 ? Color.green : Color.yellow);
            if (_selectedSquares.Count <= 1) return true;
            Debug.Log("Confirm move: " + e.Square.Pos);
            try
            {
                
                var ipiece = GameManagerInner.Instance.Board[GameManagerInner.ConvertOuterToInnerPos(_selectedSquares[0].Pos)].Outer;
                ipiece.DoAction(ActionType.Move, 
                    ipiece,
                    GameManagerInner.ConvertOuterToInnerPos(e.Square.Pos)
                );
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
                throw;
            }
            Debug.Log("OSSHandler: Move done");
            ClearHighlight();
            Debug.Log("OSSHandler: Clear highlight");
            return true;
        }

        private void ClearHighlight()
        {
            foreach (var square in _selectedSquares)
            {
                square.UnHighlight();
            }

            _selectedSquares.Clear();
        }
    }
}