using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Types;
using WinApi;
using Gothic.zTypes;

namespace GUC.GUI
{
    public class Texture : View
    {
        protected zCView view;

        protected Vec2i size = new Vec2i();

        public Texture(int id, String tex, Vec2i position, Vec2i size)
            : base(id, position)
        {
            this.size.set(size);

            Process process = Process.ThisProcess();
            view = zCView.Create(process, position.X, position.Y, position.X + this.size.X, position.Y+this.size.Y);

            if (tex != null && tex != "")
            {
                zString text = zString.Create(process, tex);
                view.InsertBack(text);
                text.Dispose();
            }
        }

        public zCView getView()
        {
            return view;
        }

        public void setTexture(String tex)
        {
            if (tex == null || tex == "")
                return;
            Process process = Process.ThisProcess();
            zString text = zString.Create(process, tex);
            view.InsertBack(text);
            text.Dispose();
        }

        public void setSize(Vec2i size)
        {
            this.size.set(size.X, size.Y);
            view.SetSize(size.X, size.Y);
        }

        public override void setPosition(Vec2i pos)
        {
            this.position.set(pos);
            view.SetPos(this.position.X, this.position.Y);
            
        }

        public override void hide()
        {
            if (!isShown)
                return;
            Process process = Process.ThisProcess();
            zCView.GetStartscreen(process).RemoveItem(this.view);
            isShown = false;

        }
        public override void show()
        {
            if (isShown)
                return;
            Process process = Process.ThisProcess();

            zCView.GetStartscreen(process).InsertItem(this.view, 0);

            isShown = true;
        }


        public override void Destroy()
        {
            hide();
            this.view.Dispose();
        }

    }
}
