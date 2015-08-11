using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Types;

namespace GUC.Network
{
    public static class BitStreamExtension
    {
        static byte singleByte;
        static byte[] twoByte = new byte[2];
        static byte[] fourByte = new byte[4];
        static byte[] eightByte = new byte[8];
        static byte[] byteArr;

        public static void mWrite(this BitStream stream, bool val)
        {
            if (val) stream.Write1();
            else stream.Write0();
        }

        public static bool mReadBool(this BitStream stream)
        {
            return stream.ReadBit();
        }

        public static void mWrite(this BitStream stream, byte val)
        {
            stream.Write(val);
        }

        public static byte mReadByte(this BitStream stream)
        {
            stream.Read(out singleByte);
            return singleByte;
        }

        public static void mWrite(this BitStream stream, short val)
        {
            stream.Write(BitConverter.GetBytes(val), 2);
        }

        public static short mReadShort(this BitStream stream)
        {
            stream.Read(twoByte, 2);
            return BitConverter.ToInt16(twoByte, 0);
        }

        public static void mWrite(this BitStream stream, ushort val)
        {
            stream.Write(BitConverter.GetBytes(val), 2);
        }

        public static ushort mReadUShort(this BitStream stream)
        {
            stream.Read(twoByte, 2);
            return BitConverter.ToUInt16(twoByte, 0);
        }

        public static void mWrite(this BitStream stream, int val)
        {
            stream.Write(BitConverter.GetBytes(val), 4);
        }

        public static int mReadInt(this BitStream stream)
        {
            stream.Read(fourByte, 4);
            return BitConverter.ToInt32(fourByte, 0);
        }

        public static void mWrite(this BitStream stream, uint val)
        {
            stream.Write(BitConverter.GetBytes(val), 4);
        }

        public static uint mReadUInt(this BitStream stream)
        {
            stream.Read(fourByte, 4);
            return BitConverter.ToUInt32(fourByte, 0);
        }

        public static void mWrite(this BitStream stream, long val)
        {
            stream.Write(BitConverter.GetBytes(val), 8);
        }

        public static long mReadLong(this BitStream stream)
        {
            stream.Read(eightByte, 8);
            return BitConverter.ToInt64(eightByte, 0);
        }

        public static void mWrite(this BitStream stream, ulong val)
        {
            stream.Write(BitConverter.GetBytes(val), 8);
        }

        public static ulong mReadULong(this BitStream stream)
        {
            stream.Read(eightByte, 8);
            return BitConverter.ToUInt64(eightByte, 0);
        }

        public static void mWrite(this BitStream stream, float val)
        {
            stream.Write(BitConverter.GetBytes(val), 4);
        }

        public static float mReadFloat(this BitStream stream)
        {
            stream.Read(fourByte, 4);
            return BitConverter.ToSingle(fourByte, 0);
        }
        
        public static void mWrite(this BitStream stream, double val)
        {
            stream.Write(BitConverter.GetBytes(val), 8);
        }

        public static double mReadDouble(this BitStream stream)
        {
            stream.Read(eightByte, 8);
            return BitConverter.ToDouble(eightByte, 0);
        }

        public static void mWrite(this BitStream stream, string val)
        {
            byteArr = System.Text.Encoding.UTF8.GetBytes(val);
            stream.Write((byte)byteArr.Length);
            stream.Write(byteArr, (uint)byteArr.Length);
        }

        public static string mReadString(this BitStream stream)
        {
            byte len;
            stream.Read(out len);
            if (len > 0)
            {
                byteArr = new byte[len];
                stream.Read(byteArr, len);
                return System.Text.Encoding.UTF8.GetString(byteArr);
            }
            return string.Empty;
        }

        public static void mWriteStringLong(this BitStream stream, string val)
        {
            byteArr = System.Text.Encoding.UTF8.GetBytes(val);
            stream.Write(BitConverter.GetBytes((ushort)byteArr.Length), 2);
            stream.Write(byteArr, (uint)byteArr.Length);
        }

        public static string mReadStringLong(this BitStream stream)
        {
            ushort len = stream.mReadUShort();
            if (len > 0)
            {
                byteArr = new byte[len];
                stream.Read(byteArr, len);
                return System.Text.Encoding.UTF8.GetString(byteArr);
            }
            return "";
        }
        
        public static void mWrite(this BitStream stream, float[] pos)
        {
            for (int i = 0; i < pos.Length; i++)
                stream.Write(BitConverter.GetBytes(pos[i]), 4);
        }

        public static void mWrite(this BitStream stream, ColorRGBA position)
        {
            stream.Write(position.R);
            stream.Write(position.G);
            stream.Write(position.B);
            stream.Write(position.A);
        }

        public static ColorRGBA mReadColor(this BitStream stream)
        {
            byte[] p = new byte[4];
            stream.Read(out p[0]);
            stream.Read(out p[1]);
            stream.Read(out p[2]);
            stream.Read(out p[3]);

            return (ColorRGBA)p;
        }

        public static void mWrite(this BitStream stream, Vec3f position)
        {
            stream.mWrite(position.X);
            stream.mWrite(position.Y);
            stream.mWrite(position.Z);
        }

        public static Vec3f mReadVec(this BitStream stream)
        {
            return new Vec3f(stream.mReadFloat(), stream.mReadFloat(), stream.mReadFloat());
        }

        public static void mWrite(this BitStream stream, sbyte s)
        {
            stream.Write(Convert.ToByte(s));
        }

        public static sbyte mReadSByte(this BitStream stream)
        {
            stream.Read(out singleByte);
            return (sbyte)singleByte;
        }
    }
}
