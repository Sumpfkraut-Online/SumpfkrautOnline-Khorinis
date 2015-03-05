using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;

namespace GUC.Scripting.Objects.Character
{
    public class NPCProto : Vob
    {
        internal NPCProto(WorldObjects.Character.NPCProto ivob)
            : base(ivob)
        {

        }



        public oCNpc getGothicInstance()
        {
            if (this.iVob.IsSpawned || this.iVob.Address == 0)
                return null;
            return new oCNpc(ScriptManager.getProcess(), this.iVob.Address);
        }
    }
}
