using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects.Collections;
using GUC.Types;

namespace GUC.WorldObjects.Collections
{
    interface VobTypeObject
    {
        GUCVobTypes VobType { get; }
    }

    class VobTypeCollection<T> where T : class, VobTypeObject
    {
        DynamicCollection<T> vobs;
        DynamicCollection<T>[] typeDict;

        internal VobTypeCollection()
        {
            vobs = new DynamicCollection<T>();
            typeDict = new DynamicCollection<T>[(int)GUCVobTypes.Maximum];
            for (int i = 0; i < typeDict.Length; i++)
            {
                typeDict[i] = new DynamicCollection<T>();
            }
        }

        public void Add(T obj, ref int id, ref int typeID)
        {
            vobs.Add(obj, ref id);
            typeDict[(int)obj.VobType].Add(obj, ref typeID);
        }

        public void Remove(T obj, ref int id, ref int typeID)
        {
            vobs.Remove(ref id);
            typeDict[(int)obj.VobType].Remove(ref typeID);
        } 

        public void ForEach(Action<T> action)
        {
            vobs.ForEach(action);
        }

        public void ForEachOfType(GUCVobTypes type, Action<T> action)
        {
            typeDict[(int)type].ForEach(action);
        }

        public int GetCount() { return this.vobs.Count; }
        public int GetCountOfType(GUCVobTypes type) { return this.typeDict[(int)type].Count; }
        
    }
}
