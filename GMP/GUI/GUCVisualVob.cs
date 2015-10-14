using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Client.WorldObjects;
using Gothic.zClasses;

namespace GUC.Client.GUI
{
    class GUCVisualVob : GUCVisual
    {
        /*This is rendering a vob item*/

        public GUCVisualVob(int x, int y, int w, int h) : base(x,y,w,h)
        {
        }

        public GUCVisualVob(int x, int y, int w, int h, bool virtuals) : base(x,y,w,h,virtuals)
        {
            zView.Blit();
            zView.FillZ = true;
        }

        protected zCVob vob;
        public void SetVob(zCVob vob)
        {
            this.vob = vob;
            if (shown)
            {
                MenuRenderer.renderList[thisView.Address] = vob;
            }
        }

        public override void Show()
        {
            if (shown) {
                MenuRenderer.renderList.Remove(thisView.Address);
            }
            base.Show();
            MenuRenderer.renderList.Add(thisView.Address, vob);
        }

        public override void Hide()
        {
            if (shown)
            {
                MenuRenderer.renderList.Remove(thisView.Address);
                base.Hide();
            }
        }
    }
}
