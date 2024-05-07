using System;
using System.Collections.Generic;
using System.Reflection;
using com.ethnicthv.Other.Ev.Exception;

namespace com.ethnicthv.Other.Ev
{
    public class HandlerStorage
    {
        private readonly Dictionary<Type, LinkedList<Delegate>> _handlers;

        protected internal HandlerStorage()
        {
            _handlers = new Dictionary<Type, LinkedList<Delegate>>();
        }
            
        public void RegisterHandler(Type eventType, Delegate handler)
        {
            if (!_handlers.ContainsKey(eventType))
            {
                _handlers[eventType] = new LinkedList<Delegate>();
            }
            _handlers[eventType].AddLast(handler);
        }
            
        public void UnregisterHandler<T>(EventHandler<T> handler) where T : Event
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
            {
                _handlers[type].Remove(handler as EventHandler<Event>);
            }
        }
        
        public void DispatchEvent<T>(T e, CallbackFunction<T> callback = null) where T : Event
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type)) return;
            foreach (var handler in _handlers[type])
            {
                try
                {
                    if (!(bool)handler.DynamicInvoke(e)) break;
                }
                catch (TargetInvocationException ex)
                {
                    // Handle the exception
                    throw new HandlerException("Error in handler", ex);
                }
            }
            callback?.Invoke(e);
        }
    }
}