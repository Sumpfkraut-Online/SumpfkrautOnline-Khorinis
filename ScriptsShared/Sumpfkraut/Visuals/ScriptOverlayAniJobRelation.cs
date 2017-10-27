using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUC.Scripts.Sumpfkraut.Visuals
{

    public struct ScriptOverlayAniJobRelation
    {

        public readonly int ScriptOverlayID;
        public readonly int ScriptAniJobID;
        public readonly int ScriptAniID;



        public ScriptOverlayAniJobRelation (int scriptOverlayID, int scriptAniJobID, int scriptAniID)
        {
            ScriptOverlayID = scriptOverlayID;
            ScriptAniJobID = scriptAniJobID;
            ScriptAniID = scriptAniID;
        }

    }

}
