using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut.CommandConsole.InfoObjects
{
    public class AbstractInfo : ScriptObject
    {

        new public static readonly String _staticName = "AbstractInfo (static)";



        public AbstractInfo ()
        {
            this._objName = "AbstractInfo (default)";
        }

    }
}
