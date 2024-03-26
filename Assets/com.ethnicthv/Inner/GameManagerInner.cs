using System;
using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Inner.Object.Piece;
using com.ethnicthv.Inner.Object.Piece.Action;
using com.ethnicthv.Outer;
using com.ethnicthv.Outer.Util;
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

        private GameManagerInner()
        {
            Init();
        }
        
        private void Init()
        {
            PieceAction.Setup();
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
    }
}