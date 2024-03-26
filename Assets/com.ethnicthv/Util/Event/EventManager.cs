using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

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

            UnityEngine.Debug.Log($"Found {eventListeners.Count()} event listeners");

            foreach (var listener in eventListeners)
            {
                var activator = Activator.CreateInstance(listener);
                _listener.Add(activator);
                var eventType = listener.GetCustomAttributes(false).OfType<EventListener>().First().EventType;
                {
                    //Get LocalEventListeners
                    var methods = ReflectionHelper.GetMethodsWithAttribute<LocalHandlerAttribute>(listener);
                    foreach (var method in methods)
                    {
                        UnityEngine.Debug.Log("Local" + method.ToSafeString());
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
                        UnityEngine.Debug.Log( "Client: " + method.ToSafeString());
                        var handler =
                            method.CreateDelegate(ReflectionHelper.GetDelegateType(typeof(bool), eventType),
                                activator);
                        RegisterHandler(HandlerType.Client, eventType, handler);
                        break;
                    }
                }
                {
                    //Get ServerNetworkingListeners
                    var methods = ReflectionHelper.GetMethodsWithAttribute<ServerNetworkingHandlerAttribute>(listener);
                    foreach (var method in methods)
                    {
                        UnityEngine.Debug.Log("Server" + method.ToSafeString());
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

        /// <summary>
        /// The DispatchEvent method in the EventManager class is responsible for dispatching events of a specific type T where T is a subclass of Event. This method can only be called from the MainThread to prevent nested call
        /// </summary>
        /// <param name="handlerType">
        /// This is an enumeration of type HandlerType which determines the type of handler storage to be used when dispatching an event. The HandlerType enumeration has three possible values: Local, Client, and Server. The chosen handlerType affects where the event is registered and subsequently dispatched.
        /// </param>
        /// <param name="e">
        /// This is an object of type T representing the event to be dispatched. This object must not be null.
        /// </param>
        /// <param name="callback">
        /// This is an optional parameter of type `CallbackFunction`. If provided, this callback function will be called after the event dispatch finishes.
        /// </param>
        /// <typeparam name="T">
        /// Type of the event
        /// </typeparam>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public async void DispatchEvent<T>(HandlerType handlerType, T e, CallbackFunction<T> callback = null) where T : Event
        {
            UnityEngine.Debug.Log("EventManager: Checking!");
            //Check if the event dispatching is on the main thread
            if (!GameManager.IsOnMainThread())
            {
                throw new Exception("Event dispatching is not on the main thread");
            }
            //Assert e not null
            Assert.IsNotNull(e);
            var storage = handlerType switch
            {
                HandlerType.Local => Local,
                HandlerType.Client => Client,
                HandlerType.Server => Server,
                _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
            };
            UnityEngine.Debug.Log("EventManager: DispatchEvent " + e.GetType().Name);
            try
            {
                await Task.Run(() =>
                {
                    storage.DispatchEvent(e, callback);
                });
            }
            catch (AggregateException exception)
            {
                UnityEngine.Debug.LogError(exception);
                throw;
            }
        }
    }
}