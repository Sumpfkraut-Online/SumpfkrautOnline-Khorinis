using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;

namespace GUC.WorldObjects.Character
{
    internal partial class NPC
    {
        protected Player controller = null;

        public override ulong Guid { get {
            if (controller == null)
                return 0;
            return controller.Guid;
        } }
        public override RakNet.RakNetGUID GUID { get { if (controller == null) return null; return controller.GUID; } }





        public virtual void Write()
        {

        }
    }
}
