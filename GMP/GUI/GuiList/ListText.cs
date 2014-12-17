using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.GUI.GuiList
{
    public class ListText : ListRow
    {
        public ListText(int id, String text, List list, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
            : base(id, text, list, aActiveRowColor, aInactiveRowColor)
        {


        }


        
    }
}
