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

        internal void CreateGVob()
        {
            this.gvob = this.instance.CreateVob();
        }

        internal void DestroyGVob()
        {
            // we are finished with this gothic object, decrease the reference counter
            int refCtr = gvob.refCtr - 1;
            gvob.refCtr = refCtr;

            // Free the gothic object if no references are left, otherwise gothic will free it
            if (refCtr <= 0)
                gvob.Dispose();

            gvob = null;
        }
    }
}
