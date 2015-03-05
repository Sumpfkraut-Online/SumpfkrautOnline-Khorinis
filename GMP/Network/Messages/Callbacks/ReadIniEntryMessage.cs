using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using RakNet;
using GUC.Enumeration;

namespace GUC.Network.Messages.Callbacks
{
    class ReadIniEntryMessage : IMessage
    {

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {

            int callBackID = 0, playerID = 0;
            String section, entry;

            stream.Read(out callBackID);
            stream.Read(out playerID);
            stream.Read(out section);
            stream.Read(out entry);

            String value = "";
            try
            {
                value = zCOption.GetOption(Process.ThisProcess()).getEntryValue(section, entry);
            }
            catch (Exception ex) {}

            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.ReadIniEntryMessage);
            stream.Write(callBackID);
            stream.Write(playerID);
            stream.Write(section);
            stream.Write(entry);
            stream.Write(value);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


        }
    }
}
