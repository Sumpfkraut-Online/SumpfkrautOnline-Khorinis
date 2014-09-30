using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_STOMPER : AbstractMisc
    {
        static ITMI_STOMPER ii;
        public static ITMI_STOMPER get()
        {
            if (ii == null)
                ii = new ITMI_STOMPER();
            return ii;
        }


        protected ITMI_STOMPER()
            : base("ITMI_STOMPER")
        {
            Name = "Krautstampfer";
            Visual = "ItMi_Stomper.3ds";
            Description = Name;


            CreateItemInstance();
        }
    }
}
