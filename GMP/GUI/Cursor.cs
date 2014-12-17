using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using WinApi;

namespace GUC.GUI
{
    class Cursor : Texture
    {
        public Cursor(int id, String tex, Vec2i position, Vec2i size)
            : base(id, tex, position, size, null, Enumeration.GUIEvents.None)
        {
            Gothic.mClasses.Cursor.Init(Process.ThisProcess());
            
        }

        public override void show()
        {
            base.show();
            Gothic.mClasses.Cursor.noHandle = true;
            Gothic.mClasses.Cursor.pView = this.view;
        }

        public override void hide()
        {
            base.hide();

            Gothic.mClasses.Cursor.noHandle = false;
            Gothic.mClasses.Cursor.pView = null;
        }

    }
}
