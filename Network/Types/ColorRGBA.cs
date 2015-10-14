using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public class ColorRGBA
    {
        protected byte[] data = new byte[4];

        public ColorRGBA()
        {
            set(255,255,255,255);
        }
        public ColorRGBA(byte[] data)
        {
            set(data);
        }
        public ColorRGBA(byte r, byte g, byte b, byte a)
        {
            set(r, g, b, a);
        }
        public ColorRGBA(byte r, byte g, byte b):this(r,g,b,255)
        {
          
        }
        public void set(ColorRGBA color)
        {
            set(color.R, color.G, color.B, color.A);
        }

        public void set(byte r, byte g, byte b, byte a)
        {
            data[0] = r;
            data[1] = g;
            data[2] = b;
            data[3] = a;
        }

        public void set(byte[] data)
        {
            if (data.Length != 4)
                throw new ArgumentException("The Data-Array needs a length of 4!");
            set(data[0], data[1], data[2], data[3]);
        }

        public byte[] Data { get { return this.data; } }

        public static explicit operator ColorRGBA(byte[] data)
        {
            return new ColorRGBA(data);
        }

        public byte R { get { return data[0]; } set { data[0] = value; } }
        public byte G { get { return data[1]; } set { data[1] = value; } }
        public byte B { get { return data[2]; } set { data[2] = value; } }
        public byte A { get { return data[3]; } set { data[3] = value; } }

        public static ColorRGBA White { get { return new ColorRGBA(255, 255, 255, 255); } }
        public static ColorRGBA Black { get { return new ColorRGBA(0, 0, 0, 255); } }
        public static ColorRGBA Red { get { return new ColorRGBA(255, 0, 0, 255); } }
        public static ColorRGBA Green { get { return new ColorRGBA(0, 255, 0, 255); } }
        public static ColorRGBA Blue { get { return new ColorRGBA(0, 0, 255, 255); } }
        public static ColorRGBA Grey { get { return new ColorRGBA(155, 155, 155, 255); } }
    }
}
