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
        public int Length { get { return this.length; } }

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

        /*internal void Decompress()
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
        }*/

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

        public byte[] ReadBytes(int count)
        {
            byte[] ret = new byte[count];
            Read(ret, 0, count);
            return ret;
        }

        // Taken from http://referencesource.microsoft.com/#mscorlib/system/io/binaryreader.cs,f30b8b6e8ca06e0f
        int Read7BitEncodedInt()
        {
            // Read out an Int32 7 bits at a time.  The high bit
            // of the byte when on means to continue reading more bytes.
            int count = 0;
            int shift = 0;
            byte b;
            do
            {
                // Check for a corrupted stream.  Read a max of 5 bytes.
                // In a future version, add a DataFormatException.
                if (shift == 5 * 7)  // 5 bytes max per Int32, shift += 7
                    throw new FormatException("Format_Bad7BitInt32");

                // ReadByte handles end of stream cases for us.
                b = ReadByte();
                count |= (b & 0x7F) << shift;
                shift += 7;
            } while ((b & 0x80) != 0);
            return count;
        }

        //const int MaxStringLength = short.MaxValue;
        //char[] charArr = new char[MaxStringLength];
        public string ReadString()
        {
            int byteLen = Read7BitEncodedInt();

            // FIXME
            var chars = Encoding.UTF8.GetChars(data, currentByte, byteLen);
            currentByte += byteLen;
            return new string(chars);
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
            return new ColorRGBA
            {
                R = data[currentByte++],
                G = data[currentByte++],
                B = data[currentByte++],
                A = data[currentByte++]
            };
        }

        public Vec3f ReadCompressedPosition()
        {
            return new Vec3f
            {
                X = ReadInt24() / 10.0f,
                Y = ReadInt24() / 10.0f,
                Z = ReadInt24() / 10.0f
            };
        }

        int ReadInt24()
        {
            int val = ((int)data[currentByte++]) | (((int)data[currentByte++]) << 8) | (((int)data[currentByte++]) << 16);
            return (val & 0x800000) != 0 ? val | (0xFF << 24) : val;
        }

        public Vec3f ReadCompressedDirection()
        {
            return new Vec3f
            {
                X = ReadSByte() / 127.0f,
                Y = ReadSByte() / 127.0f,
                Z = ReadSByte() / 127.0f
            };
        }

        public Angles ReadAngles()
        {
            Angles angles = new Angles();
            angles.Pitch = BitConverter.ToSingle(data, currentByte);
            currentByte += 4;
            angles.Yaw = BitConverter.ToSingle(data, currentByte);
            currentByte += 4;
            angles.Roll = BitConverter.ToSingle(data, currentByte);
            currentByte += 4;
            return angles;
        }

        public float ReadCompressedAngle()
        {
            return Angles.Short2Angle(ReadShort());
        }

        public Angles ReadCompressedAngles()
        {
            return new Angles()
            {
                Pitch = Angles.Short2Angle(ReadShort()),
                Yaw = Angles.Short2Angle(ReadShort()),
                Roll = Angles.Short2Angle(ReadShort()),
            };
        }

        #endregion
    }
}
