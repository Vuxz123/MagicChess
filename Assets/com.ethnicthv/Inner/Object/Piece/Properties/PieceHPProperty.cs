using com.ethnicthv.Inner.Event;
using com.ethnicthv.Outer.Behaviour.Piece;
using com.ethnicthv.Util.Event;

namespace com.ethnicthv.Inner.Object.Piece
{
    public class PieceHpProperty
    {
        public int MaxHp { get; private set; }
        public int CurrentHp { get; private set; }

        public PieceHpProperty(int maxHp)
        {
            MaxHp = maxHp;
            CurrentHp = maxHp;
        }

        public void Damage(Piece piece, DamageSource damageSource)
        {
            EventManager.Instance.DispatchEvent(EventManager.HandlerType.Local,
                new PieceEvent(PieceEvent.Type.Die, piece, damageSource.Damage, damageSource.Source, damageSource.DamageType), (e) =>
                {
                    var d = (int)e.data[0];
                    CurrentHp -= d;
                    if (CurrentHp >= 0)
                        CurrentHp = 0;
                });
            EventManager.Instance.DispatchEvent(EventManager.HandlerType.Client,
                new PieceEvent(PieceEvent.Type.Die, piece, damageSource.Damage, damageSource.Source, damageSource.DamageType), (e) => { });
        }

        public void Heal(int heal)
        {
            CurrentHp += heal;
            if (CurrentHp > MaxHp)
            {
                CurrentHp = MaxHp;
            }
        }

        public void SetMaxHp(int maxHp)
        {
            MaxHp = maxHp;
        }

        public void SetCurrentHp(int currentHp)
        {
            if (currentHp > MaxHp) return;

            CurrentHp = currentHp;
        }
    }
}