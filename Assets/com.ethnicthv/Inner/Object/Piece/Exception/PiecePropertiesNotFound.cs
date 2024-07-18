namespace com.ethnicthv.Inner.Object.Piece.Exception
{
    public class PiecePropertiesNotFound: System.Exception
    {
        public PiecePropertiesNotFound(int type) : base($"Piece properties not found for piece type {type}")
        {
        }
    }
}