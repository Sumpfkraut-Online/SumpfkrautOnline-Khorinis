using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Network
{
    public class PacketReader
    {
        int currentByte;
        int bitsRead;
        int bitByte;
        
        Decoder dec;

        byte[] data;
        int length;

        internal PacketReader()
        {
            dec = Encoding.UTF8.GetDecoder();
        }

        internal void Load(byte[] data)
        {
            this.data = data;
            currentByte = 0;
            bitsRead = 8;
            bitByte = 0;
        }

        #region Reading Methods

        public void Read(out bool val)
        {
            if (bitsRead == 8)
            {
                bitByte = data[currentByte++];
                bitsRead = 0;
            }

            val = (bitByte & (1 << bitsRead)) != 0;
            bitsRead++;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)data[currentByte++];
        }

        public byte ReadByte()
        {
            return data[currentByte++];
        }

        public short ReadShort()
        {
            return (short)(((int)data[currentByte++]) | (((int)data[currentByte++]) << 8));
        }

        public ushort ReadUShort()
        {
            return (ushort)(((int)data[currentByte++]) | (((int)data[currentByte++]) << 8));
        }

        public int ReadInt()
        {
            return ((int)data[currentByte++]) | (((int)data[currentByte++]) << 8) | (((int)data[currentByte++]) << 16) | (((int)data[currentByte++]) << 24);
        }

        public uint ReadUInt()
        {
            return (uint)(((int)data[currentByte++]) | (((int)data[currentByte++]) << 8) | (((int)data[currentByte++]) << 16) | (((int)data[currentByte++]) << 24));
        }

        public long ReadLong()
        {
            return ((long)data[currentByte++]) | (((long)data[currentByte++]) << 8) | (((long)data[currentByte++]) << 16) | (((long)data[currentByte++]) << 24) | (((long)data[currentByte++]) << 32) | (((long)data[currentByte++]) << 40) | (((long)data[currentByte++]) << 48) | (((long)data[currentByte++]) << 56);
        }

        public ulong ReadULong()
        {
            return (ulong)(((long)data[currentByte++]) | (((long)data[currentByte++]) << 8) | (((long)data[currentByte++]) << 16) | (((long)data[currentByte++]) << 24) | (((long)data[currentByte++]) << 32) | (((long)data[currentByte++]) << 40) | (((long)data[currentByte++]) << 48) | (((long)data[currentByte++]) << 56));
        }

        public float ReadFloat()
        {
            float val = BitConverter.ToSingle(data, currentByte);
            currentByte += 4;
            return val;
        }

        public double ReadDouble()
        {
            double val = BitConverter.ToDouble(data, currentByte);
            currentByte += 8;
            return val;
        }

        public void Read(byte[] arr, int index, int length)
        {
            Buffer.BlockCopy(data, currentByte, arr, index, length);
            currentByte += length;
        }

        const int MaxStringLength = short.MaxValue;
        char[] charArr = new char[MaxStringLength];
        public string ReadString()
        {
            int len = (sbyte)data[currentByte++];
            if (len < 0)
            {
                len = -(len | data[currentByte++] << 8);
            }

            dec.GetChars(data, currentByte, len, charArr, 0);
            currentByte += len;

            return new string(charArr, 0, len);
        }

        #endregion
    }
}
