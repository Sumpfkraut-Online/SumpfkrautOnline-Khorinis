using System;
using System.Text;
using GUC.Types;
using System.IO;
using System.IO.Compression;

namespace GUC.Network
{
    public enum PktPriority
    {
        IMMEDIATE_PRIORITY = 0,
        HIGH_PRIORITY = 1,
        MEDIUM_PRIORITY = 2,
        LOW_PRIORITY = 3,
        NUMBER_OF_PRIORITIES = 4
    }

    public enum PktReliability
    {
        UNRELIABLE = 0,
        UNRELIABLE_SEQUENCED = 1,
        RELIABLE = 2,
        RELIABLE_ORDERED = 3,
        RELIABLE_SEQUENCED = 4,
        UNRELIABLE_WITH_ACK_RECEIPT = 5,
        RELIABLE_WITH_ACK_RECEIPT = 6,
        RELIABLE_ORDERED_WITH_ACK_RECEIPT = 7,
        NUMBER_OF_RELIABILITIES = 8
    }

    public class PacketWriter
    {
        const int StandardCapacity = 32000;

        int currentByte;

        int currentBitByte;
        int bitsWritten;
        int bitByte;

        Encoder enc;
        byte[] data;
        int capacity;

        //saved data when in compress mode
        int SCurrentByte;
        int SCurrentBitByte;
        int SBitsWritten;
        int SBitByte;

        internal PacketWriter() : this(StandardCapacity)
        {
        }

        internal PacketWriter(int capacity)
        {
            this.capacity = capacity;
            data = new byte[capacity];
            enc = Encoding.UTF8.GetEncoder();
            Reset();
        }

        internal void Reset()
        {
            currentByte = 0;
            currentBitByte = -1;

            bitsWritten = 0;
            bitByte = 0;
        }

        internal byte[] GetData()
        {
            StopCompressing();
            FlushBits();
            return data;
        }

        public byte[] CopyData()
        {
            byte[] bytes = GetData();
            byte[] ret = new byte[currentByte];
            Array.Copy(bytes, ret, currentByte);
            return ret;
        }

        internal int GetLength()
        {
            return currentByte;
        }

        void CheckRealloc(int add)
        {
            int neededLen = currentByte + add;
            if (neededLen >= capacity)
            {
                capacity = StandardCapacity * (int)((float)neededLen / (float)StandardCapacity + 1.0f);
                byte[] newData = new byte[capacity];
                Buffer.BlockCopy(data, 0, newData, 0, currentByte);
                data = newData;
            }
        }

        #region Compressing

        bool compress = false;
        internal void StartCompressing()
        {
            if (!compress)
            {
                //save data and reset;
                SCurrentByte = currentByte;
                SCurrentBitByte = currentBitByte;
                SBitsWritten = bitsWritten;
                SBitByte = bitByte;

                currentBitByte = -1;
                bitsWritten = 0;
                bitByte = 0;

                compress = true;
            }
        }

        internal void StopCompressing()
        {
            if (compress)
            {
                FlushBits();

                //FIXME: Redo without stream + better performance
                using (MemoryStream ms = new MemoryStream())
                {
                    int uncompressedLen = currentByte - SCurrentByte;

                    using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Compress, true))
                    {
                        ds.Write(data, SCurrentByte, uncompressedLen);
                    }
                    int compressedLen = (int)ms.Length;

                    //switch current byte back
                    currentByte = SCurrentByte;

                    //write lengths
                    Write(uncompressedLen);
                    Write(compressedLen);

                    //read compressed
                    ms.Position = 0;
                    ms.Read(data, currentByte, compressedLen);

                    currentByte += compressedLen;

                    //switch rest back too
                    currentBitByte = SCurrentBitByte;
                    bitsWritten = SBitsWritten;
                    bitByte = SBitByte;
                }

