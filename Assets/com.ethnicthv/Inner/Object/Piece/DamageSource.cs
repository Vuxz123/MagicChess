namespace com.ethnicthv.Inner.Object.Piece
{
    public class DamageSource
    {
        public readonly Piece Source;
        public readonly int Damage;
        public readonly DamageType DamageType;
        
        public DamageSource(Piece source, int damage, DamageType type)
        {
            Source = source;
            Damage = damage;
            DamageType = type;
        }
    }

    public enum DamageType
    {
        Physical,
        Magical
    }
}