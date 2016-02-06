using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.WorldObjects.Collections
{
    public class VobObjCollection<TDictionary, TKey, TBase> where TBase : IVobObj<TKey> where TDictionary : VobObjDictionary<TKey, TBase>, new()
    {
        protected TDictionary[] vobDicts = new TDictionary[(int)VobTypes.Maximum];
        protected Dictionary<TKey, TBase> allDict = new Dictionary<TKey, TBase>();

        public VobObjCollection()
        {
            for (int i = 0; i < (int)VobTypes.Maximum; i++)
            {
                vobDicts[i] = new TDictionary();
            }
        }

        public TDictionary GetDict(VobTypes type)
        {
            return vobDicts[(int)type];
        }

        public TBase Get(VobTypes type, TKey id)
        {
            return GetDict(type).Get(id);
        }

        public IEnumerable<TBase> GetAll(VobTypes type)
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
