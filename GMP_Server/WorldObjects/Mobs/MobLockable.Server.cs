using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Mobs
{
    internal abstract partial class MobLockable
    {


        protected override VobSendFlags getSendInfo()
        {
            VobSendFlags b = base.getSendInfo();

            if (this.IsLocked)
                b |= VobSendFlags.IsLocked;
            if (this.KeyInstance != null)
                b |= VobSendFlags.KeyInstance;
            if (this.PickLockStr != null && this.PickLockStr.Length != 0)
                b |= VobSendFlags.PickLockStr;
            return b;
        }

        public override VobSendFlags Write(RakNet.BitStream stream)
        {
            VobSendFlags sendFlags = base.Write(stream);

            if (sendFlags.HasFlag(VobSendFlags.IsLocked))
                stream.Write(IsLocked);
            if (sendFlags.HasFlag(VobSendFlags.KeyInstance))
                stream.Write(KeyInstance.ID);
            if (sendFlags.HasFlag(VobSendFlags.PickLockStr))
                stream.Write(PickLockStr);

            return sendFlags;
        }
    }
}
