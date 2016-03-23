using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public partial class ItemContainer
    {
        partial void pAdd(Item item)
        {
            item.gvob = item.Instance.CreateVob(item.gvob);
        }
    }
}
