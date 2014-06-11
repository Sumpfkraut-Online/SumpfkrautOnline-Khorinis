using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_JUDGE : AbstractArmors
    {
        static ITAR_JUDGE ii;
        public static ITAR_JUDGE get()
        {
            if (ii == null)
                ii = new ITAR_JUDGE();
            return ii;
        }


        protected ITAR_JUDGE()
            : base("ITAR_JUDGE")
        {
            Visual = "ItAr_Governor.3ds";
            Visual_Change = "Armor_Judge.asc";
            Name = "Richterrobe";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
