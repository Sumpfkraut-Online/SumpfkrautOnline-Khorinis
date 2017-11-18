using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Utilities;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.EffectSystem.EffectHandlers
{
    partial class NPCInstEffectHandler
    {
        private static void OnHit(NPCInst attacker, NPCInst target, int damage)
        {
            throw new NotImplementedException();
        }

        LockTimer jumpLockTimer = new LockTimer(500);
        public void TryJump(JumpMoves move)
        {
            if (Host.IsDead || Host.Environment.InAir)
                return;

            if (Host.ModelInst.GetActiveAniFromLayer(1) != null)
                return;

            if (!jumpLockTimer.IsReady) // don't spam
                return;

            Host.DoJump(move, new Vec3f(0, move == JumpMoves.Fwd ? 300 : 250, 0));
        }

        internal void TryClimb(ClimbMoves move, WorldObjects.NPC.ClimbingLedge ledge)
        {
            if (Host.IsDead || Host.Environment.InAir)
                return;

            if (Host.ModelInst.IsInAnimation())
                return;

            if (!jumpLockTimer.IsReady) // don't spam
                return;

            Host.DoClimb(move, ledge);
        }

        public void TryDrawFists()
        {
            if (Host.ModelDef.Visual != "HUMANS.MDS")
                return;

            if (Host.IsDead || this.Host.ModelInst.IsInAnimation())
                return;

            if (this.Host.IsInFightMode)
            {
                if (this.Host.GetDrawnWeapon() != null)
                {
                    return;
                    //this.UnequipItem(this.DrawnWeapon);
                }
                else
                {
                    this.Host.DoUndrawFists();
                }
            }
            else
            {
                this.Host.DoDrawFists();
            }
        }

        public void TryDrawWeapon(ItemInst item)
        {
            if (Host.ModelDef.Visual != "HUMANS.MDS" && Host.ModelDef.Visual != "ORC.MDS")
                return;

            if (Host.IsDead || this.Host.ModelInst.IsInAnimation())
                return;

            if (this.Host.IsInFightMode && Host.GetDrawnWeapon() != null)
            {
                Host.DoUndrawWeapon(Host.GetDrawnWeapon());
            }
            else
            {
                this.Host.DoDrawWeapon(item);
            }
        }

        public void TryUndrawWeapon(ItemInst item)
        {
            if (Host.ModelDef.Visual != "HUMANS.MDS" && Host.ModelDef.Visual != "ORC.MDS")
                return;

            if (Host.IsDead || this.Host.ModelInst.IsInAnimation())
                return;

            if (Host.GetRightHand() == item || Host.GetLeftHand() == item)
            {
                Host.DoUndrawWeapon(item);
            }
        }

        public void TryFightMove(FightMoves move)
        {
            if (Host.IsDead || !Host.IsInFightMode
                || (Host.Environment.InAir && move != FightMoves.Run)
                || Host.ModelInst.GetActiveAniFromLayer(2) != null)
                return;

            var meleeWep = Host.GetDrawnWeapon();
            if (meleeWep != null && !meleeWep.IsWepMelee)
                return;

            var otherAni = Host.ModelInst.GetActiveAniFromLayer(1);
            if (otherAni != null && otherAni != Host.FightAnimation)
                return;

            if (Host.FightAnimation != null)
            {
                if (Host.CanCombo)
                {
                    var current = Host.CurrentFightMove;
                    if (current == FightMoves.Right && move == FightMoves.Right)
                        return;
                    if (current == FightMoves.Left && move == FightMoves.Left)
                        return;

                    Host.DoFightMove(move, current == FightMoves.Fwd ? Host.ComboNum + 1 : 0);
                }
            }
            else
            {
                Host.DoFightMove(move, 0);
            }
        }

        /// <summary> Player equips item through inventory </summary>
        public void TryEquipItem(ItemInst item)
        {
            if (this.Host.IsDead || this.Host.ModelInst.IsInAnimation() || this.Host.Environment.InAir 
                || this.Host.IsInFightMode || this.Host.HasItemInHands())
                return;

            NPCSlots slot;
            switch (item.ItemType)
            {
                case ItemTypes.AmmoBow:
                case ItemTypes.AmmoXBow:
                    slot = NPCSlots.Ammo;
                    break;
                case ItemTypes.Armor:
                    slot = NPCSlots.Armor;
                    break;
                case ItemTypes.Wep1H: // FIXME: Let player choose the slots
                    if (Host.GetEquipmentBySlot(NPCSlots.OneHanded1) == null || Host.GetEquipmentBySlot(NPCSlots.OneHanded2) != null)
                        slot = NPCSlots.OneHanded1;
                    else
                        slot = NPCSlots.OneHanded2;

                    Host.UnequipSlot(NPCSlots.TwoHanded);
                    break;
                case ItemTypes.Wep2H:
                    slot = NPCSlots.TwoHanded;
                    Host.UnequipSlot(NPCSlots.OneHanded1);
                    Host.UnequipSlot(NPCSlots.OneHanded2);
                    break;
                case ItemTypes.WepBow:
                case ItemTypes.WepXBow:
                    slot = NPCSlots.Ranged;
                    break;
                default:
                    return;
            }

            ItemInst otherItem = Host.GetEquipmentBySlot(slot);
            if (otherItem != null)
                Host.UnequipItem(otherItem);
            
            this.Host.EquipItem(slot, item);

            if (item.IsWepRanged)
            {
                var curAmmo = Host.GetAmmo();
                var ammoType = item.ItemType == ItemTypes.WepXBow ? ItemTypes.AmmoXBow : ItemTypes.AmmoBow;
                if (curAmmo == null || curAmmo.ItemType != ammoType) // no ammo equipped or wrong type
                    Host.Inventory.ForEachItemPredicate(i =>
                    {
                        if (i.ItemType == ammoType)
                        {
                            if (curAmmo != null)
                                Host.UnequipItem(curAmmo);
                            Host.EquipItem(NPCSlots.Ammo, i);
                            return false;
                        }
                        return true;
                    });
            }
        }


        public void TryUnequipItem(ItemInst item)
        {
            if (this.Host.IsDead || this.Host.ModelInst.IsInAnimation()
                || this.Host.Environment.InAir || this.Host.HasItemInHands()
                || this.Host.IsInFightMode)
                return;

            Host.UnequipItem(item);
            if (item.ItemType == ItemTypes.Wep1H)
                Host.UnequipSlot(NPCSlots.OneHanded2);
        }

        public void TryAim()
        {
            if (Host.IsDead || Host.IsAiming() || Host.ModelInst.IsInAnimation()
                || Host.Environment.InAir || !Host.IsInFightMode)
                return;

            var drawnWep = Host.GetDrawnWeapon();
            if (drawnWep == null || !drawnWep.IsWepRanged)
                return;

            Host.DoAim();
        }

        public void TryUnaim()
        {
            if (Host.IsDead || !Host.ModelInst.IsInAnimation())
                return;

            Host.DoUnaim();
        }

        public void TryShoot(Vec3f start, Vec3f end)
        {
            if (Host.IsDead || !Host.IsAiming())
                return;

            if (start.GetDistance(end) > 500000) // just in case
                end = start + 500000 * (end - start).Normalise();

            var drawnWeapon = Host.GetDrawnWeapon();
            if (drawnWeapon == null || !drawnWeapon.IsWepRanged)
                return;

            ItemInst ammo = drawnWeapon.ItemType == ItemTypes.WepBow ? Host.GetRightHand() : Host.GetLeftHand();
            if (ammo == null || !ammo.IsAmmo)
                return;

            ProjInst inst = new ProjInst(ProjDef.Get<ProjDef>("arrow"));
            inst.Item = new ItemInst(ammo.Definition);
            inst.Damage = drawnWeapon.Damage;
            inst.Velocity = 0.0004f;
            inst.Model = ammo.ModelDef;

            /*if (ammo.Amount == 1)
            {
                Host.Inventory.RemoveItem(projItem);
            }
            else
            {
                projItem = projItem.Split(1);
            }*/

            end = end - (end - start).Normalise() * (ammo.ItemType == ItemTypes.AmmoBow ? 40 : 10); // so arrows' bodies aren't 90% inside walls
            Host.DoShoot(start, end, inst);
        }
    }
}
