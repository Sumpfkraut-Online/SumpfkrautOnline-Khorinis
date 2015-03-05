using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_HONIGTABAK : AbstractMisc
    {
        static ITMI_HONIGTABAK ii;
        public static ITMI_HONIGTABAK get()
        {
            if (ii == null)
                ii = new ITMI_HONIGTABAK();
            return ii;
        }


        protected ITMI_HONIGTABAK()
            : base("ITMI_HONIGTABAK")
        {
            Name = "Honig Tabak";
            Visual = "ItMi_Pocket.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_LEATHER;


            CreateItemInstance();
        }
    }
}
