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

        zCViewText zviewText;
        public zCViewText zViewText { get { return zviewText; } }

        ViewPoint vpos;
        public ViewPoint VPos { get { return new ViewPoint(vpos.X, vpos.Y); } }

        bool shown;

        /// <summary>
        ///If you simply want one text line, use this. GUCVisualText object will be vis.Texts[0]
        /// </summary>
        public static GUCVisual Create(string text, int x, int y)
        {
            return Create(text, x, y, false);
        }

        /// <summary>
        ///If you simply want one text line, use this. GUCVisualText object will be vis.Texts[0]
        /// </summary>
        public static GUCVisual Create(string text, int x, int y, bool virtuals)
        {
            GUCVisual vis = new GUCVisual();
            vis.CreateText(text, x, y, virtuals);
            return vis;
        }

        public GUCVisualText(string text, int x, int y, GUCVisual parent, bool virtuals)
        {
            this.parent = parent;
            if (virtuals)
            {
                vpos = new ViewPoint(x, y);
            }
            else
            {
                vpos = GUCVisual.PixelToVirtual(x, y);
            }
            vpos = new ViewPoint(vpos.X * 0x2000 / parent.VSize.X, vpos.Y * 0x2000 / parent.VSize.Y);
            using (zString z = zString.Create(text))
                zviewText = parent.zView.CreateText(vpos.X, vpos.Y, z);

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
                zviewText.Text.Set(value);
                AlignText();
            }
        }

        protected void AlignText()
        {
            if (centeredX || centeredY)
            {
                if (centeredX)
                {
                    vpos.X = (0x2000 - parent.zView.FontSize(zviewText.Text)) / 2;
                    zviewText.PosX = vpos.X;
                }
                if (centeredY)
                {
                    vpos.Y = (0x2000 - parent.zView.FontY()) / 2;
                    zviewText.PosY = vpos.Y;
                }
            }
            else
            {
                if (format == TextFormat.Center)
                {
                    zviewText.PosX = vpos.X - parent.zView.FontSize(zviewText.Text) / 2;
                }
                else if (format == TextFormat.Right)
                {
                    zviewText.PosX = vpos.X - parent.zView.FontSize(zviewText.Text);
                }
                else
                {
                    zviewText.PosX = vpos.X;
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
                zviewText.Text.Set(text);
                shown = true;
            }
        }
        //FIXME: Something's crashy here
        public override void Hide()
        {
            if (shown)
            {
                zviewText.Text.Set("");
                shown = false;
            }
        }
        #endregion

        #region Color
        public void SetColor(ColorRGBA color)
        {
            zviewText.Color.R = color.R;
            zviewText.Color.G = color.G;
            zviewText.Color.B = color.B;
            zviewText.Color.A = color.A;
        }

        public ColorRGBA GetColor()
        {
            return new ColorRGBA(zviewText.Color.R, zviewText.Color.G, zviewText.Color.B);
        }
        #endregion

        #region Position & Size

        public void SetPosX(int val, bool virtuals = false)
        {
            vpos.X = virtuals ? val : PixelToVirtualX(val) * 0x2000 / parent.VSize.X;
            zviewText.PosX = vpos.X;
        }

        public void SetPosY(int val, bool virtuals = false)
        {
            vpos.Y = virtuals ? val : PixelToVirtualY(val) * 0x2000 / parent.VSize.Y;
            zviewText.PosY = vpos.Y;
        }

        #endregion

        Fonts font;
        public Fonts Font
        {
            get
            {
                return font;
            }
            set
            {
                if (this.font == value)
                    return;

                font = value;
                this.zViewText.SetFont(fontDict[font]);
            }
        }
    }
}
