using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities
{
    public struct ReadOnlyList<T> : IEnumerable<T>
    {
        List<T> list;
        public ReadOnlyList(List<T> list)
        {
            this.list = list;
        }

        public T this[int index] { get { return this.list[index]; } }
        public IEnumerator<T> GetEnumerator() { return list.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }
        public int IndexOf(T item) { return list.IndexOf(item); }
        public int IndexOf(T item, int index) { return list.IndexOf(item, index); }
        public int IndexOf(T item, int index, int count) { return list.IndexOf(item, index, count); }
        public bool Contains(T item) { return list.Contains(item); }
        public void CopyTo(T[] array, int arrayIndex) { list.CopyTo(array, arrayIndex); }
        public int Count { get { return list.Count; } }
        public static implicit operator ReadOnlyList<T>(List<T> list) { return new ReadOnlyList<T>(list); }     
        public void ForEach(Action<T> action) { list.ForEach(action); }
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) { return list.ConvertAll(converter); }
        public bool Exists(Predicate<T> match) { return list.Exists(match); }
        public T Find(Predicate<T> match) { return list.Find(match); }
        public List<T> FindAll(Predicate<T> match) { return list.FindAll(match); }
        public int FindIndex(Predicate<T> match) { return list.FindIndex(match); }
        public int FindIndex(int startIndex, Predicate<T> match) { return list.FindIndex(startIndex, match); }
        public int FindIndex(int startIndex, int count, Predicate<T> match) { return list.FindIndex(startIndex, count, match); }
        public T FindLast(Predicate<T> match) { return list.FindLast(match); }
        public int FindLastIndex(Predicate<T> match) { return list.FindLastIndex(match); }
        public int FindLastIndex(int startIndex, Predicate<T> match) { return list.FindLastIndex(startIndex, match); }
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) { return list.FindLastIndex(startIndex, count, match); }
        public List<T> GetRange(int index, int count) { return list.GetRange(index, count); }
        public int LastIndexOf(T item) { return list.LastIndexOf(item); }
        public int LastIndexOf(T item, int index) { return list.LastIndexOf(item, index); }
        public int LastIndexOf(T item, int index, int count) { return list.LastIndexOf(item, index, count); }
        public T[] ToArray() { return list.ToArray(); }
        public bool TrueForAll(Predicate<T> match) { return list.TrueForAll(match); }

        public override bool Equals(object obj) { return list.Equals(obj); }
        public override int GetHashCode() { return list.GetHashCode(); }
    }

    /*public struct ReadOnlyList<T1, T2> : IEnumerable<T1, T2>
    {
        List<T1, T2> list;
        public ReadOnlyList(List<T1, T2> list)
        {
            this.list = list;
        }

        public ValueTuple<T1, T2> this[int index] { get { return this.list[index]; } }
        public IEnumerator<ValueTuple<T1, T2>> GetEnumerator() { return list.GetEnumerator(); }
        IEnumerator IEnumerable.GetEnumerator() { return list.GetEnumerator(); }
        public int IndexOf(T1 item1, T2 item2) { return list.IndexOf(new ValueTuple<T1, T2>(item1, item2)); }
        public int IndexOf(T item, int index) { return list.IndexOf(item, index); }
        public int IndexOf(T item, int index, int count) { return list.IndexOf(item, index, count); }
        public bool Contains(T item) { return list.Contains(item); }
        public void CopyTo(T[] array, int arrayIndex) { list.CopyTo(array, arrayIndex); }
        public int Count { get { return list.Count; } }
        public static implicit operator ReadOnlyList<T>(List<T> list) { return new ReadOnlyList<T>(list); }
        public void ForEach(Action<T> action) { list.ForEach(action); }
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) { return list.ConvertAll(converter); }
        public bool Exists(Predicate<T> match) { return list.Exists(match); }
        public T Find(Predicate<T> match) { return list.Find(match); }
        public List<T> FindAll(Predicate<T> match) { return list.FindAll(match); }
        public int FindIndex(Predicate<T> match) { return list.FindIndex(match); }
        public int FindIndex(int startIndex, Predicate<T> match) { return list.FindIndex(startIndex, match); }
        public int FindIndex(int startIndex, int count, Predicate<T> match) { return list.FindIndex(startIndex, count, match); }
        public T FindLast(Predicate<T> match) { return list.FindLast(match); }
        public int FindLastIndex(Predicate<T> match) { return list.FindLastIndex(match); }
        public int FindLastIndex(int startIndex, Predicate<T> match) { return list.FindLastIndex(startIndex, match); }
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) { return list.FindLastIndex(startIndex, count, match); }
        public List<T> GetRange(int index, int count) { return list.GetRange(index, count); }
        public int LastIndexOf(T item) { return list.LastIndexOf(item); }
        public int LastIndexOf(T item, int index) { return list.LastIndexOf(item, index); }
        public int LastIndexOf(T item, int index, int count) { return list.LastIndexOf(item, index, count); }
        public T[] ToArray() { return list.ToArray(); }
        public bool TrueForAll(Predicate<T> match) { return list.TrueForAll(match); }

        public override bool Equals(object obj) { return list.Equals(obj); }
        public override int GetHashCode() { return list.GetHashCode(); }
    }*/
}
