using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Crafting
{
    class Effects
    {
        public delegate bool Effect(NPCInst npc);
        public static Dictionary<string, Effect> effectDict = new Dictionary<string, Effect>();
        public static Effects CraftingEffectsObject = new Effects();

        Effects()
        {
            effectDict.Add("hello", Effect_Emerald_Step1);
        }

        bool Effect_Emerald_Step1(NPCInst npc)
        {
            return true;
        }
    }
}
