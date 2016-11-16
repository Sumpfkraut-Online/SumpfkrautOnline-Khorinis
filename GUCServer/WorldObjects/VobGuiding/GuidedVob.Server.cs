using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;

namespace GUC.WorldObjects.VobGuiding
{
    public abstract partial class GuidedVob : BaseVob
    {
        #region Network Messages

        new internal static class Messages
        {
            public static void ReadPosDir(PacketReader stream, GameClient client, World world)
            {
                int id = stream.ReadUShort();
                GuidedVob vob;
                if (world.TryGetVob(id, out vob) && vob.guide == client)
                {
                    var pos = stream.ReadCompressedPosition();
                    var dir = stream.ReadCompressedDirection();
                    int bitfield = stream.ReadShort();

                    bool inAir = (bitfield & 0x8000) != 0;
                    float waterDepth = ((bitfield >> 8) & 0x7F) / (float)0x7F;
                    float waterLevel = (bitfield & 0xFF) / (float)0xFF;
                    vob.environment = new Environment(inAir, waterLevel, waterDepth);

                    vob.SetPosDir(pos, dir, client);
                    //vob.ScriptObject.OnPosChanged();

                    /*if (vob == client.Character)
                    {
                        client.UpdateVobList(world, pos);
                    }*/
                }
            }

            #region Add & Remove & Cmds

            static PacketWriter guideWriter = new PacketWriter(100);
            public static void WriteAddGuidable(GameClient client, GuidedVob vob, GuideCmd cmd)
            {
                if (cmd == null)
                {
                    guideWriter.Write((byte)ServerMessages.GuideAddMessage);
                }
                else
                {
                    guideWriter.Write((byte)ServerMessages.GuideAddCmdMessage);
                    guideWriter.Write(cmd.CmdType);
                    cmd.WriteStream(guideWriter);
                }

                guideWriter.Write((ushort)vob.ID);
                client.Send(guideWriter, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                guideWriter.Reset();
            }

            public static void WriteGuidableCmd(GameClient client, GuidedVob vob, GuideCmd cmd)
            {
                if (cmd == null)
                {
                    guideWriter.Write((byte)ServerMessages.GuideRemoveCmdMessage);
                }
                else
                {
                    guideWriter.Write((byte)ServerMessages.GuideSetCmdMessage);
                    guideWriter.Write(cmd.CmdType);
                    cmd.WriteStream(guideWriter);
                }

                guideWriter.Write((ushort)vob.ID);
                client.Send(guideWriter, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                guideWriter.Reset();
            }

            public static void WriteRemoveGuidable(GameClient client, GuidedVob vob)
            {
                guideWriter.Write((byte)ServerMessages.GuideRemoveMessage);
                guideWriter.Write((ushort)vob.ID);
                client.Send(guideWriter, PktPriority.Low, PktReliability.ReliableOrdered, 'W');
                guideWriter.Reset();
            }

            #endregion
        }

        #endregion

        bool needsClientGuide = false;
        public bool NeedsClientGuide { get { return this.needsClientGuide; } }
        
        partial void pSpawn(World world, Vec3f position, Vec3f direction)
        {
            if (this.needsClientGuide)
            {
                SetGuide(FindNewGuide());
            }
        }

        partial void pDespawn()
        {
            if (this.guide != null)
            {
                RemoveGuideCommand();
                SetGuide(null, false);
            }
        }

        public void SetNeedsClientGuide(bool value)
        {
            if (this.needsClientGuide == value)
                return;

            this.needsClientGuide = value;

            if (this.IsSpawned)
            {
                SetGuide(this.needsClientGuide ? FindNewGuide() : null);
            }
        }

        internal override void AddVisibleClient(GameClient client)
        {
            base.AddVisibleClient(client);

            if (this.needsClientGuide)
            {
                if (this.guide == null || (client.GuidedVobs.Count + 1) < this.guide.GuidedVobs.Count) // balance the guide vobs
                {
                    SetGuide(client);
                }
            }
        }

        internal override void RemoveVisibleClient(GameClient client)
        {
            base.RemoveVisibleClient(client);

            if (this.guide == client)
            {
                SetGuide(FindNewGuide(), false); // don't send the guide remove msg, since the despawn msg is already handling it
            }
        }

        GameClient FindNewGuide()
        {
            GameClient best = null;
            visibleClients.ForEach(client =>
            {
                if (best == null || client.GuidedVobs.Count < best.GuidedVobs.Count)
                    best = client;
            });
            return best;
        }

        public void SetGuideCommand(GuideCmd cmd)
        {
            if (this.currentCmd == cmd)
                return;

            if (this.currentCmd is TargetCmd)
            {
                BaseVob target = ((TargetCmd)this.currentCmd).Target;
                target.OnDespawn -= OnTargetDespawn;
                if (this.guide != null)
                    this.guide.RemoveGuideTarget(target);
            }
            if (cmd is TargetCmd)
            {
                BaseVob target = ((TargetCmd)cmd).Target;
                target.OnDespawn += OnTargetDespawn;
                if (this.guide != null)
                    this.guide.AddGuideTarget(target);
            }

            if (this.guide != null)
            {
                Messages.WriteGuidableCmd(this.guide, this, cmd);
            }

            this.currentCmd = cmd;
        }

        public void RemoveGuideCommand ()
        {
            SetGuideCommand(null);
        }

        void OnTargetDespawn(BaseVob vob)
        {
            RemoveGuideCommand();
        }

        void SetGuide(GameClient client, bool sendRemove = true)
        {
            if (this.guide == client)
                return;

            if (this.guide != null)
            {
                this.guide.GuidedVobs.Remove(this.ID);
                if (sendRemove)
                {
                    Messages.WriteRemoveGuidable(this.guide, this);
                }
                if (this.currentCmd is TargetCmd)
                {
                    this.guide.RemoveGuideTarget(((TargetCmd)this.currentCmd).Target);
                }
            }

            if (client != null)
            {
                client.GuidedVobs.Add(this);
                Messages.WriteAddGuidable(client, this, this.currentCmd);
                if (this.currentCmd is TargetCmd)
                {
                    client.AddGuideTarget(((TargetCmd)this.currentCmd).Target);
                }
            }

            this.guide = client;
        }
    }
}
