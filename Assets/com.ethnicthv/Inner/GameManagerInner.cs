using System;
using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Inner.Object.Piece.Action;
using com.ethnicthv.Inner.Object.Player;
using com.ethnicthv.Inner.Object.Player.Faction;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Util;

namespace com.ethnicthv.Inner
{
    public class GameManagerInner: IGameManagerInner
    {
        private static GameManagerInner _instance;

        [Obsolete("this instance is deprecated, please use Instance instead.")]
        public static GameManagerInner instance => _instance ??= new GameManagerInner();

        public static IGameManagerInner Instance => instance;
        
        private PieceActionManager _pieceActionManager;
        
        private PlayerManager _playerManager;
        private FactionManager _factionManager;
        
        private ChessBoard _chessBoard;

        private GameManagerInner()
        {
            Init();
        }
        
        private void Init()
        {
            _pieceActionManager = new PieceActionManager();
            
            _playerManager = new PlayerManager();
            _factionManager = new FactionManager();
        }

        public ChessBoard CreateChessBoard()
        {
            _chessBoard = new ChessBoard();
            return _chessBoard;
        }

        public void StartGame()
        {
            var playerFactionID = _playerManager.Player.FactionID;
            var opponentFactionID = _playerManager.Opponent.FactionID;
            var playerFaction = _factionManager.GetFaction(playerFactionID);
            var opponentFaction = _factionManager.GetFaction(opponentFactionID);
            
            playerFaction.RegisterFaction();
            opponentFaction.RegisterFaction();

            CreateChessBoard();
            
            playerFaction.SetupPieces(true , _chessBoard);
            opponentFaction.SetupPieces(false, _chessBoard);
            
            GameManagerOuter.ChessBoard.SetupBoard(_chessBoard);
        }

        public IGameManagerOuter GameManagerOuter { get; set; }

        public ChessBoard Board => _chessBoard ?? throw new NullReferenceException("ChessBoard is not created yet.");
        public PieceActionManager PieceActionManager => _pieceActionManager;
        public PlayerManager PlayerManager => _playerManager;
        public FactionManager FactionManager => _factionManager;

        public static (int, int) ConvertOuterToInnerPos(CbPos pos)
        {
            return (pos.Y, pos.X);
        }
        
        public static (int, int) ConvertOuterToInnerPos((int,int) pos)
        {
            return (pos.Item2, pos.Item1);
        }
        
        public static (int, int) ConvertOuterToInnerPos(int x, int y)
        {
            return (y, x);
        }

        #region Test

        public void TestInput()
        {
            GameManagerOuter.ChessBoard.TestCall();
        }

        #endregion
    }
}