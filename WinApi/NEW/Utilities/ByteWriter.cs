using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    public unsafe class ByteWriter
    {
        byte[] buffer;
        int capacity;
        public int Capacity { get { return this.capacity; } }

        int pos;
        public int Position { get { return this.pos; } }
        public int Length { get { return this.pos; } }

        public ByteWriter(int capacity = 0)
        {
            this.capacity = capacity;
            buffer = new byte[capacity];
        }

        public ByteWriter(byte[] buffer)
        {
            this.buffer = buffer;
            this.capacity = buffer.Length;
        }

        public void Reset()
        {
            pos = 0;
        }

        void EnsureCapacity(int add)
        {
            int neededLen = pos + add;
            if (neededLen > capacity)
            {
                capacity = 3 * capacity / 2 + add;
                byte[] newBuffer = new byte[capacity];
                Buffer.BlockCopy(buffer, 0, newBuffer, 0, pos);
                buffer = newBuffer;
            }
        }

        public void WriteByte(byte value)
        {
            EnsureCapacity(1);
            fixed (byte* ptr = buffer)
            {
                *(ptr + pos) = value;
            }
            pos += 1;
        }

        public void WriteInt(int value)
        {
            EnsureCapacity(4);
            fixed (byte* ptr = buffer)
            {
                *(int*)(ptr + pos) = value;
            }
            pos += 4;
        }

        public void WriteBytes(params byte[] bytes)
        {
            WriteBytes(bytes, bytes.Length);
        }

        public void WriteBytes(byte[] inputBuffer, int count)
        {
            if (count == 0) return;
            EnsureCapacity(count);
            Buffer.BlockCopy(inputBuffer, 0, this.buffer, pos, count);
            pos += count;
        }

        public byte[] GetBuffer()
        {
            return this.buffer;
        }

        public byte[] CopyData()
        {
            byte[] result = new byte[pos];
            Buffer.BlockCopy(buffer, 0, result, 0, pos);
            return result;
        }

    }
}
