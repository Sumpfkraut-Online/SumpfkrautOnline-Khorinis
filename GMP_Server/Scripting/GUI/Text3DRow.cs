using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;

namespace GUC.Server.Scripting.GUI
{
    public class Text3DRow
    {
        public String Text { get; set; }
        public long Time { get; set; } // <= 0  unlimited
        public long BlendTime { get; set; }
        public ColorRGBA Color { get; set; }
    }
}
