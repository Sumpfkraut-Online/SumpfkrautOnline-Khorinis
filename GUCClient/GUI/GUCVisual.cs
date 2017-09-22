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

        protected int[] vpos;
        protected int[] vsize;
        protected bool shown = false;
        public bool Shown { get { return shown; } }

        public ViewPoint VPos { get { return new ViewPoint(vpos[0], vpos[1]); } }

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
                vpos = new int[] { x, y };
                vsize = new int[] { w, h };
            }
            else
            {
                vpos = PixelToVirtual(x, y);
                vsize = PixelToVirtual(w, h);
            }
            thisView = zCView.Create(vpos[0], vpos[1], vpos[0] + vsize[0], vpos[1] + vsize[1]);
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
            GUCVisualText newText = new GUCVisualText(text, x, y, this, vsize, virtuals);
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
        public void SetPosX(int val, bool virtuals = false)
        {
            int newX = virtuals ? val : PixelToVirtualX(val);
            int diff = newX - vpos[0];
            vpos[0] = newX;
            thisView.SetPos(vpos[0], vpos[1]);

            //update children
            foreach (GUCVisual vis in children)
            {
                vis.SetPosX(vis.vpos[0] + diff);
            }
        }

        public void SetPosY(int val, bool virtuals = false)
        {
            int newY = virtuals ? val : PixelToVirtualY(val);
            int diff = newY - vpos[1];
            vpos[1] = newY;
            thisView.SetPos(vpos[0], vpos[1]);

            //update children
            foreach (GUCVisual vis in children)
            {
                vis.SetPosY(vis.vpos[1] + diff);
            }
        }

        public void SetSizeX(int val)
        {
            vsize[0] = PixelToVirtualX(val);
            thisView.SetSize(vsize[0], vsize[1]);
        }

        public void SetSizeY(int val)
        {
            vsize[1] = PixelToVirtualY(val);
            thisView.SetSize(vsize[0], vsize[1]);
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
