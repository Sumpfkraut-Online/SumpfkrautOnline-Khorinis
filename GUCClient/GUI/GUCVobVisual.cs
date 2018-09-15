using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Hooks;
using Gothic.Objects;
using GUC.Types;

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

        public float OffsetX { get { return args.Offset.X; } set { args.Offset.X = value; } }
        public float OffsetY { get { return args.Offset.Y; } set { args.Offset.Y = value; } }
        public float OffsetZ { get { return args.Offset.Z; } set { args.Offset.Z = value; } }
        public float RotationPitch { get { return args.Rotation.Pitch; } set { args.Rotation.Pitch = value; } }
        public float RotationYaw { get { return args.Rotation.Yaw; } set { args.Rotation.Yaw = value; } }
        public float RotationRoll { get { return args.Rotation.Roll; } set { args.Rotation.Roll = value; } }

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
