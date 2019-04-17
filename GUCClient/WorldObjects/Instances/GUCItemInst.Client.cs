using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;
using GUC.Network;
using GUC.Types;
using GUC.WorldObjects.ItemContainers;

namespace GUC.WorldObjects
{
    public partial class GUCItemInst
    {
        #region Network Messages

        new internal static class Messages
        {
            public static void ReadItemAmountChangedMessage(PacketReader stream)
            {
                GUCItemInst item;
                if (NPCInventory.PlayerInventory.TryGetItem(stream.ReadByte(), out item))
                {
                    item.ScriptObject.SetAmount(stream.ReadUShort());
                }
            }
        }

        #endregion

        new public oCItem gVob { get { return (oCItem)base.gVob; } }

        bool dropped = true;
        internal override void OnTick(long now)
        {
            base.OnTick(now);

            if (!dropped && GUCNPCInst.Hero != null) // FIXME
            {
                dropped = true;
                Vec3f pos = this.Position;
                Angles ang = this.Angles;
                
                GUCNPCInst.Hero.gVob.DoDropVob(this.gVob);

                this.SetPosition(pos);
                this.SetAngles(ang);
            }
        }
    }
}
