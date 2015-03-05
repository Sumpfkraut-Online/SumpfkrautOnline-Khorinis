using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SAW : AbstractMisc
    {
        static ITMI_SAW ii;
        public static ITMI_SAW get()
        {
            if (ii == null)
                ii = new ITMI_SAW();
            return ii;
        }


        protected ITMI_SAW()
            : base("ITMI_SAW")
        {
            Name = "Säge";
            Visual = "ItMi_Saw.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_METAL;

            CreateItemInstance();
        }
    }
}
