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

       /* partial void pRemove(Item item);
        public override void Remove(Item item)
        {
            base.Remove(item);
            pRemove(item);
        } */

        partial void pRemoveBefore(Item item);
        public override void Remove(Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Item is null!");
            }
            else if (item.Container != this.Owner)
            {
                throw new ArgumentException("Item is in a different container!");
            }

            pRemoveBefore(item);
            base.Remove(item);
        }


        #endregion
    }
}
