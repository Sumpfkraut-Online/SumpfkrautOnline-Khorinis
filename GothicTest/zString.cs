// This file was generated with the GothicClassGenerator (do not change this line!)
using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.Calls;

namespace Gothic
{
    public struct zString : IDisposable
    {
        #region Custom Code (put your code here!)
        
        
        
        #endregion
        
        #region Generated Code (do not change this region!)
        
        const int ByteSize = 0x14;
        
        const int VirtualTable = 0x82E6F0;
        
        const int Destructor = 0x000000;
        
        #region Calls of zString
        
        static readonly ThisCall Clear_void = new ThisCall(0x59D010);
        public void Clear()
        {
            Clear_void.Call(this.address);
        }
        
        static readonly ThisReturnCall<zString,zString> Set_zString_zString = new ThisReturnCall<zString,zString>(0x59CEB0);
        public zString Set(zString str)
        {
            return Set_zString_zString.Call(this.address, str);
        }
        
        #endregion
        
        #region Properties of zString
        
        public int VTable
        {
            get { return Process.ReadInt(this.address + (0x0)); }
        }
        
        public int Allocater
        {
            get { return Process.ReadInt(this.address + (0x4)); }
        }
        
        public int CharPtr
        {
            get { return Process.ReadInt(this.address + (0x8)); }
        }
        
        public int Length
        {
            get { return Process.ReadInt(this.address + (0xC)); }
        }
        
        public int Res
        {
            get { return Process.ReadInt(this.address + (0x10)); }
        }
        
        #endregion
        
        public static readonly zString Null = new zString(0);
        
        int address;
        public int Address { get { return this.address; } }
        public bool IsNull { get { return this.address == 0; } }
        
        public zString(int address)
        {
            this.address = address;
        }
        
        #region Equality
        
        public bool Equals(zString other)
        {
            return other.address == this.address;
        }
        
        public override bool Equals(object other)
        {
            return other is zString ? ((zString)other).address == this.address : false;
        }
        
        public static bool operator ==(zString a, zString b)
        {
            return a.address == b.address;
        }
        
        public static bool operator !=(zString a, zString b)
        {
            return a.address != b.address;
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator int(zString self)
        {
            return self.address;
        }
        
        public static implicit operator bool(zString self)
        {
            return self.address != 0;
        }
        
        #endregion
        
        #region HashCode & String
        
        public override int GetHashCode()
        {
            return this.address.GetHashCode();
        }
        
        public override string ToString()
        {
            return this.address.ToString("X8");
        }
        
        #endregion
        
        public void Dispose()
        {
            Process.Free(this.address, ByteSize);
        }
        
        #endregion
    }
}

