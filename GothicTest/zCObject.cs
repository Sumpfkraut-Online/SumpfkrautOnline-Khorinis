// This file was generated with the GothicClassGenerator (do not change this line!)
using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.Calls;

namespace Gothic
{
    public struct zCObject : IDisposable
    {
        #region Custom Code (put your code here!)
        
        
        
        #endregion
        
        #region Generated Code (do not change this region!)
        
        const int ByteSize = 0x0;
        
        const int VirtualTable = 0x82E89C;
        
        const int Destructor = 0x000000;
        
        static readonly zCClassDef ClassDef = new zCClassDef(Process.ReadInt(Process.ReadInt(VirtualTable) + 1));
        
        #region Calls of zCObject
        
        static readonly ThisReturnCall<int> Release_int = new ThisReturnCall<int>(0x40C310);
        public int Release()
        {
            return Release_int.Call(this.address);
        }
        
        #endregion
        
        #region Properties of zCObject
        
        public int RefCount
        {
            get { return Process.ReadInt(this.address + (0x4)); }
            set { Process.Write(this.address + (0x4), value); }
        }
        
        #endregion
        
        public static readonly zCObject Null = new zCObject(0);
        
        int address;
        public int Address { get { return this.address; } }
        public bool IsNull { get { return this.address == 0; } }
        
        public zCObject(int address)
        {
            this.address = address;
        }
        
        #region Equality
        
        public bool Equals(zCObject other)
        {
            return other.address == this.address;
        }
        
        public override bool Equals(object other)
        {
            return other is zCObject ? ((zCObject)other).address == this.address : false;
        }
        
        public static bool operator ==(zCObject a, zCObject b)
        {
            return a.address == b.address;
        }
        
        public static bool operator !=(zCObject a, zCObject b)
        {
            return a.address != b.address;
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator int(zCObject self)
        {
            return self.address;
        }
        
        public static implicit operator bool(zCObject self)
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
        
        static readonly ThisCall<uint> destructor = new ThisCall<uint>(Process.ReadInt(VirtualTable + 0xC));
        public void Dispose()
        {
            destructor.Call(this.address, 1);
        }
        
        #endregion
    }
}

