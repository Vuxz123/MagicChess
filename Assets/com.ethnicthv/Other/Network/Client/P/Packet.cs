namespace com.ethnicthv.Other.Network.Client.P
{
    public class Packet
    {
        private readonly byte[] _bytes;
        
        private Packet()
        {
            _bytes = new byte[64];
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
            System.Buffer.BlockCopy(bytes, 0, packet._bytes, 0, bytes.Length);
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