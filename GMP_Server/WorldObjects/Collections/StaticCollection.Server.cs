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

        partial void CheckID(T obj)
        {
            if (obj.ID < 0 || obj.ID >= GameObject.MAX_ID) //search free ID
            {
                int id;

                while (freeIDs.Count > 0)
                {
                    id = freeIDs[0];
                    freeIDs.RemoveAt(0);
                    if (arr[id] == null) // because ServerScripts can set IDs manually
                    {
                        obj.ID = id;
                        return;
                    }
                }

                while (true)
                    if (idCounter >= GameObject.MAX_ID)
                    {
                        throw new Exception("StaticCollection reached maximum! " + GameObject.MAX_ID);
                    }
                    else
                    {
                        id = idCounter++;
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
