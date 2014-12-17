using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Enumeration
{
    public enum GUIEvents : int
    {
        None = 0,

        //All Textures:
        LeftClicked = 1,
        RightClicked = LeftClicked << 1,
        Hover = RightClicked << 1,


        //TextBox
        //PressKey = 8,


        //List
        //ListButton
        ListButtonClicked = Hover << 1,
        ListTextBoxSend = ListButtonClicked << 1
    }
}
