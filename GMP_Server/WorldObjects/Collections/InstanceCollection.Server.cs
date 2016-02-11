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
            if (inst.ID < 0 || inst.ID > GameObject.MAX_ID)
            {
                //search free ID
                if (freeIDs.Count > 0)
                {
                    inst.id = freeIDs[0];
                    freeIDs.RemoveAt(0);
                }
                else
                {
                    while (true)
                        if (idCount >= GameObject.MAX_ID)
                        {
                            throw new Exception("InstanceCollection reached maximum! " + GameObject.MAX_ID);
                        }
                        else
                        {
                            inst.id = idCount++;
                            if (!instances.ContainsKey(inst.id))
                                break;
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
            inst.id = -1;
        }
    }
}
