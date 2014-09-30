using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Items.Keys
{
    public class ITKE_KEY_01 : AbstractKeys
    {
        static ITKE_KEY_01 ii;
        public static ITKE_KEY_01 get()
        {
            if (ii == null)
                ii = new ITKE_KEY_01();
            return ii;
        }


        protected ITKE_KEY_01()
            : base("ITKE_KEY_01")
        {
            Visual = "ItKe_Key_01.3ds";

            CreateItemInstance();
        }
    }
}
