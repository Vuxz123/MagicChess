using System;

namespace com.ethnicthv.Util.Event
{
    public delegate bool EventHandler<in T>(T e) where T : Event;

    internal class EventListener : Attribute
    {
        public Type EventType { get; private set; }
        
        public EventListener(Type eventType)
        {
            if (!eventType.IsSubclassOf(typeof(Event)))
            {
                throw new NotSupportedException("Event type must be a subclass of Event");
            }
            EventType = eventType;
        }
    }
    
    internal class LocalListenerAttribute : Attribute
    {
    }
    
    internal class ServerNetworkingListenerAttribute : Attribute
    {
    }
    
    internal class ClientNetworkingSenderAttribute : Attribute
    {
    }
}