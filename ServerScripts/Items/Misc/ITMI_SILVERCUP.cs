using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SILVERCUP : AbstractMisc
    {
        static ITMI_SILVERCUP ii;
        public static ITMI_SILVERCUP get()
        {
            if (ii == null)
                ii = new ITMI_SILVERCUP();
            return ii;
        }


        protected ITMI_SILVERCUP()
            : base("ITMI_SILVERCUP")
        {
            Name = "Silberkelch";
            Visual = "ItMi_SilverCup.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
