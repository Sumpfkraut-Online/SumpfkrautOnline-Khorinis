using System;
using System.Collections.Generic;
using System.Text;

namespace GUC.WorldObjects
{
    interface IContainer
    {
        void addItem(Item item);
        void removeItem(Item item);
    }
}
