using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects
{
    public interface IScriptWorldObject
    {
    }

    public abstract class WorldObject
    {
        public IScriptWorldObject ScriptObj { get; protected set; }

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
