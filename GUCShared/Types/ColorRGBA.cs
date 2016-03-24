using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public struct ColorRGBA
    {
        public byte R;
        public byte G;
        public byte B;
        public byte A;

        public ColorRGBA(byte[] data)
        {
            if (data != null)
            {
                if (data.Length >= 4)
                {
                    this.R = data[0];
                    this.G = data[1];
                    this.B = data[2];
                    this.A = data[3];
                    return;
                }
                else if (data.Length == 3)
                {
                    this.R = data[0];
                    this.G = data[1];
                    this.B = data[2];
                    this.A = 255;
                    return;
                }
                else if (data.Length == 2)
                {
                    this.R = data[0];
                    this.G = data[1];
                    this.B = 255;
                    this.A = 255;
                    return;
                }
                else if (data.Length == 1)
                {
                    this.R = data[0];
                    this.G = 255;
                    this.B = 255;
                    this.A = 255;
                    return;
                }
            }
            this.R = 255;
            this.G = 255;
            this.B = 255;
            this.A = 255;
        }
        public byte this[int i]
        {
            get
            {
                if (i == 0) return R;
                else if (i == 1) return G;
                else if (i == 2) return B;
                else if (i == 3) return A;
                else throw new ArgumentOutOfRangeException("ColorRGBA index is out of range (0..3) <> " + i);
            }
            set
            {
                if (i == 0) R = value;
                else if (i == 1) G = value;
                else if (i == 2) B = value;
                else if (i == 3) A = value;
                else throw new ArgumentOutOfRangeException("ColorRGBA index is out of range (0..3) <> " + i);
            }
        }

        public ColorRGBA(byte r, byte g, byte b)
            : this(r, g, b, 255)
        {
        }

        public ColorRGBA(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public void Set(byte r, byte g, byte b, byte a)
        {
            this.R = r;
            this.G = g;
            this.B = b;
            this.A = a;
        }

        public static explicit operator ColorRGBA(byte[] data)
        {
            return new ColorRGBA(data);
        }

        public static ColorRGBA White { get { return new ColorRGBA(255, 255, 255, 255); } }
        public static ColorRGBA Black { get { return new ColorRGBA(0, 0, 0, 255); } }
        public static ColorRGBA Red { get { return new ColorRGBA(255, 0, 0, 255); } }
        public static ColorRGBA Green { get { return new ColorRGBA(0, 255, 0, 255); } }
        public static ColorRGBA Blue { get { return new ColorRGBA(0, 0, 255, 255); } }
        public static ColorRGBA Grey { get { return new ColorRGBA(155, 155, 155, 255); } }
    }
}
