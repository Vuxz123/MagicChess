using UnityEngine.ProBuilder;
using Math = System.Math;

namespace com.ethnicthv.Other.Network.Client.P
{
    public class Packet
    {
        private readonly byte[] _bytes;
        
        private Packet(int size = 1024)
        {
            _bytes = new byte[size];
        }
        
        public byte[] GetBytes()
        {
            return _bytes;
        }
        
        public static Packet Create()
        {
            Packet packet;
            
            lock (Pool)
            {
                packet = Pool.Take();
            }
            
            return packet;
        }
        
        public static Packet Create(byte[] bytes)
        {
            var packet = Create();
            var l1 = bytes.Length;
            var l2 = packet._bytes.Length;
            System.Buffer.BlockCopy(bytes, 0, packet._bytes, 0, Math.Min(l1, l2));
            return packet;
        }
        
        public void Clear()
        {
            lock (Pool)
            {
                Pool.Return(this);
            }
        }
        
        private static readonly Pool<Packet> Pool = new(() => new Packet());
    }
}