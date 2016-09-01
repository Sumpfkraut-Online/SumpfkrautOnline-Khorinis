using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.VobGuiding
{
    public abstract partial class GuideCmd
    {
        public abstract void Start(GuidedVob vob);
        public abstract void Update(GuidedVob vob, long now);
        public abstract void Stop(GuidedVob vob);
    }
}
