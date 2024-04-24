using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace com.ethnicthv.Util.Networking.Packet
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
        /// (Counting from the left hand-side) <br/>
        /// Note: if possible, please use the other GetByte function instead, for memory efficiency, cause this function will create a new byte array.
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
        /// if the length of the byte array to be appended is less than the length, the byte array will be padded with 0s. <br/>
        /// if the length of the byte array to be appended is greater than the length, the byte array will be truncated. <br/>
        /// </summary>
        /// <param name="addition">
        /// byte array to be appended
        /// </param>
        /// <param name="ori">
        /// byte to be appended to
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
        public static byte[] AppendByte(byte addition, [NotNull] byte[] ori, in int start, in int length)
        {
            // check start
            var oriMaxLength = ori.Length * 8;
            if (start > oriMaxLength - 1 || start < 0)
            {
                throw new ArgumentException("Start must be from 0 to input length * 8 - 1");
            }

            // check length
            var maxAppendLength = Math.Min(oriMaxLength - start, 8);
            if (length > maxAppendLength || length < 1)
            {
                throw new ArgumentException($"Length {length} must be from 1 to {maxAppendLength}");
            }

            // in special case where the original byte array is empty
            if (ori.Length == 0)
            {
                ori[0] = addition;
                return ori;
            }

            // in special case where the addition lies in two byte
            if ((start % 8) + length > 8)
            {
                var last2 = start / 8;
                var last1 = last2 + 1;
                var firstPart = ori[last2];
                Debug.Log($"First Part: {Convert.ToString(firstPart, toBase: 2).PadLeft(8, '0')}");
                var secondPart = ori[last1];
                Debug.Log($"Second Part: {Convert.ToString(secondPart, toBase: 2).PadLeft(8, '0')}");
                var firstPartAdditionLength = last1 * 8 - start;
                Debug.Log($"First Part Addition Length: {firstPartAdditionLength}");
                var secondPartAdditionLength = length - firstPartAdditionLength;
                Debug.Log($"Second Part Addition Length: {secondPartAdditionLength}");
                var temp = (byte)(addition << (8 - length));
                var firstPartAddition = GetByte(temp, 0, firstPartAdditionLength);
                Debug.Log($"First Part Addition: {Convert.ToString(firstPartAddition, toBase: 2).PadLeft(8, '0')}");
                var secondPartAddition = GetByte(temp, firstPartAdditionLength, secondPartAdditionLength);
                Debug.Log($"Second Part Addition: {Convert.ToString(secondPartAddition, toBase: 2).PadLeft(8, '0')}");
                ori[last2] = AppendByte(firstPartAddition, firstPart, start % 8, firstPartAdditionLength);
                Debug.Log($"First Part After: {Convert.ToString(ori[last2], toBase: 2).PadLeft(8, '0')}");
                ori[last1] = AppendByte(secondPartAddition, secondPart, 0, secondPartAdditionLength);
                Debug.Log($"Second Part After: {Convert.ToString(ori[last1], toBase: 2).PadLeft(8, '0')}");
                return ori;
            }
            else
            {
                var last1 = start / 8;
                Debug.Log("Append position: " + last1);
                var insertPart = ori[last1];
                Debug.Log("Insert Part: " + Convert.ToString(insertPart, toBase: 2).PadLeft(8, '0'));
                Debug.Log("Addition: " + Convert.ToString(addition, toBase: 2).PadLeft(8, '0'));
                Debug.Log("Start: " + start);
                Debug.Log("Length: " + length);
                ori[last1] = AppendByte(addition, insertPart, start % 8, length);
                Debug.Log("Insert Part After: " + Convert.ToString(ori[last1], toBase: 2).PadLeft(8, '0'));
                return ori;
            }
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

            Debug.Log("Addition: " + Convert.ToString(addition, toBase: 2).PadLeft(8, '0'));
            var mask = CreateByteBitMask(start, length);
            Debug.Log("Mask: " + Convert.ToString(mask, toBase: 2).PadLeft(8, '0'));
            var nMask = (byte)~mask;
            Debug.Log("Negative Mask: " + Convert.ToString(nMask, toBase: 2).PadLeft(8, '0'));
            var oriMasked = (byte)(ori & nMask);
            Debug.Log("Ori Masked: " + Convert.ToString(oriMasked, toBase: 2).PadLeft(8, '0'));
            var addMasked = (byte)(addition & mask);
            Debug.Log("Add Masked: " + Convert.ToString(addMasked, toBase: 2).PadLeft(8, '0'));
            var result = (byte)(oriMasked | addMasked);
            Debug.Log("Result: " + Convert.ToString(result, toBase: 2).PadLeft(8, '0'));
            return result;
        }

        public static byte[] AppendBytes(byte[] addition, byte[] ori, in int start, in int length)
        {
            // check start
            var oriMaxLength = ori.Length * 8;
            if (start > oriMaxLength - 1 || start < 0)
            {
                throw new ArgumentException($"Start {start} must be from 0 to {(oriMaxLength - 1)}");
            }

            // check length
            var maxAppendLength = oriMaxLength - start;
            if (length > maxAppendLength || length < 1)
            {
                throw new ArgumentException($"Length {length} must be from 1 to " + maxAppendLength);
            }

            // for each byte in the addition, add it to the ori
            var tempStart = start;
            var tempLength = length;
            foreach (var a in addition)
            {
                Debug.Log($"Compare {tempLength} and {8} from {length}");
                var l = Math.Min(tempLength, 8);
                ori = AppendByte(a, ori, tempStart, l);
                tempStart += l;
                // subtract the remain input length by 8 when the length is greater than 8
                tempLength -= 8;
                Debug.Log("New length: " + tempLength);
                Debug.Log(
                    "Append Byte: \n" + string.Join("\n", ori.Select(b => Convert.ToString(b, 2).PadLeft(8, '0'))));
                // if the remain input length is 0, break the loop
                if (tempLength <= 0) break;
            }

            return ori;
        }

        public static byte CreateByteBitMask(int start, int length)
        {
            // Check if start and length are valid
            if (start < 0 || length <= 0 || start + length > 8)
            {
                throw new ArgumentException("Invalid start or length for bit mask");
            }

            // Create the mask
            var temp = (byte)(0xFF << (8 - length));
            temp = (byte)(temp >> start);
            return temp;
        }

        public static void IntToBytes(int i, byte[] bytes)
        {
            bytes[0] = (byte)(i >> 24);
            bytes[1] = (byte)(i >> 16);
            bytes[2] = (byte)(i >> 8);
            bytes[3] = (byte)i;
        }

        public static void ShortToBytes(short s, byte[] bytes)
        {
            bytes[0] = (byte)(s >> 8);
            bytes[1] = (byte)s;
        }

        public static void FloatToBytes(float f, byte[] bytes)
        {
            var i = BitConverter.ToInt32(BitConverter.GetBytes(f), 0);
            IntToBytes(i, bytes);
        }
    }
}