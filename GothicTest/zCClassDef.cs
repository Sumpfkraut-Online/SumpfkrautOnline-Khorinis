// This file was generated with the GothicClassGenerator (do not change this line!)
using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.Calls;

namespace Gothic
{
    public struct zCClassDef : IDisposable
    {
        #region Custom Code (put your code here!)
        
        
        
        #endregion
        
        #region Generated Code (do not change this region!)
        
        const int ByteSize = 0x0;
        
        const int VirtualTable = 0x000000;
        
        const int Destructor = 0x000000;
        
        #region Calls of zCClassDef
        
        static readonly CdeclCall<zCObject,zCClassDef> ObjectCreated_void_zCObjectzCClassDef = new CdeclCall<zCObject,zCClassDef>(0x5AAEB0);
        public static void ObjectCreated(zCObject obj, zCClassDef def)
        {
            ObjectCreated_void_zCObjectzCClassDef.Call(obj, def);
        }
        
        #endregion
        
        #region Properties of zCClassDef
        
        #endregion
        
        public static readonly zCClassDef Null = new zCClassDef(0);
        
        int address;
        public int Address { get { return this.address; } }
        public bool IsNull { get { return this.address == 0; } }
        
        public zCClassDef(int address)
        {
            this.address = address;
        }
        
        #region Equality
        
        public bool Equals(zCClassDef other)
        {
            return other.address == this.address;
        }
        
        public override bool Equals(object other)
        {
            return other is zCClassDef ? ((zCClassDef)other).address == this.address : false;
        }
        
        public static bool operator ==(zCClassDef a, zCClassDef b)
        {
            return a.address == b.address;
        }
        
        public static bool operator !=(zCClassDef a, zCClassDef b)
        {
            return a.address != b.address;
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator int(zCClassDef self)
        {
            return self.address;
        }
        
        public static implicit operator bool(zCClassDef self)
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

