using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Types
{
    public struct ColorRGBA
    {
        public readonly static ColorRGBA White = new ColorRGBA(255, 255, 255, 255);
        public readonly static ColorRGBA Black = new ColorRGBA(0, 0, 0, 255);
        public readonly static ColorRGBA Red = new ColorRGBA(255, 0, 0, 255);
        public readonly static ColorRGBA Green = new ColorRGBA(0, 255, 0, 255);
        public readonly static ColorRGBA Blue = new ColorRGBA(0, 0, 255, 255);
        public readonly static ColorRGBA Grey = new ColorRGBA(155, 155, 155, 255);
        public readonly static ColorRGBA Yellow = new ColorRGBA(255, 255, 0, 255);

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

        public static bool operator ==(ColorRGBA a, ColorRGBA b)
        {
            return a.R == b.R && a.G == b.G && a.B == b.B && a.A == b.A;
        }

        public static bool operator !=(ColorRGBA a, ColorRGBA b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is ColorRGBA)
            {
                return this == (ColorRGBA)obj;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (new byte[4] { this.R, this.G, this.B, this.A }).GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("ColorRGBA({0} / {1} / {2} / {3})", this.R, this.G, this.B, this.A);
        }
    }
}
