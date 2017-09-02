using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.WorldObjects.Instances
{
    public abstract partial class BaseVobInstance
    {
        #region Network Messages

        internal static class Messages
        {
            public static void WriteCreate(BaseVobInstance instance)
            {
                var stream = GameServer.SetupStream(ServerMessages.VobInstanceCreateMessage);
                stream.Write((byte)instance.VobType);
                instance.WriteStream(stream);
                GameClient.ForEach(c => c.Send(stream, NetPriority.Low, NetReliability.Reliable, '\0'));
            }

            public static void WriteDelete(BaseVobInstance instance)
            {
                var stream = GameServer.SetupStream(ServerMessages.VobInstanceDeleteMessage);
                stream.Write((ushort)instance.ID);
                GameClient.ForEach(c => c.Send(stream, NetPriority.Low, NetReliability.Reliable, '\0'));
            }
        }

        #endregion

        partial void pAfterCreate()
        {
            if (!this.IsStatic)
                Messages.WriteCreate(this);
        }

        partial void pBeforeDelete()
        {
            if (!this.IsStatic)
                Messages.WriteDelete(this);
        }
    }
}
