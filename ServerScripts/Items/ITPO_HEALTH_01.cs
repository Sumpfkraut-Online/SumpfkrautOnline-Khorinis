using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Enumeration;

namespace GUC.Server.Scripts.Items
{
    public class ITPO_HEALTH_01 : ItemInstance
    {
        public ITPO_HEALTH_01()
            : base("ITPO_HEALTH_01")
        {
            Name = "Trank";
            Value = 25;
            Visual = "ItPo_Health_01.3ds";
            MainFlags = MainFlags.ITEM_KAT_POTIONS;
            Flags = Flags.ITEM_MULTI;

            Materials = MaterialTypes.MAT_GLAS;
            Description = "Essenz der Heilung";

            

            CreateItemInstance();
        }
    }
}
