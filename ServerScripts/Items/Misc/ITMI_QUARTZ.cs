using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_QUARTZ : AbstractMisc
    {
        static ITMI_QUARTZ ii;
        public static ITMI_QUARTZ get()
        {
            if (ii == null)
                ii = new ITMI_QUARTZ();
            return ii;
        }


        protected ITMI_QUARTZ()
            : base("ITMI_QUARTZ")
        {
            Name = "Gletscher Quartz";
            Visual = "ItMi_Quartz.3ds";
            Description = Name;

            Materials = Enumeration.MaterialTypes.MAT_STONE;


            CreateItemInstance();
        }
    }
}
