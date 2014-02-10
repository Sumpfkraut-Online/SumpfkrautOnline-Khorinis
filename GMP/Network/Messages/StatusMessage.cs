using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;
using Injection;
using WinApi;
using System.Windows.Forms;
using GMP.Modules;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GMP.Net.Messages
{
    public class StatusMessage : Message
    {
        public override void Read(BitStream stream, Packet packet, Client client)
        {
            StaticVars.serverConfig = new ServerConfig();
            byte chatOptions = 0;
            byte flag = 0;
            short otherFlags = 0;
            int spawnCount = 0;
            int moduleCount = 0;

            

            
            stream.Read(out StaticVars.serverConfig.ServerName);
            stream.Read(out StaticVars.serverConfig.World);

            StaticVars.StartWorld = StaticVars.serverConfig.World;


            stream.Read(out StaticVars.serverConfig.Slots);
            stream.Read(out StaticVars.serverConfig.Port);

            stream.Read(out chatOptions);
            stream.Read(out StaticVars.serverConfig.chatOptions.DistanceRange);
            stream.Read(out flag);
            stream.Read(out otherFlags);

            stream.Read(out StaticVars.serverConfig.staminaValue);
            stream.Read(out StaticVars.serverConfig.InventoryLimit);
            stream.Read(out spawnCount);
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Config: Spawn-Count: " + spawnCount, 0, "StatusMessage.cs", 0);


            for (int i = 0; i < spawnCount; i++)
            {
                byte type = 0;

                stream.Read(out type);
                if (type == 0)
                {
                    Vec3 pos = new Vec3();
                    stream.Read(out pos.x); stream.Read(out pos.y); stream.Read(out pos.z);
                    StaticVars.serverConfig.Spawn.Add(pos);
                }
                else if (type == 1)
                {
                    String wp = "";
                    stream.Read(out wp);
                    StaticVars.serverConfig.Spawn.Add(wp);
                }
            }
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Config: Spawn Informations Loaded", 0, "StatusMessage.cs", 0);
            stream.Read(out moduleCount);
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Config: Modules-Count: " + moduleCount, 0, "StatusMessage.cs", 0);
            for (int i = 0; i < moduleCount; i++)
            {
                String name = "";
                String hash = "";
                stream.Read(out name);
                stream.Read(out hash);
                Module module = new Module();
                module.name = name;
                module.Hash = hash;

                StaticVars.serverConfig.Modules.Add(module);
            }
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Config: Module Informations Loaded", 0, "StatusMessage.cs", 0);


            StaticVars.serverConfig.chatOptions.SetOptions(chatOptions);
            StaticVars.serverConfig.chatOptions.BlockOptions.blockInWater = ((flag & 1) == 1);
            StaticVars.serverConfig.chatOptions.BlockOptions.blockWhenDead = ((flag & 2) == 2);
            StaticVars.serverConfig.chatOptions.BlockOptions.blockWhenSleep = ((flag & 4) == 4);
            StaticVars.serverConfig.chatOptions.BlockOptions.blockWhenUnconscious = ((flag & 8) == 8);

            StaticVars.serverConfig.HideNames = ((otherFlags & 1) == 1);
            StaticVars.serverConfig.OnlyHumanNames = ((otherFlags & 2) == 2);
            StaticVars.serverConfig.ShowConnectionMessages = ((otherFlags & 4) == 4);
            StaticVars.serverConfig.AvailableFriends = ((otherFlags & 8) == 8);
            StaticVars.serverConfig.ImmortalFriends = ((otherFlags & 16) == 16);
            StaticVars.serverConfig.UnLockAll = ((otherFlags & 32) == 32);
            StaticVars.serverConfig.LoadItemFromFile = ((otherFlags & 64) == 64);
            StaticVars.serverConfig.enableStamina = ((otherFlags & 128) == 128);
            StaticVars.serverConfig.removeAllContainers = ((otherFlags & 256) == 256);
            StaticVars.serverConfig.DamageOnServer = ((otherFlags & 512) == 512);
            StaticVars.serverConfig.NPCAIOnServer = ((otherFlags & 1024) == 1024);
            if (moduleCount != 0 && !Program.clientOptions.allowModules)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(4, 'G', "Dieser Server nutzt Module, die du ausgeschaltet hast", 0, "Module Error", 0);
                
                return;
            }


            DownloadModulesMessage.Next(client);

            //Process Process = Process.ThisProcess();
            //LoadingScreen.Hide(Process);
        }

        public override void Write(RakNet.BitStream stream, Client client)
        {
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.Status);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.Status))
                StaticVars.sStats[(int)NetWorkIDS.Status] = 0;
            StaticVars.sStats[(int)NetWorkIDS.Status] += 1;
        }
    }
}
