using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class BarrierController : SkyController
    {
        #region Network Messages

        internal static class Messages
        {
            public static void WriteSetWeight(BarrierController barrierController)
            {
                PacketWriter stream = GameServer.SetupStream(ServerMessages.WorldBarrierMessage);
                barrierController.WriteNextWeight(stream);
                barrierController.World.ForEachClient(client => client.Send(stream, PktPriority.Low, PktReliability.Reliable, 'W'));
            }
        }

        #endregion

        partial void pSetNextWeight()
        {
            if (this.World.IsCreated)
                Messages.WriteSetWeight(this);
        }
    }
}
