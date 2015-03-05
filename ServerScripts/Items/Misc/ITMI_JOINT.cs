using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_JOINT : AbstractMisc
    {
        static ITMI_JOINT ii;
        public static ITMI_JOINT get()
        {
            if (ii == null)
                ii = new ITMI_JOINT();
            return ii;
        }


        protected ITMI_JOINT()
            : base("ITMI_JOINT")
        {
            Name = "Ein Stengel Sumpfkraut";
            Visual = "ItMi_Joint_US.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_LEATHER;
            ScemeName = "JOINT";

            CreateItemInstance();
        }
    }
}
