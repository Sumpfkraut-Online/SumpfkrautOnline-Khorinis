using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GameObjects;
using GUC.Animations;
using GUC.Network;

namespace GUC.Models
{
    public partial class Model : GameObject
    {
        #region Network Messages

        internal static class Messages
        {
            #region Overlays

            public static void WriteOverlayAdd(Model model, Overlay overlay)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.ModelOverlayAddMessage);
                stream.Write((ushort)model.vob.ID);
                stream.Write((byte)overlay.ID);
                model.vob.ForEachVisibleClient(c => c.Send(stream, NetPriority.Low, NetReliability.Reliable, 'W'));
            }

            public static void WriteOverlayRemove(Model model, Overlay overlay)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.ModelOverlayRemoveMessage);
                stream.Write((ushort)model.vob.ID);
                stream.Write((byte)overlay.ID);
                model.vob.ForEachVisibleClient(c => c.Send(stream, NetPriority.Low, NetReliability.Reliable, 'W'));
            }

            #endregion

            #region Animations

            public static void WriteAniStartUncontrolled(Model model, AniJob job)
            {
                PacketWriter stream = stream = GameServer.SetupStream(ServerMessages.ModelAniUncontrolledMessage);
                stream.Write((ushort)model.vob.ID);
                stream.Write((ushort)job.ID);
                model.vob.ForEachVisibleClient(c => c.Send(stream, NetPriority.High, NetReliability.ReliableOrdered, 'W'));
            }

            public static void WriteAniStart(Model model, AniJob job, float fpsMult)
            {
                PacketWriter stream;
                if (fpsMult == 1.0f)
                {
                    stream = GameServer.SetupStream(ServerMessages.ModelAniStartMessage);
                }
                else
                {
                    stream = GameServer.SetupStream(ServerMessages.ModelAniStartFPSMessage);
                    stream.Write(fpsMult);
                }
                stream.Write((ushort)model.vob.ID);
                stream.Write((ushort)job.ID);
                model.vob.ForEachVisibleClient(c => c.Send(stream, NetPriority.High, NetReliability.ReliableOrdered, 'W'));
            }

            public static void WriteAniStop(Model model, AniJob job, bool fadeout)
            {
                PacketWriter stream = GameServer.SetupStream(fadeout ? ServerMessages.ModelAniFadeMessage : ServerMessages.ModelAniStopMessage);
                stream.Write((ushort)model.vob.ID);
                stream.Write((ushort)job.ID);
                model.vob.ForEachVisibleClient(c => c.Send(stream, NetPriority.High, NetReliability.ReliableOrdered, 'W'));
            }

            #endregion
        }

        #endregion

        #region Overlays

        partial void pAddOverlay(Overlay overlay)
        {
            if (this.vob.IsSpawned)
                Messages.WriteOverlayAdd(this, overlay);
        }

        partial void pRemoveOverlay(Overlay overlay)
        {
            if (this.vob.IsSpawned)
                Messages.WriteOverlayRemove(this, overlay);
        }

        #endregion

        #region Animations

        partial void pStartUncontrolledAni(AniJob aniJob)
        {
            if (this.vob.IsSpawned)
                Messages.WriteAniStartUncontrolled(this, aniJob);
        }

        partial void pStartAnimation(ActiveAni aa, float fpsMult)
        {
            if (this.vob.IsSpawned)
                Messages.WriteAniStart(this, aa.AniJob, fpsMult);
        }

        partial void pStopAnimation(ActiveAni aa, bool fadeOut)
        {
            if (this.vob.IsSpawned)
                Messages.WriteAniStop(this, aa.AniJob, fadeOut);
        }

        #endregion
    }
}
