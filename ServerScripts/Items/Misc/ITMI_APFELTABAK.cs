using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_APFELTABAK : AbstractMisc
    {
        static ITMI_APFELTABAK ii;
        public static ITMI_APFELTABAK get()
        {
            if (ii == null)
                ii = new ITMI_APFELTABAK();
            return ii;
        }


        protected ITMI_APFELTABAK()
            : base("ITMI_APFELTABAK")
        {
            Name = "Apfel Tabak";
            Visual = "ItMi_Pocket.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_LEATHER;


            CreateItemInstance();
        }
    }
}
