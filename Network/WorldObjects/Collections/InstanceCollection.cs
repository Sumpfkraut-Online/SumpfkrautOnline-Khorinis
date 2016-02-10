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

        public static IEnumerable<BaseVobInstance> GetAll()
        {
            return instances.Values;
        }

        public static int GetCount()
        {
            return instances.Count;
        }

        public static IEnumerable<BaseVobInstance> GetAll(int type)
        {
            return typeDicts[type].Values;
        }

        public static int GetCount(int type)
        {
            return typeDicts[type].Count;
        }

        public static IEnumerable<BaseVobInstance> GetAllDynamics()
        {
            return dynInstances;
        }

        public static IEnumerable<BaseVobInstance> GetAllDynamics(int type)
        {
            return dynDicts[type];
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
        }
        #endregion

        #region Remove
        static partial void pRemove(BaseVobInstance inst);
        public static void Remove(BaseVobInstance inst)
        {
            if (inst == null)
                throw new ArgumentNullException("Instance is null!");

            BaseVobInstance other;
            if (instances.TryGetValue(inst.ID, out other))
            {
                if (other == inst)
                {
                    instances.Remove(inst.ID);
                    typeDicts[(int)inst.VobType].Remove(inst.ID);
                    if (!inst.IsStatic)
                    {
                        dynInstances.Remove(inst);
                        dynDicts[(int)inst.VobType].Remove(inst);
                    }

                    pRemove(inst);
                }
            }
        }
        #endregion
    }
}
