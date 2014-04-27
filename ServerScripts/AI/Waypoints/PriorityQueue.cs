using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.AI.Waypoints
{
    public class PriorityQueue<E> where E : IComparable, IEquatable<E>
    {
        private const int DEFAULT_CAPACITY = 32;

        /// <summary>
        /// An array of objects, which will be created with a default capacity of 32!
        /// Some of the elements can be null, look at the variable size to get the real element-count!
        /// </summary>
        protected E[] mList;

        protected int mSize = 0;

        public PriorityQueue()
            : this(DEFAULT_CAPACITY)
        {
            
        }

        public PriorityQueue(int capacity)
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
            for(int i = 0; i  < mSize; i++)
            {
                if (obj.Equals( mList[i]))
                    return i;
            }
            return -1;
        }

        public E getAt(int i)
        {
            return mList[i];
        }

        public bool contains(E obj)
        {
            return getIndexOf(obj) != -1;
        }
        public E removeAt(int i)
        {
            if (i < 0 || i >= mSize)
                throw new ArgumentNullException();
            int size = mSize;
            mSize -= 1;
            E result = mList[0];
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

            return result;

        }

        protected void shiftUp(int i, E obj)
        {
            while (i > 0)
            {
                int previous = (i - 1) >> 1;
                E e = mList[previous];
                if (((IComparable)obj).CompareTo(e) >= 0)
                    break;
                mList[i] = e;
                i = previous;
            }
            mList[i] = obj;
        }

        protected void siftDown(int i, E obj)//https://de.wikipedia.org/wiki/Bin%C3%A4rer_Heap
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
                i = next;
            }

            mList[i] = obj;
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
