using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Network.Messages;
using GUC.Types;

namespace GUC.WorldObjects.VobGuiding
{
    public abstract partial class GuidedVob
    {
        bool needsClientGuide = false;
        public bool NeedsClientGuide { get { return this.needsClientGuide; } }

        internal override void OnTick(long now)
        {
            base.OnTick(now);

            if (this.Guide != null && this.Guide.Character != null)
            {
                if (!(this.currentCmd is TargetCmd) || ((TargetCmd)this.currentCmd).Target != this.Guide.Character)
                    SetGuideCommand(Scripting.ScriptManager.Interface.GetTestCmd(this.Guide.Character));
            }
        }

        partial void pSpawn(World world, Vec3f position, Vec3f direction)
        {
            if (this.needsClientGuide)
            {
                SetGuide(FindNewGuide());
            }
        }

        partial void pDespawn()
        {
            if (this.Guide != null)
            {
                SetGuideCommand(null);
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
                if (this.Guide == null || (client.GuidedVobs.Count + 1) < this.Guide.GuidedVobs.Count) // balance the guide vobs
                {
                    SetGuide(client);
                }
            }
        }

        internal override void RemoveVisibleClient(GameClient client)
        {
            base.RemoveVisibleClient(client);

            if (this.Guide == client)
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
                if (this.Guide != null)
                    this.Guide.RemoveGuideTarget(target);
            }
            if (cmd is TargetCmd)
            {
                BaseVob target = ((TargetCmd)cmd).Target;
                target.OnDespawn += OnTargetDespawn;
                if (this.Guide != null)
                    this.Guide.AddGuideTarget(target);
            }

            if (this.Guide != null)
            {
                GuideMessage.WriteGuidableCmdMessage(this.Guide, this, cmd);
            }

            this.currentCmd = cmd;
        }

        void OnTargetDespawn(BaseVob vob)
        {
            SetGuideCommand(null);
        }

        void SetGuide(GameClient client, bool sendRemove = true)
        {
            if (this.Guide == client)
                return;

            if (this.Guide != null)
            {
                this.Guide.GuidedVobs.Remove(this.ID);
                if (sendRemove)
                {
                    GuideMessage.WriteRemoveGuidableMessage(this.Guide, this);
                }
                if (this.currentCmd is TargetCmd)
                {
                    this.Guide.RemoveGuideTarget(((TargetCmd)this.currentCmd).Target);
                }
            }

            if (client != null)
            {
                client.GuidedVobs.Add(this);
                GuideMessage.WriteAddGuidableMessage(client, this, this.currentCmd);
                if (this.currentCmd is TargetCmd)
                {
                    client.AddGuideTarget(((TargetCmd)this.currentCmd).Target);
                }
            }

            this.Guide = client;
        }
    }
}
