﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zString : zClass, IDisposable
    {
        public const int ByteSize = 20;

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
                Process.Free(new IntPtr(Address), ByteSize);
                disposed = true;
            }
        }

        #region Gothic Methods

        public abstract class FuncAddresses
        {
            public const int Insert = 0x0046B400; /// <summary> zString::Insert(uint, zSTRING const &) </summary>
            public const int Clear = 0x0059D010; /// <summary> zSTRING::Clear(void) </summary>

            public const int OperatorAssignZString = 0x0059CEB0; /// <summary> zSTRING::operator=(zSTRING const &) </summary>
            public const int OperatorAssignConstChar = 0x004CFAF0; /// <summary> zSTRING::operator=(char const &) </summary>

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

        public abstract class VarOffsets
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
            set { Process.Write(Address + VarOffsets.VTBL, value); }
        }

        public int ALLOCATER
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Allocater); }
            set { Process.Write(this.Address + VarOffsets.Allocater, value); }
        }

        public int PTR
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Ptr); }
            set { Process.Write(this.Address + VarOffsets.Ptr, value); }
        }

        public int Length
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Length); }
            set { Process.Write(this.Address + VarOffsets.Length, value); }
        }

        public int Res
        {
            get { return Process.ReadInt(this.Address + VarOffsets.Res); }
            set { Process.Write(this.Address + VarOffsets.Res, value); }
        }

        #endregion

        public static zString Create(String value)
        {
            int self = Process.Alloc(ByteSize).ToInt32();
            if (value != null && value.Length > 0)
            {
                byte[] bytes = Encoding.Default.GetBytes(value);
                int charArr = Process.Alloc((uint)bytes.Length + 1).ToInt32();

                Process.Write(charArr, bytes);
                Process.Write(charArr + bytes.Length, (byte)0);

                Process.THISCALL<NullReturnCall>(self, FuncAddresses.ConstructorConstChar, (IntArg)charArr);
                Process.Free(charArr, (uint)bytes.Length + 1);
            }
            return new zString(self);
        }

        public void Set(String str)
        {
            if (str != null && str.Length > 0)
            {
                byte[] bytes = Encoding.Default.GetBytes(str);
                int charArr = Process.Alloc((uint)bytes.Length + 1).ToInt32();

                Process.Write(charArr, bytes);
                Process.Write(charArr + bytes.Length, (byte)0);

                Process.THISCALL<zString>(Address, FuncAddresses.OperatorAssignConstChar, (IntArg)charArr);
                Process.Free(charArr, (uint)str.Length + 1);
            }
            else
            {
                Process.THISCALL<zString>(Address, FuncAddresses.OperatorAssignConstChar, (IntArg)0);
            }
        }

        public override string ToString()
        {
            int ptr = this.PTR;
            int len = this.Length;
            if (ptr != 0 && len > 0)
            {
                return Encoding.Default.GetString(Process.ReadBytes(ptr, (uint)len));
            }
            return string.Empty;
        }
    }
}
