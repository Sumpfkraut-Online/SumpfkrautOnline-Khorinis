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
                vob.SetPickLockStr(this.PickLockStr);


            if (IsLocked)
                vob.SetLocked(1);
            else
                vob.SetLocked(0);
        }


        public void setKeyInstance(ItemInstance keyInstance)
        {
            this.KeyInstance = keyInstance;

            if (this.Address == 0)
                return;

            oCMobLockable mobInter = new oCMobLockable(Process.ThisProcess(), this.Address);
            if (this.KeyInstance != null)
                mobInter.SetKeyInstance("ITGUC_" + this.KeyInstance.ID);
            else
                mobInter.SetKeyInstance("");
        }

        public void setPickLockStr(String picklockString)
        {
            this.PickLockStr = picklockString;

            if (this.Address == 0)
                return;

            oCMobLockable mobInter = new oCMobLockable(Process.ThisProcess(), this.Address);
            if (this.PickLockStr != null && PickLockStr.Length != 0)
                mobInter.SetPickLockStr(PickLockStr);
            else
                mobInter.SetPickLockStr("");
        }

        public void setIsLocked(bool locked)
        {
            this.IsLocked = locked;

            if (this.Address == 0)
                return;

            oCMobLockable mobInter = new oCMobLockable(Process.ThisProcess(), this.Address);
            if (IsLocked)
                mobInter.SetLocked(1);
            else
                mobInter.SetLocked(0);
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
