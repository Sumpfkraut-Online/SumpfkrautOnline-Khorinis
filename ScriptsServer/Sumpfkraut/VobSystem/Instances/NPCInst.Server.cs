using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.Visuals.AniCatalogs;
using GUC.Types;
using GUC.Log;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Animations;
using GUC.WorldObjects;
using GUC.WorldObjects.Instances;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        #region Constructors

        public NPCInst(NPCDef def) : this()
        {
            this.Definition = def;
        }

        partial void pConstruct()
        {
            //this.hitTimer = new GUCTimer(CalcHit);
            //this.comboTimer = new GUCTimer(AbleCombo);
        }

        #endregion

        public NPCCatalog AniCatalog { get { return (NPCCatalog)this.ModelDef?.Catalog; } }

        #region Jumps

        public void DoJump(bool jumpUp = false)
        {
            if (this.IsDead || this.BaseInst.GetEnvironment().InAir)
                return;

            ScriptAniJob job;
            if (jumpUp)
            {
                job = AniCatalog.Jumps.Up;
            }
            else
            {
                if (this.Movement == NPCMovement.Forward)
                {
                    job = AniCatalog.Jumps.Run;
                }
                else if (this.Movement == NPCMovement.Stand)
                {
                    job = AniCatalog.Jumps.Fwd;
                }
                else
                {
                    return;
                }
            }

            if (job == null)
                return;

            if (this.BaseInst.Model.GetActiveAniFromLayerID(1) != null)
                return;

            this.ModelInst.StartAnimation(job);
            Vec3f velocity = this.GetDirection();
            velocity.Y = 500;
            velocity.X *= 250;
            velocity.Z *= 250;
            this.Throw(velocity);
        }
        #endregion

        #region ItemHandling
        public void DropItem(byte itemID, ushort amount)
        {
            ItemInst item = Inventory.GetItem(itemID);
            if (item == null)
                return;
            if (item.IsEquipped)
                UnequipItem(item);

            ModelInst.StartAnimation(this.AniCatalog.DropItem);

            int newAmount = item.Amount - amount;
            int droppedItemAmount;
            if (newAmount >= 0)
            {
                item.SetAmount(newAmount);
                droppedItemAmount = amount;
            }
            else
            {
                item.SetAmount(0);
                droppedItemAmount = item.Amount;
            }

            ItemInst newItem = new ItemInst(item.Definition);
            newItem.SetAmount(droppedItemAmount);
            newItem.Spawn(this.World, this.GetPosition(), this.GetDirection());
        }

        public void EquipItem(byte itemID)
        {
            ItemInst item = Inventory.GetItem(itemID);
            if (item == null)
                return;

            if (!item.IsEquipped)
                EquipItem(item);
        }

        public void UnequipItem(byte itemID)
        {
            ItemInst item = Inventory.GetItem(itemID);
            if (item == null)
                return;

            if (item.IsEquipped)
                UnequipItem(item);
        }

        public void UseItem(byte itemID)
        {
            ItemInst item = Inventory.GetItem(itemID);
            if (item == null)
                return;

            if (this.ModelInst.BaseInst.IsInAnimation())
                return;

            if (this.Environment.InAir)
                return;

            if (this.Movement != NPCMovement.Stand)
                return;

            switch (item.ItemType)
            {
                case ItemTypes.SmallEatable:
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.EatSmall, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.LargeEatable:
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.EatLarge, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Mutton:
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.EatMutton, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Rice:
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.EatRice, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Drinkable:
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.DrinkPotion, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Readable:
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.ReadScroll, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Torch:
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.UseTorch, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
            }
        }
        #endregion

        #region Fight Moves

        public enum FightMoves
        {
            Fwd,
            Left,
            Right,
            Run,

            Dodge,
            Parry
        }

        public void DoFightMove(FightMoves move)
        {
            if (this.IsDead || this.BaseInst.GetEnvironment().InAir)
                return;

            var catalog = AniCatalog.FightFist;
            if (this.DrawnWeapon != null)
            {
                switch (this.DrawnWeapon.ItemType)
                {
                    case ItemTypes.Wep1H:
                        catalog = AniCatalog.Fight1H;
                        break;
                    case ItemTypes.Wep2H:
                        catalog = AniCatalog.Fight2H;
                        break;
                    case ItemTypes.WepBow:
                        break;
                    case ItemTypes.WepXBow:
                        break;
                }
            }

            if (this.BaseInst.Model.GetActiveAniFromLayerID(1) != null)
                return;

            ScriptAniJob job;
            switch (move)
            {
                case FightMoves.Fwd:
                    job = catalog.Fwd;
                    break;
                case FightMoves.Left:
                    job = catalog.Left;
                    break;
                case FightMoves.Right:
                    job = catalog.Right;
                    break;
                case FightMoves.Parry:
                    job = catalog.Parry1;
                    break;
                case FightMoves.Dodge:
                    job = catalog.Dodge;
                    break;
                default:
                    return;
            }

            if (job == null)
                return;

            this.ModelInst.StartAnimation(job, 1.0f);
        }

        #endregion

        #region Weapon Drawing

        public void DrawWeapon(byte itemID)
        {
            if (this.BaseInst.IsInFightMode)
            {
                if (this.DrawnWeapon != null)
                {
                    ItemInst weapon = this.DrawnWeapon;
                    this.ModelInst.StartAnimation(this.AniCatalog.Conceal1H, () =>
                    {
                        this.UnequipItem(weapon); // take weapon from hand
                        this.EquipItem(weapon); // place weapon into parking slot
                    });
                }
                this.SetFightMode(false);
            }
            else
            {
                ItemInst item = Inventory.GetItem((int)itemID);
                if (item == null)
                    return;
                this.UnequipItem(item); // take weapon from parking slot
                this.EquipItem((int)SlotNums.Righthand, item); // put weapon into hand
                this.ModelInst.StartAnimation(this.AniCatalog.Draw1H, () => this.SetFightMode(true));
            }

        }

        public void DrawFists()
        {
            if (this.BaseInst.IsInFightMode)
            {
                if (this.DrawnWeapon != null)
                {
                    this.UnequipItem(this.DrawnWeapon);
                }
                else
                {
                    this.SetFightMode(false);
                }
            }
            else
            {
                this.SetFightMode(true);
            }
        }

        #endregion

        public bool IsPlayer { get { return this.BaseInst.IsPlayer; } }

        partial void pSetHealth(int hp, int hpmax)
        {
            if (hp <= 0)
            {
                // hitTimer.Stop(false);
                // comboTimer.Stop(false);
            }
        }

        GUCTimer hitTimer;
        GUCTimer comboTimer;
        bool canCombo = true;

        /*void AbleCombo()
        {
            comboTimer.Stop();
            if (this.Movement != MoveState.Stand)
            {
                var aa = this.GetFightAni();
                if (aa != null)
                    this.StopAnimation(aa);
            }
            else
            {
                canCombo = true;
            }
        }

        public void Hit(NPCInst attacker, int damage)
        {
            var strm = this.BaseInst.GetScriptVobStream();
            strm.Write((byte)NetWorldMsgID.HitMessage);
            strm.Write((ushort)this.ID);
            this.BaseInst.SendScriptVobStream(strm);
            
            if (damage > 0)
            {
                if (sOnHit != null)
                    sOnHit(attacker, this, damage);
            }
        }

        public delegate void OnHitHandler(NPCInst attacker, NPCInst target, int damage);
        public static event OnHitHandler sOnHit;

        void CalcHit()
        {
            try
            {
                hitTimer.Stop();

                if (this.BaseInst.IsDead || this.drawnWeapon == null)
                    return;

                ScriptAniJob attackerAni = (ScriptAniJob)this.GetFightAni()?.Ani.AniJob.ScriptObject;

                Vec3f attPos = this.BaseInst.GetPosition();
                Vec3f attDir = this.BaseInst.GetDirection();
                float range = this.DrawnWeapon.Definition.Range + this.Model.Radius + ModelDef.LargestNPC.Radius;
                this.BaseInst.World.ForEachNPCRough(attPos, range, npc =>
                {
                    NPCInst target = (NPCInst)npc.ScriptObject;
                    if (target != this && !target.BaseInst.IsDead)
                    {
                        Vec3f targetPos = npc.GetPosition();
                        Vec3f targetDir = npc.GetDirection();
                        float realRange = this.DrawnWeapon.Definition.Range + this.Model.Radius + target.Model.Radius;

                        ScriptAniJob targetAni = (ScriptAniJob)target.GetFightAni()?.Ani.AniJob.ScriptObject;

                        if (targetAni != null && targetAni.IsDodge)
                            realRange /= 2.0f;

                        if ((targetPos - attPos).GetLength() <= realRange) // target is in range
                        {
                            if (targetPos.Y + target.Model.Height / 2.0f >= attPos.Y && targetPos.Y - target.Model.Height / 2.0f <= attPos.Y) // same height
                            {
                                Vec3f dir = (attPos - targetPos).Normalise();
                                float dot = attDir.Z * dir.Z + dir.X * attDir.X;

                                if (dot < -0.2f) // target is in front of attacker
                                {
                                    float dist = attDir.X * (targetPos.Z - attPos.Z) - attDir.Z * (targetPos.X - attPos.X);
                                    dist = (float)Math.Sqrt(dist * dist / (attDir.X * attDir.X + attDir.Z * attDir.Z));

                                    if (dist <= target.Model.Radius + 10.0f) // distance to attack direction is smaller than radius + 10
                                    {
                                        dir = (targetPos - attPos).Normalise();
                                        dot = targetDir.Z * dir.Z + dir.X * targetDir.X;

                                        if (targetAni != null && targetAni.IsParade && dot <= -0.2f) // PARRY
                                        {
                                            var strm = this.BaseInst.GetScriptVobStream();
                                            strm.Write((byte)NetWorldMsgID.ParryMessage);
                                            strm.Write((ushort)npc.ID);
                                            this.BaseInst.SendScriptVobStream(strm);
                                        }
                                        else // HIT
                                        {
                                            int damage = (this.DrawnWeapon.Definition.Damage + attackerAni.AttackBonus) - (target.Armor == null ? 0 : target.Armor.Definition.Protection);
                                            if (this.GetJumpAni() != null || this.Environment == EnvironmentState.InAir) // Jump attaaaack!
                                                damage += 5;

                                            target.Hit(this, damage);
                                        }
                                    }
                                }
                            }
                        }
                    }
                });
            }
            catch (Exception e)
            {
                Log.Logger.Log("CalcHit of npc " + this.ID + " " + this.BaseInst.HP + " " + this.IsInAni() + " " + e);
            }
        }*/



    }
}
