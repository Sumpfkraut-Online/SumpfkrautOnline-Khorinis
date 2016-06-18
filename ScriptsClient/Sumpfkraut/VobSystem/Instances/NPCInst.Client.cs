using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;
using GUC.Scripting;
using Gothic.Objects;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
        partial void pRemoveItem(ItemInst item)
        {
        }

        public override void Spawn(WorldInst world)
        {
            base.Spawn(world);
            if (UseCustoms)
            {
                using (var vec = Gothic.Types.zVec3.Create(ModelScale.X, 1, ModelScale.Z))
                    this.BaseInst.gVob.SetModelScale(vec);

                this.BaseInst.gVob.SetAdditionalVisuals(HumBodyMeshs.HUM_BODY_NAKED0.ToString(), (int)CustomBodyTex, 0, CustomHeadMesh.ToString(), (int)CustomHeadTex, 0, -1);
                this.BaseInst.gVob.Voice = (int)CustomVoice;
                this.BaseInst.gVob.SetFatness(Fatness);
            }

            // TFFA
            foreach (TFFA.ClientInfo ci in TFFA.ClientInfo.ClientInfos.Values)
            {
                if (ci.CharID == this.ID)
                    this.BaseInst.gVob.Name.Set(Client.Scripts.TFFA.InputControl.ClientsShown ? string.Format("({0}){1}", ci.ID, ci.Name) : ci.Name);
            }

            this.BaseInst.ForEachEquippedItem(i => this.pEquipItem(i.Slot, (ItemInst)i.ScriptObject));
        }

        #region Equipment

        partial void pEquipItem(int slot, ItemInst item)
        {
            if (!this.IsSpawned)
                return;

            oCNpc gNpc = this.BaseInst.gVob;
            oCItem gItem = item.BaseInst.gVob;

            if (item.BaseInst.IsEquipped)
            {
                pUnequipItem(item);
            }

            switch ((SlotNums)slot)
            {
                case SlotNums.Sword:
                    gNpc.PutInSlot(oCNpc.NPCNodes.Sword, gItem, true);
                    break;

                case SlotNums.Longsword:
                    gNpc.PutInSlot(oCNpc.NPCNodes.Longsword, gItem, true);
                    break;

                case SlotNums.Bow:
                    gNpc.PutInSlot(oCNpc.NPCNodes.Bow, gItem, true);
                    break;

                case SlotNums.XBow:
                    gNpc.PutInSlot(oCNpc.NPCNodes.Crossbow, gItem, true);
                    break;

                case SlotNums.Torso:
                    gItem.VisualChange.Set(item.Definition.VisualChange);
                    gNpc.PutInSlot(oCNpc.NPCNodes.Torso, gItem, true);
                    break;

                case SlotNums.Righthand:
                    gNpc.PutInSlot(oCNpc.NPCNodes.RightHand, gItem, true);
                    if (item.ItemType == ItemTypes.WepXBow && this.ammo != null)
                    {
                        gNpc.PutInSlot(oCNpc.NPCNodes.LeftHand, ammo.BaseInst.gVob, true);
                    }
                    break;
                case SlotNums.Lefthand:
                    gNpc.PutInSlot(oCNpc.NPCNodes.LeftHand, gItem, true);
                    if (item.ItemType == ItemTypes.WepBow && this.ammo != null)
                    {
                        gNpc.PutInSlot(oCNpc.NPCNodes.RightHand, ammo.BaseInst.gVob, true);
                    }
                    break;
                default:
                    break;
            }
        }

        partial void pUnequipItem(ItemInst item)
        {
            if (!this.IsSpawned)
                return;

            oCNpc gNpc = this.BaseInst.gVob;
            oCItem gItem = item.BaseInst.gVob;

            switch ((SlotNums)item.BaseInst.Slot)
            {
                case SlotNums.Sword:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Sword, true, true);
                    break;

                case SlotNums.Longsword:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Longsword, true, true);
                    break;

                case SlotNums.Bow:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Bow, true, true);
                    break;

                case SlotNums.XBow:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Crossbow, true, true);
                    break;

                case SlotNums.Torso:
                    gItem.VisualChange.Set(item.Definition.VisualChange);
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.Torso, true, true);
                    break;

                case SlotNums.Righthand:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.RightHand, true, true);
                    if (item.ItemType == ItemTypes.WepXBow)
                    {
                        gNpc.RemoveFromSlot(oCNpc.NPCNodes.LeftHand, true, true);
                    }
                    break;
                case SlotNums.Lefthand:
                    gNpc.RemoveFromSlot(oCNpc.NPCNodes.LeftHand, true, true);
                    if (item.ItemType == ItemTypes.WepBow)
                    {
                        gNpc.RemoveFromSlot(oCNpc.NPCNodes.RightHand, true, true);
                    }
                    break;

                default:
                    break;
            }
        }

        #endregion

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
                    break;
            }
        }

        public void OnTick(long now)
        {
            if (this.BaseInst.IsDead)
                return;

            UpdateFightStance();

            var fightAni = (ScriptAniJob)this.GetFightAni()?.Ani.AniJob.ScriptObject;
            if (fightAni != null && fightAni.IsAttack)
            {
                this.BaseInst.gVob.AniCtrl.ShowWeaponTrail();
            }

            var activeJumpAni = GetJumpAni();
            if (activeJumpAni != null && activeJumpAni.GetPercent() >= 0.2f)
            {
                var gVob = this.BaseInst.gVob;
                var ai = gVob.HumanAI;
                if (((gVob.BitField1 & zCVob.BitFlag0.physicsEnabled) != 0) && ai.AboveFloor <= 0)
                {
                    // LAND
                    int id = this.Movement == MoveState.Forward ? ai._t_jump_2_runl : ai._t_jump_2_stand;
                    ai.LandAndStartAni(gVob.GetModel().GetAniFromAniID(id));
                }
            }

            if (this.drawnWeapon != null)
            {
                var gModel = this.BaseInst.gVob.GetModel();

                int aniID;
                if (this.DrawnWeapon.ItemType == ItemTypes.WepBow)
                {
                    aniID = gModel.GetAniIDFromAniName("S_BOWAIM");
                }
                else if (this.DrawnWeapon.ItemType == ItemTypes.WepXBow)
                {
                    aniID = gModel.GetAniIDFromAniName("S_CBOWAIM");
                }
                else
                {
                    return;
                }

                var aa = gModel.GetActiveAni(aniID);
                
                if (this.isAiming)
                {
                    if (aa.Address == 0)
                        gModel.StartAni(aniID, 0);
                }
                else
                {
                    if (aa.Address != 0)
                        gModel.StopAni(aa);
                }
            }
        }

        public void StartAnimation(Animations.Animation ani, object[] netArgs)
        {
            ScriptAni a = (ScriptAni)ani.ScriptObject;

            if (a.AniJob.IsJump)
            {
                this.StartAniJump(a, (int)netArgs[0], (int)netArgs[1]);
            }
            else if (a.AniJob.IsClimb)
            {
                this.StartAniClimb(a, (WorldObjects.NPC.ClimbingLedge)netArgs[0]);
            }
            else if (a.AniJob.IsDraw)
            {
                int itemID = (int)netArgs[0];
                WorldObjects.Item item;
                if (this.BaseInst.Inventory.TryGetItem(itemID, out item) && item.IsEquipped)
                {
                    this.StartAniDraw(a, (ItemInst)item.ScriptObject);
                }
            }
            else if (a.AniJob.IsUndraw)
            {
                int itemID = (int)netArgs[0];
                WorldObjects.Item item;
                if (this.BaseInst.Inventory.TryGetItem(itemID, out item) && item.IsEquipped)
                {
                    this.StartAniUndraw(a, (ItemInst)item.ScriptObject);
                }
            }

        }

        public void StartAniJump(ScriptAni ani, int fwdVelocity, int upVelocity)
        {
            this.StartAnimation(ani);

            var ai = this.BaseInst.gVob.HumanAI;
            ai.AniCtrlBitfield &= ~(1 << 3);
            //this.BaseInst.gVob.SetBodyState(8);

            var vel = new Gothic.Types.zVec3(ai.Address + 0x90);
            var dir = this.BaseInst.GetDirection();

            vel.X = dir.X * fwdVelocity;
            vel.Z = dir.Z * fwdVelocity;
            vel.Y = upVelocity;

            this.BaseInst.SetPhysics(true);

            this.BaseInst.SetVelocity((Vec3f)vel);
        }

        public void StartAniClimb(ScriptAni ani, WorldObjects.NPC.ClimbingLedge ledge)
        {
            this.BaseInst.SetGClimbingLedge(ledge);
            this.StartAnimation(ani);
        }

        static Client.SoundInstance sfx_DrawGeneric = new Client.SoundInstance("wurschtel.wav");

        static Client.SoundInstance sfx_DrawMetal = new Client.SoundInstance("Drawsound_ME.wav");
        static Client.SoundInstance sfx_DrawWood = new Client.SoundInstance("Drawsound_WO.wav");

        static Client.SoundInstance sfx_UndrawMetal = new Client.SoundInstance("Undrawsound_ME.wav");
        static Client.SoundInstance sfx_UndrawWood = new Client.SoundInstance("Undrawsound_WO.wav");

        GUCTimer drawTimer = new GUCTimer();
        public void StartAniDraw(ScriptAni ani, ItemInst item)
        {
            this.StartAnimation(ani);
            if (item.IsWepMelee)
            {
                drawTimer.SetInterval(ani.DrawTime);
                drawTimer.SetCallback(() =>
                {
                    drawTimer.Stop();
                    if (this.BaseInst.IsDead)
                        return;

                    if (item.Definition.Material == ItemMaterials.Metal)
                    {
                        Client.SoundHandler.PlaySound3D(sfx_DrawMetal, this.BaseInst);
                    }
                    else if (item.Definition.Material == ItemMaterials.Wood)
                    {
                        Client.SoundHandler.PlaySound3D(sfx_DrawWood, this.BaseInst);
                    }
                    else
                    {
                        Client.SoundHandler.PlaySound3D(sfx_DrawGeneric, this.BaseInst);
                    }
                });
                drawTimer.Start();
            }
        }

        public void StartAniUndraw(ScriptAni ani, ItemInst item)
        {
            this.StartAnimation(ani);
            if (item.IsWepMelee)
            {
                drawTimer.SetInterval(ani.DrawTime);
                drawTimer.SetCallback(() =>
                {
                    drawTimer.Stop();
                    if (this.BaseInst.IsDead)
                        return;

                    if (item.Definition.Material == ItemMaterials.Metal)
                    {
                        Client.SoundHandler.PlaySound3D(sfx_UndrawMetal, this.BaseInst);
                    }
                    else if (item.Definition.Material == ItemMaterials.Wood)
                    {
                        Client.SoundHandler.PlaySound3D(sfx_UndrawWood, this.BaseInst);
                    }
                    else
                    {
                        Client.SoundHandler.PlaySound3D(sfx_DrawGeneric, this.BaseInst);
                    }
                });
                drawTimer.Start();
            }
        }

        int fmode = 0;
        void UpdateFightStance()
        {
            int fmode;
            if (this.BaseInst.IsInFightMode)
            {
                if (this.drawnWeapon == null)
                {
                    fmode = 1; // fists
                }
                else
                {
                    if (this.drawnWeapon.ItemType == Definitions.ItemTypes.Wep1H)
                        fmode = 3;
                    else if (this.drawnWeapon.ItemType == Definitions.ItemTypes.Wep2H)
                        fmode = 4;
                    else if (this.drawnWeapon.ItemType == Definitions.ItemTypes.WepBow)
                        fmode = 5;
                    else if (this.drawnWeapon.ItemType == Definitions.ItemTypes.WepXBow)
                        fmode = 6;
                    else
                        fmode = 0;
                }
            }
            else
            {
                fmode = 0;
            }

            if (this.fmode != fmode)
            {
                var gNpc = this.BaseInst.gVob;
                var ai = gNpc.HumanAI;
                int oldRunID = ai._s_walkl;

                gNpc.FMode = fmode;
                ai.SetFightAnis(fmode);
                ai.SetWalkMode(0);

                var gModel = gNpc.GetModel();
                if (gModel.GetActiveAni(oldRunID).Address != 0)
                {
                    gModel.StartAni(ai._s_walkl, 0);
                }

                if (this == Networking.ScriptClient.Client.Character)
                {
                    gNpc.SetWeaponMode(fmode);
                }

                this.fmode = fmode;
            }
        }

        partial void pDespawn()
        {
            drawTimer.Stop();
        }
    }
}
