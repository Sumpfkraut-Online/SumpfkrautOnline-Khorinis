using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Collections
{
    public class ItemContainer
    {
        public interface IContainer
        {
            ItemContainer Inventory { get; }
        }
    }
}
