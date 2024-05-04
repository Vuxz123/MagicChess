using System;

namespace com.ethnicthv.Other.Event
{
    public delegate bool EventHandler<in T>(T e) where T : Other.Event.Event;

    internal class EventListener : Attribute
    {
        public Type EventType { get; private set; }
        
        public EventListener(Type eventType)
        {
            if (!eventType.IsSubclassOf(typeof(Other.Event.Event)))
            {
                throw new NotSupportedException("Event type must be a subclass of Event");
            }
            EventType = eventType;
        }
    }
    
    internal class LocalHandlerAttribute : Attribute
    {
    }
    
    internal class ServerNetworkingHandlerAttribute : Attribute
    {
    }
    
    internal class ClientNetworkingSenderAttribute : Attribute
    {
    }
}