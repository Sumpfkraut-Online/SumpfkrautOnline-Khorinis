using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.WorldObjects.Instances;

namespace GUC.WorldObjects.Collections
{
    public static partial class InstanceCollection
    {
        /// <summary>
        /// The upper (excluded) limit for Instances (ushort.MaxValue+1).
        /// </summary>
        public const int MAX_INSTANCES = 65536;

        static Dictionary<int, BaseVobInstance> instances;
        static Dictionary<int, BaseVobInstance>[] typeDicts;

        static List<BaseVobInstance> dynInstances;
        static List<BaseVobInstance>[] dynDicts;

        static InstanceCollection()
        {
            instances = new Dictionary<int, BaseVobInstance>();
            dynInstances = new List<BaseVobInstance>();
            typeDicts = new Dictionary<int, BaseVobInstance>[(int)VobTypes.Maximum];
            dynDicts = new List<BaseVobInstance>[(int)VobTypes.Maximum];
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
            {
                typeDicts[i] = new Dictionary<int, BaseVobInstance>();
                dynDicts[i] = new List<BaseVobInstance>();
            }
        }

        #region Access

        public static BaseVobInstance Get(int id)
        {
            BaseVobInstance inst;
            instances.TryGetValue(id, out inst);
            return inst;
        }

        public static void ForEach(Action<BaseVobInstance> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach (BaseVobInstance instance in instances.Values)
            {
                action(instance);
            }
        }

        public static int GetCount()
        {
            return instances.Count;
        }

        public static void ForEach(int type, Action<BaseVobInstance> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach (BaseVobInstance instance in typeDicts[type].Values)
            {
                action(instance);
            }
        }

        public static int GetCount(int type)
        {
            return typeDicts[type].Count;
        }

        public static void ForEachDynamic(Action<BaseVobInstance> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = 0; i < dynInstances.Count; i++)
                action(dynInstances[i]);
        }

        public static void ForEachDynamic(int type, Action<BaseVobInstance> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            List<BaseVobInstance> dict = dynDicts[type];

            for (int i = 0; i < dict.Count; i++)
                action(dict[i]);
        }

        public static int GetCountDynamics()
        {
            return dynInstances.Count;
        }

        public static int GetCountDynamics(int type)
        {
            return dynDicts[type].Count;
        }

        #endregion

        #region Add
        static partial void CheckID(BaseVobInstance inst);
        static partial void pAdd(BaseVobInstance inst);
        public static void Add(BaseVobInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException("Instance is null!");

            CheckID(inst);

            instances.Add(inst.ID, inst);
            typeDicts[(int)inst.VobType].Add(inst.ID, inst);

            if (!inst.IsStatic)
            {
                dynInstances.Add(inst);
                dynDicts[(int)inst.VobType].Add(inst);
            }

            pAdd(inst);

            inst.Added = true;
        }
        #endregion

        #region Remove
        static partial void pRemove(BaseVobInstance inst);
        public static void Remove(BaseVobInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException("Instance is null!");

            if (inst.Added)
            {
                instances.Remove(inst.ID);
                typeDicts[(int)inst.VobType].Remove(inst.ID);
                if (!inst.IsStatic)
                {
                    dynInstances.Remove(inst);
                    dynDicts[(int)inst.VobType].Remove(inst);
                }

                pRemove(inst);

                inst.Added = false;
            }
        }
        #endregion
    }
}
