using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Inner.Object.Piece.Action;
using com.ethnicthv.Inner.Object.Player;
using com.ethnicthv.Inner.Object.Player.Faction;
using com.ethnicthv.Outer;

namespace com.ethnicthv.Inner
{
    public interface IGameManagerInner
    {
        public IGameManagerOuter GameManagerOuter { get; }
        void TestInput();
        
        public ChessBoard Board { get; }
        public PieceActionManager PieceActionManager { get; }
        public PlayerManager PlayerManager { get; }
        public FactionManager FactionManager { get; }
        
        ChessBoard CreateChessBoard();
        void StartGame();
    }
}