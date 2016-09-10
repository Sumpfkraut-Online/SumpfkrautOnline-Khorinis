using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.ItemContainers
{
    internal partial class NPCInventory : ItemInventory
    {
        #region Constructors

        internal NPCInventory(NPC owner, IScriptItemInventory scriptObject) : base(owner, scriptObject)
        {
        }

        #endregion

        #region Properties

        new public NPC Owner { get { return (NPC)base.Owner; } }

        #endregion

        #region Add & Remove

        partial void pAdd(Item item);
        public override void Add(Item item)
        {
            base.Add(item);
            pAdd(item);
        }

        partial void pRemove(Item item);
        public override void Remove(Item item)
        {
            base.Remove(item);
            pRemove(item);
        }

        #endregion
    }
}
