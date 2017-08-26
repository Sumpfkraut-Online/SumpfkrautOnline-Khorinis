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
    public partial class Item
    {
        #region Network Messages

        new internal static class Messages
        {
            public static void ReadItemAmountChangedMessage(PacketReader stream)
            {
                Item item;
                if (NPCInventory.PlayerInventory.TryGetItem(stream.ReadByte(), out item))
                {
                    item.ScriptObject.SetAmount(stream.ReadUShort());
                }
            }
        }

        #endregion

        new public oCItem gVob { get { return (oCItem)base.gVob; } }

        bool dropped = false;
        internal override void OnTick(long now)
        {
            base.OnTick(now);

            if (!dropped && NPC.Hero != null) // FIXME
            {
                dropped = true;
                Vec3f pos = this.GetPosition();
                Vec3f dir = this.GetDirection();
                
                NPC.Hero.gVob.DoDropVob(this.gVob);

                this.SetPosition(pos);
                this.SetDirection(dir);
            }
        }
    }
}
