using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Hooks;
using Gothic.Objects;

namespace GUC.GUI
{
    public class GUC3DVisual : GUCVisual
    {
        /*This is rendering a vob item*/

        public GUC3DVisual(int x, int y, int w, int h) : this(x, y, w, h, false)
        {
        }

        public GUC3DVisual(int x, int y, int w, int h, bool virtuals) : base(x, y, w, h, virtuals)
        {
            zView.Blit();
            zView.FillZ = true;
        }

        zCVob vob = zCVob.Create();

        public void SetVob(zCVob vob)
        {
            this.vob = vob;
        }

        public void SetVisual(string name)
        {
            vob.SetVisual(name);
        }

        public override void Show()
        {
            if (shown)
            {
                hView.VobRenderList.Remove(thisView.Address);
            }
            base.Show();
            hView.VobRenderList.Add(thisView.Address, vob);
        }

        public override void Hide()
        {
            if (shown)
            {
                hView.VobRenderList.Remove(thisView.Address);
                base.Hide();
            }
        }
    }
}
