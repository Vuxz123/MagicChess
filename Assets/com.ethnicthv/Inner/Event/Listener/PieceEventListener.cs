using System;
using com.ethnicthv.Util.Event;
using UnityEngine;

namespace com.ethnicthv.Inner.Event.Listener
{
    [EventListener(eventType: typeof(PieceEvent))]
    public class PieceEventListener
    {
        [ClientNetworkingSender]
        public bool NetworkingHandler(PieceEvent e)
        {
            Debug.Log(e.piece);
            switch (e.type)
            {
                case PieceEvent.Type.Move:
                    Debug.Log($"PieceEventListener: -Move- from {e.data[0]} to {e.data[1]}");
                    return true;
                case PieceEvent.Type.Attack:
                    Debug.Log($"PieceEventListener: -Attack- from {e.data[0]} to {e.data[1]}");
                    return true;
                case PieceEvent.Type.Defend:
                    Debug.Log($"PieceEventListener: -Defend- from {e.data[0]} to {e.data[1]}");
                    return true;
                case PieceEvent.Type.Die:
                    Debug.Log($"PieceEventListener: -Die- from {e.data[0]} to {e.data[1]}");
                    return true;
                default:
                {
                
                }
                    return true;
            }
        }
        
        [LocalListener]
        public bool HandleEventLocal(PieceEvent e)
        {
            Debug.Log(e.piece);
            switch (e.type)
            {
                case PieceEvent.Type.Move:
                    Debug.Log($"PieceEventListener: -Move- from {e.data[0]} to {e.data[1]}");
                    return true;
                case PieceEvent.Type.Attack:
                    Debug.Log($"PieceEventListener: -Attack- from {e.data[0]} to {e.data[1]}");
                    return true;
                case PieceEvent.Type.Defend:
                    Debug.Log($"PieceEventListener: -Defend- from {e.data[0]} to {e.data[1]}");
                    return true;
                case PieceEvent.Type.Die:
                    Debug.Log($"PieceEventListener: -Die- from {e.data[0]} to {e.data[1]}");
                    return true;
                default:
                    return true;
            }
        }
    }
}