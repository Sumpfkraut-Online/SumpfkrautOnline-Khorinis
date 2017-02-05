using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
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

        public zCList(int Address)
            : base(Address)
        {

        }

        public zCList()
        {
        }

        public T get(int index)
        {
            zCList<T> listTemp = this;
            int temp = this.Address;
            uint idx = 0;

            do
            {
                listTemp = new zCList<T>(temp);
                if (idx == index)
                {
                    T val = new T();
                    val.Initialize(listTemp.Data);
                    return val;
                }
                idx++;
            } while ((temp = listTemp.Next) != 0);

            return null;
        }

        public int size()
        {
            zCList<T> listTemp = this;
            int temp = this.Address;
            int idx = 0;

            while ((temp = listTemp.Next) != 0)
            {
                listTemp = new zCList<T>(temp);
                idx++;
            }

            return idx;
        }
    }
}
