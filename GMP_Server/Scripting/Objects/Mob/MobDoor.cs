using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting.Objects.Mob
{
    public class MobDoor : MobLockable
    {
        internal MobDoor(GUC.WorldObjects.Mobs.MobDoor vob)
            : base(vob)
        {
            
        }

        public MobDoor(String visual)
            : this(visual, null)
        { }

        public MobDoor(String visual, String focusName)
            : this(visual, focusName, false, null, null)
        { }

        public MobDoor(String visual, String focusName, bool isLocked, ItemInstance keyInstance, String pickLockString)
            : this(visual, focusName, isLocked, keyInstance, pickLockString, null, null, true, true)
        { }

        public MobDoor(String visual, String focusName, bool isLocked, ItemInstance keyInstance, String pickLockString, bool cdDyn, bool cdStatic)
            : this(visual, focusName, isLocked, keyInstance, pickLockString, null, null, cdDyn, cdStatic)
        { }

        public MobDoor(String visual, String focusName, bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic)
            : this(new GUC.WorldObjects.Mobs.MobDoor(), visual, focusName, isLocked, keyInstance, pickLockString, useWithItem, triggerTarget, cdDyn, cdStatic, true)
        { }

        protected MobDoor()
            : this(new GUC.WorldObjects.Mobs.MobDoor(), null, null, false, null, null, null, null, false, false, false)
        { }

        internal MobDoor(GUC.WorldObjects.Mobs.MobDoor mobInter, String visual, String focusName, bool isLocked, ItemInstance keyInstance, String pickLockString, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic, bool useCreate)
            : base(mobInter, visual, focusName, isLocked, keyInstance, pickLockString, useWithItem, triggerTarget, cdDyn, cdStatic)
        {
            
            if (useCreate)
                CreateVob();
        }
    }
}
