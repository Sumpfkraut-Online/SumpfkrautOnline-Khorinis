using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_DARKPEARL : AbstractMisc
    {
        static ITMI_DARKPEARL ii;
        public static ITMI_DARKPEARL get()
        {
            if (ii == null)
                ii = new ITMI_DARKPEARL();
            return ii;
        }


        protected ITMI_DARKPEARL()
            : base("ITMI_DARKPEARL")
        {
            Name = "Schwarze Perle";
            Visual = "ItMi_DarkPearl.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_STONE;


            CreateItemInstance();
        }
    }
}
