using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_DOPPELTABAK : AbstractMisc
    {
        static ITMI_DOPPELTABAK ii;
        public static ITMI_DOPPELTABAK get()
        {
            if (ii == null)
                ii = new ITMI_DOPPELTABAK();
            return ii;
        }


        protected ITMI_DOPPELTABAK()
            : base("ITMI_DOPPELTABAK")
        {
            Name = "Doppelter Apfel";
            Visual = "ItMi_Pocket.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_LEATHER;


            CreateItemInstance();
        }
    }
}
