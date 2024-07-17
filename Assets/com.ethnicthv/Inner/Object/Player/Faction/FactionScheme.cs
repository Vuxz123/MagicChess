using System;
using System.Collections.Generic;

namespace com.ethnicthv.Inner.Object.Player.Faction
{
    public abstract class FactionScheme
    {
        internal int ID;
        
        protected readonly List<PlayerAction> actions;
        public IReadOnlyList<PlayerAction> Actions => actions;
        
        protected FactionScheme(List<PlayerAction> actions)
        {
            this.actions = actions;
        }

        #region Turn-Action
        
        private int _currentActionIndex;

        public void StartTurn()
        {
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
        }
        
        public void CancelAction()
        {
            // cancel current action
            actions[_currentActionIndex].OnActionCancel();
        }

        #endregion
    }
}