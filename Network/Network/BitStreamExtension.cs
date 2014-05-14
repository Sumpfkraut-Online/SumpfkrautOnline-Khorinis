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
        public static void Write(this BitStream stream, int[] pos)
        {
            for (int i = 0; i < pos.Length; i++)
                stream.Write(pos[i]);
        }

        public static void Write(this BitStream stream, float[] pos)
        {
            for (int i = 0; i < pos.Length; i++)
                stream.Write(pos[i]);
        }

        public static void Read(this BitStream stream, out int[] position, int length)
        {
            position = new int[length];
            for (int i = 0; i < position.Length; i++)
                stream.Read(out position[i]);
        }

        public static void Read(this BitStream stream, out float[] position, int length)
        {
            position = new float[length];
            for (int i = 0; i < position.Length; i++)
                stream.Read(out position[i]);
        }



        public static void Write(this BitStream stream, Vec2i position)
        {
            stream.Write(position.X);
            stream.Write(position.Y);
        }

        public static void Read(this BitStream stream, out Vec2i position)
        {
            int[] p = new int[2];
            stream.Read(out p[0]);
            stream.Read(out p[1]);

            position = (Vec2i)p;
        }

        public static void Write(this BitStream stream, ColorRGBA position)
        {
            stream.Write(position.R);
            stream.Write(position.G);
            stream.Write(position.B);
            stream.Write(position.A);
        }

        public static void Read(this BitStream stream, out ColorRGBA position)
        {
            byte[] p = new byte[4];
            stream.Read(out p[0]);
            stream.Read(out p[1]);
            stream.Read(out p[2]);
            stream.Read(out p[3]);

            position = (ColorRGBA)p;
        }

        public static void Write(this BitStream stream, Vec3f position)
        {
            stream.Write(position.X);
            stream.Write(position.Y);
            stream.Write(position.Z);
        }

        public static void Read(this BitStream stream, out Vec3f position)
        {
            float[] p = new float[3];
            stream.Read(out p[0]);
            stream.Read(out p[1]);
            stream.Read(out p[2]);

            position = (Vec3f)p;
        }

        public static void Write(this BitStream stream, ulong val)
        {
            byte[] arr = BitConverter.GetBytes(val);
            stream.Write(arr, 8);
        }
        public static void Read(this BitStream stream, out ulong val)
        {
            byte[] arr = new byte[8];
            stream.Read(arr, 8);

            val = BitConverter.ToUInt64(arr, 0);
        }

        public static void Write(this BitStream stream, ushort val)
        {
            byte[] arr = BitConverter.GetBytes(val);
            stream.Write(arr, 2);
        }

        public static void Read(this BitStream stream, out ushort val)
        {
            byte[] arr = new byte[2];
            stream.Read(arr, 2);

            val = BitConverter.ToUInt16(arr, 0);
        }
    }
}
