using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Injection;
using GMP.Modules;
using Gothic.zClasses;
using WinApi;

namespace AccountModule.Login
{
    class AccountMessage : Message
    {
        /// <summary>
        /// 0xdf
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Write(RakNet.BitStream stream, Client client,int type, String username, String password)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xdf);
            stream.Write((byte)type);
            stream.Write(username);
            stream.Write(password);

            client.client.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public static int AccountID = -1;
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte regType = 0;
            byte type = 0;

            stream.Read(out regType);
            stream.Read(out type);

            if (type == 2 || type == 1)
            {
                AccountStart.loginBox.Message(regType, type);
            }
            else
            {
                stream.Read(out AccountID);
                AccountStart.loginBox.Message(regType, type);
            }
        }
    }
}
