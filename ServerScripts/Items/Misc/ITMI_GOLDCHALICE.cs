using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLDCHALICE : AbstractMisc
    {
        static ITMI_GOLDCHALICE ii;
        public static ITMI_GOLDCHALICE get()
        {
            if (ii == null)
                ii = new ITMI_GOLDCHALICE();
            return ii;
        }


        protected ITMI_GOLDCHALICE()
            : base("ITMI_GOLDCHALICE")
        {
            Name = "Goldene Schale";
            Visual = "ItMi_GoldChalice.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialType.MAT_METAL;


            CreateItemInstance();
        }
    }
}
