using com.ethnicthv.Other.Ev;

namespace com.ethnicthv.Outer.Event.Listener
{
    [EventListener(typeof(OnRightClickEvent))]
    public class OnRightClickHandler
    {
        [LocalHandler]
        public bool HandleEventLocal(OnRightClickEvent e)
        {
            OnSquareSelectedListener.CancelSelecting();
            return true;
        }
    }
}