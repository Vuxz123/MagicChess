using com.ethnicthv.Other;

namespace com.ethnicthv.Inner.Object.Player.Impl
{
    public class PlayerActionImpl: PlayerAction
    {
        public override void OnActionPrepare()
        {
            Debug.Log("OnActionPrepare called.");
        }

        public override void OnActionStart()
        {
            Debug.Log("OnActionStart called.");
        }

        public override void OnActionSubmit()
        {
            Debug.Log("OnActionSubmit called.");
        }

        public override void OnActionCleanUp()
        {
            Debug.Log("OnActionCleanUp called.");
        }

        public override void OnActionCancel()
        {
            Debug.Log("OnActionCancel called.");
        }
    }
}