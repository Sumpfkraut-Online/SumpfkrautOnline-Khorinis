using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace GUC.Server.Network
{
    class OldStream : IDisposable
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

        public OldStream()
            : this(65536)
        {
        }

        public OldStream(int capacity)
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
                currentByte = IntPtr.Add(startPtr, bytesWritten);
                currentBitByte = IntPtr.Add(startPtr, bitByteOffset);
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
                currentByte = IntPtr.Add(currentByte, 1); //inc current byte pointer
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
            currentByte = IntPtr.Add(currentByte, 1);
        }

        public void Write(byte val)
        {
            AddBytesAndRealloc(1);
            Marshal.WriteByte(currentByte, val);
            currentByte = IntPtr.Add(currentByte, 1);
        }

        public void Write(byte[] arr, int index, int count)
        {
            AddBytesAndRealloc(count);
            Marshal.Copy(arr, index, currentByte, count);
            currentByte = IntPtr.Add(currentByte, count);
        }

        public void Write(short val)
        {
            AddBytesAndRealloc(2);
            Marshal.WriteInt16(currentByte, val);
            currentByte = IntPtr.Add(currentByte, 2);
        }

        public void Write(ushort val)
        {
            AddBytesAndRealloc(2);
            Marshal.WriteInt16(currentByte, (short)val);
            currentByte = IntPtr.Add(currentByte, 2);
        }

        public void Write(int val)
        {
            AddBytesAndRealloc(4);
            Marshal.WriteInt32(currentByte, val);
            currentByte = IntPtr.Add(currentByte, 4);
        }

        public void Write(uint val)
        {
            AddBytesAndRealloc(4);
            Marshal.WriteInt32(currentByte, (int)val);
            currentByte = IntPtr.Add(currentByte, 4);
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
                currentByte = IntPtr.Add(currentByte, rest);

                charPos += rest;
            } while (charPos < len);

            enc.Reset();
        }
    }

    /*class OldStream : IDisposable
    {
        MemoryStream ms;
        Encoder enc;

        const int bufSize = 1024;
        byte[] byteArr;
        char[] charArr;
        int currentPos;
        int currentBit;

        public OldStream()
        {
            ms = new MemoryStream();
            byteArr = new byte[bufSize];
            charArr = new char[bufSize];
            currentPos = 0;
            currentBit = 0;

            enc = Encoding.UTF8.GetEncoder();
        }

        public void Dispose()
        {
            ms.Dispose();
        }

        public void Reset()
        {
            ms.Position = 0;
            ms.SetLength(0);
            currentBit = 0;
            currentPos = 0;
        }

        public byte[] GetData() // Attention: Don't continue writing after calling this method
        {
            if (currentBit > 0)
            {
                currentPos++; //write the last bits too
            }
            ms.Write(byteArr, 0, currentPos);
            currentPos = 0;
            currentBit = 0;
            return ms.ToArray();
        }

        public int GetLength()
        {
            return (int)ms.Length;
        }

        public void Write(bool val)
        {
            if (val)
            {
                byteArr[currentPos] |= (byte)(1 << (7 - currentBit));
            }
            else
            {
                byteArr[currentPos] &= (byte)~(1 << (7 - currentBit));
            }

            currentBit++;
            if (currentBit >= 8)
            {
                currentBit = 0;
                currentPos++;
            }
            if (currentPos >= bufSize)
            {
                ms.Write(byteArr, 0, bufSize);
                currentPos = 0;
            }
        }

        public void Write(byte val)
        {
            if (currentBit > 0)
            {
                int shiftBits = 8 - currentBit;

                byteArr[currentPos] = (byte)((byteArr[currentPos] >> shiftBits) << shiftBits);
                byteArr[currentPos] |= (byte)(val >> currentBit);
                currentPos++;

                if (currentPos >= bufSize)
                {
                    ms.Write(byteArr, 0, bufSize);
                    currentPos = 0;
                }

                byteArr[currentPos] = (byte)(val << shiftBits);
            }
            else
            {
                byteArr[currentPos] = val;

                currentPos++;
                if (currentPos >= bufSize)
                {
                    ms.Write(byteArr, 0, bufSize);
                    currentPos = 0;
                }
            }
        }

        public void Write(sbyte val)
        {
            Write((byte)val);
        }

        public void Write(short val)
        {
            Write((byte)(val >> 8));
            Write((byte)(val & 0xFF));
        }

        public void Write(ushort val)
        {
            Write((byte)(val >> 8));
            Write((byte)(val & 0xFF));
        }

        public void Write(int val)
        {
            Write((byte)(val >> 24));
            Write((byte)((val >> 16) & 0xFF));
            Write((byte)((val >> 8) & 0xFF));
            Write((byte)(val & 0xFF));
        }

        public void Write(uint val)
        {
            Write((byte)(val >> 24));
            Write((byte)((val >> 16) & 0xFF));
            Write((byte)((val >> 8) & 0xFF));
            Write((byte)(val & 0xFF));
        }
        
        public void Write(byte[] arr, int index, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Write(arr[index + i]);
            }
        }

        byte[] bufArr = new byte[bufSize];
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

            do
            {
                rest = len - charPos;
                if (rest > bufSize)
                {
                    rest = bufSize;
                }

                val.CopyTo(charPos, charArr, 0, rest);
                enc.GetBytes(charArr, 0, rest, bufArr, 0, false);
                Write(bufArr, 0, rest);

                charPos += rest;
            } while (charPos < len);

            enc.Reset();
        }
    }*/
}
