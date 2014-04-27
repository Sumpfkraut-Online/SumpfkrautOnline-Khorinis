using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripts.AI.Waypoints;

namespace GUC.Server.Scripts.AI
{
    public static class AISystem
    {
        public static Dictionary<String, WayNet> WayNets = new Dictionary<string, WayNet>();
        public static void Init()
        {
            WayNets.Add("NEWWORLD\\NEWWORLD.ZEN", WayNet.loadFromFile("newworld.wp"));
        }
    }
}
