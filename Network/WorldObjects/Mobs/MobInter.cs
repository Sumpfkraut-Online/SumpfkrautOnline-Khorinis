using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobInter : Vob
    {
        protected String focusName;
        protected ItemInstance useWithItem;
        protected String triggerTarget;

        protected byte state = 0;

        public byte State { get { return this.state; } set { this.state = value; } }
        public String FocusName { get { return this.focusName; } set { this.focusName = value; } }
        public ItemInstance UseWithItem { get { return useWithItem; } set { this.useWithItem = value; } }
        public String TriggerTarget { get { return triggerTarget; } set { triggerTarget = value; } }
        public MobInter()
            : base()
        {
            this.VobType = Enumeration.VobType.MobInter;
        }



    }
}
