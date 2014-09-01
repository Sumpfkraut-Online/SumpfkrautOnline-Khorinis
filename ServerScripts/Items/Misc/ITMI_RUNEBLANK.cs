using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_RUNEBLANK : AbstractMisc
    {
        static ITMI_RUNEBLANK ii;
        public static ITMI_RUNEBLANK get()
        {
            if (ii == null)
                ii = new ITMI_RUNEBLANK();
            return ii;
        }


        protected ITMI_RUNEBLANK()
            : base("ITMI_RUNEBLANK")
        {
            Name = "Runenstein";
            Visual = "ItMi_RuneBlank.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_STONE;

            CreateItemInstance();
        }
    }
}
