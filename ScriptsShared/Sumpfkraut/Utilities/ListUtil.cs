using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Utilities
{

    public class ListUtil : ExtendedObject
    {

        protected ListUtil () { }



        public static List<T> Populate<T> (List<T> list, T value, bool createNew = false)
        {
            if (createNew)
            {
                var newList = new List<T>(list.Capacity);
                for (int i = 0; i < list.Count; i++)
                {
                    newList.Add(value);
                }
                return newList;
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = value;
                }
                return list;
            }
        }

    }

}
