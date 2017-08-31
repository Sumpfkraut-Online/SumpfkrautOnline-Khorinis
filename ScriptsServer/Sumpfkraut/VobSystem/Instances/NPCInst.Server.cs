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

        public delegate void NPCInstMoveHandler(NPCInst npc, Vec3f oldPos, Vec3f oldDir, NPCMovement oldMovement);
        public static event NPCInstMoveHandler sOnNPCInstMove;
        static NPCInst()
        {
            WorldObjects.NPC.OnNPCMove += (npc, p, d, m) => sOnNPCInstMove((NPCInst)npc.ScriptObject, p, d, m);
            sOnNPCInstMove += (npc, p, d, m) => npc.ChangePosDir(p, d, m);
        }

        void ChangePosDir(Vec3f oldPos, Vec3f oldDir, NPCMovement oldMovement)
        {
            if (this.FightAnimation != null && this.CanCombo && this.Movement != NPCMovement.Stand)
            { // so the npc can instantly stop the attack and run into a direction
                this.ModelInst.StopAnimation(this.fightAni, false);
            }
        }

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

        public bool IsPlayer { get { return this.BaseInst.IsPlayer; } }

        public ScriptClient Client { get { return (ScriptClient)this.BaseInst.Client?.ScriptObject; } }

        #region Jumps

        /// <summary>
        /// Starts an uncontrolled jump animation, throws the npc with velocity.
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
            
            this.ModelInst.StartAniJobUncontrolled(job);
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

        Animations.ActiveAni fightAni;
        public Animations.ActiveAni FightAnimation { get { return this.fightAni; } }

        int comboNum;
        public int ComboNum { get { return comboNum; } }

        bool canCombo = true;
        public bool CanCombo { get { return canCombo; } }

        FightMoves currentFightMove = FightMoves.None;
        public FightMoves CurrentFightMove { get { return this.currentFightMove; } }

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
                    default:
                    case ItemTypes.Wep1H:
                        fightCatalog = AniCatalog.Fight1H;
                        break;
                    case ItemTypes.Wep2H:
                        fightCatalog = AniCatalog.Fight2H;
                        break;
                }
            }

            switch (move)
            {
                case FightMoves.Fwd:
                    DoAttack(fightCatalog.Fwd[combo], move, combo);
                    break;
                case FightMoves.Run:
                    DoAttack(fightCatalog.Run, move);
                    break;
                case FightMoves.Left:
                    DoAttack(fightCatalog.Left, move);
                    break;
                case FightMoves.Right:
                    DoAttack(fightCatalog.Right, move);
                    break;
                case FightMoves.Parry:
                    DoParry(fightCatalog.GetRandomParry());
                    break;
                case FightMoves.Dodge:
                    DoDodge(fightCatalog.Dodge);
                    break;
            }
        }

        void DoAttack(ScriptAniJob job, FightMoves move, int fwdCombo = 0)
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

            var comboPair = new Animations.FrameActionPair(comboFrame, () => OpenCombo());

            // hit frame
            float hitFrame;
            if (!ani.TryGetSpecialFrame(SpecialFrame.Hit, out hitFrame))
                hitFrame = comboFrame;

            if (hitFrame > comboFrame)
                hitFrame = comboFrame;

            var hitPair = new Animations.FrameActionPair(hitFrame, () => this.CalcHit());

            // end of animation
            var endPair = Animations.FrameActionPair.OnEnd(() => EndFightAni());
            
            // start ani first, because the OnEnd-Callback from the former attack resets the fight stance
            this.fightAni = this.ModelInst.StartAniJob(job, comboPair, hitPair, endPair);
            this.currentFightMove = move;
            this.canCombo = false;
            this.comboNum = fwdCombo;
        }

        void OpenCombo()
        {
            if (this.Movement != NPCMovement.Stand && this.fightAni != null)
            { // so the npc can instantly stop the attack and run into a direction
                this.ModelInst.StopAnimation(this.fightAni, false);
            }
            else
            {
                canCombo = true;
            }
        }

        void EndFightAni()
        {
            this.currentFightMove = FightMoves.None;
            this.canCombo = true;
            this.comboNum = 0;
            this.fightAni = null;
        }

        void DoParry(ScriptAniJob job)
        {
            if (job == null)
                return;

            ScriptAni ani;
            if (!ModelInst.TryGetAniFromJob(job, out ani))
                return;

            // end of animation
            var endPair = Animations.FrameActionPair.OnEnd(() => EndFightAni());

            this.fightAni = this.ModelInst.StartAniJob(job, endPair);
            this.currentFightMove = FightMoves.Parry;
            this.canCombo = false;
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
            var endPair = Animations.FrameActionPair.OnEnd(() => EndFightAni());

            this.fightAni = this.ModelInst.StartAniJob(job, endPair);
            this.currentFightMove = FightMoves.Dodge;
            this.canCombo = false;
            this.comboNum = 0;
        }

        #endregion

        #region Weapon Drawing

        public void DoDrawWeapon(ItemInst item)
        {
            NPCCatalog.DrawWeaponAnis catalog;
            switch (item.ItemType)
            {
                default:
                case ItemTypes.Wep1H:
                    catalog = AniCatalog.Draw1H;
                    break;
                case ItemTypes.Wep2H:
                    catalog = AniCatalog.Draw2H;
                    break;
                case ItemTypes.WepBow:
                    catalog = AniCatalog.DrawBow;
                    break;
                case ItemTypes.WepXBow:
                    catalog = AniCatalog.DrawXBow;
                    break;
                case ItemTypes.Rune:
                case ItemTypes.Scroll:
                    catalog = AniCatalog.DrawMagic;
                    break;
            }

            if (this.IsInFightMode || this.DrawnWeapon != null)
            {
                /*if (this.DrawnWeapon != null)
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
                this.SetFightMode(false);*/
            }
            else
            {
                ScriptAniJob job = this.Movement == NPCMovement.Stand ? catalog.Draw : catalog.DrawWhileRunning;
                ScriptAni ani;
                if (job == null || !this.ModelInst.TryGetAniFromJob(job, out ani)) // no animation
                {
                    DrawWeapon(item);
                }
                else
                {
                    float drawFrame;
                    if (!ani.TryGetSpecialFrame(SpecialFrame.Draw, out drawFrame))
                        drawFrame = 0;

                    this.ModelInst.StartAniJob(job, new Animations.FrameActionPair(drawFrame, () => DrawWeapon(item)));
                }
            }
        }

        /// <summary>
        /// Instantly puts the item in the right hand and activates fight mode
        /// </summary>
        void DrawWeapon(ItemInst item)
        {
            this.UnequipItem(item); // take weapon from parking slot
            this.EquipItem((int)SlotNums.Righthand, item); // put weapon into hand
            this.drawnWeapon = item;
            this.SetFightMode(true); // look angry
        }

        public void DoUndrawWeapon(ItemInst item)
        {
            NPCCatalog.DrawWeaponAnis catalog;
            switch (item.ItemType)
            {
                default:
                case ItemTypes.Wep1H:
                    catalog = AniCatalog.Draw1H;
                    break;
                case ItemTypes.Wep2H:
                    catalog = AniCatalog.Draw2H;
                    break;
                case ItemTypes.WepBow:
                    catalog = AniCatalog.DrawBow;
                    break;
                case ItemTypes.WepXBow:
                    catalog = AniCatalog.DrawXBow;
                    break;
                case ItemTypes.Rune:
                case ItemTypes.Scroll:
                    catalog = AniCatalog.DrawMagic;
                    break;
            }

            if (this.DrawnWeapon == null)
            {
            }
            else
            {
                ScriptAniJob job = this.Movement == NPCMovement.Stand ? catalog.Undraw : catalog.UndrawWhileRunning;
                ScriptAni ani;
                if (job == null || !this.ModelInst.TryGetAniFromJob(job, out ani)) // no animation
                {
                    UndrawWeapon(item);
                }
                else
                {
                    float drawFrame;
                    if (!ani.TryGetSpecialFrame(SpecialFrame.Draw, out drawFrame))
                        drawFrame = 0;

                    this.ModelInst.StartAniJob(job, new Animations.FrameActionPair(drawFrame, () => UndrawWeapon(item)));
                }
            }
        }

        /// <summary>
        /// Instantly puts the item back and deactivates fight mode
        /// </summary>
        void UndrawWeapon(ItemInst item)
        {
            this.UnequipItem(item); // take weapon from right hand
            this.EquipItem(item); // put weapon into parking slot
            this.drawnWeapon = null;
            this.SetFightMode(false);
        }

        /// <summary>
        /// Plays the draw animation and activates fight mode
        /// </summary>
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

        public void Hit(NPCInst attacker, int damage)
        {
            var strm = this.BaseInst.GetScriptVobStream();
            strm.Write((byte)ScriptVobMessageIDs.HitMessage);
            strm.Write((ushort)this.ID);
            this.BaseInst.SendScriptVobStream(strm);

            if (this.Armor != null)
                damage -= this.Armor.Protection;
            if (damage < 0)
                damage = 1;

            this.SetHealth(this.GetHealth() - damage);

            if (sOnHit != null)
                sOnHit(attacker, this, damage);
        }


        public delegate void OnHitHandler(NPCInst attacker, NPCInst target, int damage);
        public static event OnHitHandler sOnHit;

        public delegate bool OnHitCheckHandler(NPCInst attacker, NPCInst target);
        public static event OnHitCheckHandler sOnHitCheck;

        void CalcHit()
        {
            try
            {
                if (this.IsDead || this.FightAnimation == null)
                    return;

                Vec3f attPos = this.BaseInst.GetPosition();
                Vec3f attDir = this.BaseInst.GetDirection();
                float weaponRange = this.ModelDef.Radius + (this.DrawnWeapon == null ? this.ModelDef.FistRange : this.DrawnWeapon.Definition.Range);
                this.BaseInst.World.ForEachNPCRough(attPos, 2 * weaponRange, npc => // fixme: enemy model radius
                  {
                      NPCInst target = (NPCInst)npc.ScriptObject;
                      if (target == this || target.IsDead)
                          return;

                      if (sOnHitCheck != null && !sOnHitCheck(this, target))
                          return;

                      Vec3f targetPos = npc.GetPosition();
                      Vec3f targetDir = npc.GetDirection();

                      float realRange = weaponRange + target.ModelDef.Radius;
                      if (target.CurrentFightMove == FightMoves.Dodge) realRange /= 2.0f; // extra radius if target is backing up

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

                      if (target.CurrentFightMove == FightMoves.Parry && dot <= -0.2f && target.DrawnWeapon != null) // PARRY
                      {
                          var strm = this.BaseInst.GetScriptVobStream();
                          strm.Write((byte)ScriptVobMessageIDs.ParryMessage);
                          strm.Write((ushort)npc.ID);
                          this.BaseInst.SendScriptVobStream(strm);
                      }
                      else // HIT
                      {
                          int damage = (this.DrawnWeapon == null ? 10 : this.DrawnWeapon.Damage);

                          target.Hit(this, damage);
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
