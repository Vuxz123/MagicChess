using System;

namespace com.ethnicthv.Other.Network
{
    public interface NetworkSender
    {
        public bool Send(ArraySegment<byte> message);
    }
}