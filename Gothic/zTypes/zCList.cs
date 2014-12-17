using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace Gothic.zTypes
{
    public class zCList<T> : zClass where T : zClass, new()
    {
        public int Data
        {
            get
            {
                return Process.ReadInt(Address + 4);
            }
        }

        public int Next
        {
            get
            {
                return Process.ReadInt(Address + 8);
            }
        }

        public zCList(Process process, int Address)
            : base(process, Address)
        {

        }

        public T get(int index)
        {
            zCList<T> listTemp = this;
            int temp = this.Address;
            uint idx = 0;

            while ((temp = listTemp.Next) != 0)
            {
                listTemp = new zCList<T>(Process, temp);
                if (idx == index)
                {
                    T val = new T();
                    val.Initialize(Process, listTemp.Data);
                    return val;
                }
                idx++;
            }

            return null;
        }

        public int size()
        {
            zCList<T> listTemp = this;
            int temp = this.Address;
            int idx = 0;

            while ((temp = listTemp.Next) != 0)
            {
                listTemp = new zCList<T>(Process, temp);
                idx++;
            }

            return idx;
        }
    }
}
