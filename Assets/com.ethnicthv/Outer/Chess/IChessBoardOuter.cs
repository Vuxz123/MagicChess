using com.ethnicthv.Outer.Chess.Square;
using com.ethnicthv.Outer.Piece;
using UnityEngine;

namespace com.ethnicthv.Outer.Chess
{
    public interface IChessBoardOuter
    {
        bool DoMove(int fromX, int fromY, int toX, int toY, int moveType);
        
        /// <summary>
        /// Get square by Position
        /// </summary>
        /// <param name="x"> pos x </param>
        /// <param name="y"> pos y</param>
        /// <returns> an ISquare object </returns>
        ISquare GetSquare(int x, int y);
        
        /// <summary>
        /// Get square by Square GameObject
        /// </summary>
        /// <param name="square"></param>
        /// <returns> an ISquare object </returns>
        ISquare GetSquare(GameObject square);
        
        /// <summary>
        /// Get piece at Position
        /// </summary>
        /// <param name="x"> pos x </param>
        /// <param name="y"> pos y</param>
        /// <returns>
        /// <list type="bullet">
        ///     <item> an IPiece object if available </item>
        ///     <item> if not, return null </item>
        /// </list>
        /// </returns>
        IPiece GetPiece(int x, int y);
        
        /// <summary>
        /// Get piece from GameObject
        /// </summary>
        /// <param name="piece"></param>
        /// <returns> an IPiece if present, if not, return null</returns>
        IPiece GetPiece(GameObject piece);

        void TestCall();
    }
}