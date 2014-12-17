using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SILVERCHALICE : AbstractMisc
    {
        static ITMI_SILVERCHALICE ii;
        public static ITMI_SILVERCHALICE get()
        {
            if (ii == null)
                ii = new ITMI_SILVERCHALICE();
            return ii;
        }


        protected ITMI_SILVERCHALICE()
            : base("ITMI_SILVERCHALICE")
        {
            Name = "Silberne Schale";
            Visual = "ItMi_SilverChalice.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialTypes.MAT_METAL;


            CreateItemInstance();
        }
    }
}
