using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_PLIERS : AbstractMisc
    {
        static ITMI_PLIERS ii;
        public static ITMI_PLIERS get()
        {
            if (ii == null)
                ii = new ITMI_PLIERS();
            return ii;
        }


        protected ITMI_PLIERS()
            : base("ITMI_PLIERS")
        {
            Name = "Zange";
            Visual = "ItMi_Pliers.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_METAL;

            CreateItemInstance();
        }
    }
}
