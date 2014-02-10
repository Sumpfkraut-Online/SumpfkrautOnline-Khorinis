using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using Gothic.zTypes;
using Gothic.zClasses;
using WinApi;
using GMP.Network.Messages;
using GMP.Net.Messages;
using Network;
using Injection;
using Gothic.mClasses;
using GMP.Helper;

namespace GMP.Injection
{
    public class World
    {
        public static Int32 hChangeLevelStart(String message)
        {
            try
            {
                Process Process = Process.ThisProcess();
                zERROR.GetZErr(Process).Report(2, 'G', "Level-Change Started!", 0, "Program.cs", 0);
                if (StaticVars.Ingame)
                {

                    foreach (Player pl in Program.playerList)
                    {
                        NPCHelper.RemovePlayer(pl, false);
                    }
                }
                else
                {
                    while (!Program.FullLoaded)
                    {
                        Program.client.Update();
                    }

                    StaticVars.Ingame = true;
                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(3, 'G', "ocGameChangeLevel_Start: " + ex.ToString(), 0, "Program.cs", 3);
            }
            return 0;
        }

        /*
         * 
         */
        public static Int32 hChangeLevelEnd(String message)
        {
            try
            {
                Process Process = Process.ThisProcess();
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Level-Change Ended!", 0, "Program.cs", 0);
                if (!StaticVars.Ingame)
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "First-Level-Change", 0, "Program.cs", 0);

                    if (Program.chatBox == null)
                    {
                        Program.chatBox = new Chatbox(Process);
                        Program.client.messageListener.Add((byte)NetWorkIDS.ChatMessage, new ChatMessage(Program.chatBox));
                    }
                    
                    StaticVars.Ingame = true;


                    return 0;
                }
                else
                {
                    try
                    {
                        String levelname = oCGame.Game(Process).World.WorldFileName.Value;


                        Program.Player.actualMap = Player.getMap(levelname);
                        Program.Player.NPCAddress = oCNpc.Player(Process).Address;
                        foreach (Player pl in Program.playerList)
                        {
                            NPCHelper.RemovePlayer(pl, false);
                        }
                        new LevelChangeMessage().Write(Program.client.sentBitStream, Program.client, levelname);
                        new LevelDataMessage().Write(Program.client.sentBitStream, Program.client);
                    }
                    catch (Exception ex)
                    {
                        zERROR.GetZErr(Process).Report(2, 'G', ex.ToString(), 0, "Program.cs", 0);
                    }


                    TimeMessage.firstTimeUpdate = false;
                }

                if (!StaticVars.serverConfig.LoadItemFromFile || StaticVars.serverConfig.removeAllContainers || StaticVars.serverConfig.UnLockAll)
                {
                    zERROR.GetZErr(Process).Report(2, 'G', "LITFF:" + StaticVars.serverConfig.LoadItemFromFile + ", rAC: " + StaticVars.serverConfig.removeAllContainers + ", UA:" + StaticVars.serverConfig.UnLockAll, 0, "Program.cs", 0);
                    Dictionary<zCVob.VobTypes, List<zCVob>> vobList = oCGame.Game(Process).World.getVobLists(zCVob.VobTypes.Item, zCVob.VobTypes.MobContainer, zCVob.VobTypes.MobDoor);

                    zERROR.GetZErr(Process).Report(2, 'G', "Count:" + vobList.Count, 0, "Program.cs", 0);
                    if(vobList.ContainsKey(zCVob.VobTypes.MobDoor))
                        unlockDoors(vobList[zCVob.VobTypes.MobDoor]);

                    if (vobList.ContainsKey(zCVob.VobTypes.MobContainer))
                        removeContainers(vobList[zCVob.VobTypes.MobContainer]);

                    if (vobList.ContainsKey(zCVob.VobTypes.Item))
                        removeItems(vobList[zCVob.VobTypes.Item]);
                }
                
                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(3, 'G', "ocGameChangeLevel_END: " + ex.ToString(), 0, "World.cs", 1);
            }
            return 0;
        }

        public static void unlockDoors(List<zCVob> vobs)
        {
            if (!StaticVars.serverConfig.UnLockAll)
                return;
            
            Process process = Process.ThisProcess();

            zERROR.GetZErr(process).Report(2, 'G', "Unlock Doors: Count:" + vobs.Count, 0, "Program.cs", 0);

            foreach (zCVob vob in vobs)
            {
                oCMobDoor mc = new oCMobDoor(process, vob.Address);
                mc.SetLocked(0);
            }
        }

        public static void removeContainers(List<zCVob> vobs)
        {
            if (StaticVars.serverConfig.LoadItemFromFile && !StaticVars.serverConfig.removeAllContainers && !StaticVars.serverConfig.UnLockAll)
                return;


            Process process = Process.ThisProcess();

            zERROR.GetZErr(process).Report(2, 'G', "Remove Containers: Count:" + vobs.Count, 0, "Program.cs", 0);

            foreach (zCVob vob in vobs)
            {
                if (StaticVars.serverConfig.removeAllContainers)
                {
                    oCGame.Game(process).World.RemoveVob(vob);
                    continue;
                }

                oCMobContainer mc = new oCMobContainer(process, vob.Address);
                if (StaticVars.serverConfig.UnLockAll)
                    mc.SetLocked(0);

                //Remove items from container:
                if (!StaticVars.serverConfig.LoadItemFromFile)
                {
                    List<oCItem> items = mc.getItemList();

                    foreach (oCItem item in items)
                    {
                        mc.Remove(item);
                    }
                }
            }
        }


        public static void removeItems(List<zCVob> vobs)
        {

            if (StaticVars.serverConfig.LoadItemFromFile)
                return;


            Process process = Process.ThisProcess();
            zERROR.GetZErr(process).Report(2, 'G', "Remove Items: Count:" + vobs.Count, 0, "Program.cs", 0);
            //remove all vobs from the world
            foreach (zCVob vob in vobs)
            {
                oCGame.Game(process).World.RemoveVob(vob);
            }
           
        }
    }
}
