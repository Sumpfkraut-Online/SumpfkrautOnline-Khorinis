using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using WinApi;

namespace GUC.Network.Messages.PlayerCommands
{
    public class InterfaceOptionsMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type = 0;
            bool enabled = false;
            stream.Read(out type);
            stream.Read(out enabled);

            /*if (type == 0)
            {
                Player.EnableStatusMenu = enabled;
                Gothic.mClasses.InputHooked.deactivateStatusScreen(Process.ThisProcess(), enabled);
            }
            else if (type == 1)
            {
                Player.EnableLogMenu = enabled;
                Gothic.mClasses.InputHooked.deactivateLogScreen(Process.ThisProcess(), enabled);
            }*/
        }
    }
}
