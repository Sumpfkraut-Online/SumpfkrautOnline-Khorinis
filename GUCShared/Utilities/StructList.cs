using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities
{
    public struct ValueTuple<T1, T2>
    {
        public T1 Item1;
        public T2 Item2;

        public ValueTuple(T1 item1, T2 item2)
        {
            this.Item1 = item1;
            this.Item2 = item2;
        }
    }
    public class List<T1, T2> : List<ValueTuple<T1, T2>>
    {
        public List() : base() { }
        public List(int capacity) : base(capacity) { }
        public List(IEnumerable<ValueTuple<T1,T2>> collection) : base(collection) { }

        public void Add(T1 item1, T2 item2)
        {
            this.Add(new ValueTuple<T1, T2>(item1, item2));
        }
    }

    public struct ValueTuple<T1, T2, T3>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;

        public ValueTuple(T1 item1, T2 item2, T3 item3)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
        }
    }
    public class List<T1, T2, T3> : List<ValueTuple<T1, T2, T3>>
    {
        public List() : base() { }
        public List(int capacity) : base(capacity) { }
        public List(IEnumerable<ValueTuple<T1, T2, T3>> collection) : base(collection) { }

        public void Add(T1 item1, T2 item2, T3 item3)
        {
            this.Add(new ValueTuple<T1, T2, T3>(item1, item2, item3));
        }
    }

    public struct ValueTuple<T1, T2, T3, T4>
    {
        public T1 Item1;
        public T2 Item2;
        public T3 Item3;
        public T4 Item4;

        public ValueTuple(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.Item1 = item1;
            this.Item2 = item2;
            this.Item3 = item3;
            this.Item4 = item4;
        }
    }
    public class List<T1, T2, T3, T4> : List<ValueTuple<T1, T2, T3, T4>>
    {
        public List() : base() { }
        public List(int capacity) : base(capacity) { }
        public List(IEnumerable<ValueTuple<T1, T2, T3, T4>> collection) : base(collection) { }

        public void Add(T1 item1, T2 item2, T3 item3, T4 item4)
        {
            this.Add(new ValueTuple<T1, T2, T3, T4>(item1, item2, item3, item4));
        }
    }
}
