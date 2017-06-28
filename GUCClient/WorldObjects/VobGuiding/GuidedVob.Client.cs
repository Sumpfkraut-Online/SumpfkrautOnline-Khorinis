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

        new internal static class Messages
        {
            #region Positions

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
                int id = stream.ReadUShort();

                // add id to the dictionary
                GameClient.Client.guidedIDs.Add(id, null);

                // check if the vob is in the world
                GuidedVob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    vob.guide = GameClient.Client;
                }
            }

            public static void ReadGuideAddCmdMessage(PacketReader stream)
            {
                // read the guide command
                var cmd = ScriptManager.Interface.CreateGuideCommand(stream.ReadByte());
                cmd.ReadStream(stream);

                int id = stream.ReadUShort();
                // add id to the dictionary
                GameClient.Client.guidedIDs.Add(id, cmd);

                // check if the vob is in the world
                GuidedVob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    vob.SetGuideCommand(cmd);
                    vob.guide = GameClient.Client;
                }
            }

            public static void ReadGuideRemoveMessage(PacketReader stream)
            {
                int id = stream.ReadUShort();
                // remove id from the dictionary
                GameClient.Client.guidedIDs.Remove(id);

                // check if the vob is in the world
                GuidedVob vob;
                if (World.Current.TryGetVob(id, out vob))
                {
                    vob.SetGuideCommand(null);
                    vob.guide = null;
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

        internal override void OnTick(long now)
        {
            base.OnTick(now);

            if (this.guide != GameClient.Client)
                return;

            if (this.currentCmd != null)
                this.currentCmd.Update(this, now);

            UpdateGuidePos(now);
        }

        const long updateInterval = 1400000; // 140ms

        const float MinPositionDistance = 18.0f;
        const float MinDirectionDifference = 0.06f;

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
                this.guide = GameClient.Client;
            }
        }

        partial void pDespawn()
        {
            // GameClient.Client.guidedIDs.Remove(id);  is already done in the VobDespawnMessage
            if (GameClient.Client.guidedIDs.ContainsKey(this.ID))
            {
                this.guide = null;
            }
        }

        #endregion
    }
}
