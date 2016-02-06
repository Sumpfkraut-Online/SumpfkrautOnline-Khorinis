using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public abstract class VobObjDictionary<TKey, TBase> where TBase : IVobObj<TKey>
    {
        Dictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        new public TValue Get(TKey id)
        {
            TValue vob;
            dict.TryGetValue(id, out vob);
            return vob;
        }

        new public IEnumerable<TValue> GetAll()
        {
            return dict.Values;
        }

        internal void Add(TBase vob)
        {
            dict.Add(vob.ID, vob);
        }

        internal void Remove(TBase vob)
        {
            dict.Remove(vob.ID);
        }

        public TBase Get(TKey id)
        {
            TBase vob;
            dict.TryGetValue(id, out vob);
            return (TBase)vob;
        }

        public IEnumerable<TBase> GetAll()
        {
            return dict.Values;
        }

        public int GetCount()
        {
            return dict.Count;
        }
    }
}
