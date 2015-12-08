using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects
{
    public abstract class ServerObject
    {
        public object ScriptObj { get; private set; }

        public T GetScriptObject<T>()
        {
            return (T)ScriptObj;
        }

        public ServerObject(object scriptObject)
        {
            this.ScriptObj = scriptObject;
        }
    }
}
