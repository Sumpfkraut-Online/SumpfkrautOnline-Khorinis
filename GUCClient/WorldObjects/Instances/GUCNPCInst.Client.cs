using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using Gothic.Objects;
using Gothic.Objects.Meshes;
using Gothic.Types;
using WinApi;
using GUC.Network;
using GUC.Scripting;

namespace GUC.WorldObjects.Instances
{
    public partial class GUCNPCInst
    {
        const long HeroPosUpdateInterval = 40 * TimeSpan.TicksPerMillisecond;
        const long NPCPosUpdateInterval = 80 * TimeSpan.TicksPerMillisecond;

        #region Network Messages

        new internal static class Messages
        {
            #region Equipment

            public static void ReadEquipMessage(PacketReader stream)
            {
                if (World.Current.TryGetVob(stream.ReadUShort(), out GUCNPCInst npc))
                {
                    int slot = stream.ReadByte();
                    byte type = stream.ReadByte();

                    GUCItemInst item;
                    if (npc != Hero)
                    {
                        item = (GUCItemInst)ScriptManager.Interface.CreateVob(type);
                        item.ReadEquipProperties(stream);
                        npc.Inventory.ScriptObject.AddItem(item);
                    }
                    else if (!npc.Inventory.TryGetItem(stream.ReadByte(), out item)) // fixme
                        return;

                    npc.ScriptObject.EquipItem(slot, item);
                }
            }

            public static void ReadEquipSwitchMessage(PacketReader stream)
            {
                if (World.Current.TryGetVob(stream.ReadUShort(), out GUCNPCInst npc)
                    && npc.Inventory.TryGetItem(stream.ReadByte(), out GUCItemInst item))
                {
                    npc.ScriptObject.EquipItem(stream.ReadByte(), item);
                }
            }

            public static void ReadUnequipMessage(PacketReader stream)
            {
                if (World.Current.TryGetVob(stream.ReadUShort(), out GUCNPCInst npc)
                    && npc.Inventory.TryGetItem(stream.ReadByte(), out GUCItemInst item))
                {
                    npc.ScriptObject.UnequipItem(item);
                    if (npc != Hero)
                    {
                        npc.Inventory.ScriptObject.RemoveItem(item);
                    }
                }
            }


            public static void ReadPlayerEquipMessage(PacketReader stream)
            {
                if (Hero.Inventory.TryGetItem(stream.ReadByte(), out GUCItemInst item))
                {
                    Hero.ScriptObject.EquipItem(stream.ReadByte(), item);
                }
            }

            #endregion

            #region Health

            public static void ReadHealth(PacketReader stream)
            {
                int id = stream.ReadUShort();

                if (World.Current.TryGetVob(id, out GUCNPCInst npc))
                {
                    int hpmax = stream.ReadUShort();
                    int hp = stream.ReadUShort();
                    npc.ScriptObject.SetHealth(hp, hpmax);
                }
            }

            #endregion

            #region Fight Mode

            public static void ReadFightMode(PacketReader stream, bool fightMode)
            {
                if (World.Current.TryGetVob(stream.ReadUShort(), out GUCNPCInst npc))
                {
                    npc.ScriptObject.SetFightMode(fightMode);
                }
            }

            #endregion

            #region Position Updates

            public static void ReadPosAngMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();

                if (World.Current.TryGetVob(id, out GUCNPCInst npc))
                {
                    Vec3f newPos = stream.ReadCompressedPosition();
                    Angles newAng = stream.ReadCompressedAngles();
                    npc.Interpolate(newPos, newAng);

                    npc.ScriptObject.SetMovement((NPCMovement)stream.ReadByte());

                    //vob.ScriptObject.OnPosChanged();
                }
                else
                {
                    VobGuiding.TargetCmd.CheckPos(id, stream.ReadCompressedPosition());
                }
            }

            public static void WritePosAngMessage(GUCNPCInst npc, Vec3f pos, Angles ang, VobEnvironment env)
            {
                PacketWriter stream = GameClient.SetupStream(ClientMessages.GuidedNPCMessage);
                stream.Write((ushort)npc.ID);
                stream.WriteCompressedPosition(pos);
                stream.WriteCompressedAngles(ang);

                // compress environment & npc movement
                int bitfield = env.InAir ? 0x8000 : 0;
                bitfield |= (int)npc.movement << 12;
                bitfield |= (int)(env.WaterDepth * 0x3F) << 6;
                bitfield |= (int)(env.WaterLevel * 0x3F);
                stream.Write((short)bitfield);

                GameClient.Send(stream, NetPriority.Low, NetReliability.Unreliable);
            }

