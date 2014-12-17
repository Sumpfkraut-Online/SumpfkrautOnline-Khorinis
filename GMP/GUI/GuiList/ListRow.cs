using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using Gothic.mClasses;
using GUC.Enumeration;
using GUC.Types;

namespace GUC.GUI.GuiList
{
    public abstract class ListRow : View, InputReceiver
    {
        public bool isSelectable = true;

        protected bool isActive = false;
        protected zCViewText mTextView;

        protected List mList = null;

        protected String mText = "";

        protected ColorRGBA m_ActiveRowColor = ColorRGBA.Red;
        protected ColorRGBA m_InactiveRowColor = ColorRGBA.White;

        public ListRow(int viewID, String text, List list, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
            : base( viewID, new Types.Vec2i(0, 0))
        {
            mList = list;
            mText = text;

            m_ActiveRowColor = aActiveRowColor;
            m_InactiveRowColor = aInactiveRowColor;
        }


        public virtual void setControl(zCViewText text)
        {
            if (text == null)
            {
                isActive = false;
                mTextView = null;
                return;
            }
            mTextView = text;
            isActive = true;

            mTextView.Text.Set(mText);

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

        protected bool isInputActive = false;
        public virtual bool IsInputActive
        {
            get { return isInputActive; }
            set {
                if (this.isActive && mTextView != null)
                {
                    if (value)
                    {
                        mTextView.Color.R = m_ActiveRowColor.R;
                        mTextView.Color.G = m_ActiveRowColor.G;
                        mTextView.Color.B = m_ActiveRowColor.B;
                        mTextView.Color.A = m_ActiveRowColor.A;
                    }
                    else
                    {
                        mTextView.Color.R = m_InactiveRowColor.R;
                        mTextView.Color.G = m_InactiveRowColor.G;
                        mTextView.Color.B = m_InactiveRowColor.B;
                        mTextView.Color.A = m_InactiveRowColor.A;
                        
                    }
                }

                if (value == isInputActive)
                    return;
                isInputActive = value;
                if (!value)
                    InputHooked.receivers.Remove(this);
                else
                    InputHooked.receivers.Add(this);

                
                    return;

                
            }
        }








        /* Disable some of the View functions! */
        public override void show()
        {
            throw new NotImplementedException();
        }

        public override void hide()
        {
            throw new NotImplementedException();
        }

        public override void setPosition(Types.Vec2i position)
        {
            throw new NotImplementedException();
        }

        public override void Destroy()
        {
            throw new NotImplementedException();
        }
    }
}
