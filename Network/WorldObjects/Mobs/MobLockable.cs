using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Mobs
{
    internal abstract partial class MobLockable : MobInter
    {
        protected bool isLocked = false;
        protected ItemInstance keyInstance = null;
        protected String pickLockStr = "";

        public bool IsLocked { get { return isLocked; } set { isLocked = value; } }
        public ItemInstance KeyInstance { get { return keyInstance; } set { keyInstance = value; } }
        public String PickLockStr { get { return pickLockStr; } set { pickLockStr = value; } }

        public MobLockable()
            : base()
        {
            
        }
    }
}
