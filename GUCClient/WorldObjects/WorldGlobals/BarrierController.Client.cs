using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Hooks;
using GUC.Network;

namespace GUC.WorldObjects.WorldGlobals
{
    public partial class BarrierController
    {
        #region Network Messages

        internal static class Messages
        {
            public static void ReadBarrier(PacketReader stream)
            {
                var barrierCtrl = World.Current.BarrierCtrl;
                barrierCtrl.ReadSetNextWeight(stream);
                barrierCtrl.ScriptObject.SetNextWeight(barrierCtrl.EndTime, barrierCtrl.EndWeight);
            }
        }

        #endregion

        partial void pUpdateWeight()
        {
            hBarrier.BarrierAlpha = (byte)(byte.MaxValue * CurrentWeight);
        }
    }
}
