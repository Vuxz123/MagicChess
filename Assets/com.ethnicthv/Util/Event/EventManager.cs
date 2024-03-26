using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace com.ethnicthv.Util.Event
{
    public delegate void CallbackFunction<in T>(T e) where T : Event;

    public class EventManager
    {
        private static EventManager _instance;

        private bool _isInitialized;

        private List<object> _listener = new();

        public static EventManager Instance => _instance ??= new EventManager();

        public readonly HandlerStorage Local; //For Inner Event
        public readonly HandlerStorage Client; //For Client communication
        public readonly HandlerStorage Server; //For Server communication

        private EventManager()
        {
            Client = new HandlerStorage();
            Server = new HandlerStorage();
            Local = new HandlerStorage();
        }

        public void Init()
        {
            if (_isInitialized) return;

            _isInitialized = true;

            var eventListeners = ReflectionHelper.GetClassesWithAttribute<EventListener>();

            Debug.Log($"Found {eventListeners.Count()} event listeners");

            foreach (var listener in eventListeners)
            {
                var activator = Activator.CreateInstance(listener);
                _listener.Add(activator);
                var eventType = listener.GetCustomAttributes(false).OfType<EventListener>().First().EventType;
                {
                    //Get LocalEventListeners
                    var methods = ReflectionHelper.GetMethodsWithAttribute<LocalListenerAttribute>(listener);
                    foreach (var method in methods)
                    {
                        Debug.Log("Local" + method.ToSafeString());
                        var handler =
                            method.CreateDelegate(ReflectionHelper.GetDelegateType(typeof(bool), eventType),
                                activator);
                        RegisterHandler(HandlerType.Local,eventType, handler);
                    }
                }
                {
                    //Get ClientNetworkingSenders
                    var methods = ReflectionHelper.GetMethodsWithAttribute<ClientNetworkingSenderAttribute>(listener);
                    foreach (var method in methods)
                    {
                        Debug.Log( "Client: " + method.ToSafeString());
                        var handler =
                            method.CreateDelegate(ReflectionHelper.GetDelegateType(typeof(bool), eventType),
                                activator);
                        RegisterHandler(HandlerType.Client, eventType, handler);
                        break;
                    }
                }
                {
                    //Get ServerNetworkingListeners
                    var methods = ReflectionHelper.GetMethodsWithAttribute<ServerNetworkingListenerAttribute>(listener);
                    foreach (var method in methods)
                    {
                        Debug.Log("Server" + method.ToSafeString());
                        var handler =
                            method.CreateDelegate(ReflectionHelper.GetDelegateType(typeof(bool), eventType),
                                activator);
                        RegisterHandler(HandlerType.Server,eventType, handler);
                        break;
                    }
                }
            }
        }

        public void RegisterHandler(HandlerType handlerType ,Type eventType, Delegate handler)
        {
            var storage = handlerType switch
            {
                HandlerType.Local => Local,
                HandlerType.Client => Client,
                HandlerType.Server => Server,
                _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
            };
            storage.RegisterHandler(eventType, handler);
        }

        public void RegisterHandler<T>(HandlerType handlerType ,EventHandler<T> handler) where T : Event
        {
            var type = typeof(T);
            RegisterHandler(handlerType ,type, handler);
        }

        public enum HandlerType
        {
            Local,
            Client,
            Server
        }

        public async void DispatchEvent<T>(HandlerType handlerType, T e, CallbackFunction<T> callback = null) where T : Event
        {
            Debug.Log("Dispatching event: " + e.GetType().Name);
            var storage = handlerType switch
            {
                HandlerType.Local => Local,
                HandlerType.Client => Client,
                HandlerType.Server => Server,
                _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
            };
            await Task.Run(() =>
            {
                storage.DispatchEvent(e, callback);
            });
        }
    }
}