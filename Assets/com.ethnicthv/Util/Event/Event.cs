﻿using System;
using com.ethnicthv.Util.Networking;

namespace com.ethnicthv.Util.Event
{
    public abstract class Event
    {
        public abstract Packet PackEvent();
        
        public abstract void UnpackEvent(Packet packet);
    }
}