using com.ethnicthv.Inner.Event;
using com.ethnicthv.Inner.Object.ChessBoard.Exception;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Outer.Behaviour.Chess;
using com.ethnicthv.Outer.Behaviour.Piece;
using Debug = com.ethnicthv.Other.Debug;

namespace com.ethnicthv.Inner.Object.ChessBoard
{
    public class ChessBoard
    {
        private readonly Piece.Piece[,] _board;

        private IChessBoardOuter _outer;

        public ChessBoard()
        {
            _board = new Piece.Piece[8, 8];
            _board[0, 0] = new Piece.Piece(Piece.Piece.Type.Rook, Piece.Piece.Side.White);
            _board[0, 1] = new Piece.Piece(Piece.Piece.Type.Knight, Piece.Piece.Side.White);
            _board[0, 2] = new Piece.Piece(Piece.Piece.Type.Bishop, Piece.Piece.Side.White);
            _board[0, 4] = new Piece.Piece(Piece.Piece.Type.Queen, Piece.Piece.Side.White);
            _board[0, 3] = new Piece.Piece(Piece.Piece.Type.King, Piece.Piece.Side.White);
            _board[0, 5] = new Piece.Piece(Piece.Piece.Type.Bishop, Piece.Piece.Side.White);
            _board[0, 6] = new Piece.Piece(Piece.Piece.Type.Knight, Piece.Piece.Side.White);
            _board[0, 7] = new Piece.Piece(Piece.Piece.Type.Rook, Piece.Piece.Side.White);

            for (var i = 0; i < 8; i++)
            {
                _board[1, i] = new Piece.Piece(Piece.Piece.Type.Pawn, Piece.Piece.Side.White);
                _board[6, i] = new Piece.Piece(Piece.Piece.Type.Pawn, Piece.Piece.Side.Black);
            }

            _board[7, 0] = new Piece.Piece(Piece.Piece.Type.Rook, Piece.Piece.Side.Black);
            _board[7, 1] = new Piece.Piece(Piece.Piece.Type.Knight, Piece.Piece.Side.Black);
            _board[7, 2] = new Piece.Piece(Piece.Piece.Type.Bishop, Piece.Piece.Side.Black);
            _board[7, 3] = new Piece.Piece(Piece.Piece.Type.Queen, Piece.Piece.Side.Black);
            _board[7, 4] = new Piece.Piece(Piece.Piece.Type.King, Piece.Piece.Side.Black);
            _board[7, 5] = new Piece.Piece(Piece.Piece.Type.Bishop, Piece.Piece.Side.Black);
            _board[7, 6] = new Piece.Piece(Piece.Piece.Type.Knight, Piece.Piece.Side.Black);
            _board[7, 7] = new Piece.Piece(Piece.Piece.Type.Rook, Piece.Piece.Side.Black);
        }

        public IChessBoardOuter Outer
        {
            get => _outer;
            protected internal set => _outer = value;
        }

        public Piece.Piece this[int x, int y]
        {
            get => _board[x, y];
            set => _board[x, y] = value;
        }

        public Piece.Piece this[(int, int) pos]
        {
            get => _board[pos.Item1, pos.Item2];
            set => _board[pos.Item1, pos.Item2] = value;
        }

        public (int, int) GetPiecePosition(Piece.Piece piece)
        {
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (_board[i, j] == piece)
                    {
                        return (i, j);
                    }
                }
            }

            throw new PieceNotFoundException($"Piece {piece} not found in the board");
        }

        public void MovePiece(IPiece controller, int fromX, int fromY, int toX, int toY)
        {
            Debug.Log("ChessBoard: MovePiece!!");
            try
            {
                Debug.Log("ChessBoard: Call Client Event");
                var e = new ChessBoardMoveEvent(
                    (fromX, fromY),
                    (toX, toY)
                );
                EventManager.Instance.DispatchEvent(EventManager.HandlerType.Client, e,
                    e => { });
                Debug.Log("ChessBoard: Call Local Event");
                EventManager.Instance.DispatchEvent(EventManager.HandlerType.Local, e,
                    e =>
                    {
                        var dest = e.To;
                        var origin = e.From;
                        //replace the piece in the board

                        (_board[dest.Item1, dest.Item2], _board[origin.Item1, origin.Item2]) =
                            (_board[origin.Item1, origin.Item2], _board[dest.Item1, dest.Item2]);
                        Debug.Log("Callback: a piece has been moved!");
                    });
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        public override string ToString()
        {
            return $"{nameof(_board)}: {_board}";
        }
    }
}