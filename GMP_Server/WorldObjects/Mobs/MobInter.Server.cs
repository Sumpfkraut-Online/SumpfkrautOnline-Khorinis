using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobInter
    {
        protected override VobSendFlags getSendInfo()
        {
            VobSendFlags b = base.getSendInfo();

            if (focusName != null && focusName.Length != 0)
                b |= VobSendFlags.FocusName;
            if (state != 0)
                b |= VobSendFlags.State;
            if(useWithItem != null)
                b |= VobSendFlags.UseWithItem;
            if (triggerTarget != null && triggerTarget.Length != 0)
                b |= VobSendFlags.TriggerTarget;
            
            return b;
        }

        public override VobSendFlags Write(RakNet.BitStream stream)
        {
            VobSendFlags sendFlags = base.Write(stream);

            if (sendFlags.HasFlag(VobSendFlags.FocusName))
                stream.Write(focusName);
            if (sendFlags.HasFlag(VobSendFlags.State))
                stream.Write(state);
            if (sendFlags.HasFlag(VobSendFlags.UseWithItem))
                stream.Write(useWithItem.ID);
            if (sendFlags.HasFlag(VobSendFlags.TriggerTarget))
                stream.Write(triggerTarget);
            return sendFlags;
        }
    }
}
