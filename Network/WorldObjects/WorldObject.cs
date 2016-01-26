using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects
{
    public abstract partial class WorldObject
    {
        public partial interface IScriptWorldObject
        {
        }

        public IScriptWorldObject ScriptObj { get; protected set; }

        public WorldObject(IScriptWorldObject scriptObj)
        {
            if (scriptObj == null)
                throw new ArgumentNullException("ScriptObject can't be null!");
            this.ScriptObj = scriptObj;
        }

        public bool IsCreated { get; protected set; }
        public virtual void Create()
        {
            IsCreated = true;
        }
        public virtual void Delete()
        {
            IsCreated = false;
        }
    }
}
