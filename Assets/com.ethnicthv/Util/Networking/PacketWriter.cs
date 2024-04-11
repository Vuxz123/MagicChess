using System;
using System.Collections.Generic;

namespace com.ethnicthv.Util.Networking
{
    public class PacketWriter
    {
        private List<byte> _bytes;
        private int _length;

        private PacketWriter()
        {
            _bytes = new List<byte>();
            _length = 0;
        }
        
        public static PacketWriter Create()
        {
            return new PacketWriter();
        }
        
        public PacketWriter Write(byte[] bytes)
        {
            _bytes.AddRange(bytes);
            _length += bytes.Length * 8;
            return this;
        }
        
        public PacketWriter Write(byte b)
        {
            _bytes.Add(b);
            _length += 8;
            return this;
        }
        
        public PacketWriter Write(short s)
        {
            _bytes.AddRange(BitConverter.GetBytes(s));
            _length += 16;
            return this;
        }
        
        public PacketWriter Write(int i)
        {
            _bytes.AddRange(BitConverter.GetBytes(i));
            _length += 32;
            return this;
        }
        
        public PacketWriter Write(long l)
        {
            _bytes.AddRange(BitConverter.GetBytes(l));
            _length += 64;
            return this;
        }
        
        public PacketWriter Write(float f)
        {
            _bytes.AddRange(BitConverter.GetBytes(f));
            _length += 32;
            return this;
        }
        
        public PacketWriter Write(double d)
        {
            _bytes.AddRange(BitConverter.GetBytes(d));
            _length += 64;
            return this;
        }
        
        public PacketWriter Write(string str)
        {
            Write(str.Length);
            _bytes.AddRange(System.Text.Encoding.ASCII.GetBytes(str));
            _length += str.Length * 8;
            return this;
        }
        
        public PacketWriter Write(bool b)
        {
            _bytes.Add((byte) (b ? 1 : 0));
            _length += 1;
            return this;
        }
        
        public Packet GetPacket()
        {
            return new Packet(_bytes.ToArray());
        }
        
    }
}