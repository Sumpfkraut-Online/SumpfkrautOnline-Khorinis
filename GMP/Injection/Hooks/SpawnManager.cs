using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Network;
using GMP.Modules;
using Gothic.zTypes;
using Injection;
using GMP.Network.Messages;

namespace GMP.Injection.Hooks
{
    public class SpawnManager
    {

        class spawnedNPC
        {
            public String instance;
            public String wp;
            public String world;
            public bool isSpawned; //false = summoned, no wp
            public float[] pos = new float[3];

        }

        public static int CountNPCSpawned(String instance, String wp, String world)
        {
            int x = 0;

            if (spawnedNPCList.ContainsKey(wp))
            {
                foreach (spawnedNPC npc in spawnedNPCList[wp])
                {
                    if (npc.instance == instance && world == npc.world)
                    {
                        x++;
                    }
                }
            }


            return x;
        }

        static Dictionary<String, List<spawnedNPC>> spawnedNPCList = new Dictionary<string, List<spawnedNPC>>();//Dictionary with all spawned npc on a a wp ( key )

        static bool blockedSpawnNPC = false;
        public static Int32 SpawnNPC(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int address = Convert.ToInt32(message);


                int index = process.ReadInt(address + 4);
                zString wpStr = new zString(process, process.ReadInt(address + 8));
                int id = process.ReadInt(address + 8);

                if(!blockedSpawnNPC){
                    process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x0C, 0x00 }, Program.oCSpawnManager_SpawnNPC.oldFuncInNewFunc.ToInt32());
                    blockedSpawnNPC = true;
                }

                if (blockedSpawnNPC)
                {
                    //new SpawnNPCMessage().Write(Program.client.sentBitStream, Program.client, true, instanceNPC, player.pos, "", Program.Player.actualMap, count);
                    float[] pos = oCGame.Game(process).World.WayNet.getWaypointPosition(wpStr.Value);
                    String instance = zCParser.getParser(process).GetSymbol(index).Name.Value;
                    String waypoint = wpStr.Value;
                    //new SpawnNPCMessage().Write(Program.client.sentBitStream, Program.client, false, instance, pos, waypoint, Program.Player.actualMap, CountNPCSpawned(instance, waypoint, Program.Player.actualMap));
                }
                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "SpawnManager.cs", 0);
            }
            return 0;
        }



        static bool blockedSummonNPC = false;
        public static Int32 SummonNPC(String message)
        {
            Process process = Process.ThisProcess();
            try
            {
                int address = Convert.ToInt32(message);


                

                if (!blockedSummonNPC)
                {
                    process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x0C, 0x00 }, Program.oCSpawnManager_SummonNPC.oldFuncInNewFunc.ToInt32());
                    blockedSummonNPC = true;
                }

                if (blockedSummonNPC)
                {
                    int index = process.ReadInt(address + 4);
                    zVec3 pos = new zVec3(process, process.ReadInt(address + 8));
                    float id = process.ReadFloat(address + 8);

                    //String instance = zCParser.getParser(process).GetSymbol(index).Name.Value;
                    //new SpawnNPCMessage().Write(Program.client.sentBitStream, Program.client, true, instanceNPC, player.pos, "", Program.Player.actualMap, count);
                }

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.ToString(), 0, "SpawnManager.cs", 0);
            }
            return 0;
        }

    }
}
