using System.Collections.Generic;
using UnityEngine;
using Debug = com.ethnicthv.Util.Debug;
using PieceInner = com.ethnicthv.Inner.Object.Piece;

namespace com.ethnicthv.Outer.Util
{
    public static class PiecePrefabProvider
    {
        private static Dictionary<PieceInner.Piece.Type, GameObject> _prefabCache = new();
        public static GameObject GetPiecePrefab(PieceInner.Piece.Type type)
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
        
        private static GameObject LoadPrefab(PieceInner.Piece.Type type)
        {
            return type switch
            {
                PieceInner.Piece.Type.Pawn => Resources.Load<GameObject>("Prefab/Chess/Pieces/Pawn"),
                PieceInner.Piece.Type.Rook => Resources.Load<GameObject>("Prefab/Chess/Pieces/Rook"),
                PieceInner.Piece.Type.Knight => Resources.Load<GameObject>("Prefab/Chess/Pieces/Knight"),
                PieceInner.Piece.Type.Bishop => Resources.Load<GameObject>("Prefab/Chess/Pieces/Bishop"),
                PieceInner.Piece.Type.Queen => Resources.Load<GameObject>("Prefab/Chess/Pieces/Queen"),
                PieceInner.Piece.Type.King => Resources.Load<GameObject>("Prefab/Chess/Pieces/King"),
                _ => null
            };
        }
    }
}