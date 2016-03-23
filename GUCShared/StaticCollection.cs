using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    partial class StaticCollection<T> where T : GameObject
    {
        T[] arr;
        int capacity;

        internal StaticCollection(int capacity = GameObject.MAX_ID)
        {
            this.capacity = capacity;
            arr = new T[0];
        }

        void Resize(int newSize)
        {
            T[] newArr = new T[newSize];
            Array.Copy(arr, newArr, arr.Length);
            arr = newArr;
        }

        partial void pCheckID(T obj);
        internal void Add(T obj)
        {
            //if (obj == null)
            //    throw new ArgumentNullException("Object is null!");

            pCheckID(obj); // give the object a legit ID

            if (obj.ID >= arr.Length)
            {
                this.Resize(obj.ID + 1);
            }
            else if (arr[obj.ID] != null)
            {
                throw new ArgumentException("There is already an object with this ID! " + obj.ID);
            }

            arr[obj.ID] = obj;
        }

        partial void pRemove(T obj);
        internal void Remove(T obj)
        {
            //if (obj == null)
            //    throw new ArgumentNullException("Object is null!");

            //if (arr[obj.ID] != obj)
            //    throw new ArgumentException("Object is not in this collection!");

            arr[obj.ID] = null;
            pRemove(obj);
        }

        internal bool TryGet(int id, out T ret)
        {
            if (id >= 0 && id < arr.Length)
            {
                ret = arr[id];
                return ret != null;
            }
            ret = null;
            return false;
        }

        internal bool TryGet<TSpecific>(int id, out TSpecific ret) where TSpecific : T
        {
            if (id >= 0 && id < arr.Length)
            {
                T obj = arr[id];
                if (obj != null && obj is TSpecific)
                {
                    ret = (TSpecific)obj;
                    return true;
                }
            }
            ret = null;
            return false;
        }

        internal bool ContainsID(int id)
        {
            return arr[id] != null;
        }
    }
}
