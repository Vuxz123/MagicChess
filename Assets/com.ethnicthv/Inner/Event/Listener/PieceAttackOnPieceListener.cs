using com.ethnicthv.Other.Ev;
using UnityEngine;

namespace com.ethnicthv.Inner.Event.Listener
{
    [EventListener(eventType: typeof(PieceAttackOnPieceEvent))]
    public class PieceAttackOnPieceListener
    {
        [LocalHandler]
        public bool HandleEventLocal(PieceAttackOnPieceEvent e)
        {
            Debug.Log("HandleEventLocal: " + e);
            var attackerPos = e.AttackerPos;
            var attackedPos = e.AttackedPos;
            var attacker = GameManagerInner.Instance.Board[attackerPos];
            var attacked = GameManagerInner.Instance.Board[attackedPos];

            if (attacked.IsAttackable())
            {
                
            }
            return true;
        }
        
        [ServerNetworkingHandler]
        public bool HandleEventServer(PieceAttackOnPieceEvent e)
        {
            Debug.Log("HandleEventServer: " + e);
            var attackerPos = e.AttackerPos;
            var attackedPos = e.AttackedPos;
            var attacker = GameManagerInner.Instance.Board[attackerPos];
            var attacked = GameManagerInner.Instance.Board[attackedPos];

            if (attacked.IsAttackable())
            {
                
            }
            return true;
        }
        
        [ClientNetworkingSender]
        public bool SendEventToServer(PieceAttackOnPieceEvent e)
        {
            Debug.Log("HandleEventForSender: " + e);
            
            // Calculate the damage to send to the Server;
            
            return true;
        }
    }
}