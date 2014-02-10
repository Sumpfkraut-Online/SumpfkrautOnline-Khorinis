using System;
using System.Collections.Generic;
using System.Text;
using Network;

namespace GUC.GUI
{
    public abstract class View
    {
        protected int id;
        protected bool isShown = false;

        protected int x;
        protected int y;

        public View(int id)
        {
            this.id = id;
        }

        public abstract void setPosition(int x, int y);

        public abstract void hide();
        public abstract void show();


        public abstract void Destroy();
    }
}
