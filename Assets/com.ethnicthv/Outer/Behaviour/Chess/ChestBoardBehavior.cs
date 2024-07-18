using System.Linq;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Outer.Behaviour.Chess.Square;
using com.ethnicthv.Outer.Behaviour.Piece;
using com.ethnicthv.Outer.Util;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Outer.Behaviour.Chess
{
    public class ChestBoardBehavior: MonoBehaviour, IChessBoardOuter
    {
        private ChessInitBehaviour _chessInitBehaviour;
        private void Awake()
        {
            GameManagerOuter.instance.ChessBoard = this;
        }

        private void Start()
        {
            _chessInitBehaviour = GetComponent<ChessInitBehaviour>();
        }

        public bool DoMove(int fromX, int fromY, int toX, int toY, int moveType)
        {
            throw new System.NotImplementedException();
        }

        public ISquare GetSquare(int x, int y)
        {
            return _chessInitBehaviour.Squares[x, y].Item2;
        }
        
        public ISquare GetSquare(GameObject square)
        {
            var array = _chessInitBehaviour.Squares;
            return (from (GameObject, ISquare) s in _chessInitBehaviour.Squares where s.Item1 == square select s.Item2).FirstOrDefault();
        }

        public IPiece GetPiece(int x, int y)
        {
            var square = GameManagerInner.Instance.Board[GameManagerInner.ConvertOuterToInnerPos(x, y)];
            return square.Outer;
        }

        public IPiece GetPiece(GameObject piece)
        {
            return (from p in _chessInitBehaviour.Pieces where p.Item1 == piece select p.Item2).FirstOrDefault();
        }

        public IPiece GetPiece(CbPos pos)
        {
            return GetPiece(pos.X, pos.Y);
        }

        public void SetupBoard(ChessBoard board)
        {
            _chessInitBehaviour.InitPieces(board);
        }

        public void TestCall()
        {
            SendMessage("TestMethod");
        }
    }
}