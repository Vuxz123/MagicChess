using System;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Movement;
using UnityEngine;

namespace com.ethnicthv.Outer.Piece
{
    public class PieceBehaviour : MonoBehaviour, IPiece
    {
        private Inner.Object.Piece.Piece _piece;

        private ModifiablePositionMovementBehaviour _movementBehaviour;
        
        private void Start()
        {
            
            _movementBehaviour = gameObject.AddComponent<ModifiablePositionMovementBehaviour>();
        }
        
        public void SetPiece(Inner.Object.Piece.Piece piece)
        {
            _piece = piece;
        }

        public ModifiablePositionMovementBehaviour GetMovementBehaviour()
        {
            if (_movementBehaviour)
            {
                throw new NullReferenceException("MovementBehaviour is null. Please set it first.");
            }
            return _movementBehaviour;
        }
        
        public void SetPosToSquare(int x, int y)
        {
            var t = GameManagerOuter.Instance.ChessBoard.GetSquare(x,y).transform;
            transform.position = t.position;
        }
    }
}