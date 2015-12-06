using System;
using System.Text;

namespace GUC.Network
{
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
            FlushBits();
            return data;
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

        public void Write(byte[] arr, int index, int length)
        {
            CheckRealloc(length);
            Buffer.BlockCopy(arr, index, data, currentByte, length);
            currentByte += length;
        }

        const int MaxStringLength = short.MaxValue;
        char[] charArr = new char[MaxStringLength];
        public void Write(string val)
        {
            int len = val.Length > MaxStringLength ? MaxStringLength : val.Length; // cut off everything > short.maxValue

            if (len > 127)
            {
                Write((short)-len);
            }
            else
            {
                Write((sbyte)len);
            }

            CheckRealloc(len);

            val.CopyTo(0, charArr, 0, len);
            enc.GetBytes(charArr, 0, len, data, currentByte, true);
            currentByte += len;
        }

        #endregion
    }
}
