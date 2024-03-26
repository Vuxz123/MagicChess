using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Outer;

namespace com.ethnicthv.Inner
{
    public interface IGameManagerInner
    {
        public IGameManagerOuter GameManagerOuter { get; }
        void TestInput();
        
        public ChessBoard Board { get; }
        
        ChessBoard CreateChessBoard();
    }
}