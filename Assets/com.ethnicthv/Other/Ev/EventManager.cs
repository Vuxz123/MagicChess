using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using com.ethnicthv.Other.Config;
using UnityEngine.Assertions;

namespace com.ethnicthv.Other.Ev
{
    public delegate void CallbackFunction<in T>(T e) where T : Event;

    public class EventManager
    {
        private static EventManager _instance;

        private bool _isInitialized;

        public static EventManager Instance => _instance ??= new EventManager();

        private readonly HandlerStorage _local; //For Inner Event
        private readonly HandlerStorage _client; //For Client sending
        private readonly HandlerStorage _server; //For Server receiving

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

            EventHandlerCrawler.Crawl(RegisterHandler);
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
            SafeMechanism.Enqueue(handlerType, e);
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

        private readonly Pool<SafeDispatchData> _pool;

        private SafeMechanism()
        {
            var safeMechanismConfig = ConfigProvider.GetConfig().SafeMechanismConfig;
            _blockingCollection = new BlockingCollection<SafeDispatchData>(
                safeMechanismConfig.QueueSize
            );

            _et = safeMechanismConfig.EnqueueTimeout;
            _dt = safeMechanismConfig.DequeueTimeout;

            _pool = new Pool<SafeDispatchData>(safeMechanismConfig.PoolSize, () => new SafeDispatchData(default, null));
        }

        /// <summary>
        /// A Static version of Enqueue method that can be called from any thread. <br/>
        /// This method is for life easier when calling from a static context.
        /// </summary>
        public static void Enqueue<T>(EventManager.HandlerType handlerType, T e) where T : Event
        {
            Instance.Enqueue(handlerType, e);
        }

        public void Enqueue(EventManager.HandlerType handlerType, Event e)
        {
            if (GameManager.IsOnMainThread())
            {
                Debug.LogWarning(
                    "SafeDispatchEvent should not be called from the main thread. Use DispatchEvent instead.");
            }

            SafeDispatchData data;

            //Get data from pool
            //If the pool is empty, a new instance will be created
            lock (_pool)
            {
                data = _pool.Take();
                data.Type = handlerType;
                data.E = e;
            }

            _blockingCollection.TryAdd(data, _et);
        }

        /// <summary>
        /// DrainDispatchQueue is a method that dispatches events from the SafeDispatch queue. <br/>
        /// </summary>
        /// <param name="max">
        /// maximum number of events to dispatch in a single frame. Default is 5
        /// </param>
        public int DrainDispatchQueue(int max = 5)
        {
            //max dispatch 10 events per frame
            for (var i = 0; i < max; i++)
            {
                if (_blockingCollection.TryTake(out var data, _dt))
                {
                    EventManager.Instance.DispatchEvent(data.Type, data.E);

                    //Return data to pool
                    //This is to prevent memory leak
                    //If the pool is full, the data will be discarded
                    lock (_pool)
                    {
                        _pool.Return(data);
                    }
                }
                else break;
            }
            
            return _blockingCollection.Count;
        }

        private class SafeDispatchData
        {
            internal EventManager.HandlerType Type;
            internal Event E;

            public SafeDispatchData(EventManager.HandlerType type, Event e)
            {
                Type = type;
                E = e;
            }
        }
    }
}