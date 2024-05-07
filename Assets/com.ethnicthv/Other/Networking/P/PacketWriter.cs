using System;
using System.Linq;

namespace com.ethnicthv.Other.Networking.P
{
    /// <summary>
    /// Thread-Safe PacketWriter class that can be used to write data to a Packet object. <br/>
    /// </summary>
    public class PacketWriter
    {
        private static readonly Pool<PacketWriter> Pool = new(() => new PacketWriter());
        
        private int _length;

        private Packet _p;

        private byte[] Bytes => _p.GetBytes();
        
        private readonly byte[] _tempBuffer = new byte[8];

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

        public PacketWriter Write(byte b)
        {
            return WriteBits(b, 8);
        }

        public PacketWriter Write(short s)
        {
            BytesUtil.ShortToBytes(s, _tempBuffer);
            return WriteBits(_tempBuffer, 16);
        }

        public PacketWriter Write(int i)
        {
            BytesUtil.IntToBytes(i, _tempBuffer);
            Debug.Log("Temp Bytes: \n" + string.Join("\n", _tempBuffer.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
            return WriteBits(_tempBuffer, 32);
        }

        public PacketWriter Write(float f)
        {
            BytesUtil.FloatToBytes(f, _tempBuffer);
            return WriteBits(_tempBuffer, 32);
        }

        public PacketWriter Write(bool b)
        {
            return WriteBits(b? (byte) 0b_1000_0000 : (byte) 0, 1);
        }

        public PacketWriter WriteBits(byte bits, int length)
        {
            CheckWriteValidity(length);
            if (length is < 1 or > 8)
                throw new ArgumentException("Length must be from 1 to 8");
            BytesUtil.AppendByte(bits, Bytes, _length, length);
            _length += length;
            return this;
        }

        public PacketWriter WriteBits(byte[] bits, int length)
        {
            CheckWriteValidity(length);
            // Debug.Log("Bits: \n" + string.Join("\n", bits.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
            var l = bits.Length * 8;
            if (length < 1 || length > l )
                throw new ArgumentException($"Length {l} must be from 1 to " + l);
            BytesUtil.AppendBytes(bits, Bytes, _length, length);
            _length += length;
            return this;
        }
        
        private void CheckWriteValidity(int length)
        {
            var remaining = 256 - _length;
            if (length > remaining)
                throw new ArgumentException($"Length {length} is exceeding the remaining space in the packet.");
            if (length < 1)
                throw new ArgumentException($"Length {length} must be greater than 0.");
        }

        public Packet GetPacket()
        {
            // Debug.Log(_length);
            lock (Pool)
            {
                _length = 0;
                Pool.Return(this);
            }
            return _p;
        }
    }
}