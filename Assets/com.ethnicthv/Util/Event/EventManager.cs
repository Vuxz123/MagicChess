using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using com.ethnicthv.Util.Config;
using Unity.VisualScripting;
using UnityEngine.Assertions;

namespace com.ethnicthv.Util.Event
{
    public delegate void CallbackFunction<in T>(T e) where T : Event;

    public class EventManager
    {
        private static EventManager _instance;

        private bool _isInitialized;

        public static EventManager Instance => _instance ??= new EventManager();

        private readonly HandlerStorage _local; //For Inner Event
        private readonly HandlerStorage _client; //For Client communication
        private readonly HandlerStorage _server; //For Server communication

        private EventManager()
        {
            _client = new HandlerStorage();
            _server = new HandlerStorage();
            _local = new HandlerStorage();
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
                        RegisterHandler(HandlerType.Local, eventType, handler);
                    }
                }
                {
                    //Get ClientNetworkingSenders
                    var methods = ReflectionHelper.GetMethodsWithAttribute<ClientNetworkingSenderAttribute>(listener);
                    foreach (var method in methods)
                    {
                        UnityEngine.Debug.Log("Client: " + method.ToSafeString());
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
                        RegisterHandler(HandlerType.Server, eventType, handler);
                        break;
                    }
                }
            }
        }

        public void RegisterHandler(HandlerType handlerType, Type eventType, Delegate handler)
        {
            var storage = handlerType switch
            {
                HandlerType.Local => _local,
                HandlerType.Client => _client,
                HandlerType.Server => _server,
                _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
            };
            storage.RegisterHandler(eventType, handler);
        }

        public void RegisterHandler<T>(HandlerType handlerType, EventHandler<T> handler) where T : Event
        {
            var type = typeof(T);
            RegisterHandler(handlerType, type, handler);
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
        public async void DispatchEvent<T>(HandlerType handlerType, T e, CallbackFunction<T> callback = null)
            where T : Event
        {
            Debug.Log("EventManager: Checking!");
            //Check if the event dispatching is on the main thread
            if (!GameManager.IsOnMainThread())
            {
                throw new System.Exception("Event dispatching is not on the main thread");
            }

            //Assert e not null
            Assert.IsNotNull(e);
            var storage = handlerType switch
            {
                HandlerType.Local => _local,
                HandlerType.Client => _client,
                HandlerType.Server => _server,
                _ => throw new ArgumentOutOfRangeException(nameof(handlerType), handlerType, null)
            };
            Debug.Log("EventManager: DispatchEvent " + e.GetType().Name);
            try
            {
                await Task.Run(() => { storage.DispatchEvent(e, callback); });
            }
            catch (AggregateException exception)
            {
                Debug.LogError(exception);
                throw;
            }
        }

        /// <summary>
        /// SafeDispatchEvent is a Variant of DispatchEvent that can be called from any thread. It is used to dispatch events of a specific type T where T is a subclass of Event. This method can be called from any thread.
        /// </summary>
        public void SafeDispatchEvent<T>(HandlerType handlerType, T e) where T : Event
        {
            //check if SafeMechanism is initialized
            if (!SafeMechanism.IsInitialized)
            {
                throw new System.Exception("Event Safe Mechanism is not initialized");
            }

            //Assert e not null
            Assert.IsNotNull(e);
            SafeMechanism.Instance.Enqueue(handlerType, e);
        }
    }

    /// <summary>
    /// SafeMechanism is a class that contains methods for safely dispatching events from any thread. <br/>
    /// To use this class, you must first create an instance of the SafeMechanism class, then call it from the Safe Thread. (Commonly is MainThread)
    /// </summary>
    public class SafeMechanism
    {
        public static SafeMechanism Instance { get; private set; }

        public static bool IsInitialized { get; private set; }

        public static void Init()
        {
            Instance = new SafeMechanism();
            IsInitialized = true;
        }

        private readonly BlockingCollection<SafeDispatchData> _blockingCollection;

        private readonly int _et;
        private readonly int _dt;

        private SafeMechanism()
        {
            var safeMechanismConfig = ConfigProvider.GetConfig().GetSafeMechanismConfig();
            _blockingCollection = new BlockingCollection<SafeDispatchData>(
                safeMechanismConfig.QueueSize
                );

            _et = safeMechanismConfig.EnqueueTimeout;
            _dt = safeMechanismConfig.DequeueTimeout;
        }
        
        public void Enqueue(EventManager.HandlerType handlerType, Event e)
        {
            if (GameManager.IsOnMainThread())
            {
                Debug.LogWarning(
                    "SafeDispatchEvent should not be called from the main thread. Use DispatchEvent instead.");
            }

            _blockingCollection.TryAdd(new SafeDispatchData(handlerType, e), _et);
        }

        /// <summary>
        /// DrainDispatchQueue is a method that dispatches events from the SafeDispatch queue. <br/>
        /// </summary>
        /// <param name="max">
        /// maximum number of events to dispatch in a single frame. Default is 5
        /// </param>
        public void DrainDispatchQueue(int max = 5)
        {
            //max dispatch 10 events per frame
            for (var i = 0; i < max; i++)
            {
                if (_blockingCollection.TryTake(out var data, _dt))
                {
                    EventManager.Instance.DispatchEvent(data.Type, data.E);
                }
                else
                {
                    break;
                }
            }
        }

        private class SafeDispatchData
        {
            internal readonly EventManager.HandlerType Type;
            internal readonly Event E;

            public SafeDispatchData(EventManager.HandlerType type, Event e)
            {
                Type = type;
                E = e;
            }
        }
    }
}