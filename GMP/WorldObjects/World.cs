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

        public static Dictionary<uint, Vob> VobDict = new Dictionary<uint, Vob>();

        public static void ChangeLevel(string newMap)
        {
            for (int i = VobDict.Count-1; i >= 0; i--)
            {
                VobDict.Values.ToArray()[i].Despawn();
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
                //FIXME: leave menu section & close init screen
                

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
