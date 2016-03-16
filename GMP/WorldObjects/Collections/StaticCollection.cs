using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    internal partial class StaticCollection<T> where T : GameObject
    {
        T[] arr;

        internal StaticCollection()
        {
            arr = new T[GameObject.MAX_ID];
        }

        partial void CheckID(T obj);
        internal void Add(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object is null!");

            CheckID(obj);

            if (arr[obj.ID] != null)
                throw new ArgumentException("There is already an object with this ID! " + obj.ID);

            arr[obj.ID] = obj;      
        }

        internal void Remove(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object is null!");

            if (arr[obj.ID] != obj)
                throw new ArgumentException("Object is not in this collection!");

            arr[obj.ID] = null;
        }

        internal bool TryGet(int id, out T ret)
        {
            ret = arr[id];
            return ret != null;
        }

        internal bool TryGet<TSpecific>(int id, out TSpecific ret) where TSpecific : T
        {
            T obj = arr[id];
            if (obj != null && obj is TSpecific)
            {
                ret = (TSpecific)obj;
                return true;
            }
            else
            {
                ret = null;
                return false;
            }
        }
    }
}
