namespace com.ethnicthv.Util.Networking.Packet
{
    public class Packet
    {
        private readonly byte[] _bytes;
        
        private Packet()
        {
            _bytes = new byte[32];
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