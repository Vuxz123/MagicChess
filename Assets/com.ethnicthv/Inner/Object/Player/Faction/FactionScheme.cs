using System;
using System.Collections.Generic;

namespace com.ethnicthv.Inner.Object.Player.Faction
{
    public abstract class FactionScheme
    {
        internal int ID;
        internal string FactionNameSpace;
        
        protected readonly List<PlayerAction> actions;
        public IReadOnlyList<PlayerAction> Actions => actions;
        
        protected FactionScheme(string factionNameSpace, List<PlayerAction> actions)
        {
            FactionNameSpace = factionNameSpace;
            this.actions = actions;
        }

        #region Setup

        public void RegisterFaction()
        {
            RegisterActions();
            RegisterPrefabs();
            RegisterPiecesProperties();
        }

        /// <summary>
        /// Called when the faction is registered as Player's faction. <br/>
        /// This method should register all actions that the faction's pieces can perform.
        /// </summary>
        public abstract void RegisterActions();
        
        /// <summary>
        /// Called when the faction is registered as Player's faction. <br/>
        /// This method should register all prefabs that the faction's pieces use.
        /// </summary>
        public abstract void RegisterPrefabs();
        
        /// <summary>
        /// Called when the faction is registered as Player's faction. <br/>
        /// This method should register all properties that the faction's pieces have.
        /// </summary>
        public abstract void RegisterPiecesProperties();
        
        /// <summary>
        /// Called when the chess board is created to setup the faction's pieces. <br/>
        /// This method should place all the faction's pieces on the board.
        /// </summary>
        /// <param name="isPlayer1">Is Player 1 or not</param>
        /// <param name="board">The chess board to place the pieces on.</param>
        public abstract void SetupPieces(bool isPlayer1, ChessBoard.ChessBoard board);

        #endregion

        #region Turn-Action
        
        private int _currentActionIndex;

        public void StartTurn()
        {
            OnStartTurn();
            // prepare first action
            _currentActionIndex = 0;
            actions[_currentActionIndex].OnActionPrepare();
        }
        
        public void StartAction()
        {
            // start current action
            actions[_currentActionIndex].OnActionStart();
        }
        
        public void SubmitAction()
        {
            // submit current action
            var playerAction = actions[_currentActionIndex];
            playerAction.OnActionSubmit();
            playerAction.OnActionCleanUp();
            
            // prepare next action
            _currentActionIndex++;
            if (_currentActionIndex < actions.Count)
            {
                actions[_currentActionIndex].OnActionPrepare();
            }
            else
            {
                OnEndTurn();
            }
        }
        
        public void CancelAction()
        {
            // cancel current action
            actions[_currentActionIndex].OnActionCancel();
        }

        public abstract void OnEndTurn();

        public abstract void OnEndGame();

        public abstract void OnStartTurn();

        public abstract void OnStartGame();

        #endregion
    }
}