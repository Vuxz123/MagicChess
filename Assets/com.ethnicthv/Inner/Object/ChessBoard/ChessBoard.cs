using com.ethnicthv.Inner.Event;
using com.ethnicthv.Inner.Object.ChessBoard.Exception;
using com.ethnicthv.Other.Ev;
using com.ethnicthv.Outer;
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
                // dispatch the event for sending message to network
                EventManager.Instance.DispatchEvent(EventManager.HandlerType.Client, e,
                    ev =>
                    {
                        var destination = ev.To;
                        var origin = ev.From;
                        var board = GameManagerInner.Instance.Board;
                        
                        // update the outer board
                        var outer = GameManagerInner.Instance.Board[origin].Outer;
                        var outerDest = GameManagerOuter.ConvertInnerToOuterPos(destination);
                        outer.SetPosToSquare(outerDest);
                        
                        // replace the piece in the board
                        (board[destination.Item1, destination.Item2], board[origin.Item1, origin.Item2]) = (board[origin.Item1, origin.Item2], board[destination.Item1, destination.Item2]);
                        Debug.Log("Callback: a piece has been moved!");
                    }
                );
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                throw;
            }
        }

        public override string ToString()
        {
            // print the board
            var str = "";
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    if (_board[i, j] == null)
                    {
                        str += "null ";
                        continue;
                    }

                    str += _board[i, j].ToName() + " ";
                }

                str += "\n";
            }

            return $"ChessBoard: \n{str}";
        }
    }
}