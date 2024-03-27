using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace com.ethnicthv.Inner.Object.Piece
{
    public class PieceProperties
    {
        public PieceProperties(int health, int attack, int defense, int movementRange, int attackRange,
            int movementCost, MovementStyle movementStyle)
        {
            Health = health;
            Attack = attack;
            Defense = defense;
            MovementRange = movementRange;
            AttackRange = attackRange;
            MovementCost = movementCost;
            MovementStyle = movementStyle;
        }

        public int Health { get; set; }

        public int Attack { get; set; }

        public int Defense { get; set; }

        public int MovementRange { get; set; }

        public int AttackRange { get; set; }

        public int MovementCost { get; set; }

        public MovementStyle MovementStyle { get; set; }

        public static class Provider
        {
            private static readonly Dictionary<Piece.Type, PieceProperties> PiecePropertiesMap = new();

            public static PieceProperties GetProperties(Piece.Type type)
            {
                if (PiecePropertiesMap.TryGetValue(type, out var properties)) return properties;
                ImportProperties();
                return PiecePropertiesMap[type];
            }

            private static void ImportProperties()
            {
                var jsonString = Resources.Load<TextAsset>("config/piece_config").text;
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonString);
                foreach (var t in Enum.GetValues(typeof(Piece.Type)))
                {
                    var type = (Piece.Type) t;
                    var properties = data[type.ToString()];
                    var movementStyle = MovementStyle.Resolver.GetMovementStyle(properties["move_type"]);
                    PiecePropertiesMap[type] = new PieceProperties(
                        int.Parse(properties["health"]),
                        int.Parse(properties["damage"]),
                        int.Parse(properties["defence"]),
                        int.Parse(properties["move_range"]),
                        int.Parse(properties["range"]),
                        int.Parse(properties["cost_per_move"]),
                        movementStyle
                    );
                }
            }
        }
    }
}