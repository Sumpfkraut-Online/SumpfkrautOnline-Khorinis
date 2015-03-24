using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using WinApi;
using GUC.States;
using GUC.WorldObjects;
using System.IO;
using Gothic.zTypes;
using GUC.Types;
using System.Management;
using System.Security.Cryptography;

namespace GUC.Network.Messages.Connection
{
    class ConnectionMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            
            int id = 0;

            stream.Read(out id);
            Player.Hero.ID = id;
            
            sWorld.addVob(Player.Hero);

            using (BitStream stream2 = new BitStream())
            {
                Zip.Decompress(stream, stream2);
                stream = stream2;



                stream.Read(out Player.EnableStatusMenu);
                stream.Read(out Player.EnableLogMenu);

                //Player-Defaults:
                if(Player.EnableStatusMenu)
                    Gothic.mClasses.InputHooked.deactivateStatusScreen(Process.ThisProcess(), true);
                if (Player.EnableLogMenu)
                    Gothic.mClasses.InputHooked.deactivateLogScreen(Process.ThisProcess(), true);




                stream.Read(out Player.sSendAllKeys);
                int playerKeyCount = 0;
                stream.Read(out playerKeyCount);
                for (int i = 0; i < playerKeyCount; i++)
                {
                    byte key = 0;
                    stream.Read(out key);
                    Player.sSendKeys.Add(key);
                }


                int day = 0;
                byte hour = 0, minute = 0;
                stream.Read(out day);
                stream.Read(out hour);
                stream.Read(out minute);

                sWorld.Day = day;
                sWorld.Hour = hour;
                sWorld.Minute = minute;


                byte wt = 0, starthour = 0, startminute = 0, endhour = 0, endminute;

                stream.Read(out wt);
                stream.Read(out starthour);
                stream.Read(out startminute);
                stream.Read(out endhour);
                stream.Read(out endminute);

                sWorld.WeatherType = wt;
                sWorld.StartRainHour = starthour;
                sWorld.StartRainMinute = startminute;
                sWorld.EndRainHour = endhour;
                sWorld.EndRainMinute = endminute;


                short spellCount = 0;
                stream.Read(out spellCount);
                for (int i = 0; i < spellCount; i++)
                {
                    Spell spell = new Spell();
                    spell.Read(stream);
                    Spell.addItemInstance(spell);
                }

                short itemInstancesCount = 0;
                stream.Read(out itemInstancesCount);
                for (int i = 0; i < itemInstancesCount; i++)
                {
                    ItemInstance ii = new ItemInstance();
                    ii.Read(stream);


                    ItemInstance.addItemInstance(ii);
                }
                CreateItems();

                //ItemList:
                int iLC = 0;
                stream.Read(out iLC);
                for (int i = 0; i < iLC; i++)
                {
                    Item item = new Item();
                    item.Read(stream);
                    sWorld.addVob(item);
                }

                //Vob-List:
                int vLC = 0;
                stream.Read(out vLC);
                for (int i = 0; i < vLC; i++)
                {
                    int vobType = 0;
                    stream.Read(out vobType);
                    Vob vob = Vob.createVob((VobType)vobType);
                    vob.Read(stream);
                    sWorld.addVob(vob);
                }

                //NPC-List:
                int nLC = 0;
                stream.Read(out nLC);
                for (int i = 0; i < nLC; i++)
                {
                    NPC npc = new NPC();
                    npc.Read(stream);
                    sWorld.addVob(npc);
                }

                //Player-List:
                int pLC = 0;
                stream.Read(out pLC);
                for (int i = 0; i < pLC; i++)
                {
                    Player player = new Player(false, "");
                    player.Read(stream);

                    if (player.ID == id)
                        continue;
                    sWorld.addVob(player);
                }

                //WorldSpawnList:
                int worldListCount = 0;
                stream.Read(out worldListCount);
                for (int i = 0; i < worldListCount; i++)
                {

                    World w = new World();
                    w.Read(stream);

                    sWorld.WorldDict.Add(w.Map, w);

                }
            }

            //edited by Showdown
            Sumpfkraut.Ingame.IngameInterface.Init();
        }

        protected static void CreateItems()
        {
            //Creation of Items:
            StringBuilder sb = new StringBuilder();
            foreach (ItemInstance ii in ItemInstance.ItemInstanceList)
            {
                sb.Append(ii.getDeadalusScript());
                sb.AppendLine("");
                sb.AppendLine("");
            }

            String itemListFileName = StartupState.getRandomScriptString(".d");
            File.WriteAllText(itemListFileName, sb.ToString());

            zString gStr = zString.Create(Process.ThisProcess(), itemListFileName);
            zCParser.getParser(Process.ThisProcess()).ParseFile(gStr);
            gStr.Dispose();
        }

        public static void Write()
        {
            String connString = getDefaultConnectionString(0);
            String macString = x();


            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ConnectionMessage);

            stream.Write(Player.Hero.Name);
            stream.Write(connString);
            stream.Write(macString);
            stream.Write(Player.Hero.Position);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        public static String getDefaultConnectionString(UInt32 y)
        {
            System.Management.ManagementObjectSearcher a = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            String r = "";
            foreach (System.Management.ManagementObject b in a.Get())
            {
                if ((UInt32)b["Index"] == y && b["Signature"] != null)
                {
                    r = b["Signature"].ToString();
                }

                if ((UInt32)b["Index"] == y && b["Signature"] == null)
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Signature not found ", 0, "Program.cs", 0);
                }
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(r);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result);
        }

        private static string x()
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
