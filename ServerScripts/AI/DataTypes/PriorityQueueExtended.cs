using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace GUC.Server.Scripts.AI.DataTypes
{
    public class PriorityQueueExtended<E> where E : IEquatable<E>
    {
        private const int DEFAULT_CAPACITY = 32;

        /// <summary>
        /// An array of objects, which will be created with a default capacity of 32!
        /// Some of the elements can be null, look at the variable size to get the real element-count!
        /// </summary>
        protected E[] mList;

        protected int mSize = 0;

        protected Dictionary<E, int> mIndexDict = new Dictionary<E, int>();

        protected Comparer<E> mComparer = null;

        public PriorityQueueExtended()
            : this(DEFAULT_CAPACITY)
        {
            
        }

        public PriorityQueueExtended(Comparer<E> comparer)
            : this(DEFAULT_CAPACITY)
        {
            mComparer = comparer;
        }

        

        public PriorityQueueExtended(int capacity)
        {
            mList = new E[capacity];
        }


        protected void grow(int capacity)
        {
            int newCap = mList.Length + ((mList.Length > 128) ? mList.Length : mList.Length >> 1);

            if (newCap > int.MaxValue)
                newCap = capacity;

            E[] obj = new E[newCap];
            Array.Copy(mList, obj, mSize);
            mList = obj;
        }

        public void add(E obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Object can't be null!");

            if (mSize >= mList.Length)
                grow(mSize + 1);

            mSize += 1;

            if (mSize == 1)
                mList[0] = obj;
            else
                shiftUp(mSize - 1, obj);
        }

        public bool isEmpty()
        {
            if (mSize == 0)
                return true;
            return false;
        }

        public int Size { get { return mSize; } }
        public int Count { get { return mSize; } }
        public int Length { get { return mSize; } }

        public E remove(E obj)
        {
            return removeAt(getIndexOf(obj));
        }

        public E minRemove()
        {
            return removeAt(0);
        }

        public E peek()
        {
            return getMin();
        }
        public E getMin()
        {
            if (mSize == 0)
                return default(E);
            return mList[0];
        }

        public int getIndexOf(E obj)
        {
            int index = 0;
            mIndexDict.TryGetValue(obj, out index);
            return index;
        }

        public E getAt(int i)
        {
            return mList[i];
        }

        public bool contains(E obj)
        {
            return mIndexDict.ContainsKey(obj);
        }
        public E removeAt(int i)
        {
            if (i < 0 || i >= mSize)
                throw new ArgumentNullException();
            int size = mSize;
            mSize -= 1;

            E result = mList[i];
            

            if (mSize == 0)
            {
                mList[0] = default(E);
                return result;
            }
            else
            {
                E obj = mList[mSize];
                mList[mSize] = default(E);//Remove the last element
                siftDown(i, obj);
                if (mList[i].Equals(obj))
                {
                    shiftUp(i, obj);
                }
            }
            mIndexDict.Remove(result);

            return result;

        }

        protected void shiftUp(int i, E obj)
        {
            if (mComparer == null)
                shiftUpCompareable(i, obj);
            else
                shiftUpComparer(i, obj);
        }

        protected void shiftUpCompareable(int i, E obj)
        {
            while (i > 0)
            {
                int previous = (i - 1) >> 1;
                E e = mList[previous];
                if (((IComparable)obj).CompareTo(e) >= 0)
                    break;
                mList[i] = e;
                mIndexDict.Remove(e);
                mIndexDict.Add(e, i);
                i = previous;
            }
            mList[i] = obj;
            mIndexDict.Remove(obj);
            mIndexDict.Add(obj, i);
        }

        protected void shiftUpComparer(int i, E obj)
        {
            while (i > 0)
            {
                int previous = (i - 1) >> 1;
                E e = mList[previous];
                if (mComparer.Compare(obj, e) >= 0)
                    break;
                mList[i] = e;
                mIndexDict.Remove(e);
                mIndexDict.Add(e, i);
                i = previous;
            }
            mList[i] = obj;
            mIndexDict.Remove(obj);
            mIndexDict.Add(obj, i);
        }

        protected void siftDown(int i, E obj)
        {
            if (mComparer == null)
                siftDownCompareable(i, obj);
            else
                siftDownComparer(i, obj);
        }

        protected void siftDownCompareable(int i, E obj)//https://de.wikipedia.org/wiki/Bin%C3%A4rer_Heap
        {
            int halfSize = mSize >> 1;
            while (i < halfSize)
            {
                int next = (i << 1) + 1;
                int right = next + 1;

                E n = mList[next];

                if (right < mSize && ((IComparable)n).CompareTo(mList[right]) > 0)
                {
                    n = mList[right];
                    next = right;
                }
                if (((IComparable)obj).CompareTo(n) <= 0)
                    break;

                mList[i] = n;
                mIndexDict.Remove(n);
                mIndexDict.Add(n, i);
                i = next;
            }

            mList[i] = obj;
            mIndexDict.Remove(obj);
            mIndexDict.Add(obj, i);
        }


        protected void siftDownComparer(int i, E obj)
        {
            int halfSize = mSize >> 1;
            while (i < halfSize)
            {
                int next = (i << 1) + 1;
                int right = next + 1;

                E n = mList[next];

                if (right < mSize && (mComparer.Compare(n, mList[right]) > 0))
                {
                    n = mList[right];
                    next = right;
                }
                if (mComparer.Compare(obj, n) <= 0)
                    break;

                mList[i] = n;
                mIndexDict.Remove(n);
                mIndexDict.Add(n, i);
                i = next;
            }

            mList[i] = obj;
            mIndexDict.Remove(obj);
            mIndexDict.Add(obj, i);
        }


        public void printValues()
        {
            Console.WriteLine("Start-Print!");
            for (int i = 0; i < mSize; i++)
            {
                Console.WriteLine(mList[i]);
            }
            Console.WriteLine("Stop-Print!");
        }

    }
}