                compress = false;
            }
        }

        #endregion

        #region Writing Methods

        public void Write(bool val)
        {
            if (currentBitByte == -1)
            {
                //CheckRealloc(1); // except when the capacity is 0 this is useless
                currentBitByte = currentByte++;
            }

            if (bitsWritten == 8) // old byte is full
            {
                CheckRealloc(1);
                data[currentBitByte] = (byte)bitByte;
                currentBitByte = currentByte++;
                bitsWritten = 0;
                bitByte = 0;
            }

            if (val)
            {
                bitByte |= (1 << bitsWritten);
            }
            bitsWritten++;
        }

        void FlushBits()
        {
            if (bitsWritten > 0)
            {
                data[currentBitByte] = (byte)bitByte;
            }
        }

        public void Write(sbyte val)
        {
            CheckRealloc(1);
            data[currentByte++] = (byte)val;
        }

        public void Write(byte val)
        {
            CheckRealloc(1);
            data[currentByte++] = val;
        }

        public void Write(short val)
        {
            CheckRealloc(2);
            data[currentByte++] = (byte)val;
            data[currentByte++] = (byte)(val >> 8);
        }

        public void Write(ushort val)
        {
            CheckRealloc(2);
            data[currentByte++] = (byte)val;
            data[currentByte++] = (byte)(val >> 8);
        }

        public void Write(int val)
        {
            CheckRealloc(4);
            data[currentByte++] = (byte)val;
            data[currentByte++] = (byte)(val >> 8);
            data[currentByte++] = (byte)(val >> 16);
            data[currentByte++] = (byte)(val >> 24);
        }

        public void Write(uint val)
        {
            CheckRealloc(4);
            data[currentByte++] = (byte)val;
            data[currentByte++] = (byte)(val >> 8);
            data[currentByte++] = (byte)(val >> 16);
            data[currentByte++] = (byte)(val >> 24);
        }

        public void Write(long val)
        {
            CheckRealloc(8);
            data[currentByte++] = (byte)val;
            data[currentByte++] = (byte)(val >> 8);
            data[currentByte++] = (byte)(val >> 16);
            data[currentByte++] = (byte)(val >> 24);
            data[currentByte++] = (byte)(val >> 32);
            data[currentByte++] = (byte)(val >> 40);
            data[currentByte++] = (byte)(val >> 48);
            data[currentByte++] = (byte)(val >> 56);
        }

        public void Write(ulong val)
        {
            CheckRealloc(8);
            data[currentByte++] = (byte)val;
            data[currentByte++] = (byte)(val >> 8);
            data[currentByte++] = (byte)(val >> 16);
            data[currentByte++] = (byte)(val >> 24);
            data[currentByte++] = (byte)(val >> 32);
            data[currentByte++] = (byte)(val >> 40);
            data[currentByte++] = (byte)(val >> 48);
            data[currentByte++] = (byte)(val >> 56);
        }

        public void Write(float val)
        {
            CheckRealloc(4);
            byte[] arr = BitConverter.GetBytes(val);
            data[currentByte++] = arr[0];
            data[currentByte++] = arr[1];
            data[currentByte++] = arr[2];
            data[currentByte++] = arr[3];
        }

        public void Write(double val)
        {
            CheckRealloc(8);
            byte[] arr = BitConverter.GetBytes(val);
            data[currentByte++] = arr[0];
            data[currentByte++] = arr[1];
            data[currentByte++] = arr[2];
            data[currentByte++] = arr[3];
            data[currentByte++] = arr[4];
            data[currentByte++] = arr[5];
            data[currentByte++] = arr[6];
            data[currentByte++] = arr[7];
        }

        public void Write(byte[] arr, int startIndex, int length)
        {
            CheckRealloc(length);
            Buffer.BlockCopy(arr, startIndex, data, currentByte, length);
            currentByte += length;
        }

        const int MaxStringLength = short.MaxValue;
        char[] charArr = new char[MaxStringLength];
        byte[] byteArr = new byte[2 * MaxStringLength];
        public void Write(string val)
        {
            if (val == null)
            {
                throw new ArgumentNullException("String is null!");
            }

            int charLen = val.Length > MaxStringLength ? MaxStringLength : val.Length; // cut off everything > short.maxValue
            
            val.CopyTo(0, charArr, 0, charLen);
            int byteLen = enc.GetBytes(charArr, 0, charLen, byteArr, 0, true);


            if (byteLen > MaxStringLength)
                byteLen = MaxStringLength;
            
            if (byteLen > 127)
            {
                Write((short)-byteLen);
            }
            else
            {
                Write((sbyte)byteLen);
            }

            Write(byteArr, 0, byteLen);
        }

        public void Write(Vec3f vec)
        {
            CheckRealloc(12);
            byte[] arr = BitConverter.GetBytes(vec.X);
            data[currentByte++] = arr[0];
            data[currentByte++] = arr[1];
            data[currentByte++] = arr[2];
            data[currentByte++] = arr[3];

            arr = BitConverter.GetBytes(vec.Y);
            data[currentByte++] = arr[0];
            data[currentByte++] = arr[1];
            data[currentByte++] = arr[2];
            data[currentByte++] = arr[3];

            arr = BitConverter.GetBytes(vec.Z);
            data[currentByte++] = arr[0];
            data[currentByte++] = arr[1];
            data[currentByte++] = arr[2];
            data[currentByte++] = arr[3];
        }

        public void Write(ColorRGBA color)
        {
            CheckRealloc(4);
            data[currentByte++] = color.R;
            data[currentByte++] = color.G;
            data[currentByte++] = color.B;
            data[currentByte++] = color.A;
        }

        /*
                    if (asInt > 8388607)
                asInt = 8388607;
            else if (asInt < -8388608)
                asInt = -8388608;
                */

        /// <summary>
        /// Coordinates must be between -838860.8 and +838860.7 !!!
        /// </summary>
        public void WriteCompressedPosition(Vec3f position)
        {
            WriteInt24((int)(position.X * 10));
            WriteInt24((int)(position.Y * 10));
            WriteInt24((int)(position.Z * 10));
        }

        void WriteInt24(int val)
        {
            data[currentByte++] = (byte)val;
            data[currentByte++] = (byte)(val >> 8);
            data[currentByte++] = (byte)(val >> 16);
        }

        /// <summary>
        /// Coordinates must be between -1 and +1 !!!
        /// </summary>
        public void WriteCompressedDirection(Vec3f direction)
        {
            data[currentByte++] = (byte)(direction.X * 127.0f);
            data[currentByte++] = (byte)(direction.Y * 127.0f);
            data[currentByte++] = (byte)(direction.Z * 127.0f);
        }

        #endregion
    }
}
