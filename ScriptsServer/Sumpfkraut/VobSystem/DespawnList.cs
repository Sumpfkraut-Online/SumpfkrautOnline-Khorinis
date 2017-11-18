using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem
{
    class DespawnList<T> where T : BaseVobInst
    {
        T[] vobs;
        public int Capacity { get { return vobs.Length; } }

        int count = 0;
        public int Count { get { return count; } }

        int index = 0;

        public DespawnList(int capacity)
        {
            vobs = new T[capacity];
        }

        public void AddVob(T vob)
        {
            if (count == Capacity)
            {
                vobs[index].Despawn();
                vobs[index] = vob;
                index++;
                if (index == Capacity)
                    index = 0;
            }
            else
            {
                vobs[count] = vob;
                count++;
            }
        }
    }
}
