using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace com.ethnicthv.Util.Networking
{
    //TODO: Rewrite this class to be compatible with adding bits (Current not work after adding bits)
    public class PacketWriter
    {
        private byte[] _bytes;
        private int _length;

        private PacketWriter()
        {
            _bytes = Array.Empty<byte>();
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
            byte[] temp = {b};
            _bytes.AddRange(temp);
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
            return Write(b ? (byte) 1 : (byte) 0);
        }
        
        public PacketWriter WriterBits(byte bits, int length)
        {
            if(length is < 1 or > 8)
                throw new ArgumentException("Length must be from 1 to 8");

            BytesUtil.AppendByte( bits , _bytes, _length, length);
            
            _length += length;
            return this;
        }
        
        public PacketWriter WriterBits(byte[] bits, int length)
        {
            if(length is < 1 or > 8)
                throw new ArgumentException("Length must be from 1 to 8");
            
            _length += length;
            return this;
        }
        
        public Packet GetPacket()
        {
            return new Packet(_bytes.ToArray());
        }
        
    }
}