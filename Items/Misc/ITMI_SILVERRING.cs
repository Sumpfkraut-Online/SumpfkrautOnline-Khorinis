using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SILVERRING : AbstractMisc
    {
        static ITMI_SILVERRING ii;
        public static ITMI_SILVERRING get()
        {
            if (ii == null)
                ii = new ITMI_SILVERRING();
            return ii;
        }


        protected ITMI_SILVERRING()
            : base("ITMI_SILVERRING")
        {
            Name = "Silberner Ring";
            Visual = "ItMi_SilverRing.3ds";
            Description = Name;
            Flags = Enumeration.Flags.ITEM_MULTI | Enumeration.Flags.ITEM_RING;

            Materials = Enumeration.MaterialTypes.MAT_METAL;


            CreateItemInstance();
        }
    }
}
