using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLDRING : AbstractMisc
    {
        static ITMI_GOLDRING ii;
        public static ITMI_GOLDRING get()
        {
            if (ii == null)
                ii = new ITMI_GOLDRING();
            return ii;
        }


        protected ITMI_GOLDRING()
            : base("ITMI_GOLDRING")
        {
            Name = "Goldener Ring";
            Visual = "ItMi_GoldRing.3ds";
            Description = Name;
            Flags = Enumeration.Flags.ITEM_MULTI | Enumeration.Flags.ITEM_RING;

            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
