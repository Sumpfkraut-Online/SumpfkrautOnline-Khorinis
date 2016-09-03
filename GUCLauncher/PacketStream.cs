using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GUCLauncher
{
    class PacketStream : IDisposable
    {
        const int ReadBufSize = 2048;

        Stream stream;
        Encoder enc;
        Decoder dec;
        int length;
        Action<float> SetPercent;

        int readBytes;
        int lastUpdate;
        void IncreaseReadBytes(int value)
        {
            readBytes += value;
            if ((readBytes - lastUpdate) / ReadBufSize > 0)
            {
                SetPercent((float)readBytes / length);
                lastUpdate = readBytes;
            }
        }

        public PacketStream(Stream stream, int length = -1, Action<float> SetPercent = null)
        {
            if (stream == null)
                throw new ArgumentNullException("Stream is null!");

            this.stream = stream;
            this.enc = Encoding.UTF8.GetEncoder();
            this.dec = Encoding.UTF8.GetDecoder();
            this.length = length;
            this.SetPercent = SetPercent;
        }

        public void Dispose()
        {
            charBuf = null;
            byteBuf = null;
            shortBuf = null;
            intBuf = null;
            enc = null;
            dec = null;
            SetPercent = null;
            stream.Dispose();
        }

        char[] charBuf = new char[ushort.MaxValue];
        byte[] byteBuf = new byte[ushort.MaxValue];

        public void WriteStringShort(string str)
        {
            int charLen = str.Length > byte.MaxValue ? byte.MaxValue : str.Length;
            str.CopyTo(0, charBuf, 0, charLen);

            int charsUsed, bytesUsed; bool completed;
            enc.Convert(charBuf, 0, charLen, byteBuf, 0, byte.MaxValue, true, out charsUsed, out bytesUsed, out completed);

            stream.WriteByte((byte)bytesUsed);
            stream.Write(byteBuf, 0, bytesUsed);
        }

        public string ReadStringShort()
        {
            int len = stream.ReadByte();
            stream.Read(byteBuf, 0, len);
            IncreaseReadBytes(len + 1);

            int charsUsed, bytesUsed; bool completed;
            dec.Convert(byteBuf, 0, len, charBuf, 0, byte.MaxValue, true, out bytesUsed, out charsUsed, out completed);

            return new string(charBuf, 0, charsUsed);
        }

        public void WriteStringLong(string str)
        {
            int charLen = str.Length > ushort.MaxValue ? ushort.MaxValue : str.Length;
            str.CopyTo(0, charBuf, 0, charLen);

            int charsUsed, bytesUsed; bool completed;
            enc.Convert(charBuf, 0, charLen, byteBuf, 0, ushort.MaxValue, true, out charsUsed, out bytesUsed, out completed);

            WriteUShort(bytesUsed);
            stream.Write(byteBuf, 0, bytesUsed);
        }

        public string ReadStringLong()
        {
            int len = ReadUShort();
            IncreaseReadBytes(2);
            Read(byteBuf, 0, len);

            int charsUsed, bytesUsed; bool completed;
            dec.Convert(byteBuf, 0, len, charBuf, 0, ushort.MaxValue, true, out bytesUsed, out charsUsed, out completed);
            return new string(charBuf, 0, charsUsed);
        }

        public void WriteByte(int value)
        {
            stream.WriteByte((byte)value);
        }

        public int ReadByte()
        {
            IncreaseReadBytes(1);
            return stream.ReadByte();
        }

        byte[] shortBuf = new byte[2];
        public void WriteUShort(int value)
        {
            shortBuf[0] = (byte)value;
            shortBuf[1] = (byte)(value >> 8);
            stream.Write(shortBuf, 0, 2);
        }

        public int ReadUShort()
        {
            IncreaseReadBytes(2);
            stream.Read(shortBuf, 0, 2);
            return shortBuf[0] | (shortBuf[1] << 8);
        }

        byte[] intBuf = new byte[4];
        public void Write(int value)
        {
            intBuf[0] = (byte)value;
            intBuf[1] = (byte)(value >> 8);
            intBuf[2] = (byte)(value >> 16);
            intBuf[3] = (byte)(value >> 24);
            stream.Write(intBuf, 0, 4);
        }

        public int ReadInt()
        {
            IncreaseReadBytes(4);
            stream.Read(intBuf, 0, 4);
            return intBuf[0] | (intBuf[1] << 8) | (intBuf[2] << 16) | (intBuf[3] << 24);
        }

        public void WriteBytes(byte[] buf)
        {
            Write(buf, 0, buf.Length);
        }

        public byte[] ReadBytes(int count)
        {
            byte[] buf = new byte[count];
            Read(buf, 0, count);
            return buf;
        }

        public void Write(byte[] buf, int offset, int count)
        {
            stream.Write(buf, offset, count);
        }

        public void Read(byte[] buf, int offset, int count)
        {
            int index = 0;
            int left = count;
            while ((left = count - index) > 0)
            {
                int read = stream.Read(buf, index, left > ReadBufSize ? ReadBufSize : left);
                index += read;
                IncreaseReadBytes(read);
            }
        }
    }
}
