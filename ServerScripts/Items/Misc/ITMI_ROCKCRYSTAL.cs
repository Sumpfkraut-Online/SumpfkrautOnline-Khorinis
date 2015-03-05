using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_ROCKCRYSTAL : AbstractMisc
    {
        static ITMI_ROCKCRYSTAL ii;
        public static ITMI_ROCKCRYSTAL get()
        {
            if (ii == null)
                ii = new ITMI_ROCKCRYSTAL();
            return ii;
        }


        protected ITMI_ROCKCRYSTAL()
            : base("ITMI_ROCKCRYSTAL")
        {
            Name = "Bergkristall";
            Visual = "ItMi_Rockcrystal.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_STONE;


            CreateItemInstance();
        }
    }
}
