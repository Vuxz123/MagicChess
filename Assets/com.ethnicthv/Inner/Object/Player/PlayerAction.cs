namespace com.ethnicthv.Inner.Object.Player
{
    public abstract class PlayerAction
    {
        /// <summary>
        /// Event called when the action is prepared to be executed.
        /// </summary>
        public abstract void OnActionPrepare();
        
        /// <summary>
        /// Event called when the action is started.
        /// </summary>
        public abstract void OnActionStart();
        
        /// <summary>
        /// Event called when the action is submitted.
        /// </summary>
        public abstract void OnActionSubmit();
        
        /// <summary>
        /// Event called when the action is cleaned up.
        /// </summary>
        public abstract void OnActionCleanUp();
        
        /// <summary>
        /// Event called when the action is canceled.
        /// </summary>
        public abstract void OnActionCancel();
    }
}