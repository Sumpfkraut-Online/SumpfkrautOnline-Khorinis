using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Armors
{
    public class ITAR_BARKEEPER : AbstractArmors
    {
        static ITAR_BARKEEPER ii;
        public static ITAR_BARKEEPER get()
        {
            if (ii == null)
                ii = new ITAR_BARKEEPER();
            return ii;
        }


        protected ITAR_BARKEEPER()
            : base("ITAR_BARKEEPER")
        {
            Visual = "ItAr_Wirt.3ds";
            Visual_Change = "Armor_Barkeeper.asc";
            Name = "Wirt Kleidung";
            Description = Name;

            ProtectionEdge = 10;
            ProtectionBlunt = 10;
            ProtectionPoint = 10;


            CreateItemInstance();
        }
    }
}