            #endregion
        }

        #endregion

        #region Script Command Message

        public PacketWriter GetScriptCommandStream()
        {
            if (this == Hero)
            {
                return GameClient.SetupStream(ClientMessages.ScriptCommandHeroMessage);
            }
            else if (this.guide != null)
            {
                var stream = GameClient.SetupStream(ClientMessages.ScriptCommandMessage);
                stream.Write((ushort)this.ID);
                return stream;
            }
            else
            {
                throw new ArgumentException("Vob is not guided by client!");
            }
        }

        public static void SendScriptCommand(PacketWriter stream, NetPriority priority)
        {
            GameClient.Send(stream, priority, NetReliability.Unreliable, 'C');
        }

        #endregion

        /// <summary> The npc the client is controlling. </summary>
        public static GUCNPCInst Hero { get { return GameClient.Client.Character; } }

        internal static void UpdateHero(long now)
        {
            var hero = Hero;
            if (hero == null)
                return;

            hero.UpdateGuidedNPCPosition(now, HeroPosUpdateInterval, 15, 0.01f); // update our hero better
        }

        #region Vob Guiding

        protected override void UpdateGuidePos(long now)
        {
            UpdateGuidedNPCPosition(now, NPCPosUpdateInterval, 15, 0.02f);
        }

        NPCMovement guidedLastMovement;
        void UpdateGuidedNPCPosition(long now, long interval, float minPosDist, float minAngDist)
        {
            if (now < guidedNextUpdate)
                return;

            Vec3f pos = this.Position;
            Angles ang = this.Angles;
            VobEnvironment env = this.Environment;

            if (now - guidedNextUpdate < TimeSpan.TicksPerSecond)
            {
                // nothing really changed, only update every second
                if (guidedLastMovement == this.movement
                    && pos.GetDistance(guidedLastPos) < minPosDist
                    && !ang.DifferenceIsBigger(guidedLastAng, minAngDist)
                    && env == guidedLastEnv)
                {
                    return;
                }
            }

            guidedLastMovement = this.movement;
            guidedLastPos = pos;
            guidedLastAng = ang;
            guidedLastEnv = env;

            Messages.WritePosAngMessage(this, pos, ang, env);

            guidedNextUpdate = now + interval;

            //this.ScriptObject.OnPosChanged();
        }

        #endregion

        #region ScriptObject

        public partial interface IScriptNPC : IScriptVob
        {
            void SetMovement(NPCMovement state);
            void OnTick(long now);
        }

        #endregion

        #region Climbing

        public partial class ClimbingLedge
        {
            internal ClimbingLedge(zTLedgeInfo li)
            {
                this.loc = (Vec3f)li.Position;
                this.norm = (Vec3f)li.Normal;
                this.cont = (Vec3f)li.Cont;
                this.maxMoveFwd = li.MaxMoveForward;
            }

            internal void SetLedgeInfo(zTLedgeInfo li)
            {
                li.Position.X = this.loc.X;
                li.Position.Y = this.loc.Y;
                li.Position.Z = this.loc.Z;

                li.Normal.X = this.norm.X;
                li.Normal.Y = this.norm.Y;
                li.Normal.Z = this.norm.Z;

                li.Cont.X = this.cont.X;
                li.Cont.Y = this.cont.Y;
                li.Cont.Z = this.cont.Z;

                li.MaxMoveForward = this.maxMoveFwd;
            }
        }

        public ClimbingLedge DetectClimbingLedge()
        {
            ClimbingLedge result;

            using (var dummy = zVec3.Create())
                humanAI.DetectClimbUpLedge(dummy, true);

            var li = humanAI.GetLedgeInfo();
            if (li.Address != 0)
            {
                result = new ClimbingLedge(li);
            }
            else
            {
                result = null;
            }
            humanAI.ClearFoundLedge();

            return result;
        }

        public void SetGClimbingLedge(ClimbingLedge ledge)
        {
            int ai = Process.Alloc(4).ToInt32();
            Process.Write(ai, humanAI.Address);

            Process.THISCALL<NullReturnCall>(0x8D44E0, 0x512310, (IntArg)ai);
            if (Process.THISCALL<BoolArg>(0x8D44E0, 0x5123E0, (IntArg)ai))
            {
                var li = Process.THISCALL<zTLedgeInfo>(0x8D44E0, 0x512310, (IntArg)ai);
                ledge.SetLedgeInfo(li);
            }
            else
            {
                throw new Exception("SetGClimbingLedge: GetDataDangerous failed!");
            }
        }

