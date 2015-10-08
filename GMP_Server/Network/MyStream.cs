using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Runtime.InteropServices;

namespace GUC.Server.Network
{
    class MyStream : IDisposable
    {
        const int addCap = 8192; // amount of bytes added to capacity on realloc
        int capacity;
        int oriCap;

        IntPtr startPtr;
        IntPtr currentByte;
        IntPtr currentBitByte; // dank name

        int bytesWritten;
        int bitsWritten;
        int bitByteOffset;
        byte bitByte;

        Encoder enc;
        byte[] resultData;

        public MyStream()
            : this(65536)
        {
        }

        public MyStream(int capacity)
        {
            oriCap = capacity;
            this.capacity = capacity;

            startPtr = Marshal.AllocHGlobal(capacity);
            resultData = new byte[capacity];

            currentByte = startPtr;
            currentBitByte = IntPtr.Zero;

            bytesWritten = 0;
            bitsWritten = 8; // so a new byte is chosen for bit writing
            bitByteOffset = 0;
            bitByte = 0;

            enc = Encoding.UTF8.GetEncoder();
        }

        public void Dispose()
        {
            Marshal.FreeHGlobal(startPtr);
            startPtr = IntPtr.Zero;
            currentByte = IntPtr.Zero;
            currentBitByte = IntPtr.Zero;
            resultData = null;
        }

        public void Reset()
        {
            if (capacity != oriCap)
            {
                capacity = oriCap;
                startPtr = Marshal.ReAllocHGlobal(startPtr, (IntPtr)capacity);
                resultData = new byte[capacity];
            }

            currentByte = startPtr;
            currentBitByte = IntPtr.Zero;

            bytesWritten = 0;
            bitsWritten = 8;
            bitByteOffset = 0;
            bitByte = 0;
        }

        public byte[] GetData()
        {
            if (currentBitByte != IntPtr.Zero && bitsWritten > 0)
            {
                Marshal.WriteByte(currentBitByte, bitByte);
            }
            Marshal.Copy(startPtr, resultData, 0, bytesWritten);
            return resultData;
        }

        public int GetLength()
        {
            return bytesWritten;
        }

        void AddBytesAndRealloc(int bytesToWrite)
        {
            int newBytesWritten = bytesWritten + bytesToWrite;
            if (newBytesWritten > capacity)
            {
                capacity = newBytesWritten + addCap;
                startPtr = Marshal.ReAllocHGlobal(startPtr, (IntPtr)capacity);
                resultData = new byte[capacity];
                currentByte = startPtr + bytesWritten;
                currentBitByte = startPtr + bitByteOffset;
            }

            bytesWritten = newBytesWritten;
        }

        public void Write(bool val)
        {
            if (bitsWritten >= 8) // old byte is full
            {
                // write the old byte
                if (currentBitByte != IntPtr.Zero)
                {
                    Marshal.WriteByte(currentBitByte, bitByte);
                }

                // preparate the next byte
                bitByteOffset = bytesWritten;             //save offset for pointer adjustment after reallocating
                AddBytesAndRealloc(1);                    //realloc new byte for bits
                currentBitByte = currentByte;             //pointer to new byte
                currentByte += 1; //inc current byte pointer
                bitByte = 0;
                bitsWritten = 0;
            }

            if (val)
            {
                bitByte |= (byte)(1 << bitsWritten);
            }
            bitsWritten++;
        }

        public void Write(sbyte val)
        {
            AddBytesAndRealloc(1);
            Marshal.WriteByte(currentByte, (byte)val);
            currentByte += 1;
        }

        public void Write(byte val)
        {
            AddBytesAndRealloc(1);
            Marshal.WriteByte(currentByte, val);
            currentByte += 1;
        }

        public void Write(byte[] arr, int index, int count)
        {
            AddBytesAndRealloc(count);
            Marshal.Copy(arr, index, currentByte, count);
            currentByte += count;
        }

        public void Write(short val)
        {
            AddBytesAndRealloc(2);
            Marshal.WriteInt16(currentByte, val);
            currentByte += 2;
        }

        public void Write(ushort val)
        {
            AddBytesAndRealloc(2);
            Marshal.WriteInt16(currentByte, (short)val);
            currentByte += 2;
        }

        public void Write(int val)
        {
            AddBytesAndRealloc(4);
            Marshal.WriteInt32(currentByte, val);
            currentByte += 4;
        }

        public void Write(uint val)
        {
            AddBytesAndRealloc(4);
            Marshal.WriteInt32(currentByte, (int)val);
            currentByte += 4;
        }

        const int stringBufSize = 8192;
        byte[] bufArr = new byte[stringBufSize];
        char[] charArr = new char[stringBufSize];
        public void Write(string val)
        {
            int len = val.Length > 32767 ? 32767 : val.Length; // cut off everything > short.maxValue

            if (len > 127)
            {
                Write((short)-len);
            }
            else
            {
                Write((sbyte)len);
            }

            int rest;
            int charPos = 0;

            AddBytesAndRealloc(len);
            do
            {
                rest = len - charPos;
                if (rest > stringBufSize)
                {
                    rest = stringBufSize;
                }

                val.CopyTo(charPos, charArr, 0, rest);
                enc.GetBytes(charArr, 0, rest, bufArr, 0, false);
                Marshal.Copy(bufArr, 0, currentByte, rest);
                currentByte += rest;

                charPos += rest;
            } while (charPos < len);

            enc.Reset();
        }
    }
}
