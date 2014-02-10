using System;
using System.Collections.Generic;
using System.Text;
using Network;

namespace GMP_Server.Net.Message
{
    public class TextBoxSendMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id;
            int tbID;
            String content;

            stream.Read(out id);
            stream.Read(out tbID);
            stream.Read(out content);

            Player pl = Program.playerDict[id];
            Scripting.TextBox tB = (Scripting.TextBox)Scripting.View.getView(tbID);

            Program.scriptManager.OnTextBoxMessageReceived(tB, new Scripting.Player(pl), content);
        }
    }
}
