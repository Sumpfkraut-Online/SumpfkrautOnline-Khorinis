using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;

namespace GUC.Server.WorldObjects
{
    /// <summary>
    /// The complete "Server World", contains properties which concern all worlds / the server universe.
    /// </summary>
    internal static class sWorld
    {
        /// <summary>Dictionary of all Worlds. Use only for access!</summary>
        public static Dictionary<string, World> WorldDict = new Dictionary<string, World>();

        /// <summary>All Non-Players. Use only for access!</summary>
        public static Dictionary<uint, NPC> NPCDict = new Dictionary<uint, NPC>();

        /// <summary>All Players. Use only for access!</summary>
        public static Dictionary<uint, NPC> PlayerDict = new Dictionary<uint, NPC>();

        /// <summary>All Items. Use only for access!</summary>
        public static Dictionary<uint, Item> ItemDict = new Dictionary<uint, Item>();

        /// <summary>All Vobs & Mobs. Use only for access!</summary>
        public static Dictionary<uint, Mob> VobDict = new Dictionary<uint, Mob>();

        internal static void AddVob(AbstractVob vob)
        {
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    PlayerDict.Add(vob.ID, (NPC)vob);
                else
                    NPCDict.Add(vob.ID, (NPC)vob);
            }
            else if (vob is Item)
            {
                ItemDict.Add(vob.ID, (Item)vob);
            }
            else if (vob is Mob)
            {
                VobDict.Add(vob.ID, (Mob)vob);
            }
            else
            {
                Log.Logger.logError("sWorld.AddVob failed: vob is of no specified type!");
            }
        }

        internal static void RemoveVob(AbstractVob vob)
        {
            if (vob is NPC)
            {
                if (((NPC)vob).isPlayer)
                    PlayerDict.Remove(vob.ID);
                else
                    NPCDict.Remove(vob.ID);
            }
            else if (vob is Item)
            {
                ItemDict.Remove(vob.ID);
            }
            else if (vob is Mob)
            {
                VobDict.Remove(vob.ID);
            }
        }
    }
}
