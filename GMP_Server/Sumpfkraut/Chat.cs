using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Network.Messages;
using RakNet;

namespace GUC.Server.Sumpfkraut
{
    public class Chat
    {
        public delegate void MessageHandler(Player sender, string text);
        public event MessageHandler OnReceiveMessage 
        {
            add
            {
                messenger.Receive += value;
            }
            remove
            {
                messenger.Receive -= value;
            }
        }

        //Adds the NetworkID to the listener
        private ChatMessage messenger;
        public Chat()
        {
            if (Program.server != null && Program.server.MessageListener != null)
            {
                if (!Program.server.MessageListener.ContainsKey((byte)NetworkID.ChatMessage))
                {
                    messenger = new ChatMessage();
                    Program.server.MessageListener.Add((byte)NetworkID.ChatMessage, messenger);
                }
                else
                {
                    messenger = (ChatMessage)Program.server.MessageListener[(byte)NetworkID.ChatMessage];
                }
            }
        }

        //Removes the NetworkID
        /*~Chat() //removed because it likes to self-destruct
        {
            Log.Logger.log("Destroy");
            Program.server.MessageListener.Remove((byte)NetworkID.ChatMessage);
        }*/
        
        public void SendSay(Player from, Player to, String text)
        {
            if (from != null && to != null && text != null && text.Length > 0)
            {
                SendText(from, to, ChatTextType.Say, text);
            }
        }

        public void SendShout(Player from, Player to, String text)
        {
            if (from != null && to != null && text != null && text.Length > 0)
            {
                SendText(from, to, ChatTextType.Shout, text);
            }
        }

        public void SendWhisper(Player from, Player to, String text)
        {
            if (from != null && to != null && text != null && text.Length > 0)
            {
                SendText(from, to, ChatTextType.Whisper, text);
            }
        }

        public void SendAmbient(Player from, Player to, String text)
        {
            if (from != null && to != null && text != null && text.Length > 0)
            {
                SendText(from, to, ChatTextType.Ambient, text);
            }
        }

        public void SendOOC(Player from, Player to, String text)
        {
            if (from != null && to != null && text != null && text.Length > 0)
            {
                SendText(from, to, ChatTextType.OOC, text);
            }
        }

        public void SendOOCGlobal(Player from, String text)
        {
            if (from != null && text != null && text.Length > 0)
            {
                SendText(from, null, ChatTextType.OOCGlobal, text);
            }
        }

        public void SendGlobal(String text)
        {
            if (text != null && text.Length > 0)
            {
                SendText(null, null, ChatTextType.RPGlobal, text);
            }
        }

        public void SendPM(Player from, Player to, String text)
        {
            if (text != null && text.Length > 0)
            {
                SendText(from, to, ChatTextType.PM, text);
            }
        }

        public void SendOOCEvent(String text)
        {
            if (text != null && text.Length > 0)
            {
                SendText(null, null, ChatTextType.OOCEvent, text);
            }
        }

        public void SendErrorMessage(Player to, String text)
        {
            if (text != null && text.Length > 0)
            {
                SendText(null, to, ChatTextType._Error, text);
            }
        }

        public void SendHintMessage(Player to, String text)
        {
            if (text != null && text.Length > 0)
            {
                SendText(null, to, ChatTextType._Hint, text);
            }
        }

        private void SendText(Player from, Player to, ChatTextType type, String text)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);
            stream.Write((byte)type);
            if (from != null)
            {
                stream.Write(from.ID);
            }
            stream.Write(text);

            if (to == null)
            {
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            }
            else
            {
                using (RakNetGUID guid = to.proto.GUID)
                {
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }
        }
    }
}
