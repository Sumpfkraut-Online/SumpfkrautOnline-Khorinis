using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Modules
{
    public interface LoadModule
    {
        void Load(Network.Module module);
    }
}
