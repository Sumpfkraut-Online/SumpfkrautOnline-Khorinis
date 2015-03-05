using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace GUC.WorldObjects.Character
{
    internal partial class NPC
    {
        public override Scripting.Objects.Vob ScriptingInstance
        {
            get
            {
                if (m_ScriptingInstance == null)
                    m_ScriptingInstance = new Scripting.Objects.Character.NPC(this);
                return m_ScriptingInstance;
            }
        }
    }
}
