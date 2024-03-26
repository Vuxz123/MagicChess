using com.ethnicthv.Inner;
using com.ethnicthv.Outer.Chess;

namespace com.ethnicthv.Outer
{
    public interface IGameManagerOuter
    {
        IChessBoardOuter ChessBoard { get; }
        IGameManagerInner GameManagerInner { get; }
        
    }
}