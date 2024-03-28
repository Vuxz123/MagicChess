using System;
using System.Collections.Generic;
using System.Linq;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Behaviour.Chess.Square;
using com.ethnicthv.Outer.Behaviour.Piece;
using com.ethnicthv.Util.Event;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Outer.Event.Listener
{
    [EventListener(typeof(OnSquareSelectingEvent))]
    public class OnSquareSelectedListener
    {
        private static readonly List<ISquare> _selectedSquares = new();
        private static readonly List<ISquare> _possibleMoves = new();

        [LocalHandler]
        public bool HandleEventLocal(OnSquareSelectingEvent e)
        {
            Debug.Log("OSSHandler: " + e);
            // If there is no selected square
            // Select the square and highlight possible moves
            if (_selectedSquares.Count == 0)
            {
                Debug.Log("OSSHandler: First selection");
                Debug.Log("OSSHandler: HasPiece: " + e.Square.HasPiece());
                if (!e.Square.HasPiece())
                {
                    return false;
                }

                var piece = GameManagerOuter.Instance.ChessBoard.GetPiece(e.Square.Pos);
                var pieceProperties = piece.Inner.PieceProperties;
                Debug.Log("OSSHandler: Piece: " + pieceProperties.MovementStyle.ToString());
                var ip = GameManagerInner.ConvertOuterToInnerPos(e.Square.Pos);
                Debug.Log("OSSHandler: Inner pos: " + ip);
                var possibleMoves = pieceProperties.MovementStyle
                    .GetAvailableMoves(
                        piece.Inner.side,
                        ip.Item1,
                        ip.Item2,
                        pieceProperties.MovementRange
                    );
                Debug.Log($"OSSHandler: {possibleMoves.Count} Possible moves: ");
                foreach (var temp in possibleMoves.Select(GameManagerOuter.ConvertInnerToOuterPos))
                {
                    Debug.Log("OSSHandler: Possible move: " + temp);
                    var s = GameManagerOuter.Instance.ChessBoard.GetSquare(temp.Item1, temp.Item2);
                    if(s.HasPiece()) continue;
                    s.Highlight(Color.green);
                    _possibleMoves.Add(s);
                }
                _selectedSquares.Add(e.Square);
                e.Square.Highlight(Color.yellow);
            }
            else
            {
                Debug.Log("OSSHandler: Second selection");
                if (e.Square.HasPiece()) return false;
                if (!_possibleMoves.Contains(e.Square)) return false;   
                Debug.Log("OSSHandler: Confirm move: " + e.Square.Pos);
                var ipiece = GameManagerInner.Instance.Board[
                    GameManagerInner.ConvertOuterToInnerPos(_selectedSquares[0].Pos)
                ].Outer;
                try
                {
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
            }
            
            return true;
        }

        private static void ClearHighlight()
        {
            foreach (var square in _selectedSquares)
            {
                square.UnHighlight();
            }
            foreach (var square in _possibleMoves)
            {
                square.UnHighlight();
            }
            _selectedSquares.Clear();
        }
        
        public static void CancelSelecting()
        {
            ClearHighlight();
        }
    }
}