using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Utilities;

namespace GUC.Scripts.Arena.GameModes.BattleRoyale
{
    class BRBucketCollector<T> : IEnumerable
    {
        List<T, double> list = new List<T, double>();

        public void Add(T obj, double prob)
        {
            list.Add(obj, prob);
        }

        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();

        public static implicit operator BRRandomizer<T>(BRBucketCollector<T> coll)
        {
            return new BRRandomizer<T>(coll.list);
        }
    }

    class BRRandomizer<T>
    {
        ValueTuple<T, double>[] collection;

        public BRRandomizer(IEnumerable<T, double> input)
        {
            this.collection = input.ToArray();

            if (collection.Length <= 0)
                return;

            // Normalize, turn into cumulative probabilities
            double sum = collection.Sum(o => o.Item2);
            collection[0].Item2 /= sum;
            for (int i = 1; i < collection.Length; i++)
                collection[i].Item2 = collection[i - 1].Item2 + collection[i].Item2 / sum;
        }

        public T GetRandom()
        {
            return GetRandom(Randomizer.GetDouble());
        }

        public T GetRandom(double randomValue)
        {
            int upper = collection.Length;
            int lower = -1;

            int index = upper / 2;
            while (true)
            {
                if (index == upper)
                {
                    break;
                }
                if (index == lower)
                {
                    index++;
                    break;
                }

                if (collection[index].Item2 < randomValue)
                {
                    // move up
                    lower = index;
                    index += (upper - index + 1) / 2; // round up
                }
                else
                {
                    // move down
                    upper = index;
                    index -= (index - lower + 1) / 2; // round up
                }
            }
            if (index == collection.Length)
                index--;

            return collection[index].Item1;
        }
    }
}
