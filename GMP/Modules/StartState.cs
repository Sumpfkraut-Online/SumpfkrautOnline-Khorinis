using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Injection;

namespace GMP.Modules
{
    public abstract class StartState
    {
        
        public abstract void Update(Module module);
        public virtual void Next(Module module)
        {
            module.ended = true;

            ModuleLoader.startFirstStartState();

        }
    }
}
