using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_LUTE : AbstractMisc
    {
        static ITMI_LUTE ii;
        public static ITMI_LUTE get()
        {
            if (ii == null)
                ii = new ITMI_LUTE();
            return ii;
        }


        protected ITMI_LUTE()
            : base("ITMI_LUTE")
        {
            Name = "Laute";
            Visual = "ItMi_Lute.3ds";
            Description = Name;

            ScemeName = "LUTE";

            CreateItemInstance();
        }
    }
}
