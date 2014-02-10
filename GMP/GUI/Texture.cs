using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace GUC.GUI
{
    public class Texture : View
    {
        zCView view;

        protected int width;
        protected int height;

        public Texture(int id, String tex, int x, int y, int width, int height)
            : base(id)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;

            Process process = Process.ThisProcess();
            view = zCView.Create(process, x, y, width, height);

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

        public void setSize(int x, int y)
        {
            view.SetSize(x, y);
            width = x;
            height = y;
        }
        
        public override void setPosition(int x, int y)
        {
            view.SetPos(x, y);
            this.x = x;
            this.y = y;
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
