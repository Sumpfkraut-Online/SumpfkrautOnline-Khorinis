using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Hooks;
using Gothic.Objects;

namespace GUC.GUI
{
    public class GUCVobVisual : GUCVisual, IDisposable
    {
        /*This is rendering a vob item*/

        public GUCVobVisual(int x, int y, int w, int h) : this(x, y, w, h, false)
        {
        }

        public GUCVobVisual(int x, int y, int w, int h, bool virtuals) : base(x, y, w, h, virtuals)
        {
            zView.Blit();
            zView.FillZ = true;
        }

        VobRenderArgs args = new VobRenderArgs();

        public void SetVisualFromVob(zCVob vob)
        {
            args.SetVisual(vob.Visual);
        }

        public void SetVisual(string name)
        {
            args.SetVisual(name);
        }

        bool lighting;
        public bool Lighting { get { return lighting; } set { lighting = value; args.Lighting = value; } }

        int distance;
        public int Distance { get { return distance; } set { distance = value; args.ZBias = value; } }

        int rotX;
        public int RotationX { get { return rotX; } set { rotX = value; args.RotX = value; } }

        int rotY;
        public int RotationY { get { return rotY; } set { rotY = value; args.RotY = value; } }

        int rotZ;
        public int RotationZ { get { return rotZ; } set { rotZ = value; args.RotZ = value; } }

        public override void Show()
        {
            base.Show();
                VobRenderArgs.Add(thisView.Address, args);
        }

        public override void Hide()
        {
            if (shown)
            {
                VobRenderArgs.Remove(thisView.Address);
                base.Hide();
            }
        }

        public void Dispose()
        {
            args.Dispose();
        }
    }
}
