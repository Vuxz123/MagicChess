using System;
using System.Linq;
using Unity.VisualScripting;

namespace com.ethnicthv.Other.Ev
{
    public delegate void RegisterFunction(EventManager.HandlerType handlerType, Type eventType, Delegate handler);
    public static class EventHandlerCrawler
    {
        public static void Crawl(RegisterFunction registerFunction)
        {
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
                        UnityEngine.Debug.Log("Local: " + method.ToSafeString());
                        var handler =
                            method.CreateDelegate(ReflectionHelper.GetDelegateType(typeof(bool), eventType),
                                activator);
                        registerFunction(EventManager.HandlerType.Local, eventType, handler);
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
                        registerFunction(EventManager.HandlerType.Client, eventType, handler);
                        break;
                    }
                }
                {
                    //Get ServerNetworkingListeners
                    var methods = ReflectionHelper.GetMethodsWithAttribute<ServerNetworkingHandlerAttribute>(listener);
                    foreach (var method in methods)
                    {
                        UnityEngine.Debug.Log("Server: " + method.ToSafeString());
                        var handler =
                            method.CreateDelegate(ReflectionHelper.GetDelegateType(typeof(bool), eventType),
                                activator);
                        registerFunction(EventManager.HandlerType.Server, eventType, handler);
                        break;
                    }
                }
            }
        }
    }
}