using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using Gothic.zClasses;
using WinApi;
using GUC.Types;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.GUI;

namespace GUC.Network.Messages
{
    class ChatMessage : IMessage
    {
        ChatGUI gui;

        public ChatMessage()
        {
            gui = new ChatGUI();
            gui.SendInput += SendText;
        }

        //send our chat text to the server
        public void SendText(String text)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);

            stream.Write(Player.Hero.ID);
            stream.Write(text);

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        //Receive a chat text from the server
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type;
            stream.Read(out type);
            ChatTextType ctt = (ChatTextType)type;

            string addText = "";
            if (ctt != ChatTextType.Global)
            {
                int ID;
                stream.Read(out ID);

                oCNpc sender = new oCNpc(Process.ThisProcess(), sWorld.VobDict[ID].Address);
                addText = sender.Name.ToString();
            }

            string message;
            stream.Read(out message);

            ColorRGBA color = ColorRGBA.White;
            switch (ctt)
            {
                case ChatTextType.Ambient:
                    addText += " ";
                    break;
                
                case ChatTextType.Global:
                    color = new ColorRGBA(0, 255, 0, 255); //green
                    break;

                case ChatTextType.OOCGlobal:
                    addText += " (ooc): ";
                    color = new ColorRGBA(240, 100, 240, 255); //pink
                    break;

                case ChatTextType.OOC:
                    addText += " (ooc): ";
                    color = new ColorRGBA(240, 240, 100, 255); //yellow
                    break;

                case ChatTextType.Say:
                    addText += " sagt: ";
                    break;

                case ChatTextType.Shout:
                    addText += " ruft: ";
                    break;

                case ChatTextType.Whisper:
                    addText += " flüstert: ";
                    color = new ColorRGBA(255, 255, 255, 200);
                    break;
            }

            gui.AddLine(addText + message,color);
        }
    }
}
