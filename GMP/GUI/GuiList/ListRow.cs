using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using Gothic.mClasses;
using WinApi.User.Enumeration;

namespace GUC.GUI.GuiList
{
    public abstract class ListRow : InputReceiver
    {
        public bool isSelectable = true;

        protected bool isActive = false;
        protected zCViewText mTextView;

        protected List mList = null;
        public ListRow(List list)
        {
            mList = list;
        }


        public virtual void setControl(zCViewText text)
        {
            mTextView = text;
            isActive = true;
        }

        public virtual void KeyReleased(int key)
        {
            
        }

        public virtual void KeyPressed(int key)
        {
            updateActive((VirtualKeys)key);
        }

        public void wheelChanged(int steps)
        {
            if (!isInputActive)
                return;

            if (steps > 0)
                updateActive(VirtualKeys.Up);
            else
                updateActive(VirtualKeys.Down);
        }

        protected virtual void updateActive(VirtualKeys key)
        {
            if (key == VirtualKeys.Up || key == VirtualKeys.W)
                mList.ActiveID--;
            if (key == VirtualKeys.Down || key == VirtualKeys.S)
                mList.ActiveID++;
        }

        protected bool isInputActive;
        public virtual bool IsInputActive
        {
            get { return isInputActive; }
            set {
                if (value == isInputActive)
                    return;
                if (!value)
                    InputHooked.receivers.Remove(this);
                else
                    InputHooked.receivers.Add(this);
            }
        }
    }
}
