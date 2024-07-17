using com.ethnicthv.Other;
using com.ethnicthv.Other.Network;
using com.ethnicthv.Other.Network.P;

namespace com.ethnicthv.Inner.Event
{
    [Network(eventNetworkName: "piece-attack-op")]
    public class PieceAttackOnPieceEvent: NetworkEvent
    {
        public readonly int Damage;
        public readonly (int, int) AttackerPos;
        public readonly (int, int) AttackedPos;
        
        public PieceAttackOnPieceEvent() { }
        
        public PieceAttackOnPieceEvent(int damage)
        {
        }

        public PieceAttackOnPieceEvent(int damage, (int, int) attackerPos, (int, int) attackedPos)
        {
            Damage = damage;
            AttackerPos = attackerPos;
            AttackedPos = attackedPos;
        }

        public override void ToPacket(PacketWriter writer)
        {
            writer.Write((byte)Damage);
            writer.Write((byte)AttackerPos.Item1);
            writer.Write((byte)AttackerPos.Item2);
            writer.Write((byte)AttackedPos.Item1);
            writer.Write((byte)AttackedPos.Item2);
        }

        public override Other.Ev.Event FromPacket(PacketReader packet)
        {
            var damage = packet.ReadByte();
            var attackerPos = (packet.ReadByte(), packet.ReadByte());
            var attackedPos = (packet.ReadByte(), packet.ReadByte());
            return new PieceAttackOnPieceEvent(damage, attackerPos, attackedPos);
        }
    }
}