using System.Collections.Generic;
using com.ethnicthv.Util;

namespace com.ethnicthv.Inner.Object.Piece
{
    
    public delegate List<(int, int)> CalculateAvailableMoves(Piece.Side side ,int x, int y, int movementRange);
    
    public class MovementStyle
    {
        public static readonly MovementStyle Straight = new("Straight", (side ,x, y, range) =>
        {
            var list = new List<(int, int)>();
            for(var i = 1; i <= range; i++)
            {
                // Add available moves
                var p = (x + i * GetDirection(side), y);
                //check if position is valid
                if (p.Item1 < 0 || p.Item1 >= 8 || p.Item2 < 0 || p.Item2 >= 8) continue;
                //check if position has piece
                //if it has piece, stop adding moves in this direction
                if (GameManagerInner.Instance.Board[p.Item1, p.Item2] != null)
                    break;
                list.Add(p);
            }
            return list;
        });
        
        public static readonly MovementStyle Normal = new("Normal", (side ,x, y, range) =>
        {
            var list = new List<(int, int)>();
            
            var directions = new List<(int, int)>
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1)
            };
            
            for (var di = 0; di < 4; di++)
            {
                var (dx, dy) = directions[di];
                for(var i = 1; i <= range; i++)
                {
                    // Add available moves
                    var p = (x + i * dx, y + i * dy);
                    //check if position is valid
                    if (p.Item1 < 0 || p.Item1 >= 8 || p.Item2 < 0 || p.Item2 >= 8) continue;
                    //check if position has piece
                    //if it has piece, stop adding moves in this direction
                    if (GameManagerInner.Instance.Board[p.Item1, p.Item2] != null)
                        break;
                    list.Add(p);
                }
            }
            
            return list;
        });
        
        public static readonly MovementStyle Diagonal = new("Diagonal", (side ,x, y, range) =>
        {
            var list = new List<(int, int)>();
            var directions = new List<(int, int)>
            {
                (1, 1),
                (-1, 1),
                (1, -1),
                (-1, -1)
            };
            for (var di = 0; di < 4; di++)
            {
                var (dx, dy) = directions[di];
                for(var i = 1; i <= range; i++)
                {
                    // Add available moves
                    var p = (x + i * dx, y + i * dy);
                    //check if position is valid
                    if (p.Item1 < 0 || p.Item1 >= 8 || p.Item2 < 0 || p.Item2 >= 8) continue;
                    //check if position has piece
                    //if it has piece, stop adding moves in this direction
                    if (GameManagerInner.Instance.Board[p.Item1, p.Item2] != null)
                        break;
                    list.Add(p);
                }
            }
            return list;
        });
        public static readonly MovementStyle L = new("L", (side ,x, y, range) =>
        {
            var list = new List<(int, int)>();
            
            var directions = new List<(int, int)>
            {
                (1, 2),
                (-1, 2),
                (1, -2),
                (-1, -2),
                (2, 1),
                (-2, 1),
                (2, -1),
                (-2, -1)
            };
            
            for (var di = 0; di < 8; di++)
            {
                var (dx, dy) = directions[di];
                // Add available moves
                var p = (x + dx, y + dy);
                //check if position is valid
                if (p.Item1 < 0 || p.Item1 >= 8 || p.Item2 < 0 || p.Item2 >= 8) continue;
                //check if position has piece
                //if it has piece, stop adding moves in this direction
                if (GameManagerInner.Instance.Board[p.Item1, p.Item2] != null)
                    continue;
                list.Add(p);
            }

            return list;
        });

        public static readonly MovementStyle Both = new("Both", (side, x, y, range) =>
        {
            var list = new List<(int, int)>();
            
            var directions = new List<(int, int)>
            {
                (1, 0),
                (-1, 0),
                (0, 1),
                (0, -1),
                (1, 1),
                (-1, 1),
                (1, -1),
                (-1, -1)
            };
            
            for (var di = 0; di < 8; di++)
            {
                var (dx, dy) = directions[di];
                for(var i = 1; i <= range; i++)
                {
                    // Add available moves
                    var p = (x + i * dx, y + i * dy);
                    //check if position is valid
                    if (p.Item1 < 0 || p.Item1 >= 8 || p.Item2 < 0 || p.Item2 >= 8) continue;
                    //check if position has piece
                    //if it has piece, stop adding moves in this direction
                    if (GameManagerInner.Instance.Board[p.Item1, p.Item2] != null)
                        break;
                    list.Add(p);
                }
            }
            
            return list;
        });

        private readonly CalculateAvailableMoves _calculateAvailableMoves;
        private readonly string _name;

        public MovementStyle(string name, CalculateAvailableMoves calculateAvailableMoves)
        {
            _name = name;
            _calculateAvailableMoves = calculateAvailableMoves;
        }
        
        public List<(int,int)> GetAvailableMoves(Piece.Side side, int x, int y, int movementRange)
        {
            return _calculateAvailableMoves(side, x, y, movementRange);
        }
        
        private static int GetDirection(Piece.Side side)
        {
            return side == Piece.Side.White ? 1 : -1;
        }

        public override string ToString()
        {
            return $"{nameof(_name)}: {_name}";
        }

        public static class Resolver
        {
            public static MovementStyle GetMovementStyle(string style)
            {
                return style switch
                {
                    "Straight" => Straight,
                    "Normal" => Normal, // "Normal" is the default movement style
                    "Diagonal" => Diagonal,
                    "L" => L,
                    "Both" => Both,
                    _ => null
                };
            }
        }
    }
}