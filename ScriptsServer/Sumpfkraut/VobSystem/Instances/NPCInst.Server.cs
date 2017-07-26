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

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        public static readonly Networking.Requests.NPCRequestReceiver Requests = new Networking.Requests.NPCRequestReceiver();

        #region Constructors

        public NPCInst(NPCDef def) : this()
        {
            this.Definition = def;
        }

        partial void pConstruct()
        {
        }

        #endregion

        public NPCCatalog AniCatalog { get { return (NPCCatalog)this.ModelDef?.Catalog; } }

        #region Jumps

        bool isJumping = false;
        public bool IsJumping { get { return this.isJumping; } }

        /// <summary>
        /// Starts a jump animation, throws the npc with velocity and sets IsJumping to true for the time of the animation.
        /// </summary>
        /// <param name="move"></param>
        /// <param name="velocity"></param>
        public void DoJump(JumpMoves move, Vec3f velocity)
        {
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
            this.ModelInst.StartAniJob(job, () => this.isJumping = false);
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
            ModelInst.StartAniJob(this.AniCatalog.ItemHandling.DropItem, () => item.Spawn(this.World, spawnPos, spawnDir));
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
                    this.ModelInst.StartAniJob(AniCatalog.ItemHandling.EatSmall, () => { this.UnequipItem(item); this.EatItem(item); });
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.LargeEatable:
                    this.ModelInst.StartAniJob(AniCatalog.ItemHandling.EatLarge, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Mutton:
                    this.ModelInst.StartAniJob(AniCatalog.ItemHandling.EatMutton, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Rice:
                    this.ModelInst.StartAniJob(AniCatalog.ItemHandling.EatRice, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Drinkable:
                    this.ModelInst.StartAniJob(AniCatalog.ItemHandling.DrinkPotion, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Readable:
                    this.ModelInst.StartAniJob(AniCatalog.ItemHandling.ReadScroll, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
                case ItemTypes.Torch:
                    this.ModelInst.StartAniJob(AniCatalog.ItemHandling.UseTorch, () => this.UnequipItem(item));
                    this.EquipItem((int)SlotNums.Lefthand, item);
                    break;
            }
        }

        public void EatItem(ItemInst item)
        {

        }
        #endregion

        #region Fight Moves

        ScriptAni fightAni;
        public ScriptAni FightAnimation { get { return this.fightAni; } }

        int comboNum;
        public int ComboNum { get { return comboNum; } }

        bool canCombo = true;
        public bool CanCombo { get { return canCombo; } }

        bool isInAttack = false;
        public bool IsInAttack { get { return this.isInAttack; } }

        bool isParrying = false;
        public bool IsParrying { get { return this.isParrying; } }

        bool isDodging = false;
        public bool IsDodging { get { return this.isDodging; } }

        public void DoFightMove(FightMoves move, int combo = 0)
        {
            NPCCatalog.FightAnis fightCatalog;
            if (this.DrawnWeapon == null)
            {
                fightCatalog = AniCatalog.FightFist;
            }
            else
            {
                switch (this.DrawnWeapon.ItemType)
                {
                    case ItemTypes.Wep1H:
                        fightCatalog = AniCatalog.Fight1H;
                        break;
                    case ItemTypes.Wep2H:
                        fightCatalog = AniCatalog.Fight2H;
                        break;
                    default:
                        fightCatalog = AniCatalog.FightFist;
                        break;
                }
            }

            switch (move)
            {
                case FightMoves.Fwd:
                    DoAttack(fightCatalog.Fwd[combo], combo);
                    break;
                case FightMoves.Run:
                    DoAttack(fightCatalog.Run, 0);
                    break;
                case FightMoves.Left:
                    DoAttack(fightCatalog.Left, 0);
                    break;
                case FightMoves.Right:
                    DoAttack(fightCatalog.Right, 0);
                    break;
                case FightMoves.Parry:
                    DoParry(fightCatalog.GetRandomParry());
                    break;
                case FightMoves.Dodge:
                    DoDodge(fightCatalog.Dodge);
                    break;
            }
        }

        void DoAttack(ScriptAniJob job, int fwdCombo)
        {
            if (job == null)
                return;

            ScriptAni ani;
            if (!ModelInst.TryGetAniFromJob(job, out ani))
                return;

            // combo window
            float comboFrame;
            if (!ani.TryGetSpecialFrame(SpecialFrame.Combo, out comboFrame))
                comboFrame = ani.EndFrame;

            var comboPair = new Animations.FrameActionPair(comboFrame, () => canCombo = true);

            // hit frame
            float hitFrame;
            if (!ani.TryGetSpecialFrame(SpecialFrame.Hit, out hitFrame))
                hitFrame = comboFrame;

            if (hitFrame > comboFrame)
                hitFrame = comboFrame;

            var hitPair = new Animations.FrameActionPair(hitFrame, () => this.CalcHit());

            // end of animation
            var endPair = Animations.FrameActionPair.OnEnd(() =>
            {
                this.isInAttack = false;
                this.canCombo = true;
                this.comboNum = 0;
                this.fightAni = null;
            });

            // start it all
            this.ModelInst.StartAniJob(job, comboPair, hitPair, endPair);
            this.fightAni = ani;
            this.canCombo = false;
            this.isInAttack = true;
            this.isParrying = false;
            this.isDodging = false;
            this.comboNum = fwdCombo;
        }

        void DoParry(ScriptAniJob job)
        {
            if (job == null)
                return;

            ScriptAni ani;
            if (!ModelInst.TryGetAniFromJob(job, out ani))
                return;

            // end of animation
            var endPair = Animations.FrameActionPair.OnEnd(() =>
            {
                this.isParrying = false;
                this.canCombo = true;
                this.fightAni = null;
            });

            this.ModelInst.StartAniJob(job, endPair);
            this.fightAni = ani;
            this.canCombo = false;
            this.isInAttack = false;
            this.isParrying = true;
            this.isDodging = false;
            this.comboNum = 0;
        }

        void DoDodge(ScriptAniJob job)
        {
            if (job == null)
                return;

            ScriptAni ani;
            if (!ModelInst.TryGetAniFromJob(job, out ani))
                return;

            // end of animation
            var endPair = Animations.FrameActionPair.OnEnd(() =>
            {
                this.isDodging = false;
                this.canCombo = true;
                this.fightAni = null;
            });

            this.ModelInst.StartAniJob(job, endPair);
            this.fightAni = ani;
            this.canCombo = false;
            this.isInAttack = false;
            this.isParrying = false;
            this.isDodging = true;
            this.comboNum = 0;
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
                        this.ModelInst.StartAniJob(catalog.UndrawWhileRunning, 0.1f, () =>
                        {
                            this.UnequipItem(weapon); // take weapon from hand
                            this.EquipItem(weapon); // place weapon into parking slot
                        });
                    }
                    else
                    {
                        this.ModelInst.StartAniJob(catalog.Undraw, 0.1f, () =>
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
                    this.ModelInst.StartAniJob(catalog.DrawWhileRunning, 0.1f, () => this.SetFightMode(true));
                }
                else
                {
                    this.ModelInst.StartAniJob(catalog.Draw, 0.1f, () => this.SetFightMode(true));
                }
            }

        }

        public void DoDrawFists()
        {
            ScriptAniJob drawAniJob;
            if (this.Movement == NPCMovement.Stand && !this.Environment.InAir && this.Environment.WaterLevel < 0.4f)
            {
                drawAniJob = AniCatalog.DrawFists.Draw;
            }
            else
            {
                drawAniJob = AniCatalog.DrawFists.DrawWhileRunning;
            }

            if (drawAniJob == null)
                return;

            ScriptAni ani;
            if (!this.ModelInst.TryGetAniFromJob(drawAniJob, out ani))
                return;

            float drawFrame;
            if (!ani.TryGetSpecialFrame(0, out drawFrame))
                drawFrame = float.MaxValue;

            this.ModelInst.StartAniJob(drawAniJob, new Animations.FrameActionPair(drawFrame, () => this.SetFightMode(true)));
        }

        public void DoUndrawFists()
        {
            ScriptAniJob undrawAniJob;
            if (this.Movement == NPCMovement.Stand && !this.Environment.InAir && this.Environment.WaterLevel < 0.4f)
            {
                undrawAniJob = AniCatalog.DrawFists.Undraw;
            }
            else
            {
                undrawAniJob = AniCatalog.DrawFists.UndrawWhileRunning;
            }

            if (undrawAniJob == null)
                return;

            ScriptAni ani;
            if (!this.ModelInst.TryGetAniFromJob(undrawAniJob, out ani))
                return;

            float undrawFrame;
            if (!ani.TryGetSpecialFrame(0, out undrawFrame))
                undrawFrame = float.MaxValue;

            this.ModelInst.StartAniJob(undrawAniJob, new Animations.FrameActionPair(undrawFrame, () => this.SetFightMode(false)));
        }

        #endregion

        #region NPC Information

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
            
            if (damage > 0)
            {
                this.SetHealth(this.GetHealth() - damage);

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
                if (this.BaseInst.IsDead || this.FightAnimation == null)
                    return;

                Vec3f attPos = this.BaseInst.GetPosition();
                Vec3f attDir = this.BaseInst.GetDirection();
                float weaponRange = (this.DrawnWeapon == null ? 40 : this.DrawnWeapon.Definition.Range) + this.ModelDef.Radius;
                this.BaseInst.World.ForEachNPCRough(attPos, 2 * weaponRange, npc =>
                  {
                      NPCInst target = (NPCInst)npc.ScriptObject;
                      if (target == this || target.IsDead)
                          return;

                      Vec3f targetPos = npc.GetPosition();
                      Vec3f targetDir = npc.GetDirection();

                      float realRange = weaponRange + target.ModelDef.Radius;
                      if (target.IsDodging) realRange /= 2;

                      if ((targetPos - attPos).GetLength() > realRange)
                          return; // not in range

                      if (targetPos.Y + target.ModelDef.Height / 2.0f < attPos.Y || targetPos.Y - target.ModelDef.Height / 2.0f > attPos.Y)
                          return; // not same height

                      Vec3f dir = (attPos - targetPos).Normalise();
                      float dot = attDir.Z * dir.Z + dir.X * attDir.X;

                      if (dot > -0.2f) // target is not in front of attacker
                          return;

                      float dist = attDir.X * (targetPos.Z - attPos.Z) - attDir.Z * (targetPos.X - attPos.X);
                      dist = (float)Math.Sqrt(dist * dist / (attDir.X * attDir.X + attDir.Z * attDir.Z));

                      if (dist > target.ModelDef.Radius + 10.0f) // distance to attack direction is greater than radius + 10
                          return;

                      dir = (targetPos - attPos).Normalise();
                      dot = targetDir.Z * dir.Z + dir.X * targetDir.X;

                      if (target.IsParrying && dot <= -0.2f && target.DrawnWeapon != null) // PARRY
                      {
                          var strm = this.BaseInst.GetScriptVobStream();
                          strm.Write((byte)ScriptVobMessageIDs.ParryMessage);
                          strm.Write((ushort)npc.ID);
                          this.BaseInst.SendScriptVobStream(strm);
                      }
                      else // HIT
                      {
                          target.Hit(this, 5);
                      }
                  });
            }
            catch (Exception e)
            {
                Logger.Log("CalcHit of npc " + this.ID + " " + this.BaseInst.HP + " " + e);
            }
        }



    }
}
