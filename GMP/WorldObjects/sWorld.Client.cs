using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;

namespace GUC.WorldObjects
{
    internal partial class sWorld
    {
        protected static Dictionary<int, Vob> spawnedVobDict = new Dictionary<int, Vob>();
        public static Dictionary<int, Vob> SpawnedVobDict { get { return spawnedVobDict; } }



        public static void addVob(Vob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("AddVob: Vob can't be null!");
            if (vob.ID == 0)
                throw new ArgumentException("AddVob: Vob.ID can't be null!");
            if (VobDict.ContainsKey(vob.ID))
                throw new ArgumentException("AddVob: Vob.ID is already in the list: " + vob.ID + " " + vob.VobType + " " + vob);
            VobDict.Add(vob.ID, vob);
            vob.Created = true;
        }



        public static void removeVob(Vob vob)
        {
            if (vob == null)
                throw new ArgumentNullException("AddVob: Vob can't be null!");
            if (vob.ID == 0)
                throw new ArgumentException("AddVob: Vob.ID can't be null!");
            if (!VobDict.ContainsKey(vob.ID))
                throw new ArgumentException("AddVob: Vob.ID is not in the list: " + vob.ID + " " + vob.VobType + " " + vob);
            
#if D_CLIENT
            if (vob.Address != 0)
            {
                sWorld.SpawnedVobDict.Remove(vob.Address);
                vob.Address = 0;
            }
#endif
            vob.IsSpawned = false;
            vob.Created = false;
            if (vob.Map != null && vob.Map.Length != 0)
            {
                sWorld.getWorld(vob.Map).removeVob(vob);
            }

            VobDict.Remove(vob.ID);
            vob.Created = false;
        }
        

        #region Hooks
        static String lastWorldName = null;
        public static Int32 hook_StartChangeLevel(String message)
        {
            try{
                if (!Player.Hero.IsSpawnedPlayer)
                    return 0;


                lastWorldName = sWorld.getMapName(oCGame.Game(Process.ThisProcess()).World.WorldFileName.Value); ;
                sWorld.getWorld(lastWorldName).RemoveAllObjects();
                

            }catch (Exception ex){
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }

        public static Int32 hook_EndChangeLevel(String message)
        {
            try
            {
                if (!Player.Hero.IsSpawnedPlayer)
                    return 0;


                Process process = Process.ThisProcess();
                String levelname = sWorld.getMapName(oCGame.Game(process).World.WorldFileName.Value);

                World world = sWorld.getWorld(levelname);
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "level-Change!"+levelname, 0, "Program.cs", 0);
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
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "level-Change2! "+levelname, 0, "Program.cs", 0);

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
                oCGame.Game(process).World.SkyControlerOutdoor.setRainTime(sWorld.StartRainHour, sWorld.StartRainMinute, sWorld.EndRainHour, sWorld.EndRainMinute);

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
            }
            return 0;
        }
        #endregion
    }
}
