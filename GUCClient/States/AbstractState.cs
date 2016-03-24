using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;

namespace GUC.Client.States
{
    public abstract class AbstractState
    {
        public abstract Dictionary<VirtualKeys, Action> Shortcuts { get; }

        public abstract void Update();
    }
}
