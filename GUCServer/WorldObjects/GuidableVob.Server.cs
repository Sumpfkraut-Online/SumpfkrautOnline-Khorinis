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


        partial void pSpawn(World world, Vec3f position, Vec3f direction)
        {
            if (this.needsClientGuide)
            {
                this.Cell.AddGuidableVob(this);
            }
        }

        partial void pDespawn()
        {
            if (this.needsClientGuide)
            {
                if (this.Guide != null)
                {

                }
                else
                {
                    this.Cell.RemoveGuidableVob(this);
                }
            }
        }
    }
}
