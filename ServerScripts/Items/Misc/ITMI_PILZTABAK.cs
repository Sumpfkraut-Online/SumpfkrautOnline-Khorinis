using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_PILZTABAK : AbstractMisc
    {
        static ITMI_PILZTABAK ii;
        public static ITMI_PILZTABAK get()
        {
            if (ii == null)
                ii = new ITMI_PILZTABAK();
            return ii;
        }


        protected ITMI_PILZTABAK()
            : base("ITMI_PILZTABAK")
        {
            Name = "Pilz Tabak";
            Visual = "ItMi_Pocket.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_LEATHER;


            CreateItemInstance();
        }
    }
}
