using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using Gothic.mClasses;

namespace GUC.GUI.GuiList
{
    public class ListTextBox : ListRow
    {
        public String hardText = "";
        public bool Password;

        protected String textData = "";


        public ListTextBox(String hardText, String text, List list)
            : base( list )
        {
            this.hardText = hardText;
            this.textData = text;
        }

        protected override void updateActive(VirtualKeys key)
        {
            if (key == VirtualKeys.Up)
                mList.ActiveID--;
            if (key == VirtualKeys.Down || key == VirtualKeys.Return)
                mList.ActiveID++;


            if ((int)key == 8)
            {
                if (textData.Length == 0)
                    return;
                textData = textData.Substring(0, textData.Length - 1);
                mTextView.Text.Set(hardText + textData);
                return;
            }

            String keyVal = Convert.ToString((char)key);
            keyVal = Gothic.mClasses.textBox.GetCharsFromKeys((VirtualKeys)key, InputHooked.IsPressed((int)VirtualKeys.Shift), InputHooked.IsPressed((int)VirtualKeys.Control) && InputHooked.IsPressed((int)VirtualKeys.Menu));


            textData += keyVal;
            mTextView.Text.Add(keyVal);
        }
    }
}
