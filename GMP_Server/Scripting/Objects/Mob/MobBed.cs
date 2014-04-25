using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripting.Objects.Mob
{
    public class MobBed : MobInter
    {
        internal MobBed(GUC.WorldObjects.Mobs.MobBed vob)
            : base(vob)
        {
            
        }


        public MobBed(String visual, String focusName, ItemInstance useWithItem, String triggerTarget)
            : this(visual, focusName, useWithItem, triggerTarget, true, true)
        { }

        public MobBed(String visual)
            : this(visual, null, null, null, true, true)
        { }

        public MobBed(String visual, String focusName)
            : this(visual, focusName, null, null, true, true)
        { }

        public MobBed(String visual, ItemInstance useWithItem, String triggerTarget)
            : this(visual, null, useWithItem, triggerTarget, true, true)
        { }

        public MobBed(String visual, ItemInstance useWithItem)
            : this(visual, null, useWithItem, null, true, true)
        { }

        public MobBed(String visual, String focusName, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic)
            : this(new GUC.WorldObjects.Mobs.MobBed(), visual, focusName, useWithItem, triggerTarget, cdDyn, cdStatic, true)
        { }

        internal MobBed(GUC.WorldObjects.Mobs.MobInter mobInter, String visual, String focusName, ItemInstance useWithItem, String triggerTarget, bool cdDyn, bool cdStatic, bool useCreate)
            : base(mobInter, visual, focusName, useWithItem, triggerTarget, cdDyn, cdStatic, false)
        {
            
            if (useCreate)
                CreateVob();
        }
    }
}
