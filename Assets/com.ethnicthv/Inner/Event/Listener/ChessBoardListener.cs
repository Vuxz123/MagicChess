using System;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Behaviour.Piece;
using com.ethnicthv.Util.Event;
using Debug = com.ethnicthv.Util.Debug;

namespace com.ethnicthv.Inner.Event.Listener
{
    [EventListener(eventType: typeof(ChessBoardEvent))]
    public class ChessBoardListener
    {
        [LocalHandler]
        public bool HandleEventLocal(ChessBoardEvent e)
        {
            Debug.Log("HandleEventLocal: " + e);
            if(e.eventType != ChessBoardEvent.EventType.Move) return false;
            var origin = ((int, int)) e.data[1];
            var destination = ((int, int)) e.data[2];
            var outer = (IPiece) e.data[0];
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