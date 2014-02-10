using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Injection;
using Gothic.zClasses;
using WinApi;
using GMP.Logger;
using RakNet;
using GMP.Modules;
using GMP.Helper;

namespace GMP.Net.Messages
{
    public class DisconnectedMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            if (!StaticVars.Ingame)
                return;
            ErrorLog.Log(typeof(DisconnectedMessage), "Read");
            int id = 0;
            stream.Read(out id);

            Player pl = StaticVars.AllPlayerDict[id];


            if (Program.chatBox != null && pl != null && StaticVars.serverConfig.ShowConnectionMessages)
            {
                for (byte i = 0; i < Program.chatBox.maxType; i++)
                {
                    if (!StaticVars.serverConfig.HideNames || pl.knowName)
                        Program.chatBox.addRow(i, "Player " + pl.name + " disconnected");
                    else
                        Program.chatBox.addRow(i, "Player disconnected");
                }
            }

            NPCHelper.RemovePlayer(pl, true);
        }

        
    }
}
