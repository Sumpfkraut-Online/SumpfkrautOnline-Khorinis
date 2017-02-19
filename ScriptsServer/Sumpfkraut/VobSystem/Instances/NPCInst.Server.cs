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
        public static readonly Networking.Requests.NPCRequestReceiver Requests = new Networking.Requests.NPCRequestReceiver();

        private ScriptAniJob fightAni;
        public ScriptAniJob FightAnimation { get { return this.fightAni; } private set { this.fightAni = value; } }
        GUCTimer hitTimer;

        #region Constructors

        public NPCInst(NPCDef def) : this()
        {
            this.Definition = def;
        }

        partial void pConstruct()
        {
            this.isJumping = false;
            this.hitTimer = new GUCTimer(CalcHit);
        }

        #endregion

        public NPCCatalog AniCatalog { get { return (NPCCatalog)this.ModelDef?.Catalog; } }

        #region Jumps

        bool isJumping;
        public bool IsJumping { get { return this.isJumping; } }

        /// <summary>
        /// Starts a jump animation, throws the npc with velocity and sets IsJumping to true for the time of the animation.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="velocity"></param>
        public void DoJump(JumpMoves move, Vec3f velocity)
        {
            //if (this.IsDead || this.BaseInst.GetEnvironment().InAir)
            //    return;

            ScriptAniJob job;
            switch (move)
            {
                case JumpMoves.Fwd:
                    job = AniCatalog.Jumps.Fwd;
                    break;
                case JumpMoves.Run:
                    job = AniCatalog.Jumps.Run;
                    break;
                case JumpMoves.Up:
                    job = AniCatalog.Jumps.Up;
                    break;
                default:
                    Logger.Log("Not existing jump move: " + move);
                    return;
            }

            if (job == null)
                return;

            this.isJumping = true;
            // fixme: stop as soon as npc hits ground?
            this.ModelInst.StartAnimation(job, () => this.isJumping = false);
            this.Throw(velocity);
        }

        #endregion

        #region Drop & Take

        /// <summary>
        /// Starts a drop animation and drops any item in front of the npc
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void DropItem(ItemInst item, int amount, float offset = 50)
        {
            if (item == null)
                return;

            //if (item.Container != this)
            //    return;

            item = item.Split(amount);
            
            Vec3f spawnPos = this.GetPosition();
            Vec3f spawnDir = this.GetDirection();
            spawnPos += spawnDir * offset;

            // fixme: drop item at the item drop frame
            ModelInst.StartAnimation(this.AniCatalog.ItemHandling.DropItem, () => item.Spawn(this.World, spawnPos, spawnDir));
        }

        public void EquipItem(byte itemID)
        {
            ItemInst item = Inventory.GetItem(itemID);
            if (item == null)
                return;
            
            //if (this.ModelInst.BaseInst.IsInAnimation())
            //    return;

            if (!item.IsEquipped)
                EquipItem(item);
        }

        public void UnequipItem(byte itemID)
        {
            ItemInst item = Inventory.GetItem(itemID);
            if (item == null)
                return;

            // -> effectsystem
            if (this.ModelInst.BaseInst.IsInAnimation())
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
                    // TODO: eat item zu bestimmtem frame aufrufen
                    this.ModelInst.StartAnimation(AniCatalog.ItemHandling.EatSmall, () => { this.UnequipItem(item); this.EatItem(item); });
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

        public void EatItem(ItemInst item)
        {

        }
        #endregion

        #region Fight Moves



        public void DoFightMove(FightMoves move, int fwdCombo = 0)
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

            hitTimer.SetCallback(() =>
            {
                this.CalcHit();
                hitTimer.Stop();
            });
            // Frames / FPS * 0.5
            hitTimer.SetInterval(TimeSpan.TicksPerMillisecond * 250);
            hitTimer.Start();

            this.FightAnimation = job;
            this.ModelInst.StartAnimation(job, 1.0f, () => this.FightAnimation = null);
        }

        #endregion

        #region Weapon Drawing

        public void DrawWeapon(byte itemID)
        {
            // -> effectsystem
            if (this.ModelInst.BaseInst.IsInAnimation())
                return;

            NPCCatalog.DrawWeaponAnis catalog;
            if (this.BaseInst.IsInFightMode)
            {
                if (this.DrawnWeapon != null)
                {
                    ItemInst weapon = this.DrawnWeapon;
                    catalog = GetDrawWeaponCatalog(weapon.ItemType);
                    // weapon draw while running or when standing
                    if (this.Movement == NPCMovement.Forward || this.Movement == NPCMovement.Left || this.Movement == NPCMovement.Right)
                    {
                        this.ModelInst.StartAnimation(catalog.UndrawWhileRunning, 0.1f, () =>
                        {
                            this.UnequipItem(weapon); // take weapon from hand
                            this.EquipItem(weapon); // place weapon into parking slot
                        });
                    }
                    else
                    {
                        this.ModelInst.StartAnimation(catalog.Undraw, 0.1f, () =>
                        {
                            this.UnequipItem(weapon); // take weapon from hand
                            this.EquipItem(weapon); // place weapon into parking slot
                        });
                    }
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
                catalog = GetDrawWeaponCatalog(item.ItemType); // get animationset for the correct draw animation

                if (this.Movement == NPCMovement.Forward || this.Movement == NPCMovement.Left || this.Movement == NPCMovement.Right)
                {
                    this.ModelInst.StartAnimation(catalog.DrawWhileRunning, 0.1f, () => this.SetFightMode(true));
                }
                else
                {
                    this.ModelInst.StartAnimation(catalog.Draw, 0.1f, () => this.SetFightMode(true));
                }
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

        #region NPC Information
        public ScriptAniJob GetFightAni()
        {
            return fightAni;
        }

        public NPCCatalog.DrawWeaponAnis GetDrawWeaponCatalog(ItemTypes itemType)
        {
            switch (itemType)
            {
                case ItemTypes.Wep1H:
                    return AniCatalog.Draw1H;
                case ItemTypes.Wep2H:
                    return AniCatalog.Draw2H;
                case ItemTypes.WepBow:
                    return AniCatalog.DrawBow;
                case ItemTypes.WepXBow:
                    return AniCatalog.DrawXBow;
                case ItemTypes.Rune:
                    return AniCatalog.DrawMagic;
                case ItemTypes.Scroll:
                    return AniCatalog.DrawMagic;
                default:
                    return AniCatalog.Draw1H;
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

        public void Hit(NPCInst attacker, int damage)
        {
            var strm = this.BaseInst.GetScriptVobStream();
            strm.Write((byte)ScriptVobMessageIDs.HitMessage);
            strm.Write((ushort)this.ID);
            this.BaseInst.SendScriptVobStream(strm);

            this.SetHealth(this.GetHealth() - damage);

            if (damage > 0)
            {
                if (sOnHit != null)
                    sOnHit(attacker, this, damage);
            }
        }

        void AbleCombo()
        {
            //comboTimer.Stop();
            if (this.Movement != NPCMovement.Stand)
            {
                /*if (this.FightAnimation != null)
                    this.ModelInst.StopAnimation(, true);*/
            }
            else
            {
                //canCombo = true;
            }
        }


        public delegate void OnHitHandler(NPCInst attacker, NPCInst target, int damage);
        public static event OnHitHandler sOnHit;

        void CalcHit()
        {
            try
            {
                if (this.BaseInst.IsDead || this.drawnWeapon == null || this.FightAnimation == null)
                    return;

                ScriptAniJob attackerAni = this.FightAnimation;

                Vec3f attPos = this.BaseInst.GetPosition();
                Vec3f attDir = this.BaseInst.GetDirection();
                float range = this.DrawnWeapon.Definition.Range + this.ModelDef.Radius + ModelDef.LargestNPC.Radius;
                this.BaseInst.World.ForEachNPCRough(attPos, range, npc =>
                {
                    NPCInst target = (NPCInst)npc.ScriptObject;
                    if (target != this && !target.BaseInst.IsDead)
                    {
                        Vec3f targetPos = npc.GetPosition();
                        Vec3f targetDir = npc.GetDirection();
                        float realRange = this.DrawnWeapon.Definition.Range + this.ModelDef.Radius + target.ModelDef.Radius;

                        ScriptAniJob targetAni = target.FightAnimation;

                        if (targetAni != null && targetAni.IsDodge)
                            realRange /= 2.0f;

                        if ((targetPos - attPos).GetLength() <= realRange) // target is in range
                        {
                            if (targetPos.Y + target.ModelDef.Height / 2.0f >= attPos.Y && targetPos.Y - target.ModelDef.Height / 2.0f <= attPos.Y) // same height
                            {
                                Vec3f dir = (attPos - targetPos).Normalise();
                                float dot = attDir.Z * dir.Z + dir.X * attDir.X;

                                if (dot < -0.2f) // target is in front of attacker
                                {
                                    float dist = attDir.X * (targetPos.Z - attPos.Z) - attDir.Z * (targetPos.X - attPos.X);
                                    dist = (float)Math.Sqrt(dist * dist / (attDir.X * attDir.X + attDir.Z * attDir.Z));

                                    if (dist <= target.ModelDef.Radius + 10.0f) // distance to attack direction is smaller than radius + 10
                                    {
                                        dir = (targetPos - attPos).Normalise();
                                        dot = targetDir.Z * dir.Z + dir.X * targetDir.X;

                                        if (targetAni != null && targetAni.IsParade && dot <= -0.2f) // PARRY
                                        {
                                            var strm = this.BaseInst.GetScriptVobStream();
                                            strm.Write((byte)ScriptVobMessageIDs.ParryMessage);
                                            strm.Write((ushort)npc.ID);
                                            this.BaseInst.SendScriptVobStream(strm);
                                        }
                                        else // HIT
                                        {
                                            int damage = (this.DrawnWeapon.Definition.Damage + attackerAni.AttackBonus) - (target.Armor == null ? 0 : target.Armor.Definition.Protection);
                                            if (this.IsJumping || this.Environment.InAir) // Jump attaaaack!
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
                Log.Logger.Log("CalcHit of npc " + this.ID + " " + this.BaseInst.HP + " " + e);
            }
        }



    }
}
