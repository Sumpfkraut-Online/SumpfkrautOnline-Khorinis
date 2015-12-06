using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.WorldObjects
{
    public abstract class AbstractObject
    {
        public object ScriptObj { get; private set; }

        public T GetScriptObject<T>()
        {
            return (T)ScriptObj;
        }

        public AbstractObject(object scriptObject)
        {
            this.ScriptObj = scriptObject;
        }
    }
}
