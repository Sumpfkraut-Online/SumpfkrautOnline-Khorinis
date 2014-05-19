using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_LOCKPICK : AbstractKeys
    {
        static ITKE_LOCKPICK ii;
        public static ITKE_LOCKPICK get()
        {
            if (ii == null)
                ii = new ITKE_LOCKPICK();
            return ii;
        }


        protected ITKE_LOCKPICK()
            : base("ITKE_LOCKPICK")
        {
            Name = "Dietrich";
            Visual = "ItKe_Lockpick.3ds";
            Description = Name;

            CreateItemInstance();
        }
    }
}
