using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Utilities
{
    public struct BoolEvent
    {
        public delegate bool EventDelegate();

        List<EventDelegate> list;

        public void Add(EventDelegate eventHandler)
        {
            if (list == null)
                list = new List<EventDelegate>();

            list.Add(eventHandler);
        }

        public void Remove(EventDelegate eventHandler)
        {
            list.Remove(eventHandler);
            if (list.Count == 0)
                list = null;
        }

        public bool TrueForAll()
        {
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                    if (!list[i].Invoke())
                        return false;

            return true;
        }
    }

    public struct BoolEvent<T>
    {
        public delegate bool EventDelegate(T arg);

        List<EventDelegate> list;

        public void Add(EventDelegate eventHandler)
        {
            if (list == null)
                list = new List<EventDelegate>();

            list.Add(eventHandler);
        }

        public void Remove(EventDelegate eventHandler)
        {
            list.Remove(eventHandler);
            if (list.Count == 0)
                list = null;
        }

        public bool TrueForAll(T arg)
        {
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                    if (!list[i].Invoke(arg))
                        return false;

            return true;
        }
    }

    public struct BoolEvent<T0, T1>
    {
        public delegate bool EventDelegate(T0 arg0, T1 arg1);

        List<EventDelegate> list;

        public void Add(EventDelegate eventHandler)
        {
            if (list == null)
                list = new List<EventDelegate>();

            list.Add(eventHandler);
        }

        public void Remove(EventDelegate eventHandler)
        {
            list.Remove(eventHandler);
            if (list.Count == 0)
                list = null;
        }

        public bool TrueForAll(T0 arg0, T1 arg1)
        {
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                    if (!list[i].Invoke(arg0, arg1))
                        return false;

            return true;
        }
    }

    public struct BoolEvent<T0, T1, T2>
    {
        public delegate bool EventDelegate(T0 arg0, T1 arg1, T2 arg2);

        List<EventDelegate> list;

        public void Add(EventDelegate eventHandler)
        {
            if (list == null)
                list = new List<EventDelegate>();

            list.Add(eventHandler);
        }

        public void Remove(EventDelegate eventHandler)
        {
            list.Remove(eventHandler);
            if (list.Count == 0)
                list = null;
        }

        public bool TrueForAll(T0 arg0, T1 arg1, T2 arg2)
        {
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                    if (!list[i].Invoke(arg0, arg1, arg2))
                        return false;

            return true;
        }
    }
}
