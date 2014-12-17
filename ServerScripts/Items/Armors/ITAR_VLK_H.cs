using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_VLK_H : AbstractArmors
    {
        static ITAR_VLK_H ii;
        public static ITAR_VLK_H get()
        {
            if (ii == null)
                ii = new ITAR_VLK_H();
            return ii;
        }


        protected ITAR_VLK_H()
            : base("ITAR_VLK_H")
        {
            Visual = "ItAr_VLK_H.3ds";
            Visual_Change = "Armor_Vlk_H.asc";
            Name = "Bürger Kleidung";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
