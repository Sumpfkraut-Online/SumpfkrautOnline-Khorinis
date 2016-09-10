using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.WorldObjects;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ItemInst : VobInst, WorldObjects.Item.IScriptItem
    {
        #region Constructors

        partial void pConstruct();
        public ItemInst()
        {
            pConstruct();
        }

        protected override BaseVob CreateVob()
        {
            return new Item(new Visuals.ModelInst(this), this);
        }

        #endregion

        #region Properties

        public new Item BaseInst { get { return (Item)base.BaseInst; } }
        public new ItemDef Definition { get { return (ItemDef)base.Definition; } set { base.Definition = value; } }

        public int Amount { get { return this.BaseInst.Amount; } }

        public bool IsEquipped { get { return this.BaseInst.IsEquipped; } }

        public ItemTypes ItemType { get { return this.Definition.ItemType; } }

        public bool IsWeapon { get { return this.Definition.IsWeapon; } }
        public bool IsWepRanged { get { return this.Definition.IsWepRanged; } }
        public bool IsWepMelee { get { return this.Definition.IsWepMelee; } }

        #endregion


        public void SetAmount(int amount)
        {
            this.BaseInst.SetAmount(amount);
        }

        // Nur das Wichtigste was von aussen zu sehen ist!
        public void ReadEquipProperties(PacketReader stream)
        {
        }

        public void WriteEquipProperties(PacketWriter stream)
        {
        }

        // Alles schreiben was der Besitzer über dieses Item wissen muss
        public void WriteInventoryProperties(PacketWriter stream)
        {
        }

        public void ReadInventoryProperties(PacketReader stream)
        {
        }
    }
}
