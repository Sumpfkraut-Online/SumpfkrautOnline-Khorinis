using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using Gothic.Objects;
using Gothic.Types;

namespace GUC.Scripts.Sumpfkraut.GUI
{
    class GUCWorldSprite : GUCVisual
    {
        public GUCWorldSprite(int w, int h, bool virtuals)
            : base(0, 0, w, h, virtuals, null)
        {
        }

        Vec3f targetPos;
        public void SetTarget(Vec3f position)
        {
            this.targetPos = position;
            this.targetVob = null;
        }

        BaseVobInst targetVob;
        public void SetTarget(BaseVobInst vob)
        {
            this.targetVob = vob;
            this.targetPos = Vec3f.Null;
        }

        public void SetVobOffset(Vec3f offset)
        {
            this.targetPos = offset;
        }

        public override void Show()
        {
            if (shown)
                return;

            GUCScripts.OnUpdate += this.UpdatePosition;
            base.Show();
        }

        public override void Hide()
        {
            if (!shown)
                return;

            GUCScripts.OnUpdate -= this.UpdatePosition;
            base.Hide();
        }

        /// <summary> Still shows the sprite at the screen border, even if the target is out of borders. </summary>
        public bool ShowOutOfScreen = false;

        public void UpdatePosition(long ticks)
        {
            Vec3f pos;
            if (targetVob == null)
            {
                pos = targetPos;
            }
            else
            {
                if (targetVob.BaseInst.gVob == null)
                    return;

                pos = targetVob.GetPosition();
                pos.Y += targetVob.BaseInst.gVob.BBox3D.Height / 2.0f;
            }

            var activeCam = zCCamera.ActiveCamera;
            using (var gPos = zVec3.Create(pos.X, pos.Y, pos.Z))
            using (var vec = activeCam.CamMatrix * gPos)
            {
                int x, y;
                if (vec.Z > 0)
                {
                    activeCam.Project(vec, out x, out y);
                    if (this.ShowOutOfScreen)
                    {
                        if (x < 0) x = 0;
                        else if (x > 0x2000) x = 0x2000;

                        if (y < 0) y = 0;
                        if (y > 0x2000) y = 0x2000;
                    }

                    // center it
                    x -= this.vsize[0] / 2;
                    y -= this.vsize[1] / 2;
                }
                else
                {
                    x = 0x2000;
                    y = 0x2000;
                }

                SetPosX(x);
                SetPosY(y);
            }
        }
    }
}
