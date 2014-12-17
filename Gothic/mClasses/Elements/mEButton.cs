using System;
using System.Collections.Generic;
using System.Text;
using WinApi.User.Enumeration;

namespace Gothic.mClasses.Elements
{
    public class mEButton : mEItem
    {

        public class ButtonPressedEventArg : EventArgs
        {

            public VirtualKeys Key;
            public Object Data;
            public ManagedListBox MLB;
        }
        public int actionID;
        public event EventHandler<ButtonPressedEventArg> ButtonPressed;
        public override void InputUpdate(ManagedListBox mlb, WinApi.User.Enumeration.VirtualKeys key)
        {
            base.InputUpdate(mlb, key);

            if (key == VirtualKeys.Return && ButtonPressed != null)
                ButtonPressed(this, new ButtonPressedEventArg() { Key = key, Data = this.Data, MLB = mlb });

        }
    }
}
