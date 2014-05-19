using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Misc
{
    public class ITMI_GOLDCUP : AbstractMisc
    {
        static ITMI_GOLDCUP ii;
        public static ITMI_GOLDCUP get()
        {
            if (ii == null)
                ii = new ITMI_GOLDCUP();
            return ii;
        }


        protected ITMI_GOLDCUP()
            : base("ITMI_GOLDCUP")
        {
            Name = "Goldener Kelch";
            Visual = "ItMi_GoldCup.3ds";
            Description = Name;
            
            Materials = Enumeration.MaterialTypes.MAT_METAL;


            CreateItemInstance();
        }
    }
}
