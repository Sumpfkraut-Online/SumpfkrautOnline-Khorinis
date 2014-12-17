using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_SCOOP : AbstractMisc
    {
        static ITMI_SCOOP ii;
        public static ITMI_SCOOP get()
        {
            if (ii == null)
                ii = new ITMI_SCOOP();
            return ii;
        }


        protected ITMI_SCOOP()
            : base("ITMI_SCOOP")
        {
            Name = "Kochlöffel";
            Visual = "ItMi_Scoop.3ds";
            Description = Name;


            CreateItemInstance();
        }
    }
}
