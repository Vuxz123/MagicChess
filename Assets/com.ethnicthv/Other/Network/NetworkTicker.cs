using System;

namespace com.ethnicthv.Other.Network
{
    public interface NetworkTicker
    {
        public int Tick(int processLimit, Func<bool> checkEnabled = null);
    }
}