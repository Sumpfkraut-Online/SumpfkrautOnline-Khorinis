using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;

namespace GUC.GUI.GuiList
{
    public class ListButton : ListText
    {
        public class ButtonPressedEventArg : EventArgs
        {

            public VirtualKeys Key;
            public Object Data;
            public List list;
        }
        public event EventHandler<ButtonPressedEventArg> ButtonPressed;



        public ListButton(String text, List list)
            : base(text, list)
        {

        }

        protected override void updateActive(VirtualKeys key)
        {
            base.updateActive(key);

            if (key == VirtualKeys.Return && ButtonPressed != null)
                ButtonPressed(this, new ButtonPressedEventArg() { Key = key, Data = this, list = this.mList });

        }
    }
}
