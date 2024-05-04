using com.ethnicthv.Other.Event;

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