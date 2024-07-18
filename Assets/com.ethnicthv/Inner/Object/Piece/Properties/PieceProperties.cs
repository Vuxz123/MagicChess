using System;
using System.Collections.Generic;
using com.ethnicthv.Inner.Object.ChessBoard.Exception;
using com.ethnicthv.Inner.Object.Piece.Exception;
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
            private static readonly Dictionary<int, PieceProperties> PiecePropertiesMap = new();

            private static readonly Dictionary<int, string> DefaultTypeMap = new()
            {
                { 1, "King" },
                { 2, "Queen" },
                { 3, "Rook" },
                { 4, "Bishop" },
                { 5, "Knight" },
                { 6, "Pawn" }
            };

            private static readonly Dictionary<int, string> TypeMap = new();

            public static PieceProperties GetProperties(int type)
            {
                if (PiecePropertiesMap.TryGetValue(type, out var properties)) return properties;
                throw new PiecePropertiesNotFound(type);
            }

            public static void ImportProperties()
            {
                var jsonString = Resources.Load<TextAsset>("config/piece_config").text;
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonString);
                foreach (var (key, type) in DefaultTypeMap)
                {
                    var properties = data[type];
                    var moveStyle = MovementStyle.Resolver.GetMovementStyle(properties["move_type"]);
                    var attackType = AttackType.Resolver.GetAttackType(properties["attack_type"]);
                    PiecePropertiesMap[key] = new PieceProperties(
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

            public static void SetTypeID(int key, string type)
            {
                TypeMap[key] = type;
            }
            
            public static void ImportProperties(string part)
            {
                var jsonString = Resources.Load<TextAsset>(part).text;
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, string>>>(jsonString);
                foreach (var (key, type) in TypeMap)
                {
                    var properties = data[type];
                    var moveStyle = MovementStyle.Resolver.GetMovementStyle(properties["move_type"]);
                    var attackType = AttackType.Resolver.GetAttackType(properties["attack_type"]);
                    PiecePropertiesMap[key] = new PieceProperties(
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