using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.WorldObjects.Collections
{
    public partial class VobCollection
    {
        /// <summary>
        /// The upper (excluded) limit for Vobs in a world (ushort.MaxValue+1).
        /// </summary>
        public const int MAX_VOBS = 65536;

        Dictionary<int, BaseVob> vobs;
        Dictionary<int, BaseVob>[] typeDicts;

        List<BaseVob> dynVobs;
        List<BaseVob>[] dynDicts;

        internal VobCollection()
        {
            vobs = new Dictionary<int, BaseVob>();
            dynVobs = new List<BaseVob>();
            typeDicts = new Dictionary<int, BaseVob>[(int)VobTypes.Maximum];
            dynDicts = new List<BaseVob>[(int)VobTypes.Maximum];
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
            {
                typeDicts[i] = new Dictionary<int, BaseVob>();
                dynDicts[i] = new List<BaseVob>();
            }
        }


        #region Access

        public BaseVob Get(int id)
        {
            BaseVob vob;
            vobs.TryGetValue(id, out vob);
            return vob;
        }

        public void ForEach(Action<BaseVob> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach (BaseVob vob in vobs.Values)
            {
                action(vob);
            }
        }

        public int GetCount()
        {
            return vobs.Count;
        }

        public void ForEach(int type, Action<BaseVob> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            foreach (BaseVob vob in typeDicts[type].Values)
            {
                action(vob);
            }
        }

        public int GetCount(int type)
        {
            return typeDicts[type].Count;
        }

        public void ForEachDynamic(Action<BaseVob> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = 0; i < dynVobs.Count; i++)
                action(dynVobs[i]);
        }

        public void ForEachDynamic(int type, Action<BaseVob> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            for (int i = 0; i < dynVobs.Count; i++)
                action(dynVobs[i]);
        }

        public int GetCountDynamics()
        {
            return dynVobs.Count;
        }

        public int GetCountDynamics(int type)
        {
            return dynDicts[type].Count;
        }

        #endregion

        #region Add
        partial void pAdd(BaseVob vob);
        partial void CheckID(BaseVob vob);
        public void Add(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            CheckID(vob);

            vobs.Add(vob.ID, vob);
            typeDicts[(int)vob.VobType].Add(vob.ID, vob);

            if (!vob.IsStatic)
            {
                dynVobs.Add(vob);
                dynDicts[(int)vob.VobType].Add(vob);
            }

            pAdd(vob);
        }
        #endregion

        #region Remove
        partial void pRemove(BaseVob vob);
        public void Remove(BaseVob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("Vob is null!");

            BaseVob other;
            if (vobs.TryGetValue(vob.ID, out other))
            {
                if (other == vob)
                {
                    vobs.Remove(vob.ID);
                    typeDicts[(int)vob.VobType].Remove(vob.ID);
                    if (!vob.IsStatic)
                    {
                        dynVobs.Remove(vob);
                        dynDicts[(int)vob.VobType].Remove(vob);
                    }
                }
            }

            pRemove(vob);
        }

        #endregion
    }
}
