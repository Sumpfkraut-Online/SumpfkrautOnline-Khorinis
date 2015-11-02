using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;
using GUC.Types;
using GUC.Enumeration;

namespace GUC.Server.Scripts
{
    static class NPCHandler
    {
        public static void Init()
        {
            NPC.sOnMovement += OnMovement;
            NPC.sOnTargetMovement += OnTargetMovement;
        }

        static void OnMovement(NPC npc, NPCState state, Vec3f position, Vec3f direction)
        {
            npc.DoMovement(state, position, direction);
        }

        static void OnTargetMovement(NPC npc, NPC target, NPCState state, Vec3f position, Vec3f direction)
        {
            npc.DoTargetMovement(state, position, direction, target);
        }
    }
}
