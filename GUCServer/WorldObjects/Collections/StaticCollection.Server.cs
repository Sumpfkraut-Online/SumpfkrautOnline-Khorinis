using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    partial class StaticCollection<T> where T : GameObject
    {
        List<int> freeIDs = new List<int>();
        int idCounter = 0;

        partial void pCheckID(T obj)
        {
            if (obj.ID < 0 || obj.ID >= capacity) //search free ID
            {
                for (int i = freeIDs.Count - 1; i >= 0; i--)
                {
                    int id = freeIDs[i];
                    freeIDs.RemoveAt(i);
                    if (arr[i] == null) // because ServerScripts can set IDs manually
                    {
                        obj.ID = id;
                        return;
                    }
                }

                while (true)
                    if (idCounter >= capacity)
                    {
                        throw new Exception("StaticCollection reached maximum! " + capacity);
                    }
                    else if (idCounter >= arr.Length)
                    {
                        int newSize = (int)(1.5f * (arr.Length + 1));
                        if (newSize > capacity) newSize = capacity;
                        this.Resize(newSize);

                        obj.ID = idCounter++;
                        return;
                    }
                    else
                    {
                        int id = idCounter++;
                        if (arr[id] == null)
                        {
                            obj.ID = id;
                            return;
                        }
                    }
            }
        }

        partial void pRemove(T obj)
        {
            freeIDs.Add(obj.ID);
            obj.ID = -1;
        }
    }
}
