using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.States
{
    public abstract class AbstractState
    {
        protected bool _init = false;

        public virtual void Init()
        {
            if (_init)
                return;

            _init = true;
        }

        public abstract void update();
    }
}
