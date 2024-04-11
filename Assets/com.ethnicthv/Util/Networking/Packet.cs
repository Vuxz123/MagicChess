using System.Collections.Generic;

namespace com.ethnicthv.Util.Networking
{
    public class Packet
    {
        private byte[] _bytes;
        
        public Packet(byte[] bytes)
        {
            _bytes = bytes;
        }
        
        public byte[] GetBytes()
        {
            return _bytes;
        }
    }
}