using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Network;
using GUC.Scripting;


namespace GUC.WorldObjects.VobGuiding
{
    public partial class GuidedVob : BaseVob
    {
        #region Network Messages

        internal static class Messages
        {
            #region Positions
            
            public static void ReadPosDirMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();

                BaseVob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    vob.SetPosition(stream.ReadCompressedPosition());
                    vob.SetDirection(stream.ReadCompressedDirection());

                    //vob.ScriptObject.OnPosChanged();
                }
                else
                {
                    TargetCmd.CheckPos(id, stream.ReadCompressedPosition());
                }
            }

            public static void WritePosDirMessage(GuidedVob vob, Vec3f pos, Vec3f dir, Environment env)
            {
                PacketWriter stream = GameClient.SetupStream(ClientMessages.GuidedVobMessage);
                stream.Write((ushort)vob.ID);
                stream.WriteCompressedPosition(pos);
                stream.WriteCompressedDirection(dir);

                // compress environment
                int bitfield = env.InAir ? 0x8000 : 0;
                bitfield |= (int)(env.WaterDepth * 0x7F) << 8;
                bitfield |= (int)(env.WaterLevel * 0xFF);
                stream.Write((short)bitfield);

                GameClient.Send(stream, PktPriority.Low, PktReliability.Unreliable);
            }

            #endregion

            #region Add & Remove & Set Cmds

            public static void ReadGuideAddMessage(PacketReader stream)
            {
                GameClient.Client.guidedIDs.Add(stream.ReadUShort(), null);
            }

            public static void ReadGuideAddCmdMessage(PacketReader stream)
            {
                var cmd = ScriptManager.Interface.CreateGuideCommand(stream.ReadByte());
                cmd.ReadStream(stream);
                int id = stream.ReadUShort();
                GameClient.Client.guidedIDs.Add(id, cmd);

                GuidedVob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    vob.SetGuideCommand(cmd);
                }
            }

            public static void ReadGuideRemoveMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();
                GameClient.Client.guidedIDs.Remove(id);

                GuidedVob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    vob.SetGuideCommand(null);
                }
            }

            public static void ReadGuideSetCmdMessage(PacketReader stream)
            {
                var cmd = ScriptManager.Interface.CreateGuideCommand(stream.ReadByte());
                cmd.ReadStream(stream);

                int id = stream.ReadUShort();
                if (GameClient.Client.guidedIDs.ContainsKey(id))
                {
                    GameClient.Client.guidedIDs[id] = cmd;

                    GuidedVob vob;
                    if (World.Current.TryGetVob(id, out vob))
                    {
                        vob.SetGuideCommand(cmd);
                    }
                }
            }

            public static void ReadGuideRemoveCmdMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();
                if (GameClient.Client.guidedIDs.ContainsKey(id))
                {
                    GameClient.Client.guidedIDs[id] = null;

                    GuidedVob vob;
                    if (World.Current.TryGetVob(id, out vob))
                    {
                        vob.SetGuideCommand(null);
                    }
                }
            }

            #endregion
        }

        #endregion

        #region Client Commands



        #endregion

        internal override void OnTick(long now)
        {
            base.OnTick(now);

            if (!GameClient.Client.guidedIDs.ContainsKey(this.ID))
                return;

            if (this.currentCmd != null)
                this.currentCmd.Update(this, now);

            UpdateGuidePos(now);
        }

        const long updateInterval = 1500000; // 150ms

        const float MinPositionDistance = 20.0f;
        const float MinDirectionDifference = 0.05f;

        protected Vec3f guidedLastPos;
        protected Vec3f guidedLastDir;
        protected Environment guidedLastEnv;
        protected long guidedNextUpdate;

        protected virtual void UpdateGuidePos(long now)
        {
            if (now < guidedNextUpdate)
                return;

            Vec3f pos = this.GetPosition();
            Vec3f dir = this.GetDirection();
            Environment env = this.GetEnvironment();

            if (now - guidedNextUpdate < TimeSpan.TicksPerSecond)
            {
                // nothing really changed, only update every second
                if (pos.GetDistance(guidedLastPos) < MinPositionDistance
                    && dir.GetDistance(guidedLastDir) < MinDirectionDifference
                    && env == guidedLastEnv)
                {
                    return;
                }
            }

            guidedLastPos = pos;
            guidedLastDir = dir;
            guidedLastEnv = env;

            Messages.WritePosDirMessage(this, pos, dir, env);

            guidedNextUpdate = now + updateInterval;

            //this.ScriptObject.OnPosChanged();
        }

        internal void SetGuideCommand(GuideCmd cmd)
        {
            if (cmd != this.currentCmd)
            {
                if (this.currentCmd != null)
                    this.currentCmd.Stop(this);

                this.currentCmd = cmd;

                if (this.currentCmd != null)
                    this.currentCmd.Start(this);
            }
        }

        #region Spawn & Despawn

        partial void pSpawn(World world, Vec3f position, Vec3f direction)
        {
            GuideCmd cmd;
            if (GameClient.Client.guidedIDs.TryGetValue(this.ID, out cmd))
            {
                this.SetGuideCommand(cmd);
            }
        }

        /*partial void pDespawn()
        {
            // GameClient.Client.guidedIDs.Remove(id);  is already done in the VobDespawnMessage
        }*/

        #endregion
    }
}
