using System;
using com.ethnicthv.Other;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Behaviour.Piece;

namespace com.ethnicthv.Inner.Event.Listener
{
    [EventListener(eventType: typeof(ChessBoardMoveEvent))]
    public class ChessBoardMoveListener
    {
        [ClientNetworkingSender]
        public bool HandleEventForSender(ChessBoardMoveEvent e)
        {
            Debug.Log("HandleEventForSender: " + e);
            var origin = e.From;
            var destination = e.To;
            var outer = GameManagerInner.Instance.Board[origin].Outer;
            var outerDest = GameManagerOuter.ConvertInnerToOuterPos(destination);
            outer.SetPosToSquare(outerDest);
            return true;
        }
        
        [ServerNetworkingHandler]
        public bool HandleEventServer(ChessBoardMoveEvent e)
        {
            Debug.Log("HandleEventServer: " + e);
            var origin = e.From;
            var destination = e.To;
            var board = GameManagerInner.Instance.Board;
            var outer = board[origin].Outer;
            var outerDest = GameManagerOuter.ConvertInnerToOuterPos(destination);
            outer.SetPosToSquare(outerDest);
            
            // update Inner board
            (board[destination.Item1, destination.Item2], board[origin.Item1, origin.Item2]) =
                (board[origin.Item1, origin.Item2], board[destination.Item1, destination.Item2]);
            return true;
        }
    }
}