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
        textBox tB = null;

        MessageBox mB = null;
        zCView thisView = null;

        public ChatMessage()
        {
            Process process = Process.ThisProcess();

            thisView = zCView.Create(Process.ThisProcess(), 10, 3000, 0x3000, 0x3000);

            //Input view
            tB = new textBox(thisView, process);

            tB.resetKey = (int)VirtualKey.Escape;
            tB.startWritingKey = (int)VirtualKey.U;
            tB.sendKey = (int)VirtualKey.Return;

            tB.vt.PosX = 0;
            tB.vt.PosY = 0;


            tB.vt.Color.R = 255;
            tB.vt.Color.G = 255;
            tB.vt.Color.B = 255;
            tB.vt.Color.A = 255;


            tB.SendInput += new EventHandler<EventArgs>(SendText);

            //Output view
        }

        Boolean shown = false;

        //send our chat text to the server
        private void SendText(object obj, EventArgs args)
        {
            if (!shown && tB != null && thisView != null) //FIXME: Man muss einmal eine Nachricht abschicken um die Chatbox sichtbar zu machen
            {
                zCView.GetStartscreen(Process.ThisProcess()).InsertItem(thisView, 0); //FIXME: wenn das hier im Creator steht, crasht es beim Start-Up, andere Lösung?
                mB = new MessageBox(0, 10, "FONT_DEFAULT.TGA", new Vec2i(10, 10), null);
                mB.show();
                shown = true;
            }

            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);

            stream.Write(Player.Hero.ID);
            stream.Write(tB.getText());

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            tB.setText("");
            tB.KeyDisable();
        }

        //Receive a chat text from the server
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            string text;
            stream.Read(out text);

            mB.addMessage(text, ColorRGBA.White);
        }
    }
}
