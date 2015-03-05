using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLDCHEST : AbstractMisc
    {
        static ITMI_GOLDCHEST ii;
        public static ITMI_GOLDCHEST get()
        {
            if (ii == null)
                ii = new ITMI_GOLDCHEST();
            return ii;
        }


        protected ITMI_GOLDCHEST()
            : base("ITMI_GOLDCHEST")
        {
            Name = "Schatulle";
            Visual = "ItMi_GoldChest.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
