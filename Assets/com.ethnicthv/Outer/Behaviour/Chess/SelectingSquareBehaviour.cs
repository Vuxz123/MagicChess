using System;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Outer.Behaviour.Chess.Square;
using com.ethnicthv.Outer.Event;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Outer.Behaviour.Chess
{
    public class SelectingSquareBehaviour: MonoBehaviour
    {
        private UnityEngine.Camera _main;
        
        public String checkTag = "ChessSquare";
        
        private IChessBoardOuter _chessBoard;
        
        private void Start()
        {
            _main = UnityEngine.Camera.main;
            _chessBoard = GetComponent<ChestBoardBehavior>();
        }

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0)) return;
            var ray = _main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
            var hitObject = hit.collider.gameObject;
            if (!hitObject.CompareTag("ChessSquare")) return;
            var square = _chessBoard.GetSquare(hitObject);
            EventManager.Instance.DispatchEvent(EventManager.HandlerType.Local, new OnSquareSelectingEvent(square), @event => {});
        }
    }
}