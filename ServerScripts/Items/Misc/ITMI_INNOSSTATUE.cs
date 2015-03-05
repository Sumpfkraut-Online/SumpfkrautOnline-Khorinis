using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_INNOSSTATUE : AbstractMisc
    {
        static ITMI_INNOSSTATUE ii;
        public static ITMI_INNOSSTATUE get()
        {
            if (ii == null)
                ii = new ITMI_INNOSSTATUE();
            return ii;
        }


        protected ITMI_INNOSSTATUE()
            : base("ITMI_INNOSSTATUE")
        {
            Name = "Innos Statue";
            Visual = "ItMi_InnosStatue.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialTypes.MAT_METAL;


            CreateItemInstance();
        }
    }
}
