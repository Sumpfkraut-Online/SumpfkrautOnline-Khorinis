using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Client.WorldObjects.Collections
{
    public class VobObjDictionary<TKey, TBase> where TBase : IVobObj<TKey>
    {
        Dictionary<TKey, TBase> dict = new Dictionary<TKey, TBase>();

        internal VobObjDictionary()
        {
            
        }

        internal virtual void Add(TBase vob)
        {
            dict.Add(vob.ID, vob);
        }

        internal virtual void Remove(TBase vob)
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
