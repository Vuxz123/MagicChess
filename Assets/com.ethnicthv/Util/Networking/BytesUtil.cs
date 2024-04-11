using System;

namespace com.ethnicthv.Util.Networking
{
    public static class BytesUtil
    {
        /// <summary>
        /// A function that get number of bit from the input byte <br/>
        /// (Counting from the left hand-side)
        /// </summary>
        /// <param name="input"> the input byte </param>
        /// <param name="length"> number of bit taken (value can be from 1 to 8) </param>
        /// <returns> the byte of number of bits from the original byte </returns>
        /// <exception cref="ArgumentException">
        ///    Length must be from 1 to 8. <br/>
        /// </exception>
        public static byte GetByte(byte input, int length)
        {
            if (length is < 1 or > 8)
            {
                throw new ArgumentException("Length must be from 1 to 8");
            }

            return (byte)(input >> (8 - length));
        }

        /// <summary>
        /// A function that get number of bit from the input byte from a start position <br/>
        /// (Counting from the left hand-side)
        /// </summary>
        /// <param name="input"> the input byte </param>
        /// <param name="start"> start position (value can be from 0 to 7) </param>
        /// <param name="length"> number of bit taken (value can be from 1 to 8) </param>
        /// <returns> the byte of number of bits from the original byte </returns>
        /// <exception cref="ArgumentException">
        ///   Length must be from 1 to 8. <br/>
        ///   Start must be from 0 to 7. <br/>
        ///   Sum of start and length must be less than or equal to 8. <br/>
        /// </exception>
        public static byte GetByte(byte input, int start, int length)
        {
            if (length is < 1 or > 8)
            {
                throw new ArgumentException("Length must be from 1 to 8");
            }

            if (start is < 0 or > 7)
            {
                throw new ArgumentException("Start must be from 0 to 7");
            }

            if (start + length > 8)
            {
                throw new ArgumentException("Sum of start and length must be less than or equal to 8");
            }

            return (byte)(input >> (8 - length - start) & (0xFF >> (8 - length)));
        }

        /// <summary>
        /// A function that get number of bit from the input byte array <br/>
        /// (Counting from the left hand-side)
        /// </summary>
        /// <param name="input">
        ///   the input byte array <br/>
        /// </param>
        /// <param name="start">
        ///  start position (value can be from 0 to input length * 8 - 1) <br/>
        /// </param>
        /// <param name="length">
        ///  number of bit taken (value can be from 1 to input length * 8) <br/>
        /// </param>
        /// <returns>
        ///  the byte array of number of bits from the original byte array <br/>
        /// </returns>
        /// <exception cref="ArgumentException">
        ///  <list type="dotted">
        ///   <item>Length must be less than or equal to the input length * 8</item>
        ///  <item>Length must be greater than 0</item>
        ///  <item>Start must be from 0 to input length * 8 - 1</item>
        /// </list>
        /// </exception>
        public static byte[] GetBytes(byte[] input, int start, int length)
        {
            var inputMaxLen = input.Length * 8;
            if (length > inputMaxLen || length < 1)
            {
                throw new ArgumentException("Length must be less than or equal to the input length * 8");
            }

            if (start > inputMaxLen - 1 || start < 0)
            {
                throw new ArgumentException("Start must be from 0 to input length * 8 - 1");
            }

            if (start + length > inputMaxLen)
            {
                throw new ArgumentException("Sum of start and length must be less than or equal to input length * 8");
            }

            var result = new byte[(length + 7) / 8];
            for (var i = 0; i < length; i++)
            {
                var bit = GetByte(input[start / 8], start % 8, 1);
                result[i / 8] |= (byte)(bit << (7 - i % 8));
                start++;
            }

            return result;
        }

        /// <summary>
        /// A function that take a byte array and append it to another byte array at a specific position and have a specific length. <br/>
        /// if the length of the byte array to be appended is out of range of the byte array to be appended to, it will extend the byte array to fit the length. <br/>
        /// if the length of the byte array to be appended is less than the length, the byte array will be padded with 0s. <br/>
        /// if the length of the byte array to be appended is greater than the length, the byte array will be truncated. <br/>
        /// </summary>
        /// <param name="addition">
        /// byte array to be appended
        /// </param>
        /// <param name="ori">
        /// byte array to be appended to
        /// </param>
        /// <param name="start">
        /// position to append the byte array (value can be from 0 to bytes2 length * 8 - 1)
        /// </param>
        /// <param name="length">
        /// length of the byte array to be appended
        /// </param>
        /// <returns>
        /// the byte array after appending
        /// </returns>
        public static byte[] AppendBytes(byte[] addition, byte[] ori, int start, int length)
        {
            var oriMaxLength = ori.Length * 8;
            if (start > oriMaxLength - 1 || start < 0)
            {
                throw new ArgumentException("Start must be from 0 to input length * 8 - 1");
            }

            if (start + length > oriMaxLength)
            {
                var result = new byte[(start + length + 7) / 8];
                ori.CopyTo(result, 0);

                return result;
            }
            else
            {
                var result = new byte[ori.Length];
                ori.CopyTo(result, 0);
                // 
            }

            return null;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="addition"></param>
        /// <param name="ori"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static byte AppendByte(byte addition, byte ori, int start, int length)
        {
            if (start is > 7 or < 0)
            {
                throw new ArgumentException("Start must be from 0 to 7");
            }
            
            if (length is > 8 or < 1)
            {
                throw new ArgumentException("Length must be from 1 to 8");
            }

            if (start + length > 8)
            {
                throw new ArgumentException("Sum of start and length must be less than or equal to 8");
            }
            
            var result = ori;
            var mask = CreateByteBitMask(start, length);
            var n_mask = ~mask;
            result = (byte)((result & n_mask) | ((addition << (8 - length)) & mask));
            return result;
        }
        
        public static byte CreateByteBitMask(int start, int length)
        {
            // Check if start and length are valid
            if (start < 0 || length <= 0 || start + length > 8)
            {
                throw new ArgumentException("Invalid start or length for bit mask");
            }

            // Create the mask
            byte mask = (byte)(((1 << length) - 1) << start);

            return mask;
        }
    }
}