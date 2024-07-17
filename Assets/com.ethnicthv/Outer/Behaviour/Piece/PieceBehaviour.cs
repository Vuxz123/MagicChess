using System;
using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Behaviour.Movement;

namespace com.ethnicthv.Outer.Behaviour.Piece
{
    public class PieceBehaviour : OuterObjectAbstract, IPiece
    {
        private ModifiablePositionMovementBehaviour _movementBehaviour;

        private Queue<ActionData> _actionQueue;

        private (int, int) _newPos;
        private bool _animated = true;

        private void Start()
        {
            _actionQueue = new Queue<ActionData>();
            _movementBehaviour = gameObject.AddComponent<ModifiablePositionMovementBehaviour>();
        }

        private void Update()
        {
            BaseUpdate();
            
            if (_actionQueue.TryDequeue(out var actionData))
            {
                SendActionToInner(actionData);
            }
        }

        protected override void Cleaning()
        {
            var pos = GameManagerOuter.Instance.ChessBoard
                .GetSquare(_newPos.Item1, _newPos.Item2)
                .transform
                .position;
            if(_animated == false) _movementBehaviour.MoveToNotAnimated(pos);
            else _movementBehaviour.MoveTo(pos, 1F, InterpolationFunctions.CurveUp1);
        }

        public void SetPiece(Inner.Object.Piece.Piece piece)
        {
            Inner = piece;
        }

        public ModifiablePositionMovementBehaviour GetMovementBehaviour()
        {
            if (_movementBehaviour)
            {
                throw new NullReferenceException("MovementBehaviour is null. Please set it first.");
            }

            return _movementBehaviour;
        }

        public void SetPosToSquare(int x, int y, bool animated = true)
        {
            MarkDirty();
            _newPos = (x, y);
            _animated = animated;
        }

        public void SetPosToSquare((int, int) pos, bool animated = true)
        {
            SetPosToSquare(pos.Item1, pos.Item2, animated);
        }

        public Inner.Object.Piece.Piece Inner { get; private set; }

        public void DoAction(ActionType move, params object[] data)
        {
            lock (_actionQueue)
            {
                _actionQueue.Enqueue(new ActionData(move, data));
            }
        }

        private void SendActionToInner(ActionData actionData)
        {
            Inner.DoAction(actionData.At, actionData.D);
        }

        private class ActionData
        {
            public readonly ActionType At;
            public readonly object[] D;

            internal ActionData(ActionType at, object[] d)
            {
                At = at;
                D = d;
            }
        }
    }
}