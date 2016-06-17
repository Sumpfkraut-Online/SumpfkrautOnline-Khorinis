using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ItemInst : VobInst, WorldObjects.Item.IScriptItem
    {
        #region Properties

        public new WorldObjects.Item BaseInst { get { return (WorldObjects.Item)base.BaseInst; } }
        public new ItemDef Definition { get { return (ItemDef)base.Definition; } }

        public bool IsEquipped { get { return this.BaseInst.IsEquipped; } }

        public ItemTypes ItemType { get { return this.Definition.ItemType; } }

        public bool IsWeapon { get { return this.Definition.IsWeapon; } }
        public bool IsWepRanged { get { return this.Definition.IsWepRanged; } }
        public bool IsWepMelee { get { return this.Definition.IsWepMelee; } }

        #endregion

        public ItemInst() : base(new WorldObjects.Item())
        {
        }

        // Nur das Wichtigste was von aussen zu sehen ist!
        public void ReadEquipProperties(PacketReader stream)
        {
        }

        public void WriteEquipProperties(PacketWriter stream)
        {
        }


        // Alles schreiben was man über dieses Item wissen muss
        public void WriteInventoryProperties(PacketWriter stream)
        {
        }

        public void ReadInventoryProperties(PacketReader stream)
        {
        }
    }
}
