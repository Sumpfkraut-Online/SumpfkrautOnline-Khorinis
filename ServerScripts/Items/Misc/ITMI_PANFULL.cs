using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_PANFULL : AbstractMisc
    {
        static ITMI_PANFULL ii;
        public static ITMI_PANFULL get()
        {
            if (ii == null)
                ii = new ITMI_PANFULL();
            return ii;
        }


        protected ITMI_PANFULL()
            : base("ITMI_PANFULL")
        {
            Name = "Pfanne";
            Visual = "ItMi_PanFull.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_METAL;

            CreateItemInstance();
        }
    }
}
