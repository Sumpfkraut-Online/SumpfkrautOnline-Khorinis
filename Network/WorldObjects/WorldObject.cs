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

        protected virtual void pCreate() { }
        public virtual void Create()
        {
            pCreate();
            IsCreated = true;
        }

        protected virtual void pDelete() { }
        public virtual void Delete()
        {
            pDelete();
            IsCreated = false;
        }
    }
}
