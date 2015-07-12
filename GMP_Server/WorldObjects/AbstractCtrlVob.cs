using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Network;
using RakNet;
using GUC.Enumeration;
using GUC.Network;

namespace GUC.Server.WorldObjects
{
    public abstract class AbstractCtrlVob : AbstractVob
    {
        internal Client VobController;

        internal void UpdateCtrl()
        {

        }

        internal static void UpdateCtrlNPCs()
        {
            for (int i = 0; i < sWorld.NPCList.Count; i++)
            {
                sWorld.NPCList[i].UpdateCtrl();
            }
        }
    }
}
