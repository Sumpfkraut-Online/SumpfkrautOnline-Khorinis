using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_COAL : AbstractMisc
    {
        static ITMI_COAL ii;
        public static ITMI_COAL get()
        {
            if (ii == null)
                ii = new ITMI_COAL();
            return ii;
        }


        protected ITMI_COAL()
            : base("ITMI_COAL")
        {
            Name = "Kohle";
            Visual = "ItMi_Coal.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_STONE;


            CreateItemInstance();
        }
    }
}
