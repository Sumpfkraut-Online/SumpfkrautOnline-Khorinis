using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Server.WorldObjects
{
    public interface IVobClass
    {
        VobType GetVobType();
    }

    public abstract class VobDict<TKey, TBase> where TBase : IVobClass
    {
        protected Dictionary<TKey, TBase> dict;

        internal VobDict()
        {
            dict = new Dictionary<TKey, TBase>();
        }

        internal void Add(TKey id, TBase vob)
        {
            dict.Add(id, vob);
        }

        public TBase Get(TKey id)
        {
            TBase vob;
            dict.TryGetValue(id, out vob);
            return vob;
        }

        public IEnumerable<TBase> GetAll()
        {
            return dict.Values;
        }
    }

    public class VobDict<TKey, TValue, TBase> : VobDict<TKey, TBase> where TValue : TBase where TBase : IVobClass
    {
        new public TValue Get(TKey id)
        {
            return (TValue)base.Get(id);
        }
    }

    class VobCollection<TKey, TBase> where TBase : IVobClass
    {
        VobDict<TKey, TBase>[] vobDicts;
        Dictionary<TKey, TBase> allDict;

        internal VobCollection()
        {
            vobDicts = new VobDict<TKey, TBase>[(int)VobType._Maximum];
            allDict = new Dictionary<TKey, TBase>();
            for (int i = 1 /*skip VobType.None*/; i < (int)VobType._Maximum; i++)
            {
                vobDicts[i] = new VobDict<TKey, TBase>();
            }
        }

        public TBase GetAny(TKey id)
        {
            TBase vob;
            allDict.TryGetValue(id, out vob);
            return vob;
        }

        public IEnumerable<TBase> GetAll()
        {
            return allDict.Values;
        }
    }
}
