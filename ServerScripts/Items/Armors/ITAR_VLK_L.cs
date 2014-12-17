using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_VLK_L : AbstractArmors
    {
        static ITAR_VLK_L ii;
        public static ITAR_VLK_L get()
        {
            if (ii == null)
                ii = new ITAR_VLK_L();
            return ii;
        }


        protected ITAR_VLK_L()
            : base("ITAR_VLK_L")
        {
            Visual = "ItAr_VLK_L.3ds";
            Visual_Change = "Armor_Vlk_L.asc";
            Name = "Bürger Kleidung";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
