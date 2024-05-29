using System;

namespace com.ethnicthv.Other.Network.P
{
    /// <summary>
    /// Thread-Safe PacketReader class that can be used to read data from a Packet object. <br/>
    /// </summary>
    public class PacketReader
    {
        private static readonly Pool<PacketReader> Pool = new(() => new PacketReader());
        
        private Packet _p;
        
        private int _readPos;
        
        private readonly byte[] _tempBuffer = new byte[8];
        
        private PacketReader()
        {
            _readPos = 0;
        }

        /// <summary>
        /// Return a PacketReader object from the pool. <br/>
        /// This is Thread-Safe.
        /// Remember to call Close() after you are done using the PacketReader object.
        /// </summary>
        /// <param name="packet">
        /// The Packet object to be assigned to the PacketReader.
        /// </param>
        public static PacketReader Create(Packet packet)
        {
            PacketReader writer;
            
            // Take a PacketWriter from the pool
            // If pool is empty, generate a new PacketWriter
            // lock is used for thread safety
            lock (Pool)
            {
                writer = Pool.Take();
            }
            
            // Assign the Packet object to the PacketWriter
            writer._p = packet;

            return writer;
        }
        
        public bool ReadBool()
        {
            return ReadNext8Bits(1) == 1;
        }
        
        public byte ReadByte()
        {
            return ReadNext8Bits(8);
        }
        
        public short ReadShort()
        {
            return BytesUtil.BytesToShort(ReadNextBits(16));
        }
        
        public int ReadInt()
        {
            return BytesUtil.BytesToInt(ReadNextBits(32));
        }
        
        public float ReadFloat()
        {
            return BytesUtil.BytesToFloat(ReadNextBits(32));
        }
        
        public byte ReadNext8Bits(int length)
        {
            CheckReadValidity(length);
            if(length is < 1 or > 8)
                throw new ArgumentException("Length must be between 1 and 8.");
            byte a = BytesUtil.GetByte(_p.GetBytes(), _readPos, length);
            _readPos += length;
            return a;
        }
        
        public byte[] ReadNextBits(int length)
        {
            CheckReadValidity(length);
            BytesUtil.GetBytes(_p.GetBytes(), _readPos, length, _tempBuffer);
            _readPos += length;
            return _tempBuffer;
        }
        
        private void CheckReadValidity(int length)
        {
            var remaining = 256 - _readPos;
            if (length > remaining)
                throw new ArgumentException($"Length {length} is exceeding the remaining space in the packet.");
            if (length < 1)
                throw new ArgumentException($"Length {length} must be greater than 0.");
        }
        
        /// <summary>
        /// After reading the data from the PacketReader, return it to the pool. <br/>
        /// Remember to call this method after you are done using the PacketReader object. <br/>
        /// Note: Do not use the PacketReader object after calling this method.
        /// </summary>
        public void Close(bool closePacket = true)
        {
            lock (Pool)
            {
                _readPos = 0;
                Pool.Return(this);
            }
            if (closePacket)
            {
                _p.Clear();
            }
        }
    }
}