using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects.WorldGlobals;
using GUC.Types;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class ScriptBarrierCtrl : BarrierController.IScriptBarrierController
    {
        #region Constructors

        partial void pConstruct();
        public ScriptBarrierCtrl(WorldInst world)
        {
            this.world = world ?? throw new ArgumentNullException("World is null!");
            pConstruct();
        }

        #endregion

        #region Properties

        WorldInst world;
        public WorldInst World { get { return this.world; } }
        public BarrierController BaseBarrier { get { return this.world.BaseWorld.BarrierCtrl; } }

        #endregion

        public void SetNextWeight(long time, float weight)
        {
            BaseBarrier.SetNextWeight(time, weight);
        }

        #region Read & Write

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnReadSetWeight(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        public void OnWriteSetWeight(PacketWriter stream)
        {
        }

        #endregion
    }
}
