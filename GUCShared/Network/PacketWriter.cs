using System;
using System.Text;
using GUC.Types;
using System.IO;
using System.IO.Compression;

namespace GUC.Network
{
    public class PacketWriter
    {
        const int StandardCapacity = 32000;
        
        int pos;
        public int Position
        {
            get { return this.pos; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException("Position can't be smaller than zero!");

                CheckRealloc(value - pos);
                pos = value;
            }
        }

        int currentBitByte;
        int bitsWritten;
        int bitByte;

        Encoder enc;
        byte[] data;
        int capacity;

        /*//saved data when in compress mode
        int SCurrentByte;
        int SCurrentBitByte;
        int SBitsWritten;
        int SBitByte;*/

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
            Position = 0;
            currentBitByte = -1;

            bitsWritten = 0;
            bitByte = 0;
        }

        internal byte[] GetData()
        {
            //StopCompressing();
            FlushBits();
            return data;
        }

        public byte[] CopyData()
        {
            byte[] bytes = GetData();
            byte[] ret = new byte[Position];
            Array.Copy(bytes, ret, Position);
            return ret;
        }

        internal int GetLength()
        {
            return Position;
        }

        void CheckRealloc(int add)
        {
            int neededLen = Position + add;
            if (neededLen >= capacity)
            {
                capacity = StandardCapacity * (int)((float)neededLen / (float)StandardCapacity + 1.0f);
                byte[] newData = new byte[capacity];
                Buffer.BlockCopy(data, 0, newData, 0, Position);
                data = newData;
            }
        }

        #region Compressing
        /*
        bool compress = false;
        internal void StartCompressing()
        {
            if (!compress)
            {
                //save data and reset;
                SCurrentByte = CurrentByte;
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
                    int uncompressedLen = CurrentByte - SCurrentByte;

                    using (DeflateStream ds = new DeflateStream(ms, CompressionMode.Compress, true))
                    {
                        ds.Write(data, SCurrentByte, uncompressedLen);
                    }
                    int compressedLen = (int)ms.Length;

                    //switch current byte back
                    CurrentByte = SCurrentByte;

                    //write lengths
                    Write(uncompressedLen);
                    Write(compressedLen);

                    //read compressed
                    ms.Position = 0;
                    ms.Read(data, CurrentByte, compressedLen);

                    CurrentByte += compressedLen;

                    //switch rest back too
                    currentBitByte = SCurrentBitByte;
                    bitsWritten = SBitsWritten;
                    bitByte = SBitByte;
                }

                compress = false;
            }
        }
        */
        #endregion

        #region Writing Methods

        public bool Write(bool val)
        {
            if (currentBitByte == -1)
            {
                //CheckRealloc(1); // except when the capacity is 0 this is useless
                currentBitByte = Position++;
            }

            if (bitsWritten == 8) // old byte is full
            {
                CheckRealloc(1);
                data[currentBitByte] = (byte)bitByte;
                currentBitByte = Position++;
                bitsWritten = 0;
                bitByte = 0;
            }

            if (val)
            {
                bitByte |= (1 << bitsWritten);
            }
            bitsWritten++;
            return val;
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
            data[Position++] = (byte)val;
        }

        public void Write(byte val)
        {
            CheckRealloc(1);
            data[Position++] = val;
        }

        public void Edit(int position, byte val)
        {
            if (position < 0)
                throw new ArgumentOutOfRangeException("Position can't be smaller than zero!");

            int oldPos = this.pos;
            this.pos = position;
            Write(val);
            this.pos = oldPos;
        }

        public void Write(short val)
        {
            CheckRealloc(2);
            data[Position++] = (byte)val;
            data[Position++] = (byte)(val >> 8);
        }

        public void Write(ushort val)
        {
            CheckRealloc(2);
            data[Position++] = (byte)val;
            data[Position++] = (byte)(val >> 8);
        }

        public void Write(int val)
        {
            CheckRealloc(4);
            data[Position++] = (byte)val;
            data[Position++] = (byte)(val >> 8);
            data[Position++] = (byte)(val >> 16);
            data[Position++] = (byte)(val >> 24);
        }

        public void Write(uint val)
        {
            CheckRealloc(4);
            data[Position++] = (byte)val;
            data[Position++] = (byte)(val >> 8);
            data[Position++] = (byte)(val >> 16);
            data[Position++] = (byte)(val >> 24);
        }

        public void Write(long val)
        {
            CheckRealloc(8);
            data[Position++] = (byte)val;
            data[Position++] = (byte)(val >> 8);
            data[Position++] = (byte)(val >> 16);
            data[Position++] = (byte)(val >> 24);
            data[Position++] = (byte)(val >> 32);
            data[Position++] = (byte)(val >> 40);
            data[Position++] = (byte)(val >> 48);
            data[Position++] = (byte)(val >> 56);
        }

