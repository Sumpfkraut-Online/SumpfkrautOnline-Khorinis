using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.WorldObjects.Collections
{
    public class WOStorage<TWorldObject> where TWorldObject : WorldObject
    {
        protected int maximum;
        protected TWorldObject[] objects;
        protected int count;

        public int Maximum { get { return maximum; } }
        public int Size { get { return objects.Length; } }
        public int Count { get { return count; } }

        internal WOStorage(int maximum = WorldObject.MAX_ID)
        {
            this.maximum = maximum;
            objects = new TWorldObject[0];
            count = 0;
        }

        #region Add
        internal virtual void Add(TWorldObject obj, ref int idRef)
        {
            if (obj == null)
                throw new ArgumentNullException("WorldObject is null!");

            if (idRef < 0 || idRef > maximum)
            {
                throw new ArgumentOutOfRangeException("ID is out of bounds!");
            }

            if (idRef >= objects.Length)
            {
                TWorldObject[] newArr = new TWorldObject[idRef + 1];
                Array.Copy(objects, newArr, objects.Length);
                objects = newArr;
            }
            else if (objects[idRef] != null)
            {
                throw new Exception("There is already an element with this ID!");
            }

            objects[idRef] = obj;
            count++;
        }
        #endregion

        #region Remove
        internal virtual bool Remove(TWorldObject obj, ref int idRef)
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
    }
}
