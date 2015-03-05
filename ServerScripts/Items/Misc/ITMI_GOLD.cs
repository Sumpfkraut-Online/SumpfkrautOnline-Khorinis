using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLD : AbstractMisc
    {
        static ITMI_GOLD ii;
        public static ITMI_GOLD get()
        {
            if (ii == null)
                ii = new ITMI_GOLD();
            return ii;
        }


        protected ITMI_GOLD()
            : base("ITMI_GOLD")
        {
            Name = "Gold";
            Visual = "ItMi_Gold.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_STONE;


            CreateItemInstance();
        }
    }
}
