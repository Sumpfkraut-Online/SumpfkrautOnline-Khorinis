using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_DJG_L : AbstractArmors
    {
        static ITAR_DJG_L ii;
        public static ITAR_DJG_L get()
        {
            if (ii == null)
                ii = new ITAR_DJG_L();
            return ii;
        }


        protected ITAR_DJG_L()
            : base("ITAR_DJG_L")
        {
            Visual = "ItAr_Djg_L.3ds";
            Visual_Change = "Armor_Djg_L.asc";
            Name = "Leichte Drachenjägerrüstung";
            Description = Name;

            ProtectionEdge = 100;
            ProtectionBlunt = 100;
            ProtectionPoint = 100;
            ProtectionFire = 50;
            ProtectionMagic = 25;

            CreateItemInstance();
        }
    }
}
