using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;

namespace GUC.Server.WorldObjects.Collections
{
    public class WODictionary<TWorldObject> where TWorldObject : WorldObject
    {
        protected TWorldObject[] objects;
        protected int count;
        List<int> freeIDs;
        int idCount;

        public int Maximum { get { return WorldObject.MAX_ID; } }
        public int Size { get { return objects.Length; } }
        public int Count { get { return count; } }

        internal WODictionary()
        {
            objects = new TWorldObject[0];
            count = 0;
            freeIDs = new List<int>(0);
            idCount = 0;
        }

        #region Add
        void Insert(TWorldObject obj, ref int idRef, int id)
        {
            idRef = id;
            objects[id] = obj;
            count++;
        }

        internal void Add(TWorldObject obj, ref int idRef)
        {
            if (obj == null)
                throw new ArgumentNullException("WorldObject is null!");

            if (idRef != -1)
            {
                throw new Exception("WorldObject isn't removed from old WODictionary!");
            }

            // find a new ID
            if (freeIDs.Count > 0)
            {
                Insert(obj, ref idRef, freeIDs[0]);
                freeIDs.RemoveAt(0);
            }
            else
            {
                if (idCount >= WorldObject.MAX_ID)
                {
                    throw new Exception("WODictionary reached maximum! " + WorldObject.MAX_ID);
                }
                else if (idCount >= objects.Length)
                {
                    // no free IDs
                    int newSize = 2 * (objects.Length + 10);
                    if (newSize > WorldObject.MAX_ID)
                        newSize = WorldObject.MAX_ID;
                    TWorldObject[] newArr = new TWorldObject[newSize];
                    Array.Copy(objects, newArr, objects.Length);
                    objects = newArr;
                }
                Insert(obj, ref idRef, idCount++);
            }
        }
        #endregion

        #region Remove
        internal bool Remove(TWorldObject obj, ref int idRef)
        {
            if (obj == null)
                throw new ArgumentNullException("WorldObject is null!");

            if (idRef < 0 || idRef >= objects.Length)
            {
                throw new ArgumentOutOfRangeException("ID is out of bounds!");
            }
            else
            {
                TWorldObject other = objects[idRef];
                if (other == obj)
                {
                    objects[idRef] = null;
                    freeIDs.Add(idRef);
                    idRef = -1;
                    count--;
                    return true;
                }
                throw new Exception("Tried to remove different WorldObject with same ID!");
            }
        }
        #endregion

        #region Loops

        /// <summary>
        /// Executes the given action on each object in the collection.
        /// </summary>
        /// <param name="action">The action to execute. You can use Lambda-Expressions: o => { }</param>
        public void ForEach(Action<TWorldObject> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            TWorldObject inst;
            for (int i = 0; i < objects.Length; i++)
            {
                inst = objects[i];
                if (inst != null)
                    action(inst);
            }
        }
        #endregion

        /// <summary>
        /// Seek and save free IDs within the current capacity. Call this after adding lots of WorldObjects with set IDs!
        /// </summary>
        public void SeekFreeIDs()
        {
            TWorldObject inst;
            freeIDs = new List<int>();
            for (int i = 0; i < objects.Length; i++)
            {
                inst = objects[i];
                if (inst == null)
                {
                    freeIDs.Add(i);
                }
            }
        }
    }
}
