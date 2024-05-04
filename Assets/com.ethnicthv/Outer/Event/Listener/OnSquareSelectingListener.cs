using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Other.Event;
using com.ethnicthv.Outer.Behaviour.Chess.Square;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Outer.Event.Listener
{
    [EventListener(typeof(OnSquareSelectingEvent))]
    public class OnSquareSelectedListener
    {
        private static readonly List<ISquare> SelectedSquares = new();
        private static readonly List<ISquare> PossibleMoves = new();
        
        private static readonly object _lock = new object();
        private static bool _isSelecting;
        private static SelectingAction _action;

        public static SelectingAction Action
        {
            get => _action;
            set
            {
                lock (_lock)
                {
                    if(_isSelecting) return;
                    _action = value;
                }
            }
        }

        [LocalHandler]
        public bool HandleEventLocal(OnSquareSelectingEvent e)
        {
            return Action switch
            {
                SelectingAction.Move => HandleMove(e),
                SelectingAction.Attack => HandleAttack(e),
                SelectingAction.Defend => HandleDefend(e),
                _ => false
            };
        }

        private static bool HandleDefend(OnSquareSelectingEvent onSquareSelectingEvent)
        {
            return false;
        }

        private static bool HandleMove(OnSquareSelectingEvent e)
        {
            Debug.Log("OSSHandler: " + e);
            // If there is no selected square
            // Select the square and highlight possible moves
            if (SelectedSquares.Count == 0)
            {
                _isSelecting = true;
                Debug.Log("OSSHandler: First selection");
                Debug.Log("OSSHandler: HasPiece: " + e.Square.HasPiece());
                if (!e.Square.HasPiece())
                {
                    return false;
                }

                var piece = GameManagerOuter.Instance.ChessBoard.GetPiece(e.Square.Pos);
                var pieceProperties = piece.Inner.PieceProperties;
                Debug.Log("OSSHandler: Piece: " + pieceProperties.MovementStyle);
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
                    PossibleMoves.Add(s);
                }
                SelectedSquares.Add(e.Square);
                e.Square.Highlight(Color.yellow);
            }
            else
            {
                Debug.Log("OSSHandler: Second selection");
                if (e.Square.HasPiece()) return false;
                if (!PossibleMoves.Contains(e.Square)) return false;   
                Debug.Log("OSSHandler: Confirm move: " + e.Square.Pos);
                var iPiece = GameManagerInner.Instance.Board[
                    GameManagerInner.ConvertOuterToInnerPos(SelectedSquares[0].Pos)
                ].Outer;
                try
                {
                    iPiece.DoAction(ActionType.Move,
                        iPiece,
                        GameManagerInner.ConvertOuterToInnerPos(e.Square.Pos)
                    );
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception);
                    throw;
                }

                Debug.Log("OSSHandler: Move done");
                Finish();
                Debug.Log("OSSHandler: Clear highlight");
            }
            
            return true;
        }

        private static bool HandleAttack(OnSquareSelectingEvent e)
        {
            Debug.Log("OSSHandler: " + e);
            // If there is no selected square
            // Select the square and highlight possible moves
            
            if (SelectedSquares.Count == 0)
            {
                _isSelecting = true;
                Debug.Log("OSSHandler: First selection");
                Debug.Log("OSSHandler: HasPiece: " + e.Square.HasPiece());
                if (!e.Square.HasPiece())
                {
                    return false;
                }

                var piece = GameManagerOuter.Instance.ChessBoard.GetPiece(e.Square.Pos);
                var pieceProperties = piece.Inner.PieceProperties;
                Debug.Log("OSSHandler: Piece: " + pieceProperties.MovementStyle);
                var ip = GameManagerInner.ConvertOuterToInnerPos(e.Square.Pos);
                Debug.Log("OSSHandler: Inner pos: " + ip);
                var possibleAttacks = pieceProperties.AttackType
                    .GetAvailableAttacks(
                        piece.Inner.side,
                        ip.Item1,
                        ip.Item2,
                        pieceProperties.MovementRange
                    );
                Debug.Log($"OSSHandler: {possibleAttacks.Count} Possible moves: ");
                foreach (var temp in possibleAttacks.Select(GameManagerOuter.ConvertInnerToOuterPos))
                {
                    Debug.Log("OSSHandler: Possible move: " + temp);
                    var s = GameManagerOuter.Instance.ChessBoard.GetSquare(temp.Item1, temp.Item2);
                    s.Highlight(Color.green);
                    PossibleMoves.Add(s);
                }
                SelectedSquares.Add(e.Square);
                e.Square.Highlight(Color.yellow);
            }
            else
            {
                Debug.Log("OSSHandler: Second selection");
                if (!PossibleMoves.Contains(e.Square)) return false;   
                Debug.Log("OSSHandler: Confirm move: " + e.Square.Pos);
                var iPiece = GameManagerInner.Instance.Board[
                    GameManagerInner.ConvertOuterToInnerPos(SelectedSquares[0].Pos)
                ].Outer;
                try
                {
                    iPiece.DoAction(ActionType.Attack,
                        iPiece,
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
            
            return false;
        }

        private static void ClearHighlight()
        {
            foreach (var square in SelectedSquares)
            {
                square.UnHighlight();
            }
            foreach (var square in PossibleMoves)
            {
                square.UnHighlight();
            }
            SelectedSquares.Clear();
        }

        private static void Finish()
        {
            _isSelecting = false;
            ClearHighlight();
        }
        
        public static void CancelSelecting()
        {
            _isSelecting = false;
            ClearHighlight();
        }

        public enum SelectingAction
        {
            Move,
            Attack,
            Defend,
        }
    }
}