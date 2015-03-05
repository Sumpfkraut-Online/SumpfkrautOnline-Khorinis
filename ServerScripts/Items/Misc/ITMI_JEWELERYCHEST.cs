using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_JEWELERYCHEST : AbstractMisc
    {
        static ITMI_JEWELERYCHEST ii;
        public static ITMI_JEWELERYCHEST get()
        {
            if (ii == null)
                ii = new ITMI_JEWELERYCHEST();
            return ii;
        }


        protected ITMI_JEWELERYCHEST()
            : base("ITMI_JEWELERYCHEST")
        {
            Name = "Juwelen Schatulle";
            Visual = "ItMi_JeweleryChest.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
