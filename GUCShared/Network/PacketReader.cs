using System;
using System.Text;
using GUC.Types;
using System.IO;
using System.IO.Compression;

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

        internal void Load(byte[] data, int length)
        {
            this.length = length;
            this.data = data;
            currentByte = 0;
            bitsRead = 8;
            bitByte = 0;
        }

        internal byte[] GetRemainingData()
        {
            byte[] arr = new byte[this.length - currentByte];
            Array.Copy(data, currentByte, arr, 0, arr.Length);
            return arr;
        }

        #region Decompressing

        internal void Decompress()
        {
            int uncompressedLen = ReadInt();
            int compressedLen = ReadInt();

            int newLen = length - compressedLen + uncompressedLen;
            byte[] newData = new byte[newLen];
            Buffer.BlockCopy(data, 0, newData, 0, currentByte);

            using (MemoryStream ms = new MemoryStream(compressedLen))
            {
                ms.Write(data, currentByte, compressedLen);
                ms.Position = 0;

                using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Decompress))
                {
                    int read = ds.Read(newData, currentByte, uncompressedLen);
                }
            }
            
            Buffer.BlockCopy(data, currentByte + compressedLen, newData, currentByte + uncompressedLen, length - currentByte - compressedLen);

            data = newData;
            length = newLen;
        }

        #endregion

        #region Reading Methods

        public bool ReadBit()
        {
            if (bitsRead == 8)
            {
                bitByte = data[currentByte++];
                bitsRead = 0;
            }

            return (bitByte & (1 << bitsRead++)) != 0;
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

        public void Read(byte[] arr, int startIndex, int length)
        {
            Buffer.BlockCopy(data, currentByte, arr, startIndex, length);
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

        public Vec3f ReadVec3f()
        {
            Vec3f vec = new Vec3f();
            vec.X = BitConverter.ToSingle(data, currentByte);
            currentByte += 4;
            vec.Y = BitConverter.ToSingle(data, currentByte);
            currentByte += 4;
            vec.Z = BitConverter.ToSingle(data, currentByte);
            currentByte += 4;
            return vec;
        }

        public ColorRGBA ReadColorRGBA()
        {
            ColorRGBA color = new ColorRGBA();
            color.R = data[currentByte++];
            color.G = data[currentByte++];
            color.B = data[currentByte++];
            color.A = data[currentByte++];
            return color;
        }

        public Vec3f ReadCompressedPosition()
        {
            Vec3f vec = new Vec3f();
            vec.X = ReadInt24() / 10.0f;
            vec.Y = ReadInt24() / 10.0f;
            vec.Z = ReadInt24() / 10.0f;
            return vec;
        }

        int ReadInt24()
        {
            int val = ((int)data[currentByte++]) | (((int)data[currentByte++]) << 8) | (((int)data[currentByte++]) << 16);
            return (val & 0x800000) != 0 ? val | (0xFF << 24) : val;
        }

        public Vec3f ReadCompressedDirection()
        {
            Vec3f vec = new Vec3f();
            vec.X = ReadSByte() / 127.0f;
            vec.Y = ReadSByte() / 127.0f;
            vec.Z = ReadSByte() / 127.0f;
            return vec;
        }

        #endregion
    }
}
