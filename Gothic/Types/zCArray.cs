using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.Objects;

namespace Gothic.Types
{
    public class zCArray<T> : zClass where T : zClass, new()
    {
        public zCArray(int Address)
            : base(Address)
        {

        }

        public zCArray()
        {

        }

        public int NumAlloc
        {
            get
            {
                return Process.ReadInt(Address + 4);
            }
        }

        public int GetCount()
        {
                return Process.ReadInt(Address + 8);
        }

        public T get(int index)
        {
            if (index < 0 || index >= GetCount())
                return null;
            T rValue = new T();
            rValue.Initialize(Process.ReadInt(Address) + index*4);
            return rValue;
        }

        public void set(T val, int index)
        {
            if (index < 0 || index >= GetCount())
                return;

            Process.Write(Process.ReadInt(Address) + index * 4, val.Address);
        }

        public T get(int index, int size)
        {
            if (index > GetCount())
                return null;
            T rValue = new T();
            rValue.Initialize(Process.ReadInt(Address) + index * size);
            return rValue;
        }


        public void InsertEnd(T arr)
        {
            if (arr is zCVob)
            {
                Process.THISCALL<NullReturnCall>(Address, 0x00418120, arr);
            }
            else if (arr is IntArg)
            {
                Process.THISCALL<NullReturnCall>(Address, 0x004D5A30, arr);
            }
        }
    }
}
