using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_VLK_M : AbstractArmors
    {
        static ITAR_VLK_M ii;
        public static ITAR_VLK_M get()
        {
            if (ii == null)
                ii = new ITAR_VLK_M();
            return ii;
        }


        protected ITAR_VLK_M()
            : base("ITAR_VLK_M")
        {
            Visual = "ItAr_VLK_M.3ds";
            Visual_Change = "Armor_Vlk_M.asc";
            Name = "Bürger Kleidung";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
