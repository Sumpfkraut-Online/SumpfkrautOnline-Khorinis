using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_VLKBABE_H : AbstractArmors
    {
        static ITAR_VLKBABE_H ii;
        public static ITAR_VLKBABE_H get()
        {
            if (ii == null)
                ii = new ITAR_VLKBABE_H();
            return ii;
        }


        protected ITAR_VLKBABE_H()
            : base("ITAR_VLKBABE_H")
        {
            Visual = "ItAr_VLKBabe.3ds";
            Visual_Change = "Armor_VlkBabe_H.asc";
            Name = "Bürgerin Kleidung";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
