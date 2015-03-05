using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SULFUR : AbstractMisc
    {
        static ITMI_SULFUR ii;
        public static ITMI_SULFUR get()
        {
            if (ii == null)
                ii = new ITMI_SULFUR();
            return ii;
        }


        protected ITMI_SULFUR()
            : base("ITMI_SULFUR")
        {
            Flags = Enumeration.Flags.ITEM_MULTI;
            Name = "Schwefel";
            Visual = "ItMi_Sulfur.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_WOOD;


            CreateItemInstance();
        }
    }
}
