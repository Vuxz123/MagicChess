using System;
using System.Reflection;
using JetBrains.Annotations;

namespace com.ethnicthv.Other.Networking
{
    public class NetworkAttribute : Attribute
    {
        private string FromPacketMethodName { get; set; }
        public string EventNetworkName { get; private set; }

        public NetworkAttribute(
            [InvokerParameterName] string fromPacketMethodName,
            string eventNetworkName)
        {
            FromPacketMethodName = fromPacketMethodName;
            EventNetworkName = eventNetworkName;
        }

        public MethodInfo GetFromPacketMethod(Type type)
        {
            return type.GetMethod(FromPacketMethodName,
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        }
    }
}