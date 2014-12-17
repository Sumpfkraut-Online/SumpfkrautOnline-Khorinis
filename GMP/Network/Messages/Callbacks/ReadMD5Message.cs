using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using RakNet;
using GUC.Enumeration;
using System.IO;
using GUC.States;
using System.Security.Cryptography;

namespace GUC.Network.Messages.Callbacks
{
    class ReadMd5Message : IMessage
    {

        private String FileMd5(string filePath)
        {
            using (System.IO.FileStream FileCheck = System.IO.File.OpenRead(filePath))
            {

                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] md5Hash = md5.ComputeHash(FileCheck);

                return BitConverter.ToString(md5Hash).Replace("-", "").ToLower();
            }
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {

            int callBackID = 0, playerID = 0;
            String md5File;

            stream.Read(out callBackID);
            stream.Read(out playerID);
            stream.Read(out md5File);

            String value = "";
            try
            {
                md5File = Path.GetFullPath(md5File);
                String minName = new DirectoryInfo(StartupState.getSystemPath()).Parent.FullName;
                if (md5File.StartsWith(minName))
                {
                    if (File.Exists(md5File))
                    {
                        value = FileMd5(md5File);
                    }
                }
            }
            catch (Exception ex) {}

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ReadMd5Message);
            stream.Write(callBackID);
            stream.Write(playerID);
            stream.Write(md5File);
            stream.Write(value);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


        }
    }
}
