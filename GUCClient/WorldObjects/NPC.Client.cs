using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using Gothic.Objects;
using GUC.Animations;
using Gothic.Types;
using WinApi;
using GUC.Network;
using GUC.Scripting;

namespace GUC.WorldObjects
{
    public partial class NPC
    {
        #region Network Messages

        new internal static class Messages
        {
            #region Equipment

            public static void ReadEquipMessage(PacketReader stream)
            {
                NPC npc;
                if (World.Current.TryGetVob(stream.ReadUShort(), out npc))
                {
                    int slot = stream.ReadByte();

                    Item item;
                    if (npc != Hero)
                    {
                        item = (Item)ScriptManager.Interface.CreateVob(VobTypes.Item);
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
                NPC npc; Item item;
                if (World.Current.TryGetVob(stream.ReadUShort(), out npc)
                    && npc.Inventory.TryGetItem(stream.ReadByte(), out item))
                {
                    npc.ScriptObject.EquipItem(stream.ReadByte(), item);
                }
            }

            public static void ReadUnequipMessage(PacketReader stream)
            {
                NPC npc; Item item;
                if (World.Current.TryGetVob(stream.ReadUShort(), out npc)
                    && npc.Inventory.TryGetItem(stream.ReadByte(), out item))
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
                Item item;
                if (Hero.Inventory.TryGetItem(stream.ReadByte(), out item))
                {
                    Hero.ScriptObject.EquipItem(stream.ReadByte(), item);
                }
            }

            #endregion

            #region Health

            public static void ReadHealth(PacketReader stream)
            {
                int id = stream.ReadUShort();

                NPC npc;
                if (World.Current.TryGetVob(id, out npc))
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
                NPC npc;
                if (World.Current.TryGetVob(stream.ReadUShort(), out npc))
                {
                    npc.ScriptObject.SetFightMode(fightMode);
                }
            }

            #endregion

            #region Position Updates

            public static void ReadPosDirMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();

                NPC npc;
                if (World.Current.TryGetVob(id, out npc))
                {
                    Vec3f newPos = stream.ReadCompressedPosition();
                    Vec3f newDir = stream.ReadCompressedDirection();
                    npc.Interpolate(newPos, newDir);

                    npc.ScriptObject.SetMovement((NPCMovement)stream.ReadByte());

                    //vob.ScriptObject.OnPosChanged();
                }
                else
                {
                    VobGuiding.TargetCmd.CheckPos(id, stream.ReadCompressedPosition());
                }
            }

            public static void WritePosDirMessage(NPC npc, Vec3f pos, Vec3f dir, Environment env)
            {
                PacketWriter stream = GameClient.SetupStream(ClientMessages.GuidedNPCMessage);
                stream.Write((ushort)npc.ID);
                stream.WriteCompressedPosition(pos);
                stream.WriteCompressedDirection(dir);

                // compress environment & npc movement
                int bitfield = env.InAir ? 0x8000 : 0;
                bitfield |= (int)npc.movement << 12;
                bitfield |= (int)(env.WaterDepth * 0x3F) << 6;
                bitfield |= (int)(env.WaterLevel * 0x3F);
                stream.Write((short)bitfield);

                GameClient.Send(stream, PktPriority.Low, PktReliability.Unreliable);
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

        public static void SendScriptCommand(PacketWriter stream, PktPriority priority)
        {
            GameClient.Send(stream, priority, PktReliability.Unreliable, 'C');
        }

        #endregion

        /// <summary> The npc the client is controlling. </summary>
        public static NPC Hero { get { return GameClient.Client.Character; } }

        internal static void UpdateHero(long now)
        {
            var hero = Hero;
            if (hero == null)
                return;

            hero.UpdateGuidedNPCPosition(now, 800000, 10, 0.02f); // update our hero better
        }

        #region Vob Guiding

        protected override void UpdateGuidePos(long now)
        {
            UpdateGuidedNPCPosition(now, 1200000, 14, 0.04f);
        }

        NPCMovement guidedLastMovement;
        void UpdateGuidedNPCPosition(long now, long interval, float minPosDist, float minDirDist)
        {
            if (now < guidedNextUpdate)
                return;

            Vec3f pos = this.GetPosition();
            Vec3f dir = this.GetDirection();
            Environment env = this.GetEnvironment();

            if (now - guidedNextUpdate < TimeSpan.TicksPerSecond)
            {
                // nothing really changed, only update every second
                if (guidedLastMovement == this.movement
                    && pos.GetDistance(guidedLastPos) < minPosDist
                    && dir.GetDistance(guidedLastDir) < minDirDist
                    && env == guidedLastEnv)
                {
                    return;
                }
            }

            guidedLastMovement = this.movement;
            guidedLastPos = pos;
            guidedLastDir = dir;
            guidedLastEnv = env;

            Messages.WritePosDirMessage(this, pos, dir, env);

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

            var ai = this.gVob.HumanAI;

            using (var dummy = zVec3.Create())
                ai.DetectClimbUpLedge(dummy, true);

            var li = ai.GetLedgeInfo();
            if (li.Address != 0)
            {
                result = new ClimbingLedge(li);
            }
            else
            {
                result = null;
            }
            ai.ClearFoundLedge();

            return result;
        }

        public void SetGClimbingLedge(ClimbingLedge ledge)
        {
            int ai = Process.Alloc(4).ToInt32();
            Process.Write(this.gVob.HumanAI.Address, ai);

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

        #region Environment

        partial void pGetEnvironment()
        {
            this.environment = CalculateEnvironment(this.gVob.HumanAI.StepHeight);
        }

        #endregion

        #endregion

        #region Equipment

        partial void pEquipItem(int slot, Item item)
        {
            item.CreateGVob();
        }

        partial void pUnequipItem(Item item)
        {
            item.DeleteGVob();
        }

        #endregion

        #region Spawn

        /// <summary> Spawns the NPC in the given world at the given position & direction. </summary>
        public override void Spawn(World world, Vec3f position, Vec3f direction)
        {
            base.Spawn(world, position, direction);

            gVob.HP = this.hp;
            gVob.HPMax = this.hpmax;

            gVob.Guild = 1;
            gVob.InitHumanAI();

            if (string.Compare(this.ModelInstance.Visual, "humans.mds", true) != 0)
                gVob.SetToFistMode();

            gVob.Name.Set(this.Name);

            gVob.HumanAI.Bitfield0 &= ~4; // some shitty flag which makes npcs always check their ground
        }

        partial void pAfterDespawn()
        {
            this.inventory.ForEach(item =>
            {
                if (item.gVob != null)
                    item.DeleteGVob();
            });
        }

        #endregion

        #region Health

        partial void pSetHealth()
        {
            if (this.gVob == null)
                return;

            /*var gModel = gVob.GetModel();
            var gAniCtrl = gVob.AniCtrl;

            gModel.StopAni(gAniCtrl._t_strafel);
            gModel.StopAni(gAniCtrl._t_strafer);*/

            this.gVob.HPMax = this.hpmax;
            this.gVob.HP = this.hp;

            if (this.hp == 0)
            {
                this.gVob.GetModel().StopAnisLayerRange(int.MinValue, int.MaxValue);
                this.gVob.DoDie();
            }
        }

        #endregion

        #region Move State

        public void SetMovement(NPCMovement state)
        {
            if (this.movement == state)
                return;

            if (!this.IsDead && this.gVob != null && !this.Model.IsInAnimation())
                if (this.movement == NPCMovement.Right || this.movement == NPCMovement.Left)
                {
                    if (state == NPCMovement.Forward)
                        this.gVob.GetModel().StartAni(this.gVob.AniCtrl._s_walkl, 0);
                    else
                        this.gVob.GetModel().StartAni(this.gVob.AniCtrl._s_walk, 0);
                }

            this.movement = state;

            guidedNextUpdate = 0;
            this.OnTick(GameTime.Ticks);
        }

        #endregion

        partial void pOnTick(long now)
        {
            if (gVob == null || gVob.HumanAI.Address == 0)
                return;

            this.ScriptObject.OnTick(now);

            if (!this.IsDead && this.Model.GetActiveAniFromLayerID(1) == null && !this.environment.InAir)
            {
                switch (Movement)
                {
                    case NPCMovement.Forward:
                        var gModel = this.gVob.GetModel();
                        if (gModel.IsAnimationActive("T_JUMP_2_STAND") != 0)
                        {
                            var ai = this.gVob.HumanAI;
                            ai.LandAndStartAni(gModel.GetAniFromAniID(ai._t_jump_2_runl));
                        }
                        gVob.AniCtrl._Forward();
                        break;
                    case NPCMovement.Backward:
                        gVob.AniCtrl._Backward();
                        break;
                    case NPCMovement.Right:
                        gModel = this.gVob.GetModel();
                        var strafeAni = gVob.AniCtrl._t_strafer;
                        if (!gModel.IsAniActive(gModel.GetAniFromAniID(strafeAni)))
                        {
                            gModel.StartAni(strafeAni, 0);
                        }
                        break;
                    case NPCMovement.Left:
                        gModel = this.gVob.GetModel();
                        strafeAni = gVob.AniCtrl._t_strafel;
                        if (!gModel.IsAniActive(gModel.GetAniFromAniID(strafeAni)))
                        {
                            gModel.StartAni(strafeAni, 0);
                        }
                        break;
                    case NPCMovement.Stand:
                        gVob.AniCtrl._Stand();
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Throw(Vec3f velocity)
        {
            var aiVel = new zVec3(this.gVob.HumanAI.Address + 0x90);
            base.Throw((Vec3f)aiVel + velocity);
        }

        public BaseVob GetFocusVob()
        {
            BaseVob baseVob;
            this.World.TryGetVobByAddress(this.gVob.FocusVob.Address, out baseVob);
            return baseVob;
        }
    }
}
