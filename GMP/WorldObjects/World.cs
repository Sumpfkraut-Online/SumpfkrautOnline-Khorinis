using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using Gothic.zTypes;

namespace GUC.Client.WorldObjects
{
    static class World
    {
        public static string MapName = null;

        public static Dictionary<int, Vob> vobAddr = new Dictionary<int, Vob>();
        public static List<Vob> AllVobs { get { return vobAddr.Values.ToList(); } }

        public static Dictionary<uint, NPC> npcDict = new Dictionary<uint, NPC>();
        public static Dictionary<uint, Item> itemDict = new Dictionary<uint, Item>();
        public static Dictionary<uint, Vob> vobDict = new Dictionary<uint, Vob>();

        public static Vob GetVobByID(uint id)
        {
            NPC npc;
            npcDict.TryGetValue(id, out npc);
            if (npc != null) return (Vob)npc;

            Item item;
            itemDict.TryGetValue(id, out item);
            if (item != null) return (Vob)item;

            Vob vob;
            vobDict.TryGetValue(id, out vob);
            return vob;
        }

        public static void AddVob(Vob vob)
        {
            vobAddr.Add(vob.gVob.Address, vob);

            if (vob is NPC)
            {
                npcDict.Add(vob.ID, (NPC)vob);
            }
            else if (vob is Item)
            {
                itemDict.Add(vob.ID, (Item)vob);
            }
            else
            {
                vobDict.Add(vob.ID, vob);
            }
        }

        public static void RemoveVob(Vob vob)
        {
            vobAddr.Remove(vob.gVob.Address);

            if (vob is NPC)
            {
                npcDict.Remove(vob.ID);
            }
            else if (vob is Item)
            {
                itemDict.Remove(vob.ID);
            }
            else
            {
                vobDict.Remove(vob.ID);
            }
        }

        public static void ChangeLevel(string newMap)
        {
            for (int i = AllVobs.Count - 1; i >= 0; i--)
            {
                AllVobs[i].Despawn();
            }
            
            //Change the world!
            using (zString newlevel = zString.Create(Program.Process, newMap))
            {
                oCGame.Game(Program.Process).ChangeLevel(newlevel, newlevel);
            }
            World.MapName = newMap;
        }

        #region Hooks
        public static Int32 hook_StartChangeLevel(String message)
        {
            try
            {
                Menus.GUCMenus.CloseActiveMenus();
                Menus.GUCMenus._Background.Hide();
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }

        public static Int32 hook_EndChangeLevel(String message)
        {
            try
            {
                //FIXME: Set weather accordingly at EndChangeLevel
                //FIXME: Show chat

                //CGameManager.InitScreen_Close(Program.Process);                

                /*String levelname = sWorld.getMapName(oCGame.Game(process).World.WorldFileName.Value);

                World world = sWorld.getWorld(levelname);
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "level-Change!" + levelname, 0, "Program.cs", 0);
                if (Player.Hero != null && Player.Hero.Map != null && Player.Hero.Map.Length != 0 && Player.Hero.Map.Equals(lastWorldName))
                {
                    sWorld.getWorld(lastWorldName).DespawnWorld();

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkID.ChangeWorldMessage);
                    stream.Write(Player.Hero.ID);
                    stream.Write(levelname);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                    world.addVob(Player.Hero);
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "level-Change2! " + levelname, 0, "Program.cs", 0);

                    sWorld.SpawnedVobDict.Remove(Player.Hero.Address);
                    Player.Hero.Address = oCNpc.Player(Process.ThisProcess()).Address;
                    sWorld.SpawnedVobDict.Add(Player.Hero.Address, Player.Hero);
                }

                Program.newWorld = true;
                world.RemoveAllObjects();


                if (sWorld.WeatherType != 2)
                    oCGame.Game(process).World.SkyControlerOutdoor.SetWeatherType(sWorld.WeatherType);
                oCGame.Game(process).WorldTimer.SetDay(sWorld.Day);
                oCGame.Game(process).WorldTimer.SetTime(sWorld.Hour, sWorld.Minute);

                if (sWorld.WeatherType != 2)
                    oCGame.Game(process).World.SkyControlerOutdoor.SetWeatherType(sWorld.WeatherType);
                oCGame.Game(process).World.SkyControlerOutdoor.setRainTime(sWorld.StartRainHour, sWorld.StartRainMinute, sWorld.EndRainHour, sWorld.EndRainMinute);*/

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }
        #endregion
    }
}
