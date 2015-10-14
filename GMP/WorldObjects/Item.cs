using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Enumeration;
using RakNet;
using GUC.Network;
using GUC.Types;

namespace GUC.Client.WorldObjects
{
    class Item : Vob
    {
        public oCItem gItem
        {
            get
            {
                return new oCItem(Program.Process, gVob.Address);
            }
        }

        public Item(uint id, ushort instanceID) : base(id, ItemInstance.InstanceList[instanceID].CreateItem())
        {
            CDDyn = true;
            CDStatic = true;
            gItem.Amount = 1;
        }
    }
}
