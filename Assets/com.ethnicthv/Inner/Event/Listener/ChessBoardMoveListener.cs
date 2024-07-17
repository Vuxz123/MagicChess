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
            return true;
        }
        
        [ServerNetworkingHandler]
        public bool HandleEventServer(ChessBoardMoveEvent e)
        {
            Debug.Log("HandleEventServer: " + e);
            var destination = e.To;
            var origin = e.From;
            var board = GameManagerInner.Instance.Board;
                        
            // update the outer board
            var outer = GameManagerInner.Instance.Board[origin].Outer;
            var outerDest = GameManagerOuter.ConvertInnerToOuterPos(destination);
            outer.SetPosToSquare(outerDest);
                        
            // replace the piece in the board
            var temp = board[destination.Item1, destination.Item2];
            board[destination.Item1, destination.Item2] = board[origin.Item1, origin.Item2];
            board[origin.Item1, origin.Item2] = temp;
            Debug.Log("HandleEventServer: a piece has been moved!");
            return true;
        }
    }
}