using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class NPCInst
    {
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
                this.BaseInst.gVob.Name.Set(CustomName);
            }

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
                    break;
            }
        }

        public void OnTick(long now)
        {
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
                if (((gVob.BitField1 & Gothic.Objects.zCVob.BitFlag0.physicsEnabled) != 0) && ai.AboveFloor <= 0)
                {
                    // LAND
                    int id = this.Movement == MoveState.Forward ? ai._t_jump_2_runl : ai._t_jump_2_stand;
                    ai.LandAndStartAni(gVob.GetModel().GetAniFromAniID(id));
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
            else if (a.AniJob.IsClimbing)
            {
                this.StartAniClimb(a, (WorldObjects.NPC.ClimbingLedge)netArgs[0]);
            }
        }

        public void StartAniJump(ScriptAni ani, int fwdVelocity, int upVelocity)
        {
            this.StartAnimation(ani);

            var ai = this.BaseInst.gVob.HumanAI;
            ai.BitField &= ~(1 << 3);
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
    }
}
