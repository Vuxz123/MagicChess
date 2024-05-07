using UnityEngine;

namespace com.ethnicthv.Other.Networking.P
{
    public interface IPacketConverter
    {
        public void ToPacket(PacketWriter writer);
    }
}