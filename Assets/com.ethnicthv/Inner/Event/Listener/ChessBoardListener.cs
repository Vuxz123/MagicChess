using System;
using com.ethnicthv.Util.Event;

namespace com.ethnicthv.Inner.Event.Listener
{
    [EventListener(eventType: typeof(ChessBoardEvent))]
    public class ChessBoardListener
    {
        [LocalListener]
        public bool HandleEventLocal(ChessBoardEvent e)
        {
            Console.WriteLine("ChessBoardListener: " + e);
            return true;
        }
        
        [ServerNetworkingListener]
        public bool HandleEventServer(ChessBoardEvent e)
        {
            Console.WriteLine("ChessBoardListener: " + e);
            return true;
        }
    }
}