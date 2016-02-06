using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Log;

namespace GUC.WorldObjects
{
    public partial class BaseVob
    {
        bool isStatic = false;
        public bool IsStatic
        {
            get { return isStatic; }
            set { isStatic = value; }
        }
    }
}
