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
                if (this.Host.DrawnWeapon != null)
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

            if (this.Host.IsInFightMode)
            {
                if (this.Host.DrawnWeapon != null)
                {
                    this.Host.DoUndrawWeapon(Host.DrawnWeapon);
                }
                else
                {
                    this.Host.DoUndrawFists();
                }
            }
            else
            {
                this.Host.DoDrawWeapon(item);
            }
        }

        public void TryFightMove(FightMoves move)
        {
            if (Host.IsDead || !Host.IsInFightMode
                || (Host.Environment.InAir && move != FightMoves.Run)
                || Host.ModelInst.GetActiveAniFromLayer(2) != null
                || (Host.DrawnWeapon != null && !Host.DrawnWeapon.IsWeapon))
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

        public void TryEquipItem(ItemInst item)
        {
            if (this.Host.IsDead || this.Host.ModelInst.IsInAnimation()
                || this.Host.Environment.InAir || this.Host.DrawnWeapon != null
                || this.Host.IsInFightMode)
                return;

            this.Host.EquipItem(item);
        }


        public void TryUnequipItem(ItemInst item)
        {
            if (this.Host.IsDead || this.Host.ModelInst.IsInAnimation()
                || this.Host.Environment.InAir || this.Host.DrawnWeapon != null
                || this.Host.IsInFightMode)
                return;
            
            this.Host.UnequipItem(item);
        }

        public void TryAim()
        {
            if (Host.IsDead || Host.IsAiming() || Host.ModelInst.IsInAnimation()
                || Host.Environment.InAir || Host.DrawnWeapon == null
                || !Host.IsInFightMode || !Host.DrawnWeapon.IsWepRanged)
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
                end = start + 500000 * (start - end).Normalise();

            ItemInst projItem = null;
            if (Host.DrawnWeapon.ItemType == ItemTypes.WepBow)
            {
                Host.Inventory.ForEachItemPredicate(i =>
                {
                    if (i.ItemType == ItemTypes.AmmoBow)
                    {
                        projItem = i;
                        return false;
                    }
                    return true;
                });
            }
            else if (Host.DrawnWeapon.ItemType == ItemTypes.WepXBow)
            {
                Host.Inventory.ForEachItemPredicate(i =>
                {
                    if (i.ItemType == ItemTypes.AmmoXBow)
                    {
                        projItem = i;
                        return false;
                    }
                    return true;
                });
            }

            if (projItem == null)
                return;

            if (projItem.Amount == 1)
            {
                //Host.Inventory.RemoveItem(projItem);
            }
            else
            {
               // projItem = projItem.Split(1);
            }

            // heh fixme und so

            ProjInst proj = new ProjInst(ProjDef.Get<ProjDef>(projItem.ItemType == ItemTypes.AmmoBow ? "arrow" : "bolt"));

            Host.DoShoot(start, end, proj);
        }
    }
}
