using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC
{
    public interface UpdateListener
    {
        void render(long time);
    }
}
