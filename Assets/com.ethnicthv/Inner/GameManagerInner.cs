using System;
using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Outer;
using UnityEngine;

namespace com.ethnicthv.Inner
{
    public class GameManagerInner: IGameManagerInner
    {
        private static GameManagerInner _instance;

        [Obsolete("this instance is deprecated, please use Instance instead.")]
        public static GameManagerInner instance => _instance ??= new GameManagerInner();

        public static IGameManagerInner Instance => instance;
        
        private ChessBoard _chessBoard;

        public GameManagerInner()
        {
            
        }

        public ChessBoard CreateChessBoard()
        {
            _chessBoard = new ChessBoard();
            return _chessBoard;
        }

        public IGameManagerOuter GameManagerOuter { get; set; }
        public void TestInput()
        {
            GameManagerOuter.ChessBoard.TestCall();
        }

        public ChessBoard Board => _chessBoard;
    }
}