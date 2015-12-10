using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Server.WorldObjects.Collections
{
    public class VobObjCollection<TKey, TBase> where TBase : IVobObj<TKey>
    {
        protected VobObjDictionary<TKey, TBase>[] vobDicts = new VobObjDictionary<TKey, TBase>[(int)VobType.Maximum];
        protected Dictionary<TKey, TBase> allDict = new Dictionary<TKey, TBase>();

        internal VobObjCollection()
        {
            for (int i = 0; i < (int)VobType.Maximum; i++)
            {
                vobDicts[i] = new VobObjDictionary<TKey, TBase>();
            }
        }

        public VobObjDictionary<TKey, TBase> GetDict(VobType type)
        {
            return vobDicts[(int)type];
        }

        public TBase Get(VobType type, TKey id)
        {
            return GetDict(type).Get(id);
        }

        public IEnumerable<TBase> GetAll(VobType type)
        {
            return GetDict(type).GetAll();
        }

        internal virtual void Add(TBase vob)
        {
            if (vob != null && !vob.ID.Equals(default(TKey)))
            {
                GetDict(vob.VobType).Add(vob);
                allDict.Add(vob.ID, vob);
            }
        }

        internal virtual void Remove(TBase vob)
        {
            GetDict(vob.VobType).Remove(vob);
            allDict.Remove(vob.ID);
        }

        public TBase Get(TKey id)
        {
            TBase vob;
            allDict.TryGetValue(id, out vob);
            return vob;
        }

        public IEnumerable<TBase> GetAll()
        {
            return allDict.Values;
        }

        public int GetCount()
        {
            return allDict.Count;
        }
    }
}
