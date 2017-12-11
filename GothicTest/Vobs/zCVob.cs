// This file was generated with the GothicClassGenerator (do not change this line!)
using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using WinApi.Calls;
using Types;

namespace Gothic.Vobs
{
    public struct zCVob : IDisposable // : zCObject
    {
        #region Custom Code (put your code here!)
        
        
        
        #endregion
        
        #region Generated Code (do not change this region!)
        
        const int ByteSize = 0x120;
        
        const int VirtualTable = 0x83997C;
        
        const int Destructor = 0x000000;
        
        static readonly zCClassDef ClassDef = new zCClassDef(Process.ReadInt(Process.ReadInt(VirtualTable) + 1));
        
        #region Calls of zCVob
        
        static readonly ThisCall<zString> SetVisual_void_zString = new ThisCall<zString>(Process.ReadInt(VirtualTable + 0x4C));
        public void SetVisual(zString visual)
        {
            SetVisual_void_zString.Call(this.address, visual);
        }
        
        static readonly ThisReturnCall<zCVob> Create_zCVob = new ThisReturnCall<zCVob>(0x5FE1E0);
        public static zCVob Create()
        {
            zCObject obj = new zCObject(Process.Alloc(ByteSize).ToInt32());
            zCClassDef.ObjectCreated(obj, ClassDef);
            return Create_zCVob.Call(obj);
        }
        
        static readonly ThisCall<zVec3> SetPositionWorld_void_zVec3 = new ThisCall<zVec3>(0x61BB70);
        public void SetPositionWorld(zVec3 pos)
        {
            SetPositionWorld_void_zVec3.Call(this.address, pos);
        }
        
        #region Calls of zCObject
        
        static readonly ThisReturnCall<int> Release_int = new ThisReturnCall<int>(0x40C310);
        public int Release()
        {
            return Release_int.Call(this.address);
        }
        
        #endregion
        
        #endregion
        
        #region Properties of zCVob
        
        #region Properties of zCObject
        
        public int RefCount
        {
            get { return Process.ReadInt(this.address + (0x4)); }
            set { Process.Write(this.address + (0x4), value); }
        }
        
        #endregion
        
        #endregion
        
        public static readonly zCVob Null = new zCVob(0);
        
        int address;
        public int Address { get { return this.address; } }
        public bool IsNull { get { return this.address == 0; } }
        
        public zCVob(int address)
        {
            this.address = address;
        }
        
        #region Equality
        
        public bool Equals(zCVob other)
        {
            return other.address == this.address;
        }
        
        public override bool Equals(object other)
        {
            return other is zCVob ? ((zCVob)other).address == this.address : false;
        }
        
        public static bool operator ==(zCVob a, zCVob b)
        {
            return a.address == b.address;
        }
        
        public static bool operator !=(zCVob a, zCVob b)
        {
            return a.address != b.address;
        }
        
        #endregion
        
        #region Conversion
        
        public static implicit operator int(zCVob self)
        {
            return self.address;
        }
        
        public static implicit operator bool(zCVob self)
        {
            return self.address != 0;
        }
        
        public static implicit operator zCObject(zCVob self)
        {
            return new zCObject(self.address);
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

