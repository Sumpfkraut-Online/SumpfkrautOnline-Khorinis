using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLDNECKLACE : AbstractMisc
    {
        static ITMI_GOLDNECKLACE ii;
        public static ITMI_GOLDNECKLACE get()
        {
            if (ii == null)
                ii = new ITMI_GOLDNECKLACE();
            return ii;
        }


        protected ITMI_GOLDNECKLACE()
            : base("ITMI_GOLDNECKLACE")
        {
            Name = "Goldene Halskette";
            Visual = "ItMi_GoldNecklace.3ds";
            Description = Name;
            Flags = Enumeration.Flags.ITEM_MULTI | Enumeration.Flags.ITEM_AMULET;

            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
