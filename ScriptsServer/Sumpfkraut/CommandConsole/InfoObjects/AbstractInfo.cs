using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripts.Sumpfkraut.CommandConsole.InfoObjects
{
    public class AbstractInfo : GUC.Utilities.ExtendedObject
    {

        new public static readonly String _staticName = "AbstractInfo (s)";



        public AbstractInfo ()
        {
            this._objName = "AbstractInfo (default)";
        }

    }
}
