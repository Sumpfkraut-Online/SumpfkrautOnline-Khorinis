using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_SMITH : AbstractArmors
    {
        static ITAR_SMITH ii;
        public static ITAR_SMITH get()
        {
            if (ii == null)
                ii = new ITAR_SMITH();
            return ii;
        }


        protected ITAR_SMITH()
            : base("ITAR_SMITH")
        {
            Visual = "ItAr_Smith.3ds";
            Visual_Change = "Armor_Smith.asc";
            Name = "Schmied Kleidung";
            Description = Name;

            ProtectionEdge = 15;
            ProtectionBlunt = 15;
            ProtectionPoint = 15;


            CreateItemInstance();
        }
    }
}
