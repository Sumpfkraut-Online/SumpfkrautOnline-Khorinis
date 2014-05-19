using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_BAUBABE_L : AbstractArmors
    {
        static ITAR_BAUBABE_L ii;
        public static ITAR_BAUBABE_L get()
        {
            if (ii == null)
                ii = new ITAR_BAUBABE_L();
            return ii;
        }


        protected ITAR_BAUBABE_L()
            : base("ITAR_BAUBABE_L")
        {
            Visual = "ItAr_VLKBabe.3ds";
            Visual_Change = "Armor_BauBabe_L.asc";
            Name = "Bäuerin Tracht";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
