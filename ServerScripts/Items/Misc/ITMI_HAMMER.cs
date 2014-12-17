using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_HAMMER : AbstractMisc
    {
        static ITMI_HAMMER ii;
        public static ITMI_HAMMER get()
        {
            if (ii == null)
                ii = new ITMI_HAMMER();
            return ii;
        }


        protected ITMI_HAMMER()
            : base("ITMI_HAMMER")
        {
            Name = "Hammer";
            Visual = "ItMi_Hammer.3ds";
            Description = Name;


            CreateItemInstance();
        }
    }
}
