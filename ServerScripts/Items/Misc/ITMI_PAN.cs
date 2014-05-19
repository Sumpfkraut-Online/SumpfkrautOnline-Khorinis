using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_PAN : AbstractMisc
    {
        static ITMI_PAN ii;
        public static ITMI_PAN get()
        {
            if (ii == null)
                ii = new ITMI_PAN();
            return ii;
        }


        protected ITMI_PAN()
            : base("ITMI_PAN")
        {
            Name = "Pfanne";
            Visual = "ItMi_Pan.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_METAL;

            CreateItemInstance();
        }
    }
}
