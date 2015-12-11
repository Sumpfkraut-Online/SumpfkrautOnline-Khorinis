using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Sumpfkraut
{
    class TestClass : ScriptObject
    {
        new static String _objName = "TestClass (default)";

        public TestClass ()
        {
            SetObjName(_objName);
        }
    }
}
