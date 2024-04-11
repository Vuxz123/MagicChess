using System.Collections.Generic;

namespace com.ethnicthv.Inner.Object.Piece
{
    public delegate List<(int, int)> CalculateAvailableAttacks(Piece.Side side, int x, int y, int attackRange);

    public class AttackType
    {
        /// <summary>
        /// Spear attack type will attack the 2 enemies in front of the piece
        /// </summary>
        public static readonly AttackType Spear = new("Spear", (side, x, y, range) =>
        {
            var list = new List<(int, int)>();

            var dir = GetDirection(side);

            for (int i = 1; i <= range; i++)
            {
                var p = (x + i * dir, y);
                if (AddPosition(p, side, list)) continue;
                break;
            }

            return list;
        });

        public static readonly AttackType Sword = new("Sword", (side, x, y, range) =>
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
                for (var i = 1; i <= range; i++)
                {
                    // Add available moves
                    var p = (x + i * dx, y + i * dy);
                    //check if position has piece
                    if (AddPosition(p, side, list)) continue;
                    break;
                }
            }

            return list;
        });

        private static bool AddPosition((int, int) p, Piece.Side side, List<(int, int)> list)
        {
            //check if position is valid
            if (p.Item1 < 0 || p.Item1 >= 8 || p.Item2 < 0 || p.Item2 >= 8) return true;
            //if it has piece, stop adding moves in this direction
            if (GameManagerInner.Instance.Board[p.Item1, p.Item2] == null) return true;
            var ip = GameManagerInner.Instance.Board[p.Item1, p.Item2];
            if (ip.side == side) return false;
            list.Add(p);
            return false;
        }

        private readonly CalculateAvailableAttacks _calculateAvailableAttacks;
        private string _name;

        public AttackType(string name, CalculateAvailableAttacks calculateAvailableAttacks)
        {
            _name = name;
            _calculateAvailableAttacks = calculateAvailableAttacks;
        }

        private static int GetDirection(Piece.Side side)
        {
            return side == Piece.Side.White ? -1 : 1;
        }

        public List<(int, int)> GetAvailableAttacks(Piece.Side side, int x, int y, int attackRange)
        {
            return _calculateAvailableAttacks(side, x, y, attackRange);
        }

        public override string ToString()
        {
            return $"Name: {_name}";
        }

        public static class Resolver
        {
            public static AttackType GetAttackType(string name)
            {
                return name switch
                {
                    "Spear" => Spear,
                    "Sword" => Sword,
                    _ => null
                };
            }
        }
    }
}