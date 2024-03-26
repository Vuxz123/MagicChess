using System;
using com.ethnicthv.Inner;
using com.ethnicthv.Inner.Object.ChessBoard;
using com.ethnicthv.Outer.Behaviour.Chess;

namespace com.ethnicthv.Outer
{
    public class GameManagerOuter : IGameManagerOuter
    {
        private static GameManagerOuter _instance;

        [Obsolete("this instance is deprecated, please use Instance instead.")]
        public static GameManagerOuter instance => _instance ??= new GameManagerOuter();

        public static IGameManagerOuter Instance => instance;

        public IChessBoardOuter ChessBoard { get; set; }

        public IGameManagerInner GameManagerInner { get; set; }

        private GameManagerOuter()
        {
        }
        
        public static (int, int) ConvertInnerToOuterPos(int x, int y)
        {
            return (y, x);
        }
        
        public static (int, int) ConvertInnerToOuterPos((int,int) pos)
        {
            return (pos.Item2, pos.Item1);
        }
    }
}