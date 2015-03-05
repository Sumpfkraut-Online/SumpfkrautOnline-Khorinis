using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting.Objects.Mob
{
    public class MobSwitch : MobInter
    {
        internal MobSwitch(GUC.WorldObjects.Mobs.MobSwitch vob)
            : base(vob)
        {
            
        }

        public MobSwitch(String visual, String focusName, ItemInstance useWithItem, String triggerTarget)
            : this(visual, focusName, useWithItem, triggerTarget, true, true)
        { }

        public MobSwitch(String visual)
            : this(visual, null, null, null, true, true)
        { }

        public MobSwitch(String visual, String focusName)
            : this(visual, focusName, null, null, true, true)
        { }

        public MobSwitch(String visual, ItemInstance useWithItem, String triggerTarget)
            : this(visual, null, useWithItem, triggerTarget, true, true)
        { }

        public MobSwitch(String visual, ItemInstance useWithItem)
            : this(visual, null, useWithItem, null, true, true)
        { }

        public MobSwitch(String visual, String focusName, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic)
            : this(new GUC.WorldObjects.Mobs.MobSwitch(), visual, focusName, useWithItem, triggerTarget, cdDyn, cdStatic, true)
        { }

        internal MobSwitch(GUC.WorldObjects.Mobs.MobSwitch mobInter, String visual, String focusName, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic, bool useCreate)
            : base(mobInter, visual, focusName, useWithItem, triggerTarget, cdDyn, cdStatic, false)
        {
            
            if (useCreate)
                CreateVob();
        }
    }
}
