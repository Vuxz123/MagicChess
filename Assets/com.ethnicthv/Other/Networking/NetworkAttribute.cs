using System;
using System.Reflection;
using JetBrains.Annotations;

namespace com.ethnicthv.Other.Networking
{
    public class NetworkAttribute : Attribute
    {
        public string EventNetworkName { get; private set; }

        public NetworkAttribute(
            [NotNull] string eventNetworkName
        )
        {
            EventNetworkName = eventNetworkName;
        }
    }
}