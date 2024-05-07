using System;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Behaviour.Piece;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Inner.Event.Listener
{
    [EventListener(eventType: typeof(ChessBoardEvent))]
    public class ChessBoardListener
    {
        [LocalHandler]
        public bool HandleEventLocal(ChessBoardEvent e)
        {
            Debug.Log("HandleEventLocal: " + e);
            if(e.Type != ChessBoardEvent.EventType.Move) return false;
            var origin = ((int, int)) e.Data[1];
            var destination = ((int, int)) e.Data[2];
            var outer = (IPiece) e.Data[0];
            var outerDest = GameManagerOuter.ConvertInnerToOuterPos(destination);
            outer.SetPosToSquare(outerDest);
            return true;
        }
        
        [ServerNetworkingHandler]
        public bool HandleEventServer(ChessBoardEvent e)
        {
            Console.WriteLine("ChessBoardListener: " + e);
            return true;
        }
    }
}