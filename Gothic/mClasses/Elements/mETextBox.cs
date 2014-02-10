using System;
using System.Collections.Generic;
using System.Text;
using WinApi.User.Enumeration;
using Gothic.zClasses;

namespace Gothic.mClasses.Elements
{
    public class mETextBox : mEItem
    {
        public String hardText = "lol";
        public bool Password;

        public override Object Data
        {
            get { return mData; }
            set
            {
                mData = value;
                if (mTextView != null)
                    mTextView.Text.Set(hardText + mData.ToString());
            }
        }

        public override zCViewText TextView
        {
            get { return mTextView; }
            set
            {
                mTextView = value;
                if (value == null)
                    return;
                TextView.Text.Set(hardText + mData.ToString());
            }
        }

        public override void InputUpdate(ManagedListBox mlb, WinApi.User.Enumeration.VirtualKeys key)
        {
            if (key == VirtualKeys.Up)
                mlb.ActiveID--;
            if (key == VirtualKeys.Down || key == VirtualKeys.Return)
                mlb.ActiveID++;

            //Rückgänig
            if ((int)key == 8)
            {
                if (((String)mData).Length == 0)
                    return;
                mData = ((String)mData).Substring(0, ((String)mData).Length - 1);
                TextView = TextView;
                return;
            }

            //Taste eintragen
            if (((int)key < 0x30 || (int)key > 0x5A) && (int)key != 0x20 && (int)key != 222 && (int)key != 192
                && (int)key != 186 && (int)key != 219 && (int)key != (int)VirtualKeys.OEMPeriod
                && (int)key != (int)VirtualKeys.OEMComma && (int)key != (int)VirtualKeys.OEMMinus)
                return;


            String keyVal = Convert.ToString((char)key);
            if ((int)key == 222)
                keyVal = "A";
            if ((int)key == 192)
                keyVal = "O";
            if ((int)key == 186)
                keyVal = "U";
            if ((int)key == 219)
                keyVal = "SS";

            if ((int)key == (int)VirtualKeys.N1 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "!";
            if ((int)key == (int)VirtualKeys.N8 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "(";
            if ((int)key == (int)VirtualKeys.N9 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = ")";
            if ((int)key == (int)VirtualKeys.N7 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "/";
            if ((int)key == 219 && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "?";
            if ((int)key == (int)VirtualKeys.OEMPeriod)
                keyVal = ".";
            if ((int)key == (int)VirtualKeys.OEMPeriod && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = ":";
            if ((int)key == (int)VirtualKeys.OEMComma)
                keyVal = ",";
            if ((int)key == (int)VirtualKeys.OEMMinus)
                keyVal = "-";
            if ((int)key == (int)VirtualKeys.OEMMinus && InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = "_";
            if (!InputHooked.IsPressed((int)VirtualKeys.Shift))
                keyVal = keyVal.ToLower();


            mData = (String)mData + keyVal;
            TextView = TextView;

        }
    }
}