        #endregion

        #region Properties

        /// <summary> The correlated gothic-object of this npc. </summary>
        new public oCNpc gVob { get { return (oCNpc)base.gVob; } }

        oCAIHuman humanAI;
        public oCAIHuman gAI { get { return this.humanAI; } }

        zCModel gmodel;
        public zCModel gModel { get { return this.gmodel; } }

        #region Environment

        public override void UpdateEnvironment()
        {
            this.environment = CalculateEnvironment(humanAI.StepHeight);
        }

        #endregion

        #endregion

        #region Equipment

        partial void pEquipItem(int slot, GUCItemInst item)
        {
            item.CreateGVob();
        }

        partial void pUnequipItem(GUCItemInst item)
        {
            item.DeleteGVob();
        }

        #endregion

        #region Spawn

        /// <summary> Spawns the NPC in the given world at the given position & direction. </summary>
        public override void Spawn(World world, Vec3f position, Angles angles)
        {
            base.Spawn(world, position, angles);

            gVob.HP = this.hp;
            gVob.HPMax = this.hpmax;

            gVob.InitHumanAI();
            this.humanAI = gVob.HumanAI;
            this.gmodel = gVob.GetModel();

            // do detect walk stop chasm
            humanAI.Bitfield0 &= ~zCAIPlayer.Flags.DetectWalkChasm; // some shitty flag which makes npcs always check their ground

            if (this.hp <= 0)
            {
                gModel.StartAnimation("S_DEAD");
            }
        }

        partial void pAfterDespawn()
        {
            this.inventory.ForEach(item =>
            {
                if (item.gVob != null)
                    item.DeleteGVob();
            });

            this.humanAI = null;
            this.gmodel = null;
        }

        internal override void DeleteGVob()
        {
            this.gVob.SetFocusVob(new zCVob(0));
            base.DeleteGVob();
        }

        #endregion

        #region Health

        partial void pSetHealth()
        {
            if (this.gVob == null)
                return;

            /*var gModel = gModel;
            var gAniCtrl = gAI;

            gModel.StopAni(gAniCtrl._t_strafel);
            gModel.StopAni(gAniCtrl._t_strafer);*/

            this.gVob.HPMax = this.hpmax;
            this.gVob.HP = this.hp;

            if (this.hp == 0)
            {
                gModel.StopAnisLayerRange(int.MinValue, int.MaxValue);
                this.gVob.DoDie();
            }
        }

        #endregion

        #region Move State

        public void SetMovement(NPCMovement state)
        {
            if (this.movement == state)
                return;

            if (this.gVob != null && !this.IsDead && this.Model.GetActiveAniFromLayerID(1) == null
                && !this.Environment.InAir && (gVob.BitField1 & zCVob.BitFlag0.physicsEnabled) == 0)
                if (this.movement == NPCMovement.Right || this.movement == NPCMovement.Left)
                {
                    if (state == NPCMovement.Forward)
                        gModel.StartAni(gAI._s_walkl, 0);
                    else
                        gModel.StartAni(gAI._s_walk, 0);
                }

            this.movement = state;

            guidedNextUpdate = 0;
            this.OnTick(GameTime.Ticks);
        }

        #endregion

        #region Turning

        const float MinYawDiffToAnimate = 0.001f;
        const long InterpolationTimeAng = 1200000;

        Angles iAngStart;
        Angles iAngEnd;
        long iAngEndTime = 0;

        protected override void Interpolate(Vec3f newPos, Angles newAng)
        {
            // gothic is already setting positions through animations, gravity etc. 
            // so we just set the position because it would look twitchy otherwise
            SetPosition(newPos);

            Angles oldAng = this.ang;
            Angles curAng = Angles;

            if (Movement == NPCMovement.Stand
                && !newAng.DifferenceIsBigger(curAng, Angles.PI / 2f) // don't interpolate > 90 degree turns
                && newAng.DifferenceIsBigger(curAng, 0.00001f)) // don't interpolate infinitesimal turns
            {
                iAngStart = curAng;
                iAngEnd = newAng;
                iAngEndTime = GameTime.Ticks + InterpolationTimeAng;

                float diff;
                if (!this.Model.IsInAnimation() && Math.Abs(diff = Angles.Difference(newAng.Yaw, curAng.Yaw)) > MinYawDiffToAnimate)
                {
                    StartTurnAni(diff < 0);
                    return;
                }
            }
            else
            {
                SetAngles(newAng);
                iAngEndTime = 0;
            }
            StopTurnAni();
        }

