using System.Collections.Generic;
using UnityEngine;
using Debug = com.ethnicthv.Other.Debug;
using PieceInner = com.ethnicthv.Inner.Object.Piece;

namespace com.ethnicthv.Outer.Util
{
    //TODO: Add a way to load prefabs from a folder and config file
    public static class PiecePrefabProvider
    {
        private static Dictionary<int, GameObject> _prefabCache = new();
        public static GameObject GetPiecePrefab(int type)
        {
            if (_prefabCache.TryGetValue(type, out var prefab)) return prefab;
            prefab = LoadPrefab(type);
            if (prefab != null)
            {
                _prefabCache[type] = prefab;
            }
            else
            {
                Debug.LogError($"Failed to load prefab for piece type {type}");
            }
            return prefab;
        }
        
        private static GameObject LoadPrefab(int type)
        {
            return type switch
            {
                6 => Resources.Load<GameObject>("Prefab/Chess/Pieces/Pawn"),
                5 => Resources.Load<GameObject>("Prefab/Chess/Pieces/Rook"),
                4 => Resources.Load<GameObject>("Prefab/Chess/Pieces/Knight"),
                3 => Resources.Load<GameObject>("Prefab/Chess/Pieces/Bishop"),
                2 => Resources.Load<GameObject>("Prefab/Chess/Pieces/Queen"),
                1 => Resources.Load<GameObject>("Prefab/Chess/Pieces/King"),
                _ => null
            };
        }
    }
}