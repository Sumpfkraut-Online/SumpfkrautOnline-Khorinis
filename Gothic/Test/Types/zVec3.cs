// This file was generated with the GothicClassGenerator (do not change this line!)
using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.Calls;

namespace Gothic.Test.Types
{
    public struct zVec3 : IDisposable
    {
        #region Custom Code (put your code here!)
        
        
        
        #endregion
        
        #region Generated Code (do not change this region!)
        
        const int ByteSize = 0xC;
        
        const int VirtualTable = 0x000000;
        
        const int Destructor = 0x000000;
        
        #region Calls of zVec3
        
        static readonly ThisReturnCall<zVec3,float,float,float> Create_zVec3_floatfloatfloat = new ThisReturnCall<zVec3,float,float,float>(0x408EC0);
        public static zVec3 Create(float x, float y, float z)
        {
            return Create_zVec3_floatfloatfloat.Call(Process.Alloc(ByteSize).ToInt32(), x, y, z);
        }
        
        static readonly ThisReturnCall<zVec3> Create_zVec3 = new ThisReturnCall<zVec3>(0x489F30);
        public static zVec3 Create()
        {
            return Create_zVec3.Call(Process.Alloc(ByteSize).ToInt32());
        }
        
        #endregion
        
        #region Properties of zVec3
        
        public float X
        {
            get { return Process.ReadFloat(this.address + (0x0)); }
            set { Process.Write(this.address + (0x0), value); }
        }
        
        public float Y
        {
            get { return Process.ReadFloat(this.address + (0x4)); }
            set { Process.Write(this.address + (0x4), value); }
        }
        
        public float Z
        {
            get { return Process.ReadFloat(this.address + (0x8)); }
            set { Process.Write(this.address + (0x8), value); }
        }
        
        #endregion
        
        public static readonly zVec3 Null = new zVec3(0);
        
        int address;
        public int Address { get { return this.address; } }
        public bool IsNull { get { return this.address == 0; } }
        
        public zVec3(int address)
        {
            this.address = address;
        }
        
        #region Equality
        
        public bool Equals(zVec3 other)
        {
            return other.address == this.address;
        }
        
        public override bool Equals(object other)
        {
            return other is zVec3 ? ((zVec3)other).address == this.address : false;
        }
        
        public static bool operator ==(zVec3 a, zVec3 b)
        {
            return a.address == b.address;
        }
        
        public static bool operator !=(zVec3 a, zVec3 b)
        {
            return a.address != b.address;
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator int(zVec3 self)
        {
            return self.address;
        }
        
        public static implicit operator bool(zVec3 self)
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

