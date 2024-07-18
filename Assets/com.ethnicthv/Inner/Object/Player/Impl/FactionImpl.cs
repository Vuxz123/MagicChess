using System.Collections.Generic;
using com.ethnicthv.Inner.Object.Player.Faction;
using com.ethnicthv.Other;

namespace com.ethnicthv.Inner.Object.Player.Impl
{
    public class FactionImpl: FactionScheme
    {
        public FactionImpl(): base("test_impl", new List<PlayerAction>())
        {
            // step 1:
            actions.Add(new PlayerActionImpl());
            // step 2:
            actions.Add(new PlayerActionImpl());
            // step 3:
            actions.Add(new PlayerActionImpl());
        }

        public override void RegisterActions()
        {
            Debug.Log("Registering actions.");
        }

        public override void RegisterPrefabs()
        {
            Debug.Log("Registering prefabs.");
        }

        public override void RegisterPiecesProperties()
        {
            Debug.Log("Registering pieces properties.");
        }

        public override void SetupPieces(bool isPlayer1, ChessBoard.ChessBoard board)
        {
            Debug.Log("Setting up pieces.");
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