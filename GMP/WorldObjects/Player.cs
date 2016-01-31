using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Collections;

namespace GUC.Client.WorldObjects
{
    public static class Player
    {
        public static uint ID { get; internal set; }
        public static NPC Hero { get; internal set; }
        internal static List<Vob> VobControlledList = new List<Vob>();

        public static ItemContainer Inventory { get; internal set; }
    }
}
