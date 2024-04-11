using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace com.ethnicthv.Inner.Object.Piece.Properties
{
    public class PieceProperties
    {
        public PieceProperties(
            int health, 
            int attack, 
            int defense, 
            int movementRange, 
            int attackRange,
            int movementCost, 
            int magicAttack, 
            int magicDefense, 
            int magicPenetration, 
            int amorPenetration,
            MovementStyle movementStyle, 
            AttackType attackType)
        {
            Health = new PieceHpProperty(health);
            Attack = attack;
            Defense = defense;
            MovementRange = movementRange;
            AttackRange = attackRange;
            MovementCost = movementCost;
            MovementStyle = movementStyle;
            AttackType = attackType;
            MagicAttack = magicAttack;
            MagicDefense = magicDefense;
            MagicPenetration = magicPenetration;
            AmorPenetration = amorPenetration;
        }

        public PieceHpProperty Health { get; set; }

        public int Attack { get; set; }

        public int MagicAttack { get; set; }

        public int Defense { get; set; }

        public int MagicDefense { get; set; }

        public int AmorPenetration { get; set; }

        public int MagicPenetration { get; set; }

        public int MovementRange { get; set; }

        public int AttackRange { get; set; }

        public int MovementCost { get; set; }

        public MovementStyle MovementStyle { get; set; }
        public AttackType AttackType { get; set; }

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
                    var type = (Piece.Type)t;
                    var properties = data[type.ToString()];
                    var moveStyle = MovementStyle.Resolver.GetMovementStyle(properties["move_type"]);
                    var attackType = AttackType.Resolver.GetAttackType(properties["attack_type"]);
                    PiecePropertiesMap[type] = new PieceProperties(
                        health: int.Parse(properties["health"]),
                        attack: int.Parse(properties["damage"]),
                        defense: int.Parse(properties["defence"]),
                        movementRange: int.Parse(properties["move_range"]),
                        attackRange: int.Parse(properties["range"]),
                        movementCost: int.Parse(properties["cost_per_move"]),
                        magicAttack: int.Parse(properties["magic_damage"]),
                        magicDefense: int.Parse(properties["magic_defence"]),
                        amorPenetration: int.Parse(properties["armor_penetration"]),
                        magicPenetration: int.Parse(properties["magic_penetration"]),
                        movementStyle: moveStyle,
                        attackType: attackType
                    );
                }
            }
        }
    }
}