        public void Write(ulong val)
        {
            CheckRealloc(8);
            data[Position++] = (byte)val;
            data[Position++] = (byte)(val >> 8);
            data[Position++] = (byte)(val >> 16);
            data[Position++] = (byte)(val >> 24);
            data[Position++] = (byte)(val >> 32);
            data[Position++] = (byte)(val >> 40);
            data[Position++] = (byte)(val >> 48);
            data[Position++] = (byte)(val >> 56);
        }

        public void Write(float val)
        {
            CheckRealloc(4);
            byte[] arr = BitConverter.GetBytes(val);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];
        }

        public void Write(double val)
        {
            CheckRealloc(8);
            byte[] arr = BitConverter.GetBytes(val);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];
            data[Position++] = arr[4];
            data[Position++] = arr[5];
            data[Position++] = arr[6];
            data[Position++] = arr[7];
        }

        public void Write(byte[] arr, int startIndex, int length)
        {
            CheckRealloc(length);
            Buffer.BlockCopy(arr, startIndex, data, Position, length);
            Position += length;
        }

        // Taken from http://referencesource.microsoft.com/#mscorlib/system/io/binarywriter.cs,2daa1d14ff1877bd
        void Write7BitEncodedInt(int value)
        {
            // Write out an int 7 bits at a time.  The high bit of the byte,
            // when on, tells reader to continue reading more bytes.
            uint v = (uint)value;   // support negative numbers
            while (v >= 0x80)
            {
                Write((byte)(v | 0x80));
                v >>= 7;
            }
            Write((byte)v);
        }

        //const int bufLen = 4096;
        //char[] charArr = new char[bufLen];
        //byte[] byteArr = new byte[2 * bufLen];
        public void Write(string val)
        {
            if (val == null)
            {
                throw new ArgumentNullException("String is null!");
            }

            // FIXME
            var bytes = Encoding.UTF8.GetBytes(val);
            Write7BitEncodedInt(bytes.Length);
            Write(bytes, 0, bytes.Length);
        }

        public void Write(Vec3f vec)
        {
            CheckRealloc(12);
            byte[] arr = BitConverter.GetBytes(vec.X);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];

            arr = BitConverter.GetBytes(vec.Y);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];

            arr = BitConverter.GetBytes(vec.Z);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];
        }

        public void Write(ColorRGBA color)
        {
            CheckRealloc(4);
            data[Position++] = color.R;
            data[Position++] = color.G;
            data[Position++] = color.B;
            data[Position++] = color.A;
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
            CheckRealloc(9);
            WriteInt24((int)(position.X * 10));
            WriteInt24((int)(position.Y * 10));
            WriteInt24((int)(position.Z * 10));
        }

        void WriteInt24(int val)
        {
            data[Position++] = (byte)val;
            data[Position++] = (byte)(val >> 8);
            data[Position++] = (byte)(val >> 16);
        }

        /// <summary>
        /// Coordinates must be normalised (between -1 and +1) !!!
        /// </summary>
        public void WriteCompressedDirection(Vec3f direction)
        {
            CheckRealloc(3);
            data[Position++] = (byte)(direction.X * 127.0f);
            data[Position++] = (byte)(direction.Y * 127.0f);
            data[Position++] = (byte)(direction.Z * 127.0f);
        }


        public void Write(Angles angles)
        {
            CheckRealloc(12);
            byte[] arr = BitConverter.GetBytes(angles.Pitch);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];

            arr = BitConverter.GetBytes(angles.Yaw);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];

            arr = BitConverter.GetBytes(angles.Roll);
            data[Position++] = arr[0];
            data[Position++] = arr[1];
            data[Position++] = arr[2];
            data[Position++] = arr[3];
        }

        /// <summary> Angle must be [-pi, +pi]. Writes 2 Bytes. </summary>
        public void WriteCompressedAngle(float radians)
        {
            Write(Angles.Angle2Short(radians));
        }

        public void WriteCompressedAngles(Angles angles)
        {
            CheckRealloc(6);
            byte[] arr = BitConverter.GetBytes(Angles.Angle2Short(angles.Pitch));
            data[Position++] = arr[0];
            data[Position++] = arr[1];

            arr = BitConverter.GetBytes(Angles.Angle2Short(angles.Yaw));
            data[Position++] = arr[0];
            data[Position++] = arr[1];

            arr = BitConverter.GetBytes(Angles.Angle2Short(angles.Roll));
            data[Position++] = arr[0];
            data[Position++] = arr[1];
        }

        #endregion
    }
}
