using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;
using GUC.WorldObjects;

namespace GUC.Server.WorldObjects.Collections
{
    public class InstDict
    {
        protected BaseInstance[] instances;
        protected int count;
        List<int> freeIDs;
        int idCount;

        public int Maximum { get { return WorldObject.MAX_ID; } }
        public int Size { get { return instances.Length; } }
        public int Count { get { return count; } }

        internal InstDict()
        {
            instances = new BaseInstance[0];
            count = 0;
            freeIDs = new List<int>(0);
            idCount = 0;
        }

        #region Add
        void Insert(BaseInstance inst, int id)
        {
            inst.DictID = id;
            instances[id] = inst;
            count++;
        }

        /// <summary>
        /// Don't call this outside of InstanceCollection!
        /// </summary>
        internal void Add(BaseInstance inst)
        {
            // find a new ID
            if (freeIDs.Count > 0)
            {
                Insert(inst, freeIDs[0]);
                freeIDs.RemoveAt(0);
            }
            else
            {
                if (idCount >= instances.Length)
                {
                    // no free IDs
                    int newSize = 2 * (instances.Length + 10);
                    if (newSize > WorldObject.MAX_ID)
                        newSize = WorldObject.MAX_ID;
                    BaseInstance[] newArr = new BaseInstance[newSize];
                    Array.Copy(instances, newArr, instances.Length);
                    instances = newArr;
                }
                Insert(inst, idCount++);
            }
        }
        #endregion

        #region Remove
        /// <summary>
        /// Don't call this outside of InstanceCollection!
        /// </summary>
        internal void Remove(BaseInstance inst)
        {
            BaseInstance other = instances[inst.DictID];
            if (other == inst)
            {
                instances[inst.DictID] = null;
                freeIDs.Add(inst.DictID);
                count--;
            }
        }
        #endregion

        #region Loops

        /// <summary>
        /// Executes the given action on each object in the collection.
        /// </summary>
        /// <param name="action">The action to execute. You can use Lambda-Expressions: o => { }</param>
        public void ForEach(Action<BaseInstance> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            BaseInstance inst;
            for (int i = 0; i < instances.Length; i++)
            {
                inst = instances[i];
                if (inst != null)
                    action(inst);
            }
        }
        #endregion

        internal void SeekFreeIDs()
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

    public class InstColl
    {
        protected BaseInstance[] instances;
        protected int count;
        List<int> freeIDs;
        int idCount;

        public int Count { get { return count; } }

        InstDict[] typeDicts;

        public InstDict this[VobTypes type]
        {
            get { return typeDicts[(int)type]; }
        }

        internal InstColl()
        {
            instances = new BaseInstance[0];
            count = 0;
            freeIDs = new List<int>(0);
            idCount = 0;

            typeDicts = new InstDict[(int)VobTypes.Maximum];
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
                typeDicts[i] = new InstDict();
        }

        #region Add
        void Insert(BaseInstance inst, int id)
        {
            inst.id = id;
            instances[id] = inst;
            count++;
            this[inst.VobType].Add(inst);
        }

        public void Add(BaseInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException("WorldObject is null!");

            if (inst.id < 0 || inst.ID >= WorldObject.MAX_ID)
            {
                // find a new ID
                if (freeIDs.Count > 0)
                {
                    Insert(inst, freeIDs[0]);
                    freeIDs.RemoveAt(0);
                }
                else
                {
                    if (idCount >= WorldObject.MAX_ID)
                    {
                        throw new Exception("InstanceCollection reached maximum! " + WorldObject.MAX_ID);
                    }
                    else if (idCount >= instances.Length)
                    {
                        // no free IDs
                        int newSize = 2 * (instances.Length + 10);
                        if (newSize > WorldObject.MAX_ID)
                            newSize = WorldObject.MAX_ID;
                        Resize(newSize);
                    }
                    Insert(inst, idCount++);
                }
            }
            else
            { // instance already has a legit ID?
                if (inst.id >= instances.Length)
                {
                    Resize(inst.id + 1);
                }
                else
                {
                    BaseInstance other = instances[inst.ID];
                    if (other != null)
                    {
                        throw new ArgumentException("Collection contains already an Instance with this ID!");
                    }
                }
                Insert(inst, inst.id);
            }
        }
        #endregion

        void Resize(int newSize)
        {
            BaseInstance[] newArr = new BaseInstance[newSize];
            Array.Copy(instances, newArr, instances.Length);
            instances = newArr;
        }

        #region Remove
        void RemoveInstance(BaseInstance inst)
        {
            instances[inst.id] = null;
            freeIDs.Add(inst.id);
            inst.id = -1;
            count--;
            typeDicts[(int)inst.VobType].Remove(inst);
        }

        internal void Remove(BaseInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException("WorldObject is null!");

            if (inst.id < 0 || inst.id >= instances.Length)
            {
                throw new ArgumentOutOfRangeException("ID is out of bounds!");
            }
            else
            {
                BaseInstance other = instances[inst.id];
                if (other == inst)
                {
                    RemoveInstance(inst);
                }
                else
                {
                    throw new Exception("Tried to remove different WorldObject with same ID!");
                }
            }
        }
        #endregion

        #region Loops

        /// <summary>
        /// Executes the given action on each object in the collection.
        /// </summary>
        /// <param name="action">The action to execute. You can use Lambda-Expressions: o => { }</param>
        public void ForEach(Action<BaseInstance> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            BaseInstance inst;
            for (int i = 0; i < instances.Length; i++)
            {
                inst = instances[i];
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
