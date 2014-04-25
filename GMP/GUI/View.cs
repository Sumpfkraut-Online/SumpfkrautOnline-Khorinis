using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.GUI
{
    public abstract class View
    {
        protected int id;
        protected bool isShown = false;

        protected Vec2i position = new Vec2i();

        public View(int id, Vec2i pos)
        {
            this.id = id;
            this.position.set(pos);
        }

        public abstract void setPosition(Vec2i position);

        public void setPosition(int x, int y)
        {
            setPosition(new Vec2i(x, y));
        }

        public abstract void hide();
        public abstract void show();


        public abstract void Destroy();
    }
}
