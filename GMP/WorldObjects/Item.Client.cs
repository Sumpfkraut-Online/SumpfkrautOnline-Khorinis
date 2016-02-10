using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.Objects;

namespace GUC.WorldObjects
{
    public partial class Item
    {
        new public oCItem gVob { get { return (oCItem)base.gVob; } }
    }
}
