using System;
using System.Linq;
using Unity.VisualScripting;

namespace com.ethnicthv.Util.Networking.Packet
{
    //TODO: Rewrite this class to be compatible with adding bits (Current not work after adding bits)
    
    /// <summary>
    /// Thread-Safe PacketWriter class that can be used to write data to a Packet object. <br/>
    /// </summary>
    public class PacketWriter
    {
        private int _length;

        private Packet _p;

        private byte[] Bytes => _p.GetBytes();

        private static readonly Pool<PacketWriter> Pool = new(() => new PacketWriter());

        private PacketWriter()
        {
            _length = 0;
        }

        /// <summary>
        /// Return a PacketWriter object from the pool. <br/>
        /// This is Thread-Safe.
        /// </summary>
        public static PacketWriter Create()
        {
            PacketWriter writer;
            
            // Take a PacketWriter from the pool
            // If pool is empty, generate a new PacketWriter
            // lock is used for thread safety
            lock (Pool)
            {
                writer = Pool.Take();
            }
            
            // Create a new Packet object for the PacketWriter
            writer._p = Packet.Create();

            return writer;
        }

        public PacketWriter Write(byte[] bytes, int length)
        {
            BytesUtil.AppendBytes(bytes, Bytes, _length, length);
            _length += length;
            return this;
        }

        public PacketWriter Write(byte b)
        {
            BytesUtil.AppendByte(b, Bytes, _length, 8);
            _length += 8;
            return this;
        }

        public PacketWriter Write(short s)
        {
            Bytes.AddRange(BitConverter.GetBytes(s));
            _length += 16;
            return this;
        }

        public PacketWriter Write(int i)
        {
            Bytes.AddRange(BitConverter.GetBytes(i));
            _length += 32;
            return this;
        }

        public PacketWriter Write(long l)
        {
            Bytes.AddRange(BitConverter.GetBytes(l));
            _length += 64;
            return this;
        }

        public PacketWriter Write(float f)
        {
            Bytes.AddRange(BitConverter.GetBytes(f));
            _length += 32;
            return this;
        }

        public PacketWriter Write(double d)
        {
            Bytes.AddRange(BitConverter.GetBytes(d));
            _length += 64;
            return this;
        }

        public PacketWriter Write(string str)
        {
            Write(str.Length);
            Bytes.AddRange(System.Text.Encoding.ASCII.GetBytes(str));
            _length += str.Length * 8;
            return this;
        }

        public PacketWriter Write(bool b)
        {
            return Write(b ? (byte)1 : (byte)0);
        }

        public PacketWriter WriterBits(byte bits, int length)
        {
            if (length is < 1 or > 8)
                throw new ArgumentException("Length must be from 1 to 8");

            BytesUtil.AppendByte(bits, Bytes, _length, length);

            _length += length;
            return this;
        }

        public PacketWriter WriterBits(byte[] bits, int length)
        {
            if (length is < 1 or > 8)
                throw new ArgumentException("Length must be from 1 to 8");

            _length += length;
            return this;
        }

        public Packet GetPacket()
        {
            lock (Pool)
            {
                Pool.Return(this);
            }
            return _p;
        }
    }
}