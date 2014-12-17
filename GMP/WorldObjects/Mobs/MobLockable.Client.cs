using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;

namespace GUC.WorldObjects.Mobs
{
    internal abstract partial class MobLockable
    {
        protected void setMobLockableData(Process process, oCMobLockable vob)
        {
            if (this.KeyInstance != null)
                vob.SetKeyInstance("ITGUC_"+this.KeyInstance.ID);
            if (this.PickLockStr != null && this.PickLockStr.Length != 0)
                vob.SetKeyInstance(this.PickLockStr);


            if (IsLocked)
                vob.SetLocked(1);
            else
                vob.SetLocked(0);
        }


        public override VobSendFlags Read(RakNet.BitStream stream)
        {
            VobSendFlags sendFlags = base.Read(stream);
            
            if (sendFlags.HasFlag(VobSendFlags.IsLocked))
                stream.Read(out isLocked);
            if (sendFlags.HasFlag(VobSendFlags.KeyInstance))
            {
                int keyID = 0;
                stream.Read(out keyID);
                KeyInstance = ItemInstance.ItemInstanceDict[keyID];
            }
            if (sendFlags.HasFlag(VobSendFlags.PickLockStr))
                stream.Read(out pickLockStr);

            return sendFlags;
        }
    }
}
