using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.View;
using Gothic.Types;

namespace GUC.GUI
{
    /// <summary>
    /// Basically like Gothic's zCView but with more functions and pixel position support.
    /// </summary>
    public class GUCVisual : GUCView
    {
        protected zCView thisView;
        public zCView zView { get { return thisView; } }

        protected ViewPoint vpos;
        protected ViewPoint vsize;
        protected bool shown = false;
        public bool Shown { get { return shown; } }

        public ViewPoint VPos { get { return vpos; } }
        public ViewPoint VSize { get { return vsize; } }

        /// <summary>
        /// Fullscreen.
        /// </summary>
        public GUCVisual()
            : this(0, 0, 0x2000, 0x2000, true)
        {
        }

        /// <summary>
        /// Uses pixels as standard.
        /// </summary>
        /// <param name="x">Left</param>
        /// <param name="y">Top</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        public GUCVisual(int x, int y, int w, int h)
            : this(x, y, w, h, false)
        {
        }

        /// <summary>
        /// Can use pixels or virtuals.
        /// </summary>
        /// <param name="x">Left</param>
        /// <param name="y">Top</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="virtuals">True to use virtuals (8192x8192 for any screensize).</param>
        public GUCVisual(int x, int y, int w, int h, bool virtuals)
            : this(x, y, w, h, virtuals, null)
        {
        }

        protected GUCVisual(int x, int y, int w, int h, bool virtuals, GUCVisual p)
        {
            parent = p;
            if (virtuals)
            {
                vpos = new ViewPoint(x, y);
                vsize = new ViewPoint(w, h);
            }
            else
            {
                vpos = PixelToVirtual(x, y);
                vsize = PixelToVirtual(w, h);
            }
            thisView = zCView.Create(vpos.X, vpos.Y, vpos.X + vsize.X, vpos.Y + vsize.Y);
            shown = false;
        }

        #region Show & Hide
        public override void Show()
        {
            Hide(); //bring the visual to the top

            zCView.GetScreen().InsertItem(thisView, 0);
            for (int i = 0; i < children.Count; i++)
                children[i].Show();
            shown = true;
        }

        public override void Hide()
        {
            if (shown)
            {
                zCView.GetScreen().RemoveItem(thisView);
                for (int i = 0; i < children.Count; i++)
                    children[i].Hide();
                shown = false;
            }
        }
        #endregion

        #region Fonts
        protected Fonts font = Fonts.Default;
        public Fonts Font
        {
            get { return font; }
            set
            {
                font = value;
                thisView.SetFont(fontDict[value]);
                foreach (GUCVisualText t in texts)
                {
                    t.zViewText.Font = thisView.Font; //update font for texts
                    t.CenteredX = t.CenteredX; //update centering
                }
            }
        }
        #endregion

        #region Texts
        protected List<GUCVisualText> texts = new List<GUCVisualText>();
        public List<GUCVisualText> Texts { get { return texts; } }

        public GUCVisualText CreateTextCenterX(string text, int y)
        {
            var t = CreateText(text, 0, y, false);
            t.CenteredX = true;
            return t;
        }

        public GUCVisualText CreateText(string text)
        {
            var t = CreateText(text, 0, 0, true);
            t.CenteredX = true;
            t.CenteredY = true;
            return t;
        }

        public GUCVisualText CreateText(string text, int x, int y)
        {
            return CreateText(text, x, y, false);
        }

        public GUCVisualText CreateText(string text, int x, int y, bool virtuals)
        {
            GUCVisualText newText = new GUCVisualText(text, x, y, this, virtuals);
            texts.Add(newText);
            return newText;
        }
        #endregion

        #region Texture
        public void SetBackTexture(string tex)
        {
            thisView.InsertBack(tex);
        }
        #endregion

        #region Size & Position

        public void SetPosX(int newPosX, bool virtuals = false)
        {
            if (!virtuals)
                newPosX = PixelToVirtualX(newPosX);

            int diff = newPosX - vpos.X;
            vpos.X = newPosX;
            thisView.SetPos(vpos.X, vpos.Y);

            //update children
            foreach (GUCVisual vis in children)
                vis.SetPosX(vis.vpos.X + diff);
        }

        public void SetPosY(int newPosY, bool virtuals = false)
        {
            if (!virtuals)
                newPosY = PixelToVirtualY(newPosY);

            int diff = newPosY - vpos.Y;
            vpos.Y = newPosY;
            thisView.SetPos(vpos.X, vpos.Y);

            //update children
            foreach (GUCVisual vis in children)
                vis.SetPosY(vis.vpos.Y + diff);
        }

        public void SetPos(int x, int y, bool virtuals = false)
        {
            SetPos(new ViewPoint(x, y), virtuals);
        }

        public void SetPos(ViewPoint newPos, bool virtuals = false)
        {
            if (!virtuals)
                newPos = PixelToVirtual(newPos);

            int diffX = newPos.X - vpos.X;
            int diffY = newPos.Y - vpos.Y;
            vpos = newPos;
            thisView.SetPos(vpos.X, vpos.Y);

            //update children
            foreach (GUCVisual vis in children)
                vis.SetPos(vis.vpos.X + diffX, vis.vpos.Y + diffY, true);
        }

        public void SetWidth(int width, bool virtuals = false)
        {
            vsize.X = virtuals ? width : PixelToVirtualX(width);
            thisView.SetSize(vsize.X, vsize.Y);
        }

        public void SetHeight(int height, bool virtuals = false)
        {
            vsize.Y = virtuals ? height : PixelToVirtualY(height);
            thisView.SetSize(vsize.X, vsize.Y);
        }
        
        public void SetSize(int width, int height, bool virtuals = false)
        {
            SetSize(new ViewPoint(width, height), virtuals);
        }
        
        public void SetSize(ViewPoint size, bool virtuals = false)
        {
            vsize = virtuals ? size : PixelToVirtual(size);
            thisView.SetSize(vsize.X, vsize.Y);
        }

        #endregion

        #region Children
        protected GUCView parent;
        public GUCView Parent { get { return parent; } }

        protected List<GUCView> children = new List<GUCView>();

        public GUCView AddChild(GUCView view)
        {
            children.Add(view);
            return view;
        }
        #endregion
    }
}
