using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zTypes
{
    public class zCArray<T> : zClass where T : CallValue, new()
    {
        public zCArray(Process process, int Address)
            : base(process, Address)
        {

        }

        public int NumAlloc
        {
            get
            {
                return Process.ReadInt(Address + 4);
            }
        }

        public int Size
        {
            get
            {
                return Process.ReadInt(Address + 8);
            }
        }

        public T get(int index)
        {
            if (index > Size)
                return null;
            T rValue = new T();
            rValue.Initialize(Process, Process.ReadInt(Process.ReadInt(Address) + index*4));
            return rValue;
        }

        public void set(T val, int index)
        {
            if (index > Size)
                return;

            Process.Write(val.Address, Process.ReadInt(Address) + index * 4);
        }

        public T get(int index, int size)
        {
            if (index > Size)
                return null;
            T rValue = new T();
            rValue.Initialize(Process, Process.ReadInt(Address) + index * size);
            return rValue;
        }


        public void InsertEnd(T arr)
        {
            if (arr is zCVob)
            {
                Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x00418120, new CallValue[] { arr });
            }
            else if (arr is IntArg)
            {
                Process.THISCALL<NullReturnCall>((uint)Address, (uint)0x004D5A30, new CallValue[] { arr });
            }
        }
    }
}
