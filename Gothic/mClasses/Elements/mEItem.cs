using System;
using System.Collections.Generic;
using System.Text;
using Gothic.zClasses;
using WinApi.User.Enumeration;

namespace Gothic.mClasses.Elements
{
    public class mEItem
    {
        protected Object mData = "";
        protected zCViewText mTextView;

        public mEItem()
        {

        }

        #region Fields
        public virtual Object Data
        {
            get { return mData; }
            set 
            {
                mData = value;
                if (mTextView != null)
                    mTextView.Text.Set(mData.ToString());
            }
        }
        #endregion

        public virtual zCViewText TextView
        {
            get { return mTextView; }
            set 
            { 
                mTextView = value; 
                if (value == null) 
                    return; 
                TextView.Text.Set(mData.ToString()); }
        }

        public virtual void InputUpdate(ManagedListBox mlb, VirtualKeys key)
        {
            if (key == VirtualKeys.Up || key == VirtualKeys.W)
                mlb.ActiveID--;
            if (key == VirtualKeys.Down || key == VirtualKeys.S)
                mlb.ActiveID++;
        }


    }
}
