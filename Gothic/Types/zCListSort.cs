using System;
using System.Collections.Generic;
using System.Text;
using WinApi;

namespace Gothic.Types
{
    public class zCListSort<T> : zClass where T : zClass, new()
    {
        public T Data
        {
            get
            {
                T val = new T();
                val.Initialize(Address + 4);
                return val;
            }
        }

        public zCListSort<T> Next
        {
            get 
            {
                return new zCListSort<T>(Process.ReadInt(Address + 8));
            }
        }

        public zCListSort(int Address)
            : base(Address)
        {

        }

        public T get(int index)
        {
            zCListSort<T> listTemp = this;
            uint idx = 0;

            do 
            {
                if (idx == index)
                    return listTemp.Data;
                idx++;
            } while ((listTemp = listTemp.Next).Address != 0);

            return null;
        }

        public int size()
        {
            zCListSort<T> listTemp = this;
            int idx = 0;

            do
            {
                idx++;
            } while ((listTemp = listTemp.Next).Address != 0);

            return idx;
        }
    }
}
