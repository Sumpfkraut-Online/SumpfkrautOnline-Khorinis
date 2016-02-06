using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Log;

namespace GUC.WorldObjects.Collections
{
    public class InstDict<TInstance> where TInstance : BaseInstance
    {
        TInstance[] instances = new TInstance[0];
        List<int> freeIDs = new List<int>(0);
        int idCount = 0;
        int count = 0;

        #region Add
        void Insert(int id, TInstance inst)
        {
            inst.DictID = id;
            instances[id] = inst;
            count++;
        }

        public void Add(TInstance inst)
        {
            if (inst.DictID != -1)
            {
                throw new Exception("Instance isn't removed from old InstDict!");
            }

            // find a new ID
            if (freeIDs.Count > 0)
            {
                Insert(freeIDs[0], inst);
                freeIDs.RemoveAt(0);
            }
            else
            {
                if (idCount >= WorldObject.MAX_ID)
                {
                    throw new Exception("InstDict reached maximum! " + WorldObject.MAX_ID);
                }
                else if (idCount >= instances.Length)
                {
                    // no free IDs
                    int newSize = 2 * (instances.Length + 10);
                    if (newSize > WorldObject.MAX_ID)
                        newSize = WorldObject.MAX_ID;
                    TInstance[] newArr = new TInstance[newSize];
                    Array.Copy(instances, newArr, instances.Length);
                    instances = newArr;
                }
                Insert(idCount++, inst);
            }
        }
        #endregion

        #region Remove
        public bool Remove(TInstance inst)
        {
            if (inst.ID < 0 || inst.ID >= instances.Length)
            {
                Logger.LogWarning("Tried to remove Instance with out-of-range ID!");
            }
            else
            {
                TInstance other = instances[inst.ID];
                if (other == inst)
                {
                    instances[inst.ID] = null;
                    freeIDs.Add(inst.ID);
                    inst.ID = -1;
                    count--;
                    return true;
                }
                Logger.LogWarning("Tried to remove different Instance with same ID!");
            }
            return false;
        }
        #endregion

        #region Loops
        public void ForEach(Action<TInstance> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            TInstance inst;
            for (int i = 0; i < instances.Length; i++)
            {
                inst = instances[i];
                if (inst != null)
                    action(inst);
            }
        }
        #endregion

        /// <summary>
        /// Seek and save free IDs within the current capacity. Call this after adding lots of Instances with set IDs!
        /// </summary>
        public void SeekFreeIDs()
        {
            BaseInstance inst;
            freeIDs = new List<int>();
            for (int i = 0; i < instances.Length; i++)
            {
                inst = instances[i];
                if (inst == null)
                {
                    freeIDs.Add(i);
                }
            }
        }
    }
}
