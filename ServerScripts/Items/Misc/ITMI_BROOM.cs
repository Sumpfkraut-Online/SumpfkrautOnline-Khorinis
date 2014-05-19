using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_BROOM : AbstractMisc
    {
        static ITMI_BROOM ii;
        public static ITMI_BROOM get()
        {
            if (ii == null)
                ii = new ITMI_BROOM();
            return ii;
        }


        protected ITMI_BROOM()
            : base("ITMI_BROOM")
        {
            Name = "Besen";
            Visual = "ItMi_Broom.3ds";
            Description = Name;

            ScemeName = "BROOM";

            CreateItemInstance();
        }
    }
}
