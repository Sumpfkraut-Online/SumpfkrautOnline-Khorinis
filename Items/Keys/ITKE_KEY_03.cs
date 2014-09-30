using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_KEY_03 : AbstractKeys
    {
        static ITKE_KEY_03 ii;
        public static ITKE_KEY_03 get()
        {
            if (ii == null)
                ii = new ITKE_KEY_03();
            return ii;
        }


        protected ITKE_KEY_03()
            : base("ITKE_KEY_03")
        {
            Visual = "ItKe_Key_03.3ds";

            CreateItemInstance();
        }
    }
}
