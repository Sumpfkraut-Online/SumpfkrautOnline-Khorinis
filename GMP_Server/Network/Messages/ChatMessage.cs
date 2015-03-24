using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Server.Scripting.Client;

namespace GUC.Server.Network.Messages
{
    class ChatMessage : IMessage
    {
        public ChatMessage()
        {
        }

        public event Chat.MessageHandler Receive;

        //we received a chat text from a client
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            string text = null;
            Player from = null;
            try
            {
                int playerID = 0;
                stream.Read(out playerID);
                from = (Player)sWorld.VobDict[playerID];

                stream.Read(out text);
            }
            catch {}
            finally
            {
                if (from != null && text != null && text.Length > 0 && Receive != null)
                {
                    Receive((Scripting.Objects.Character.Player)from.ScriptingNPC, text);
                }
            }
        }
    }
}
