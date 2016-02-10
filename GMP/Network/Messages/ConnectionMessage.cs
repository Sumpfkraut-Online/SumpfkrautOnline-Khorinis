using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Enumeration;
using RakNet;
using GUC.Log;
using System.Security.Cryptography;
using System.Management;

namespace GUC.Client.Network.Messages
{
    static class ConnectionMessage
    {
        public static void Read(PacketReader strm)
        {

        }

        public static void Write()
        {
            byte[] signature = GetMD5(GetSignature(0));
            byte[] mac = GetMD5(GetMacAddress());

            //System.IO.File.WriteAllText("sign", GetSignature(0));
            //System.IO.File.WriteAllText("mac", GetMacAddress());

            PacketWriter stream = GameClient.SetupStream(NetworkIDs.ConnectionMessage);
            stream.Write(signature, 0, 16);
            stream.Write(mac, 0, 16);
            GameClient.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE);
        }

        static byte[] GetMD5(string str)
        {
            byte[] buf;
            using (MD5 md5 = new MD5CryptoServiceProvider())
            {
                buf = md5.ComputeHash(Encoding.Unicode.GetBytes(str));
            }
            return buf;
        }

        static String GetSignature(UInt32 y)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject obj in searcher.Get())
            {
                if ((UInt32)obj["Index"] == y)
                {
                    object sign = obj["Signature"];
                    if (sign != null && !string.IsNullOrWhiteSpace(sign.ToString()))
                        return sign.ToString();
                }
            }
            throw new Exception("Signature not found!");
        }

        static string GetMacAddress()
        {
            ManagementClass adapter = new ManagementClass("Win32_NetworkAdapter");
            ManagementObjectCollection collection = adapter.GetInstances();
            foreach (ManagementObject MO in collection)
            {
                if (MO != null)
                {
                    object obj = MO["MacAddress"];
                    if (obj != null && !string.IsNullOrWhiteSpace(obj.ToString()))
                    {
                        return obj.ToString();
                    }
                }
            }
            throw new Exception("MacAddress not found!");
        }
    }
}
