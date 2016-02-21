using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;

namespace GUC.Scripts.Sumpfkraut.VobSystem.Instances
{
    public partial class ItemInst : VobInst, WorldObjects.Item.IScriptItem
    {
        #region Properties

        protected new WorldObjects.Item baseInst { get { return (WorldObjects.Item)base.baseInst; } }

        #endregion

        public void ReadEquipProperties(PacketReader stream)
        {
            throw new NotImplementedException();
        }

        public void ReadInventoryProperties(PacketReader stream)
        {
            throw new NotImplementedException();
        }

        public void WriteEquipProperties(PacketWriter stream)
        {
            throw new NotImplementedException();
        }

        public void WriteInventoryProperties(PacketWriter stream)
        {
            throw new NotImplementedException();
        }
    }
}
