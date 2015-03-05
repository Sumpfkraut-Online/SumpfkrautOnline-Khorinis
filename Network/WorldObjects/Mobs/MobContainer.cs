using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Mobs
{
    internal partial class MobContainer : MobLockable, IContainer
    {
        public List<Item> itemList = new List<Item>();

        public MobContainer()
            : base()
        {
            this.VobType = Enumeration.VobTypes.MobContainer;
        }

        public void addItem(Item item)
        {
            if (item.Container != null)
                item.Container.removeItem(item);

            item.Container = this;
            itemList.Add(item);

            addItemToContainer(item);
        }
        partial void addItemToContainer(Item item);
        public void removeItem(Item item)
        {
            item.Container = null;
            itemList.Remove(item);
            

#if D_CLIENT
            //if (this.Address != 0 && item.Address != 0)
            //{
            //    Gothic.zClasses.zERROR.GetZErr(WinApi.Process.ThisProcess()).Report(2, 'G', "Removed from Container!", 0, "Itemsynchro.cs", 0);

            //    new Gothic.zClasses.oCMobContainer(WinApi.Process.ThisProcess(), this.Address).Remove(new Gothic.zClasses.oCItem(WinApi.Process.ThisProcess(), item.Address));
            //}
#endif
        }
    }
}
