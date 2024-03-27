using System;
using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Outer.Behaviour.Movement;
using UnityEngine;

namespace com.ethnicthv.Outer.Behaviour.Piece
{
    public class PieceBehaviour : OuterObjectAbstract, IPiece
    {
        private ModifiablePositionMovementBehaviour _movementBehaviour;

        private Queue<ActionData> _actionQueue;

        private (int, int) _newPos;

        private void Start()
        {
            _actionQueue = new Queue<ActionData>();
            _movementBehaviour = gameObject.AddComponent<ModifiablePositionMovementBehaviour>();
        }

        private new void Update()
        {
            if (_actionQueue.TryDequeue(out var actionData))
            {
                SendActionToInner(actionData);
            }

            base.Update();
        }

        protected override void Cleaning()
        {
            _movementBehaviour.MoveTo(GameManagerOuter.Instance.ChessBoard
                .GetSquare(_newPos.Item1, _newPos.Item2)
                .transform
                .position, 1F, InterpolationFunctions.CurveUp1);
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

        public void SetPosToSquare(int x, int y)
        {
            MarkDirty();
            _newPos = (x, y);
        }

        public void SetPosToSquare((int, int) pos)
        {
            SetPosToSquare(pos.Item1, pos.Item2);
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
            Inner.DoAction(actionData.at, actionData.d);
        }

        private class ActionData
        {
            public ActionType at;
            public object[] d;

            internal ActionData(ActionType at, object[] d)
            {
                this.at = at;
                this.d = d;
            }
        }
    }
}