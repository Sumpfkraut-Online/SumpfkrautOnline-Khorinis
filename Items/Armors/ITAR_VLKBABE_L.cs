using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_VLKBABE_L : AbstractArmors
    {
        static ITAR_VLKBABE_L ii;
        public static ITAR_VLKBABE_L get()
        {
            if (ii == null)
                ii = new ITAR_VLKBABE_L();
            return ii;
        }


        protected ITAR_VLKBABE_L()
            : base("ITAR_VLKBABE_L")
        {
            Visual = "ItAr_VLKBabe.3ds";
            Visual_Change = "Armor_VlkBabe_L.asc";
            Name = "Bürgerin Kleidung";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
