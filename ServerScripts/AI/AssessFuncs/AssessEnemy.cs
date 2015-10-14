using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.AI.AssessFuncs
{
    public static class AssessEnemy
    {
        public static void OnAssessEnemy(NPC npc, NPC target)
        {
            npc.getAI().addEnemy(target);
        }
    }
}
