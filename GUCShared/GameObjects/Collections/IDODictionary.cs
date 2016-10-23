using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.GameObjects.Collections
{
    // Copy of a Dictionary, but suited for GameObjects with better performance
    public class GODictionary<T> where T : IDObject
    {
        static readonly int[] primes = {
            3, 7, 11, 17, 23, 29, 37, 47, 59, 71, 89, 107, 131, 163, 197, 239, 293, 353, 431, 521, 631, 761, 919,
            1103, 1327, 1597, 1931, 2333, 2801, 3371, 4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591,
            17519, 21023, 25229, 30293, 36353, 43627, 52361, 62851 };

        static int GetPrime(int min)
        {
            for (int i = 0; i < primes.Length; i++)
            {
                if (primes[i] > min)
                    return primes[i];
            }
            return ushort.MaxValue+1;
        }

        struct Entry
        {
            public int id;
            public int next;
            public T obj;
        }

        int[] buckets;
        Entry[] entries;
        int count;
        public int Count { get { return this.count - this.freeCount; } }

        int freeCount;
        int freeList;

        int capacity;
        public int Capacity { get { return this.capacity; } }

        public void ForEach(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException("Action is null!");

            if (buckets != null)
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].id >= 0)
                        action(entries[i].obj);
                }
        }

        public void ForEachPredicate(Predicate<T> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException("Predicate is null!");

            if (buckets != null)
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].id >= 0)
                    {
                        if (!predicate(entries[i].obj))
                            return;
                    }
                }
        }

        public GODictionary()
        {
        }

        public GODictionary(int capacity)
        {
            if (capacity >= 0)
                Initialize(capacity);
        }

        void Initialize(int capacity)
        {
            this.capacity = GetPrime(capacity);
            buckets = new int[this.capacity];
            for (int i = 0; i < this.capacity; i++) buckets[i] = -1;
            entries = new Entry[this.capacity];
            freeList = -1;
        }

        public void Clear()
        {
            if (buckets != null)
            {
                for (int i = 0; i < this.capacity; i++) buckets[i] = -1;
                Array.Clear(entries, 0, count);
                freeList = -1;
                count = 0;
                freeCount = 0;
            }
        }

        public bool Contains(int id)
        {
            if (id >= 0 && id <= ushort.MaxValue && buckets != null)
            {
                for (int i = buckets[id % this.capacity]; i >= 0; i = entries[i].next)
                {
                    if (entries[i].id == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Add(T obj)
        {
            Insert(obj, true);
        }

        void Insert(T obj, bool add)
        {
            if (obj == null)
                throw new ArgumentNullException("Object is null!");

            int id = obj.ID;
            if (id < 0 || id > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("Object id is out of range! (0..65536) <> " + id);

            if (buckets == null)
                Initialize(1);

            int targetBucket = id % this.capacity;

            for (int i = buckets[targetBucket]; i >= 0; i = entries[i].next)
            {
                if (entries[i].id == id)
                {
                    if (add)
                    {
                        throw new ArgumentException("There is already a GameObject with this ID in the collection! " + id);
                    }

                    entries[i].obj = obj;
                    return;
                }
            }

            int index;
            if (freeCount > 0)
            {
                index = freeList;
                freeList = entries[index].next;
                freeCount--;
            }
            else
            {
                if (count == entries.Length)
                {
                    Resize();
                    targetBucket = id % this.capacity;
                }
                index = count;
                count++;
            }

            entries[index].id = id;
            entries[index].next = buckets[targetBucket];
            entries[index].obj = obj;
            buckets[targetBucket] = index;
        }

        void Resize()
        {
            if (this.capacity == ushort.MaxValue)
            {
                throw new Exception("GODictionary has reached maximum capacity! " + ushort.MaxValue);
            }

            this.capacity = GetPrime(this.capacity);

            int[] newBuckets = new int[this.capacity];
            for (int i = 0; i < this.capacity; i++) newBuckets[i] = -1;
            Entry[] newEntries = new Entry[this.capacity];
            Array.Copy(entries, 0, newEntries, 0, count);

            for (int i = 0; i < count; i++)
            {
                if (newEntries[i].id != -1)
                {
                    int bucket = newEntries[i].id % this.capacity;
                    newEntries[i].next = newBuckets[bucket];
                    newBuckets[bucket] = i;
                }
            }
            buckets = newBuckets;
            entries = newEntries;
        }

        public bool Remove(int id)
        {
            if (id < 0 || id > ushort.MaxValue)
                throw new ArgumentOutOfRangeException("ID is out of range! (0..65535) <> " + id);

            if (buckets != null)
            {
                int bucket = id % this.capacity;

                int last = -1;
                for (int i = buckets[bucket]; i >= 0; last = i, i = entries[i].next)
                {
                    if (entries[i].id == id)
                    {
                        if (last < 0)
                        {
                            buckets[bucket] = entries[i].next;
                        }
                        else
                        {
                            entries[last].next = entries[i].next;
                        }

                        entries[i].id = -1;
                        entries[i].next = freeList;
                        entries[i].obj = null;
                        freeList = i;
                        freeCount++;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool TryGet(int id, out T obj)
        {
            if (buckets != null && count > 0)
            {
                for (int i = buckets[id % this.capacity]; i >= 0; i = entries[i].next)
                {
                    if (entries[i].id == id)
                    {
                        obj = entries[i].obj;
                        return true;
                    }
                }
            }
            obj = null;
            return false;
        }
    }
}
