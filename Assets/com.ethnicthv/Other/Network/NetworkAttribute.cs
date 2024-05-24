using System;
using JetBrains.Annotations;

namespace com.ethnicthv.Other.Network
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