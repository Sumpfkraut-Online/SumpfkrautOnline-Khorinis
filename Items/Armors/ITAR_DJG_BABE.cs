using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_DJG_BABE : AbstractArmors
    {
        static ITAR_DJG_BABE ii;
        public static ITAR_DJG_BABE get()
        {
            if (ii == null)
                ii = new ITAR_DJG_BABE();
            return ii;
        }


        protected ITAR_DJG_BABE()
            : base("ITAR_DJG_BABE")
        {
            Visual = "ItAr_Djg_L.3ds";
            Visual_Change = "Armor_Djg_Babe.asc";
            Name = "Rüstung einer Drachenjägerin";
            Description = Name;

            ProtectionEdge = 60;
            ProtectionBlunt = 60;
            ProtectionPoint = 60;
            ProtectionFire = 30;
            ProtectionMagic = 0;

            CreateItemInstance();
        }
    }
}
