using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Utilities
{
    public class TestObject : GUC.Utilities.ExtendedObject
    {

        new public static readonly String _staticName = "TestObject (static)";



        public TestObject ()
        {
            SetObjName("TestObject (default)");
        }

    }
}
