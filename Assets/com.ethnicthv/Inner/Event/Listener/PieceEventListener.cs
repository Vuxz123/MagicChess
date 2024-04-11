using System;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Util.Event;
using Debug = com.ethnicthv.Util.Debug;

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
                case PieceEvent.Type.Damage:
                    Debug.Log($"PieceEventListener: -Damage- deal {e.data[0]}");
                    return true;
                case PieceEvent.Type.Heal:
                    Debug.Log($"PieceEventListener: -Heal- heal {e.data[0]}");
                    return true;
                default:
                    return true;
            }
        }

        [LocalHandler]
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
                case PieceEvent.Type.Damage:
                    HandleDamage(e);
                    return true;
                case PieceEvent.Type.Heal:
                    Debug.Log($"PieceEventListener: -Heal- heal {e.data[0]}");
                    return true;
                default:
                    return true;
            }
        }

        private static void HandleDamage(PieceEvent e)
        {
            Debug.Log($"PieceEventListener: -Damage- deal {e.data[0]}");
            var preDamage = (int)e.data[0];
            var source = (Piece)e.data[1];
            var target = e.piece;
            var damageType = (DamageType)e.data[2];
            var damage = CalculateDamage(preDamage, source, target, damageType);
            e.data[0] = damage;
        }

        private static int CalculateDamage(int preDamage, Piece source, Piece target, DamageType damageType)
        {
            var damage = preDamage;

            damage -= damageType switch
            {
                DamageType.Physical => target.PieceProperties.Defense * 
                    (100 - target.PieceProperties.AmorPenetration) / 100,
                DamageType.Magical => target.PieceProperties.MagicDefense *
                    (100 - target.PieceProperties.MagicPenetration) / 100,
                _ => throw new ArgumentOutOfRangeException(nameof(damageType), damageType, null)
            };

            return damage;
        }
    }
}