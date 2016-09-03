using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Network;
using GUC.Enumeration;
using RakNet;

namespace GUC.WorldObjects.VobGuiding
{
    public partial class GuidedVob : BaseVob
    {
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
        protected long guidedNextUpdate;

        protected virtual void UpdateGuidePos(long now)
        {
            if (now < guidedNextUpdate)
                return;

            Vec3f pos = this.GetPosition();
            Vec3f dir = this.GetDirection();
            if (now - guidedNextUpdate < TimeSpan.TicksPerSecond && // send at least once per second
                pos.GetDistance(guidedLastPos) < MinPositionDistance && dir.GetDistance(guidedLastDir) < MinDirectionDifference)
                return;

            guidedLastPos = pos;
            guidedLastDir = dir;

            PacketWriter stream = GameClient.SetupStream(NetworkIDs.VobPosDirMessage);
            stream.Write((ushort)this.ID);
            stream.WriteCompressedPosition(pos);
            stream.WriteCompressedDirection(dir);
            GameClient.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE);
            
            guidedNextUpdate = now + updateInterval;

            this.ScriptObject.OnPosChanged();
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
    }
}
