using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLDPLATE : AbstractMisc
    {
        static ITMI_GOLDPLATE ii;
        public static ITMI_GOLDPLATE get()
        {
            if (ii == null)
                ii = new ITMI_GOLDPLATE();
            return ii;
        }


        protected ITMI_GOLDPLATE()
            : base("ITMI_GOLDPLATE")
        {
            Name = "Goldener Teller";
            Visual = "ItMi_GoldPlate.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