        void UpdateInterpolation(long now)
        {
            if (iAngEndTime > 0)
            {
                long timeDiff = iAngEndTime - now;
                if (timeDiff < 0)
                {
                    iAngEndTime = 0;
                    SetAngles(iAngEnd);
                    StopTurnAni();
                }
                else
                {
                    SetAngles(iAngStart + Angles.Difference(iAngEnd, iAngStart) * (float)Math.Pow(1.0 - (double)timeDiff / InterpolationTimeAng, 1 / 3d));
                }
            }
        }

        int activeTurnAni = 0;
        void StartTurnAni(bool right)
        {
            if (gModel.IsAniActive(gModel.GetAniFromAniID(gAI._s_walk)))
            {
                activeTurnAni = right ? gAI._t_turnr : gAI._t_turnl;
            }
            else if (gModel.IsAniActive(gModel.GetAniFromAniID(gAI._s_dive)))
            {
                activeTurnAni = right ? gAI._t_diveturnr : gAI._t_diveturnl;
            }
            else if (gModel.IsAniActive(gModel.GetAniFromAniID(gAI._s_swim)))
            {
                activeTurnAni = right ? gAI._t_swimturnr : gAI._t_swimturnl;
            }
            else
            {
                StopTurnAni();
                return;
            }

            gModel.StartAni(activeTurnAni, 0);
        }

        void StopTurnAni()
        {
            if (activeTurnAni != 0)
            {
                gModel.FadeOutAni(activeTurnAni);
                activeTurnAni = 0;
            }
        }

        #endregion

        partial void pOnTick(long now)
        {
            if (gVob == null || gAI == null || gModel == null)
                return;

            UpdateInterpolation(now);

            this.ScriptObject.OnTick(now);

            if (!this.IsDead)
            {
                switch (Movement)
                {
                    case NPCMovement.Right:
                        DoStrafe(true);
                        break;
                    case NPCMovement.Left:
                        DoStrafe(false);
                        break;
                    case NPCMovement.Forward:
                        if (!this.Environment.InAir && gModel.IsAnimationActive("T_JUMP_2_STAND"))
                        {
                            gAI.LandAndStartAni(gModel.GetAniFromAniID(gAI._t_jump_2_runl));
                        }
                        gAI._Forward();
                        break;
                    case NPCMovement.Backward:
                        gAI._Backward();
                        break;
                    case NPCMovement.Stand:
                        gAI._Stand();
                        break;
                    default:
                        break;
                }
            }
        }

        void DoStrafe(bool right)
        {
            if (this.Model.GetActiveAniFromLayerID(1) != null || this.Environment.InAir
                || (gVob.BitField1 & zCVob.BitFlag0.physicsEnabled) != 0)
                return;

            var strafeAni = right ? gAI._t_strafer : gAI._t_strafel;
            var ani = gModel.GetAniFromAniID(strafeAni);
            if (ani.Address == 0)
                return;

            var aa = gModel.GetActiveAni(gModel.GetAniIDFromAniName("T_FALLDN_2_STAND"));
            if (aa.Address != 0)
            {
                if (aa.GetProgressPercent() > 0.8f)
                    gAI.LandAndStartAni(ani);

                return;
            }

            aa = gModel.GetActiveAni(gModel.GetAniIDFromAniName("T_JUMP_2_STAND"));
            if (aa.Address != 0)
            {
                if (aa.GetProgressPercent() > 0.5f)
                    gAI.LandAndStartAni(ani);

                return;
            }

            if (!gModel.IsAniActive(ani))
                gModel.StartAni(ani, 0);

        }

        public override void Throw(Vec3f velocity)
        {
            var gVel = gAI.Velocity;
            velocity.SetGVec(gVel);

            SetPhysics(true);

            var rb = Process.ReadInt(gVob.Address + 224);
            Process.THISCALL<NullReturnCall>(rb, 0x5B66D0, gVel);
        }

        public GUCBaseVobInst GetFocusVob()
        {
            this.World.TryGetVobByAddress(this.gVob.FocusVob.Address, out GUCBaseVobInst baseVob);
            return baseVob;
        }
    }
}
