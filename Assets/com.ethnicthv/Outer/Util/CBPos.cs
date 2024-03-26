namespace com.ethnicthv.Outer.Util
{
    public class CbPos
    {
        public int X = 0;
        public int Y = 0;

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }
    }
}