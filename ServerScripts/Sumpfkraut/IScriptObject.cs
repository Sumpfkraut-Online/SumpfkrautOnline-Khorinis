using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut
{
    interface IScriptObject
    {

        void MakeLog (Object obj);
        void MakeLogError (Object obj);
        void MakeLogWarning (Object obj);

        void Print ();
        void Print (Object obj);
        void Print (Object obj, bool newLine);

    }
}
