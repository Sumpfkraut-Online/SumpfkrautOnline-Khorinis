using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic
{
    public class zString : zClass, IDisposable
    {
        public zString()
        {
        }

        public zString(int address) : base(address)
        {
        }

        public void Dispose()
        {
            Dispose(true);
        }

        bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Destructor);
                Process.Free(new IntPtr(Address), GetSize());
                disposed = true;
            }
        }

        public override uint GetSize()
        {
            return 20;
        }

        #region Gothic Methods

        public class FuncAddresses
        {
            public const int Insert = 0x0046B400; /// <summary> zString::Insert(uint, zSTRING const &) </summary>
            public const int Clear = 0x0059D010; /// <summary> zSTRING::Clear(void) </summary>

            public const int OperatorAssignZString = 0x0059CEB0; /// <summary> zSTRING::operator=(zSTRING const &) </summary>
            public const int OperatorAssignConstChar = 0x0059CEB0; /// <summary> zSTRING::operator=(char const &) </summary>

            public const int ConstructorConstChar = 0x004010C0; /// <summary> zSTRING::zSTRING(char const *) </summary>
            public const int Destructor = 0x00401160; /// <summary> zSTRING::~zSTRING(void) </summary>
        }

        public void Clear()
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Clear);
        }

        public void Insert(int pos, zString str)
        {
            Process.THISCALL<NullReturnCall>(Address, FuncAddresses.Insert, (IntArg)pos, str);
        }

        public void Set(zString str)
        {
            Process.THISCALL<zString>(Address, FuncAddresses.OperatorAssignZString, str);
        }

        #endregion

        #region Gothic Fields

        public struct VarOffsets
        {
            public const int VTBL = 0;
            public const int Allocater = 4;
            public const int Ptr = 8;
            public const int Length = 12;
            public const int Res = 16;
        }

        public int VTBL
        {
            get { return Process.ReadInt(Address + VarOffsets.VTBL); }
            set { Process.Write(value, Address + VarOffsets.VTBL); }
        }

        public int ALLOCATER
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Allocater); }
            set { Process.Write(value, this.Address + VarOffsets.Allocater); }
        }

        public int PTR
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Ptr); }
            set { Process.Write(value, this.Address + VarOffsets.Ptr); }
        }

        public int Length
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Length); }
            set { Process.Write(value, this.Address + VarOffsets.Length); }
        }

        public int Res
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Res); }
            set { Process.Write(value, this.Address + VarOffsets.Res); }
        }

        #endregion

        public static zString Create(String value)
        {
            IntPtr stringArr = Process.Alloc(20);

            byte[] arr = value == null ? new byte[0] : Encoding.UTF8.GetBytes(value);
            IntPtr charArr = Process.Alloc((uint)arr.Length + 1);

            if (arr.Length > 0)
            {
                Process.Write(arr, charArr.ToInt32());
            }

            Process.Write(new byte[] { 0 }, charArr.ToInt32() + arr.Length);
            Process.THISCALL<NullReturnCall>(stringArr.ToInt32(), FuncAddresses.ConstructorConstChar, (IntArg)charArr.ToInt32());
            Process.Free(charArr, (uint)arr.Length + 1);

            return new zString(stringArr.ToInt32());
        }

        public void Set(String str)
        {
            byte[] arr = str == null ? new byte[0] : Encoding.UTF8.GetBytes(str);
            IntPtr charArr = Process.Alloc((uint)arr.Length + 1);

            if (arr.Length > 0)
            {
                Process.Write(arr, charArr.ToInt32());
            }

            Process.Write(new byte[] { 0 }, charArr.ToInt32() + arr.Length);
            Process.THISCALL<zString>(Address, FuncAddresses.OperatorAssignConstChar, (IntArg)charArr.ToInt32());
            Process.Free(charArr, (uint)str.Length + 1);
        }

        public override string ToString()
        {
            byte[] arr = Process.ReadBytes(this.PTR, (uint)this.Length);
            return Encoding.UTF8.GetString(arr);
        }
    }
}
