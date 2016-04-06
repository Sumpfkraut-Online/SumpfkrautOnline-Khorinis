using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        public override void Spawn(WorldInst world)
        {
            base.Spawn(world);
            this.BaseInst.ForEachEquippedItem(i => this.pEquipItem((ItemInst)i.ScriptObject));
        }

        partial void pEquipItem(ItemInst item)
        {
            if (!this.IsSpawned)
                return;

            item.BaseInst.gVob.Material = (int)item.Definition.Material;

            switch (item.ItemType)
            {
                case Definitions.ItemTypes.Wep1H:
                    Log.Logger.Log("Equip 1H: " + item.BaseInst.Model.Visual);
                    item.BaseInst.gVob.MainFlag = Gothic.Objects.oCItem.MainFlags.ITEM_KAT_NF;
                    item.BaseInst.gVob.Flags = Gothic.Objects.oCItem.ItemFlags.ITEM_SWD;
                    item.BaseInst.gVob.Flags |= item.BaseInst.gVob.MainFlag;
                    this.BaseInst.gVob.EquipWeapon(item.BaseInst.gVob);
                    using (var str = Gothic.Types.zString.Create("1H"))
                        this.BaseInst.gVob.SetWeaponMode2(str);
                    break;

                case Definitions.ItemTypes.Wep2H:
                    Log.Logger.Log("Equip 2H: " + item.BaseInst.Model.Visual);
                    item.BaseInst.gVob.MainFlag = Gothic.Objects.oCItem.MainFlags.ITEM_KAT_NF;
                    item.BaseInst.gVob.Flags = Gothic.Objects.oCItem.ItemFlags.ITEM_2HD_SWD;
                    item.BaseInst.gVob.Flags |= item.BaseInst.gVob.MainFlag;
                    this.BaseInst.gVob.EquipWeapon(item.BaseInst.gVob);
                    using (var str = Gothic.Types.zString.Create("2H"))
                        this.BaseInst.gVob.SetWeaponMode2(str);
                    break;

                case Definitions.ItemTypes.Armor:
                    Log.Logger.Log("Equip Armor: " + item.BaseInst.Model.Visual);
                    item.BaseInst.gVob.MainFlag = Gothic.Objects.oCItem.MainFlags.ITEM_KAT_ARMOR;
                    item.BaseInst.gVob.VisualChange.Set(item.Definition.VisualChange);
                    item.BaseInst.gVob.Flags = 0;
                    item.BaseInst.gVob.Flags |= item.BaseInst.gVob.MainFlag;
                    item.BaseInst.gVob.Wear = 1; // WEAR_TORSO
                    this.BaseInst.gVob.EquipArmor(item.BaseInst.gVob);
                    break;

                default:
                    break;
            }
        }

        partial void pUnequipItem(ItemInst item)
        {
            this.BaseInst.gVob.UnequipItem(item.BaseInst.gVob);
        }

        public override void OnReadScriptVobMsg(PacketReader stream)
        {
            var msgID = (Networking.NetVobMsgIDs)stream.ReadByte();
            switch (msgID)
            {
                case Networking.NetVobMsgIDs.HitMessage:
                    var targetID = stream.ReadUShort();
                    WorldObjects.BaseVob target;
                    if (WorldInst.Current.BaseWorld.TryGetVob(targetID, out target))
                    {
                        this.BaseInst.gVob.AniCtrl.CreateHit(target.gVob);
                    }
                    break;
                case Networking.NetVobMsgIDs.ParryMessage:
                    targetID = stream.ReadUShort();
                    WorldObjects.NPC targetNPC;
                    if (WorldInst.Current.BaseWorld.TryGetVob(targetID, out targetNPC))
                    {
                        this.BaseInst.gVob.AniCtrl.StartParadeEffects(targetNPC.gVob);
                    }
                    break;
                default:
                    return;
            }
        }
    }
}
