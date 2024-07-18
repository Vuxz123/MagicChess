using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Piece.Action.DefaultImpl;
using com.ethnicthv.Inner.Object.Piece.Exception;
using com.ethnicthv.Inner.Object.Piece.Properties;
using com.ethnicthv.Other;

namespace com.ethnicthv.Inner.Object.Player.Faction.DefaultImpl
{
    public class NormalChess : FactionScheme
    {
        const int King = 1;
        const int Queen = 2;
        const int Bishop = 3;
        const int Knight = 4;
        const int Rook = 5;
        const int Pawn = 6;

        const int KingPrefab = 1;
        const int QueenPrefab = 2;
        const int BishopPrefab = 3;
        const int KnightPrefab = 4;
        const int RookPrefab = 5;
        const int PawnPrefab = 6;

        const int KingProperties = 1;
        const int QueenProperties = 2;
        const int BishopProperties = 4;
        const int KnightProperties = 5;
        const int RookProperties = 3;
        const int PawnProperties = 6;

        public NormalChess() : base("normal_chess", new List<PlayerAction>())
        {
        }

        public override void RegisterActions()
        {
            var pieceActionManager = GameManagerInner.Instance.PieceActionManager;
            pieceActionManager.RegisterAction(King, new KingAction());
            pieceActionManager.RegisterAction(Queen, new QueenAction());
            pieceActionManager.RegisterAction(Bishop, new BishopAction());
            pieceActionManager.RegisterAction(Knight, new KnightAction());
            pieceActionManager.RegisterAction(Rook, new RookAction());
            pieceActionManager.RegisterAction(Pawn, new PawnAction());
        }

        public override void RegisterPrefabs()
        {
        }

        public override void RegisterPiecesProperties()
        {
            // Note: This is a temporary solution. In the future, this should be done in a more elegant way.
            PieceProperties.Provider.ImportProperties();
        }

        public override void SetupPieces(bool isPlayer1, ChessBoard.ChessBoard board)
        {
            var side = isPlayer1 ? Piece.Piece.Side.White : Piece.Piece.Side.Black;
            var temp1 = isPlayer1 ? 0 : 7;
            var kingPos = isPlayer1 ? 3 : 4;
            var queenPos = isPlayer1 ? 4 : 3;
            board[temp1, 0] = new Piece.Piece(Rook, RookPrefab, RookProperties, side: side);
            board[temp1, 1] = new Piece.Piece(Knight, KnightPrefab, KnightProperties, side: side);
            board[temp1, 2] = new Piece.Piece(Bishop, BishopPrefab, BishopProperties, side: side);
            board[temp1, queenPos] = new Piece.Piece(Queen, QueenPrefab, QueenProperties, side: side);
            board[temp1, kingPos] = new Piece.Piece(King, KingPrefab, KingProperties, side: side);
            board[temp1, 5] = new Piece.Piece(Bishop, BishopPrefab, BishopProperties, side: side);
            board[temp1, 6] = new Piece.Piece(Knight, KnightPrefab, KnightProperties, side: side);
            board[temp1, 7] = new Piece.Piece(Rook, RookPrefab, RookProperties, side: side);
            var temp2 = isPlayer1 ? 1 : 6;
            for (var i = 0; i < 8; i++)
            {
                board[temp2, i] = new Piece.Piece(Pawn, PawnPrefab, PawnProperties, side: side);
            }
        }

        public override void OnEndTurn()
        {
            Debug.Log("End Turn");
        }

        public override void OnEndGame()
        {
            Debug.Log("End Game");
        }

        public override void OnStartTurn()
        {
            Debug.Log("Start Turn");
        }

        public override void OnStartGame()
        {
            Debug.Log("Start Game");
        }
    }
}