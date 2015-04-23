using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut.Ingame.GUI;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame
{
    class Chat : IMessage, GUCMInputReceiver
    {
        Dictionary<string[], string> rpCmdList = new Dictionary<string[], string>() { { new string[] {"/say", "/sagen"}, "Sagen: "}, //<-Default
                                                                                        { new string[] {"!", "/shout", "/rufen"}, "Rufen: " }, 
                                                                                        { new string[] {".", "/whisper", "/flüstern"},"Flüstern: "}, 
                                                                                        { new string[] {"/ambient", "/me", "/do", "/machen"}, "Machen: "} };

        Dictionary<string[], string> oocCmdList = new Dictionary<string[], string>() { { new string[] {"/ooc", "//"}, "OOC: "}, //<-Default
                                                                                        { new string[] {"!","/ooc !", "// !", "/ooc!", "//!"} , "Global: "},
                                                                                        { new string[] {"/pm","/pn" } , "PN: "},
                                                                                        { new string[] {"/"}, "Befehl: "} };

        GUCMenuTextBox tb;
        GUCMenuTextList rpList;
        GUCMenuTextList oocList;
        bool ooc;

        bool inputEnabled;
        GUCMenuText helpText;

        long animationTime;
        bool colorVar; //used to toggle message colors between white and darker white

        public Chat()
        {
            int[] size = InputHooked.GetScreenSize(Process.ThisProcess());
            size[1] /= 4;

            //text lists
            rpList = new GUCMenuTextList(0, 0, size[0], size[1], "");
            rpList.Show();
            oocList = new GUCMenuTextList(0, 0, size[0], size[1], "Inv_Back.tga");
            ooc = false;

            //input menu
            tb = new GUCMenuTextBox(0, size[1], size[0], false);
            tb.InputChangedEvent += InputChanged;
            inputEnabled = false;

            helpText = new GUCMenuText("", 0, size[1]);
            InputChanged();

            animationTime = 0;
            colorVar = true;

            IngameInput.menus.Add(this);
        }

        public void KeyPressed(int key)
        {
            if (!inputEnabled)
            {
                if (key == (int)VirtualKeys.Return) //Open chat
                {
                    helpText.Show();
                    tb.Show();
                    inputEnabled = true;
                    IngameInput.activateFullControl(this);
                }
            }
            else
            {
                if (key == (int)VirtualKeys.Return) //Send Message
                {
                    string message = tb.input.Trim();
                    if (message.Length > 0)
                    {
                        if (ooc && !(message.StartsWith("/ooc") || message.StartsWith("//")))
                        {
                            SendMessage("/ooc " + message);
                        }
                        else
                        {
                            SendMessage(message);
                        }

                        if (message[0] == '/' && !(message.StartsWith("/ooc") || message.StartsWith("//")))
                        {
                            tb.ResetInput(true); //save the input if it's a command
                        }
                        else
                        {
                            tb.ResetInput(false);
                        }
                    }
                    InputChanged();
                    helpText.Hide();
                    tb.Hide();
                    inputEnabled = false;
                    IngameInput.deactivateFullControl();
                }
                else if (key == (int)VirtualKeys.Escape) //close chat
                {
                    helpText.Hide();
                    tb.Hide();
                    inputEnabled = false;
                    IngameInput.deactivateFullControl();
                }
                else if (key == (int)VirtualKeys.Tab) //toggle between ooc & rp
                {
                    if (ooc)
                    {
                        oocList.Hide();
                        rpList.Show();
                        ooc = false;
                    }
                    else
                    {
                        oocList.Show();
                        rpList.Hide();
                        ooc = true;
                    }
                    InputChanged();
                }
                else
                {
                    if (ooc)
                    {
                        oocList.KeyPressed(key);
                    }
                    else
                    {
                        rpList.KeyPressed(key);
                    }
                    tb.KeyPressed(key);
                }
            }
        }

        public void Update(long ticks)
        {
            if (inputEnabled)
                tb.Update(ticks);
        }

        //check for commands
        private void InputChanged()
        {
            string message = tb.input.Trim(); int w;
            int screenWidth = InputHooked.GetScreenSize(Process.ThisProcess())[0];

            if (message.Length > 0)
            {
                Dictionary<string[], string> cmdList = rpCmdList;

                while (true)
                {
                    foreach (KeyValuePair<string[], string> pair in cmdList)
                    {
                        foreach (string cmd in pair.Key)
                        {
                            if (message.StartsWith(cmd + ' '))
                            {
                                if (cmd != "/")
                                {
                                    tb.numHideChars = cmd.Length+1;
                                    if (message.Length > cmd.Length+1)
                                        SendDialogueAnimation(pair.Value);
                                }
                                else
                                {
                                    tb.numHideChars = 0;
                                }
                                w = IngameInput.StringPixelWidth(pair.Value);
                                tb.SetXSize(w, screenWidth - w);
                                helpText.Text = pair.Value;
                                return;
                            }
                        }
                    }
                    if (cmdList == oocCmdList)
                        break;
                    else
                        cmdList = oocCmdList;
                }
                SendDialogueAnimation(null);
            }
            //Default
            tb.numHideChars = 0;
            w = IngameInput.StringPixelWidth(rpCmdList.Values.ToArray()[0]);
            tb.SetXSize(w, screenWidth - w);
            helpText.Text = rpCmdList.Values.ToArray()[0];
        }

        private void SendDialogueAnimation(string textType)
        {
            if (ooc)
                return;

            if (textType != null)
            {
                if (oocCmdList.ContainsValue(textType) || textType == "Flüstern: ")
                {
                    return;
                }
            }

            long now = DateTime.Now.Ticks;
            if (now < animationTime)
                return;

            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);
            stream.Write(Player.Hero.ID);
            stream.Write("/StartDialogueAnimation");

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            animationTime = now + 60000000; //every 6 secs
        }

        //send our chat text to the server
        private void SendMessage(string text)
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
            if (ctt != ChatTextType.Global && ctt < ChatTextType.Event)
            {
                int ID;
                stream.Read(out ID);

                oCNpc sender = new oCNpc(Process.ThisProcess(), sWorld.VobDict[ID].Address);
                addText = sender.Name.ToString();
            }

            string message;
            stream.Read(out message);

            ColorRGBA color = ToggleMessageColor();
            switch (ctt)
            {
                case ChatTextType.Ambient:
                    addText += " ";
                    break;

                case ChatTextType.Say:
                    addText += ": ";
                    break;

                case ChatTextType.Shout:
                    addText += " ruft: ";
                    break;

                case ChatTextType.Whisper:
                    addText += " flüstert: ";
                    color = new ColorRGBA(255, 255, 255, 200);
                    break;

                case ChatTextType.Global:
                    color = new ColorRGBA(0, 255, 0, 255); //green
                    break;

                case ChatTextType.OOCGlobal:
                    addText += " (global): ";
                    color = new ColorRGBA(255, 150, 255, 255); //pink
                    break;

                case ChatTextType.OOC:
                    addText += ": ";
                    color = new ColorRGBA(255, 255, 150, 255); //yellow
                    break;

                case ChatTextType.PM:
                    addText += " (pm): ";
                    color = new ColorRGBA(255, 255, 255, 255);
                    break;

                case ChatTextType.Event:
                    color = new ColorRGBA(255, 200, 200, 255);
                    break;

                case ChatTextType.Error:
                    color = new ColorRGBA(255, 0, 0, 255);
                    break;

                case ChatTextType.Hint:
                    color = new ColorRGBA(255, 255, 255, 255);
                    break;
            }

            if (ctt >= ChatTextType.OOC)
            {
                oocList.AddLine(addText + message, color);
            }
            else
            {
                rpList.AddLine(addText + message, color);
            }
        }

        private ColorRGBA ToggleMessageColor()
        {
            colorVar = !colorVar;
            return colorVar ? new ColorRGBA(230, 230, 230, 255) : new ColorRGBA(255, 255, 255, 255);
        }
    }
}
