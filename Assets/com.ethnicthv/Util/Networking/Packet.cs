using System;

namespace com.ethnicthv.Util.Networking
{
    public class Packet
    {
        private byte[] innerData;
        
        public Packet(byte typeID ,byte[] innerData)
        {
            this.innerData = new byte[innerData.Length + 1];
            this.innerData[0] = typeID;
            innerData.CopyTo(this.innerData, 1);
        }

        public byte[] GetBytes()
        {
            return innerData;
        }

        public void SetBytes(byte[] data)
        {
            this.innerData = data;
        }
        
        public static Packet operator+(Packet self ,byte[] data)
        {
            byte[] newData = new byte[self.innerData.Length + data.Length];
            self.innerData.CopyTo(newData, 0);
            data.CopyTo(newData, self.innerData.Length);
            self.innerData = newData;
            return self;
        }
        
        public static Packet operator+(Packet self ,Packet other)
        {
            var newData = new byte[self.innerData.Length + other.innerData.Length];
            self.innerData.CopyTo(newData, 0);
            other.innerData.CopyTo(newData, self.innerData.Length);
            self.innerData = newData;
            return self;
        }

    }
}