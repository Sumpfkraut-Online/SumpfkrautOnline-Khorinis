using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using System.Management;
using System.Security.Cryptography;
using GUC.Network;
using System.IO;
using System.IO.Compression;
using Gothic.zClasses;
using GUC.Client.WorldObjects;

namespace GUC.Client.Network.Messages
{
    static class ConnectionMessage
    {
        public static void Write()
        {
            String connString = getDefaultConnectionString(0);
            String macString = x();
            byte[] npcTableHash = NPCInstance.Table.ReadFile();
            byte[] itemTableHash = ItemInstance.Table.ReadFile();
            byte[] mobTableHash = MobInstance.Table.ReadFile();

            PacketWriter stream = Program.client.SetupSendStream(NetworkID.ConnectionMessage);
            stream.Write(connString);
            stream.Write(macString);
            stream.Write(npcTableHash, 0, 16);
            stream.Write(itemTableHash, 0, 16);
            stream.Write(mobTableHash, 0, 16);
            Program.client.SendStream(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED);
        }

        static String getDefaultConnectionString(UInt32 y)
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
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "Signature not found ", 0, "Program.cs", 0);
                }
            }
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] textToHash = Encoding.Default.GetBytes(r);
            byte[] result = md5.ComputeHash(textToHash);

            return System.BitConverter.ToString(result);
        }

        static string x()
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

        public static void Read(PacketReader stream)
        {
            if (stream.ReadBit()) // new npc instances for us
            {
                int len = stream.ReadInt(); // length of packed data

                byte[] data = new byte[len];
                stream.Read(data, 0, len);

                NPCInstance.Table.ReadData(data);

                NPCInstance.Table.WriteFile();
            }

            if (stream.ReadBit())
            {
                int len = stream.ReadInt(); // length of packed data

                byte[] data = new byte[len];
                stream.Read(data, 0, len);

                ItemInstance.Table.ReadData(data);

                ItemInstance.Table.WriteFile();
            }

            if (stream.ReadBit())
            {
                int len = stream.ReadInt(); // length of packed data

                byte[] data = new byte[len];
                stream.Read(data, 0, len);

                MobInstance.Table.ReadData(data);

                MobInstance.Table.WriteFile();
            }
        }
    }
}
