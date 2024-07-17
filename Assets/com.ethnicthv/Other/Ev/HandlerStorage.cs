using System;
using System.Collections.Generic;
using System.Reflection;
using com.ethnicthv.Other.Ev.Exception;

namespace com.ethnicthv.Other.Ev
{
    public class HandlerStorage
    {
        private readonly Dictionary<Type, LinkedList<Delegate>> _handlers;
        
        public int Count(Type eventType)
        {
            return _handlers.TryGetValue(eventType, out var handler) ? handler.Count : 0;
        }

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
            var type = e.GetType();
            Debug.Log("Dispatching event: " + type);
            if (!_handlers.TryGetValue(type, out var handlerList)) return;
            Debug.Log("Handlers found: " + handlerList.Count);
            var isCancelled = false;
            foreach (var handler in handlerList)
            {
                try
                {
                    isCancelled = !(bool) handler.DynamicInvoke(e);
                    if (isCancelled) break;
                }
                catch (TargetInvocationException ex)
                {
                    // Handle the exception
                    Debug.LogError(ex.InnerException);
                }
            }
            if(!isCancelled) callback?.Invoke(e);
        }
    }
}