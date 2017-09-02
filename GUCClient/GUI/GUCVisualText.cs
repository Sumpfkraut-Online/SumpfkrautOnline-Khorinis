using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using GUC.Types;
using Gothic.Types;
using Gothic.Objects;

namespace GUC.GUI
{
    public class GUCVisualText : GUCView
    {
        GUCVisual parent;
        public GUCVisual Parent { get { return parent; } }
        int[] parentSize;

        zCViewText viewText;
        public zCViewText zViewText { get { return viewText; } }
        int[] vpos;

        bool shown;

        public static GUCVisual Create(string text, int x, int y)
        {
            //If you simply want one text line, use this.
            //GUCVisualText object will be vis.Texts[0]
            return Create(text, x, y, false);
        }

        public static GUCVisual Create(string text, int x, int y, bool virtuals)
        {
            GUCVisual vis = new GUCVisual();
            vis.CreateText(text, x, y, virtuals);
            return vis;
        }

        public GUCVisualText(string text, int x, int y, GUCVisual parent, int[] psize, bool virtuals)
        {
            this.parent = parent;
            parentSize = psize;
            if (virtuals)
            {
                vpos = new int[] { x, y };
            }
            else
            {
                vpos = GUCVisual.PixelToVirtual(x, y);
            }
            vpos = new int[] { vpos[0] * 0x2000 / parentSize[0], vpos[1] * 0x2000 / parentSize[1] };
            using (zString z = zString.Create(text))
                viewText = parent.zView.CreateText(vpos[0], vpos[1], z);

            shown = true;
        }

        #region Text & Format
        public enum TextFormat
        {
            Left,
            Center,
            Right
        }

        protected TextFormat format = TextFormat.Left;
        public TextFormat Format
        {
            get { return format; }
            set
            {
                format = value;
                AlignText();
            }
        }

        protected string text = "";
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                viewText.Text.Set(value);
                AlignText();
            }
        }

        protected void AlignText()
        {
            if (centeredX || centeredY)
            {
                if (centeredX)
                {
                    viewText.PosX = (0x2000 - parent.zView.FontSize(viewText.Text)) / 2;
                }
                if (centeredY)
                {
                    viewText.PosY = (0x2000 - parent.zView.FontY()) / 2;
                }
            }
            else
            {
                if (format == TextFormat.Center)
                {
                    viewText.PosX = vpos[0] - parent.zView.FontSize(viewText.Text) / 2;
                }
                else if (format == TextFormat.Right)
                {
                    viewText.PosX = vpos[0] - parent.zView.FontSize(viewText.Text);
                }
                else
                {
                    viewText.PosX = vpos[0];
                }
            }
        }

        protected bool centeredX = false;
        protected bool centeredY = false;
        public bool CenteredX { get { return centeredX; } set { centeredX = value; AlignText(); } }
        public bool CenteredY { get { return centeredY; } set { centeredY = value; AlignText(); } }
        #endregion

        #region Show & Hide
        public override void Show()
        {
            if (!shown)
            {
                viewText.Text.Set(text);
                shown = true;
            }
        }
        //FIXME: Something's crashy here
        public override void Hide()
        {
            if (shown)
            {
                viewText.Text.Set("");
                shown = false;
            }
        }
        #endregion

        #region Color
        public void SetColor(ColorRGBA color)
        {
            viewText.Color.R = color.R;
            viewText.Color.G = color.G;
            viewText.Color.B = color.B;
            viewText.Color.A = color.A;
        }
        #endregion

        #region Position & Size
        public void SetPosX(int val)
        {
            vpos[0] = PixelToVirtualX(val) * 0x2000 / parentSize[0];
            viewText.PosX = vpos[0];
        }

        public void SetPosY(int val)
        {
            vpos[1] = PixelToVirtualY(val) * 0x2000 / parentSize[1];
            viewText.PosY = vpos[1];
        }

        public void Set3DPos(Vec3f pos)
        {
            var activeCam = zCCamera.ActiveCamera;
            if (activeCam.Address == 0)
                return;

            using (var gPos = zVec3.Create())
            {
                pos.SetGVec(gPos);
                using (var vec = activeCam.CamMatrix * gPos)
                {
                    if (vec.Z > 0)
                    {
                        int x, y;
                        activeCam.Project(vec, out x, out y);
                        this.SetPosX(x);
                        this.SetPosY(y);
                    }
                    else
                    {
                        viewText.PosX = 0x2000;
                        viewText.PosY = 0x2000;
                    }
                }
            }
        }

        #endregion
    }
}
