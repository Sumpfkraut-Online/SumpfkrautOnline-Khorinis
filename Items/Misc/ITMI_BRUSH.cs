using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_BRUSH : AbstractMisc
    {
        static ITMI_BRUSH ii;
        public static ITMI_BRUSH get()
        {
            if (ii == null)
                ii = new ITMI_BRUSH();
            return ii;
        }


        protected ITMI_BRUSH()
            : base("ITMI_BRUSH")
        {
            Name = "Bürste";
            Visual = "ItMi_Brush.3ds";
            Description = Name;

            ScemeName = "BRUSH";

            CreateItemInstance();
        }
    }
}
