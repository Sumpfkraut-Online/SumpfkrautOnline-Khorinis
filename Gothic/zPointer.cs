using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gothic
{
    public struct zPointer
    {
        public static readonly zPointer Null = new zPointer(0);

        int address;
        public int Address { get { return this.address; } }
        public bool IsNull { get { return this.address == 0; } }

        public zPointer(int address)
        {
            this.address = address;
        }

        #region Equality

        public bool Equals(zPointer other)
        {
            return other.Address == this.Address;
        }

        public override bool Equals(object obj)
        {
            return obj is zPointer ? ((zPointer)obj).Address == this.Address : false;
        }

        public static bool operator ==(zPointer a, zPointer b)
        {
            return a.Address == b.Address;
        }

        public static bool operator !=(zPointer a, zPointer b)
        {
            return a.Address != b.Address;
        }

        #endregion

        #region Conversion

        public static implicit operator int(zPointer ptr)
        {
            return ptr.Address;
        }

        public static implicit operator zPointer(int address)
        {
            return new zPointer(address);
        }

        public static implicit operator bool(zPointer ptr)
        {
            return ptr.Address != 0;
        }

        #endregion

        #region HashCode & String

        public override int GetHashCode()
        {
            return Address.GetHashCode();
        }

        public override string ToString()
        {
            return Address.ToString("X4");
        }

        #endregion
    }
}
