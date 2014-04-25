using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobContainer
    {

        public override Vob.VobSendFlags Write(RakNet.BitStream stream)
        {
            VobSendFlags b = base.Write(stream);

            if (b.HasFlag(VobSendFlags.MCItemList))
            {
                stream.Write(this.itemList.Count);
                for (int i = 0; i < this.itemList.Count; i++)
                {
                    stream.Write(this.itemList[i].ID);
                }
            }

            return b;
        }

        protected override VobSendFlags getSendInfo()
        {
            VobSendFlags b = base.getSendInfo();

            if (itemList != null && this.itemList.Count != 0)
                b |= VobSendFlags.MCItemList;

            return b;
        }
    }
}
