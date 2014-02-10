using System;
using System.Collections.Generic;
using System.Text;
using Injection;
using RakNet;
using Network;
using Gothic.zClasses;
using WinApi;
using GMP.Logger;
using GMP.Modules;
using Gothic.zTypes;
using GMP.Helper;
using System.Security.Cryptography;
using System.Management;

namespace GMP.Net.Messages
{
    public class ConnectionRequest : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            ErrorLog.Log(typeof(ConnectionRequest), "Read");
            int id = 0;
            String name = "";
            String guid = "";
            String world = "";
            stream.Read(out id);
            stream.Read(out name);
            stream.Read(out guid);
            stream.Read(out world);



            Process process = Process.ThisProcess();
           
            Program.clientOptions.name = name;

            Player pl = new Player(guid);
            pl.id = id;
            pl.name = name;
            pl.isPlayer = true;
            pl.actualMap = Player.getMap(world);
            pl.isSpawned = true;
            pl.knowName = true;

            
            pl.NPCAddress = oCNpc.Player(process).Address;

            lock (StaticVars.spawnedPlayerList)
            {
                StaticVars.spawnedPlayerList.Add(pl);
                StaticVars.spawnedPlayerDict.Add(pl.NPCAddress, pl);
                StaticVars.spawnedPlayerList.Sort(new Player.PlayerAddressComparer());
            }
            Program.playerList.Add(pl);
            StaticVars.AllPlayerDict.Add(pl.id, pl);
            StaticVars.PlayerDict.Add(pl.id, pl);
            Program.playerList.Sort(new Player.PlayerComparer());

            StaticVars.playerlist.Add(pl);
            Program.Player = pl;

            //Random rand = new Random();
            //int lang = rand.Next(0, StaticVars.serverConfig.Spawn.Count);
            //NPCHelper.SetRespawnPoint(StaticVars.serverConfig.Spawn[lang]);

            
            new PlayerListRequest().Write(stream, client);//Liste der Spieler anfordern
            
        }

        

        public override void Write(RakNet.BitStream stream, Client client)
        {
            Process process = Process.ThisProcess();

            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.ConnectionRequest);

            stream.Write(getDefaultConnectionString(0));
            stream.Write(x());
            stream.Write(Program.clientOptions.name);
            stream.Write(StaticVars.StartWorld);//StartWelt!!! 

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.ConnectionRequest))
                StaticVars.sStats[(int)NetWorkIDS.ConnectionRequest] = 0;
            StaticVars.sStats[(int)NetWorkIDS.ConnectionRequest] += 1;
        }

        public String getDefaultConnectionString(UInt32 y)
        {
            System.Management.ManagementObjectSearcher a = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            String r = "";
            foreach (System.Management.ManagementObject b in a.Get())
            {
                if ((UInt32)b["Index"] == y)
                {
                    r = b["Signature"].ToString();
                }
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(r);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result); 
        }

        private string x()
        {
            string a = string.Empty;
            ManagementClass b = new ManagementClass("Win32_NetworkAdapter");
            ManagementObjectCollection c = b.GetInstances();
            foreach (ManagementObject MO in c)
                if (MO != null)
                {
                    if (MO["MacAddress"] != null)
                    {
                        a = MO["MACAddress"].ToString();
                        if (a != string.Empty)
                            break;
                    }
                }
            return a;
        }
    }
}
