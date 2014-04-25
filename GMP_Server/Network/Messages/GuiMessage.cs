using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Server.Network.Messages
{
    class GuiMessage : IMessage
    {
        protected View getViewFromList(int id)
        {
            if (!View.AllViewDict.ContainsKey(id))
                throw new Exception("The key: " + id + " was not found in the viewlist!");
            return View.AllViewDict[id];
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int playerID = 0;
            byte type = 0;
            stream.Read(out type);
            stream.Read(out playerID);

            Player pl = (Player)sWorld.VobDict[playerID];

            GuiMessageType gmT = (GuiMessageType)type;
            int viewID = 0;
            if (gmT == GuiMessageType.TextBoxCallSend)
            {
                String message;
                stream.Read(out viewID);
                stream.Read(out message);

                View v = getViewFromList(viewID);
                if (!(v is TextBox))
                    throw new Exception("TextBoxCallSend needs a TextBox "+v);
                TextBox tex = (TextBox)v;
                TextBox.OnTextSends(tex, (Scripting.Objects.Character.Player)pl.ScriptingNPC, message);
            }
            else if (gmT == GuiMessageType.ButtonPressed)
            {
                stream.Read(out viewID);

                View v = getViewFromList(viewID);
                if (!(v is Button))
                    throw new Exception("ButtonPressed needs a Button " + v);
                Button button = (Button)v;
                Button.OnButtonPress(button, (Scripting.Objects.Character.Player)pl.ScriptingNPC);
            }
        }
    }
}
