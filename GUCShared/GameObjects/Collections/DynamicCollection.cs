using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.GameObjects.Collections
{
    class DynamicCollection<T> where T : class
    {
        T[] arr;

        List<int> freeIDs;
        int idCounter;
        int count;
        int maximum;

        public int Count { get { return this.count; } }
        public int Size { get { return this.arr.Length; } }

        public DynamicCollection(int maximum = IDObject.MAX_ID)
        {
            arr = new T[0];
            freeIDs = new List<int>(0);
            idCounter = 0;
            count = 0;
            this.maximum = maximum;
        }

        void Insert(T obj, ref int id, int newID)
        {
            id = newID;
            arr[newID] = obj;
            count++;
        }

        public void Add(T obj, ref int id)
        {
            if (obj == null)
                throw new ArgumentNullException("Object is null!");

            if (id != -1)
                throw new ArgumentException("Object is already added to a different collection.");

            // find a new ID
            if (freeIDs.Count > 0)
            {
                Insert(obj, ref id, freeIDs[0]);
                freeIDs.RemoveAt(0);
            }
            else // no free IDs
            {
                if (idCounter >= maximum)
                {
                    throw new Exception("DynamicCollection reached maximum! " + maximum);
                }
                else if (idCounter >= arr.Length) // check allocation
                {
                    int newSize = (int)(1.5f * (arr.Length + 1)); // 2 * (arr.Length + 10);
                    if (newSize > maximum)
                        newSize = maximum;
                    T[] newArr = new T[newSize];
                    Array.Copy(arr, newArr, arr.Length);
                    arr = newArr;
                }
                Insert(obj, ref id, idCounter++);
            }
        }

        public void Remove(ref int id)
        {
            //if (obj == null)
            //    throw new ArgumentNullException("Object is null!");

            if (id < 0 || id >= arr.Length)
                throw new ArgumentOutOfRangeException("Object id is out of range!");

            //if (arr[id] != obj)
            //    throw new ArgumentException("Object is not in this collection!");

            arr[id] = null;
            freeIDs.Add(id);
            id = -1;
            count--;
        }

        public void ForEach(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = 0; i < idCounter; i++)
            {
                if (arr[i] != null)
                    action(arr[i]);
            }
        }

        /// <summary>
        /// return FALSE to break the loop.
        /// </summary>
        public void ForEachPredicate(Predicate<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = 0; i < idCounter; i++)
            {
                if (arr[i] != null)
                {
                    if (!action(arr[i]))
                        break;
                }
            }
        }
    }
}
