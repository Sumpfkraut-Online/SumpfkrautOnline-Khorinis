using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_POCKET : AbstractMisc
    {
        static ITMI_POCKET ii;
        public static ITMI_POCKET get()
        {
            if (ii == null)
                ii = new ITMI_POCKET();
            return ii;
        }


        protected ITMI_POCKET()
            : base("ITMI_POCKET")
        {
            Name = "Lederbeutel";
            Visual = "ItMi_Pocket.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_LEATHER;
            ScemeName = "MAPSEALED";

            CreateItemInstance();
        }
    }
}
