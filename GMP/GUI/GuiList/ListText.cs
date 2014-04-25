using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.GUI.GuiList
{
    public class ListText : ListRow
    {
        protected String mText = "";

        public ListText(String text, List list)
            : base(list)
        {


        }

        public override void setControl(Gothic.zClasses.zCViewText text)
        {
            base.setControl(text);

            if (text == null)
                text.Text.Set("");
            else
                text.Text.Set(mText);
        }

        public override bool IsInputActive
        {
            get
            {
                return base.IsInputActive;
            }
            set
            {
                base.IsInputActive = value;
                if (value)
                {
                    mTextView.Color.R = 255;
                    mTextView.Color.G = 255;
                    mTextView.Color.B = 255;
                }
                else
                {
                    mTextView.Color.R = 0;
                    mTextView.Color.G = 0;
                    mTextView.Color.B = 0;
                }
            }
        }
    }
}
