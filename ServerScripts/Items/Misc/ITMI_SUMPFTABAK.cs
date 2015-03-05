using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SUMPFTABAK : AbstractMisc
    {
        static ITMI_SUMPFTABAK ii;
        public static ITMI_SUMPFTABAK get()
        {
            if (ii == null)
                ii = new ITMI_SUMPFTABAK();
            return ii;
        }


        protected ITMI_SUMPFTABAK()
            : base("ITMI_SUMPFTABAK")
        {
            Name = "Krautabak";
            Visual = "ItMi_Pocket.3ds";
            Description = Name;

            Materials = Enumeration.MaterialType.MAT_LEATHER;


            CreateItemInstance();
        }
    }
}
