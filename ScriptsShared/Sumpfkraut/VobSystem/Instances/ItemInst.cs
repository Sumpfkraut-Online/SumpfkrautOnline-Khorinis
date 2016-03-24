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

        new public static readonly String _staticName = "ItemInst (static)";

        public new WorldObjects.Item BaseInst { get { return (WorldObjects.Item) base.BaseInst; } }

        #endregion

        public ItemInst (PacketReader stream) : base(new WorldObjects.Item(), stream)
        { }

        protected ItemInst() : base(new WorldObjects.Item())
        { }

        public static ItemInst ReadFromInvMsg (PacketReader stream)
        {
            var i = new ItemInst();
            i.BaseInst.ReadInventoryProperties(stream);
            return i;
        }

        public void ReadEquipProperties (PacketReader stream)
        { }

        public void ReadInventoryProperties (PacketReader stream)
        { }

        public void WriteEquipProperties (PacketWriter stream)
        { }

        public void WriteInventoryProperties (PacketWriter stream)
        { }
    }
}
