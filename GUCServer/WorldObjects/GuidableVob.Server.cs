using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.WorldObjects.Cells;

namespace GUC.WorldObjects
{
    public abstract partial class GuidableVob
    {
        bool needsClientGuide = false;
        public bool NeedsClientGuide { get { return this.needsClientGuide; } }

        public void SetNeedsClientGuide(bool value)
        {
            if (this.needsClientGuide == value)
                return;

            this.needsClientGuide = value;

            if (this.IsSpawned)
            {

            }
        }
    }
}
