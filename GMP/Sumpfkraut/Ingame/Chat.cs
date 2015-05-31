using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Types;
using GUC.Sumpfkraut.GUI;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.mClasses;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame
{
    class Chat : GUCInputReceiver
    {
        private static Chat chat;
        public static Chat GetChat()
        {
            if (chat == null)
            {
                chat = new Chat();
            }
            return chat;
        }

        public VirtualKeys ToggleKey = VirtualKeys.F2;
        public VirtualKeys ActivationKey = VirtualKeys.Return;

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

        GUCMenuText helpText;

        long animationTime;
        bool colorVar; //used to toggle message colors between white and darker white

        private ChatMessage messenger;
        public Chat()
        {
            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.ChatMessage))
            {
                Program.client.messageListener.Add((byte)NetworkID.ChatMessage, new ChatMessage());
            }
            messenger = (ChatMessage)Program.client.messageListener[(byte)NetworkID.ChatMessage];
            messenger.OnReceiveMessage += ReceiveMessage;

            //chat takes up 1/4 of the top screen
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

            //text at the beginning of chat input
            helpText = new GUCMenuText("", 0, size[1], false);
            InputChanged(); //update

            animationTime = 0;
            colorVar = true;
        }

        public void Open()
        {
            helpText.Show();
            tb.Show();
            tb.Enabled = true;
            InputHandler.activateFullControl(this);
        }

        public void ToggleChat()
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

        public void KeyPressed(int key)
        {
            //toggle between ooc & rp
            if (key == (int)ToggleKey)
            {
                ToggleChat();
            }

            if (key == (int)ActivationKey) //Send Message
            {
                string message = tb.input.Trim();
                if (message.Length > 0)
                {
                    if (ooc && !(message.StartsWith("/ooc") || message.StartsWith("//")))
                    {
                        messenger.SendMessage("/ooc " + message);
                    }
                    else
                    {
                        messenger.SendMessage(message);
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
                tb.Enabled = false;
                InputHandler.deactivateFullControl(this);
            }
            else if (key == (int)VirtualKeys.Escape) //close chat
            {
                helpText.Hide();
                tb.Hide();
                tb.Enabled = false;
                InputHandler.deactivateFullControl(this);
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

        public void Update(long ticks)
        {
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
                                    tb.numHideChars = cmd.Length + 1;
                                    if (message.Length > cmd.Length + 1)
                                        SendDialogueAnimation(pair.Value);
                                }
                                else
                                {
                                    tb.numHideChars = 0;
                                }

                                w = InputHandler.StringPixelWidth(pair.Value);
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
            w = InputHandler.StringPixelWidth(rpCmdList.Values.ToArray()[0]);
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

            /*BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);
            stream.Write(Player.Hero.ID);
            stream.Write("/StartDialogueAnimation");

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);*/

            animationTime = now + 60000000; //every 6 secs
        }

        //Receive a chat text from the server
        private void ReceiveMessage(ChatTextType type, string sender, string message)
        {
            string addText = "";
            if (sender != null)
            {
                addText = sender;
            }

            ColorRGBA color = ToggleMessageColor();
            switch (type)
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
                    color = new ColorRGBA(250, 250, 250, 200);  //slight transparency
                    break;

                case ChatTextType.RPGlobal:
                    color = new ColorRGBA(0, 255, 0, 255);      //green
                    break;

                case ChatTextType.RPEvent:
                    color = new ColorRGBA(255, 230, 230, 255);  //light red
                    break;



                case ChatTextType.OOCGlobal:
                    addText += " (global): ";
                    color = new ColorRGBA(255, 150, 255, 255);  //pink
                    break;

                case ChatTextType.OOC:
                    addText += ": ";
                    color = new ColorRGBA(255, 255, 150, 255);  //yellow
                    break;

                case ChatTextType.PM:
                    addText += " (pm): ";
                    color = new ColorRGBA(255, 255, 255, 255);  //white
                    break;

                case ChatTextType.OOCEvent:
                    color = new ColorRGBA(255, 230, 230, 255);  //light red
                    break;



                case ChatTextType._Error:
                    color = new ColorRGBA(255, 0, 0, 255);      //red
                    break;

                case ChatTextType._Hint:
                    color = new ColorRGBA(255, 255, 255, 255);  //white
                    break;
            }

            if (type < ChatTextType.MAX_RP)
            {
                rpList.AddLine(addText + message, color);
            }
            else if (type < ChatTextType.MAX_OOC)
            {
                oocList.AddLine(addText + message, color);
            }
            else
            {
                if (ooc)
                {
                    oocList.AddLine(addText + message, color);
                }
                else
                {
                    rpList.AddLine(addText + message, color);
                }
            }
        }

        ColorRGBA color1 = new ColorRGBA(230, 230, 230, 255);
        ColorRGBA color2 = new ColorRGBA(255, 255, 255, 255);
        private ColorRGBA ToggleMessageColor()
        {
            colorVar = !colorVar;
            return colorVar ? color1 : color2;
        }
    }
}
