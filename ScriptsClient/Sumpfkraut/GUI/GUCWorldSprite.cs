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
        public GUCWorldSprite(int w, int h, bool virtuals = false)
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
                pos.Y += targetVob.BaseInst.gVob.BBox3D.Height / 1.5f;
            }

            var activeCam = zCCamera.ActiveCamera;
            using (var gPos = zVec3.Create(pos.X, pos.Y, pos.Z))
            using (var vec = activeCam.CamMatrix * gPos)
            {
                ViewPoint screenPos;
                if (vec.Z > 0)
                {
                    activeCam.Project(vec, out screenPos.X, out screenPos.Y);

                    screenPos = PixelToVirtual(screenPos.X, screenPos.Y);
                    if (this.ShowOutOfScreen)
                    {
                        if (screenPos.X < 0) screenPos.X = 0;
                        else if (screenPos.X > 0x2000) screenPos.X = 0x2000;

                        if (screenPos.Y < 0) screenPos.Y = 0;
                        if (screenPos.Y > 0x2000) screenPos.Y = 0x2000;
                    }

                    // center it
                    screenPos.X -= this.vsize.X / 2;
                    screenPos.Y -= this.vsize.Y / 2;
                }
                else
                {
                    screenPos.X = 0x2000;
                    screenPos.Y = 0x2000;
                }

                SetPos(screenPos, true);
            }
        }
    }
}
