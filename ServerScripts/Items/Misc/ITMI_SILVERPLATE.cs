using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SILVERPLATE : AbstractMisc
    {
        static ITMI_SILVERPLATE ii;
        public static ITMI_SILVERPLATE get()
        {
            if (ii == null)
                ii = new ITMI_SILVERPLATE();
            return ii;
        }


        protected ITMI_SILVERPLATE()
            : base("ITMI_SILVERPLATE")
        {
            Name = "Silberteller";
            Visual = "ItMi_SilverPlate.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
