using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Network.Messages;

namespace GUC.WorldObjects
{
    public abstract partial class GuidedVob
    {
        bool needsClientGuide = false;
        public bool NeedsClientGuide { get { return this.needsClientGuide; } }

        partial void pDespawn()
        {
            if (this.Guide != null)
            {
                this.Guide.GuidedVobs.Remove(this.ID);
                this.Guide = null;
            }
        }

        public void SetNeedsClientGuide(bool value)
        {
            if (this.needsClientGuide == value)
                return;

            this.needsClientGuide = value;

            if (this.IsSpawned)
            {
                throw new NotImplementedException();
                if (this.needsClientGuide)
                {
                    this.Guide = FindNewGuide();
                    this.Guide.GuidedVobs.Add(this);
                }
                else
                {
                    this.Guide.GuidedVobs.Remove(this.ID);
                    this.Guide = null;
                }
            }
        }

        internal override void AddVisibleClient(GameClient client)
        {
            base.AddVisibleClient(client);            
            
            // if client.GuidedVobs.Count < this.Guide.GuidedVobs.Count
            if (this.needsClientGuide && this.Guide == null)
            {
                this.Guide = client;
                this.Guide.GuidedVobs.Add(this);
                GuideMessage.WriteAddGuidableMessage(this.Guide, this);
            }
        }

        internal override void RemoveVisibleClient(GameClient client)
        {
            base.RemoveVisibleClient(client);

            if (this.Guide == client)
            {
                client.GuidedVobs.Remove(this.ID);
                this.Guide = FindNewGuide();
                if (this.Guide != null)
                {
                    this.Guide.GuidedVobs.Add(this);
                    GuideMessage.WriteAddGuidableMessage(this.Guide, this);
                }
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
    }
}
