using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Instances;
using GUC.Server.Network;
using GUC.Server.Network.Messages;

namespace GUC.WorldObjects.Collections
{
    public static partial class InstanceCollection
    {
        static List<int> freeIDs = new List<int>();
        static int idCount = 0;
        static partial void CheckID(BaseVobInstance inst)
        {
            if (inst.ID < 0 || inst.ID > MAX_INSTANCES) //search free ID
            {
                int id;
                
                while (freeIDs.Count > 0)
                {
                    id = freeIDs[0];
                    freeIDs.RemoveAt(0);
                    if (!instances.ContainsKey(id)) // because ServerScripts can set IDs manually
                    {
                        inst.ID = id;
                        return;
                    }
                }

                while (true)
                    if (idCount >= MAX_INSTANCES)
                    {
                        throw new Exception("InstanceCollection reached maximum! " + MAX_INSTANCES);
                    }
                    else
                    {
                        id = idCount++;
                        if (!instances.ContainsKey(id))
                        {
                            inst.ID = id;
                            return;
                        }
                    }
            }
        }

        static partial void pAdd(BaseVobInstance inst)
        {
            if (!inst.IsStatic)
            {
                inst.WriteCreate();
            }
        }

        static partial void pRemove(BaseVobInstance inst)
        {
            if (!inst.IsStatic)
            {
                inst.WriteDelete();
            }
            freeIDs.Add(inst.ID);
            inst.ID = -1;
        }
    }
}
