using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Crafting
{
    class Conditions
    {
        public delegate bool Condition(NPCInst npc);
        public static Dictionary<string, Condition> conditionDict = new Dictionary<string, Condition>();
        public static Conditions CraftingConditionsObject = new Conditions();

        Conditions()
        {
            conditionDict.Add("hello", Condition_Emerald_Step1);
        }

        bool Condition_Emerald_Step1(NPCInst npc)
        {
            return true;
        }

    }
}
