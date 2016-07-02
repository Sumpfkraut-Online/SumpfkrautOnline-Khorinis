using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WinApi;
using WinApi.User.Enumeration;
using GUC.Client.GUI;
using GUC.Types;
using GUC.Client.Network.Messages;
using Gothic.zClasses;

//TODO: replace 
//messageListener.Add((byte)NetworkID.ChatMessage, ChatMessage.Read);
// with onReadMenuMsg or something

//TODO: wo sind die gamestates? es fehlt dort noch { Menus.GUCMenus.Chat.Hotkey, Menus.GUCMenus.Chat.Open}
//https://github.com/Sumpfkraut-Online/SumpfkrautOnline-Khorinis/blob/492c6700c5f260f94bc9c61655e3b28b80bbbb7e/GMP/States/GameState.cs

namespace GUC.Client.Menus
{
    class ChatMenu : GUCMenu
    {
        static ChatMenu chatCtrl;
        public static ChatMenu GetChat()
        {
            if (chatCtrl == null)
            {
                chatCtrl = new ChatMenu();
            }
            return chatCtrl;
        }
        public VirtualKeys Hotkey = VirtualKeys.Return;
        public VirtualKeys SendMessageKey = VirtualKeys.Return;
        public VirtualKeys CloseMenuKey = VirtualKeys.Escape;
        public VirtualKeys ClearChatKey = VirtualKeys.Delete;
        public VirtualKeys ResetBackupKey = VirtualKeys.Insert;
        public VirtualKeys ToggleChatKey = VirtualKeys.Tab;

        public List<string> Players;

        GUCVisual chatwindow;
        int[] screenSize;
        int height;
        int lines;

        GUCTextBox textInput;
        GUCTextList rpChat;
        GUCTextList oocChat;

        string inputBackup;

        Dictionary<string, string> CommandList = new Dictionary<string, string>()
        {
            {"/s", "Rufen:"},
            {"/w", "Flüstern:"},
            {"/me", "Sagen:"}
        };
        string replaced, replacedBackup;

        // Liste der Zuletzt benutzen befehle
        List<string> latestCommands;
        int lcShift;
        string lcBackup;

        // new unread message?
        bool highlighted;
        bool chatVisible;
        bool oocChatShown;

        public ChatMenu()
        {
            screenSize = GUCView.GetScreenSize();

            int distance = 13;
            lines = 10;
            height = distance * lines + 5;

            rpChat = new GUCTextList(0, 0, screenSize[0], height, "", lines, distance);
            oocChat = new GUCTextList(0, 0, screenSize[0], height, "Inv_Back.tga", lines, distance);

            textInput = new GUCTextBox(0, height, screenSize[0], false);
            replaced = "";
            replacedBackup = "";

            latestCommands = new List<string>();
            lcShift = 0;
            lcBackup = "";

            chatVisible = false;
            oocChatShown = false;

            Players = new List<string>();
        }

        public void AddRPMessage(string text, ColorRGBA color)
        {
            rpChat.AddText(text, color);
            NewMessageVisualUpdate(1);
        }

        public void AddOOCMessage(string text, ColorRGBA color)
        {
            oocChat.AddText(text, color);
            NewMessageVisualUpdate(2);
        }

        public void AddToShown(string text, ColorRGBA color)
        {
            if (rpChat.IsShown)
            {
                AddRPMessage(text, color);
            }
            else
            {
                AddOOCMessage(text, color);
            }
        }

        public void NewMessageVisualUpdate(int type)
        {
            // Eine neue Nachricht ist eingetroffen, die noch nicht gelesen wurde. Dies soll dem Nutzer angezeigt werden.
            // Fenster öffnet sich bzw. Anzeichen das im anderen Chat eine Nachricht eingetroffen ist
            if (type == 1) // RP
            {
                if (!rpChat.IsShown && !oocChat.IsShown)
                {
                    // beide chats zu, rp chat anzeigen
                    chatVisible = true;
                    oocChatShown = false;
                    rpChat.Show();
                    KeepOpen = KeepOpenMaxTime;
                }
                else if (rpChat.IsShown && rpChat.Scrolled)
                {
                    rpChat.Highlight(true);
                }
                else if (oocChat.IsShown)
                {
                    oocChat.Highlight(true);
                }

            }
            else // OOC
            {
                if (!rpChat.IsShown && !oocChat.IsShown)
                {
                    // beide chats zu, ooc chat anzeigen
                    chatVisible = true;
                    oocChatShown = true;
                    oocChat.Show();
                    KeepOpen = KeepOpenMaxTime;
                }
                else if (oocChat.IsShown && oocChat.Scrolled)
                {
                    oocChat.Highlight(true);
                }
                else if (rpChat.IsShown)
                {
                    rpChat.Highlight(true);
                }
            }
        }

        public void ToggleChat()
        {
            if (oocChat.IsShown)
            {
                oocChatShown = false;
                oocChat.Hide();
                rpChat.Show();
            }
            else
            {
                oocChatShown = true;
                oocChat.Show();
                rpChat.Hide();
            }
        }

        public void SendMessage(string text)
        {
            zERROR.GetZErr(Program.Process).Report(2, 'G', "attempt sending " + text.ToString(), 0, "hGame.cs", 0);
            foreach (KeyValuePair<string, string> pair in CommandList)
            {
                if (text.StartsWith(pair.Value))
                {
                    text = pair.Key + text.Substring(pair.Value.Length);
                    ChatMessage.SendMessage(text);
                    return;
                }
            }

            // Command List
            if (text.StartsWith("/"))
            {
                if (lcShift < latestCommands.Count)
                {
                    // used one command of the list at lcShift
                    latestCommands.RemoveAt(lcShift);
                    latestCommands.Add(text);
                    lcShift = latestCommands.Count;
                }
                else
                {
                    // typed same command twice
                    for (int i = 0; i < latestCommands.Count; i++)
                    {
                        if (latestCommands[i] == text)
                        {
                            latestCommands.RemoveAt(i);
                            break;
                        }
                    }
                    latestCommands.Add(text);
                    lcShift = latestCommands.Count;
                }

                // prevent wrong commands
                if (!UsedCommandCorrectly(text))
                    return;
            }

            if (oocChatShown && !text.StartsWith("/"))
            {
                text = "/ooc " + text;
            }

            ChatMessage.SendMessage(text);
        }

        public override void Open()
        {
            textInput.Show();
            textInput.Enabled = true;
            chatVisible = true;
            if (oocChatShown)
            {
                oocChat.Show();
            }
            else
            {
                rpChat.Show();
            }
            base.Open();
        }

        public override void Close()
        {
            textInput.Hide();
            textInput.Enabled = false;
            KeepOpen = KeepOpenMaxTime;
            if (oocChatShown)
            {
                oocChat.Hide();
            }
            else
            {
                rpChat.Hide();
            }
            base.Close();
        }

        public override void KeyPressed(VirtualKeys key)
        {
            if (key == CloseMenuKey)
            {
                if (textInput.Input.Length > 0)
                {
                    inputBackup = textInput.Input;
                    replacedBackup = replaced;
                    textInput.Input = "";
                }
                Close();
            }
            else if (key == SendMessageKey)
            {
                if (textInput.Input.Length == 0)
                {
                    Close();
                    return;
                }
                SendMessage(textInput.Input);
                textInput.Input = "";
                replaced = "";
            }
            else if (key == ClearChatKey)
            {
                textInput.Input = "";
                replaced = "";
            }
            else if (key == ResetBackupKey)
            {
                textInput.Input = inputBackup;
                replaced = replacedBackup;
            }
            else if (key == VirtualKeys.Up)
            {
                if (rpChat.IsShown)
                {
                    rpChat.Scroll(-1);
                }
                else
                {
                    oocChat.Scroll(-1);
                }
            }
            else if (key == VirtualKeys.Down)
            {
                if (rpChat.IsShown)
                {
                    rpChat.Scroll(1);
                }
                else
                {
                    oocChat.Scroll(1);
                }
            }
            else if (key == VirtualKeys.Prior)
            {
                // bild auf
                if (lcShift == latestCommands.Count)
                    lcBackup = textInput.Input;
                if (lcShift > 0)
                {
                    lcShift--;
                    textInput.Input = latestCommands[lcShift];
                }
            }
            else if (key == VirtualKeys.Next)
            {
                // bild ab
                if (lcShift < latestCommands.Count - 1)
                {
                    lcShift++;
                    textInput.Input = latestCommands[lcShift];
                }
                else if (lcShift < latestCommands.Count)
                {
                    lcShift++;
                    textInput.Input = lcBackup;
                }
            }
            else if (key == ToggleChatKey)
            {
                ToggleChat();
            }

            if (replaced.Length > 0)
            {
                if (textInput.Input.Length <= replaced.Length)
                {
                    zERROR.GetZErr(Program.Process).Report(2, 'G', "delete", 0, "hGame.cs", 0);
                    textInput.Input = "";
                    replaced = "";
                }
            }
            else
            {
                foreach (KeyValuePair<string, string> pair in CommandList)
                {
                    if (textInput.Input.StartsWith(pair.Key))
                    {
                        textInput.Input = pair.Value;
                        replaced = pair.Value;
                    }
                }
            }
            textInput.KeyPressed(key);
        }

        long KeepOpen;
        long KeepOpenMaxTime = 300000;
        public override void Update(long now)
        {
            textInput.Update(now);

            /* Doesnt work yet -> timer stops when chat menu closed so chat cant be closed anymore

             if (!chatVisible && !textInput.Enabled)
                 return;

             if (KeepOpen > 0)
             {
                 KeepOpen--;
                 zERROR.GetZErr(Program.Process).Report(2, 'G', "Currently: "+KeepOpen.ToString(), 0, "hGame.cs", 0);
                 return;
             }

             if(oocChatShown)
             {
                 oocChat.Hide();
             }
             else
             {
                 rpChat.Hide();
             }
             chatVisible = false;

             zERROR.GetZErr(Program.Process).Report(2, 'G', "closing chat by time ending", 0, "hGame.cs", 0); */

        }

        // the ugliest function should be at the very bottom
        // parameter control hard coded
        public bool UsedCommandCorrectly(string text)
        {
            if (!Players.Contains(Client.WorldObjects.Player.Hero.Name))
                Players.Add(Client.WorldObjects.Player.Hero.Name);

            // do any stuff to prevent messaging wrong commands
            string[] parameters = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parameters[0] == "/tp")
            {
                if (parameters.Length == 2)
                {
                    if (!Players.Contains(parameters[1]))
                    {
                        AddToShown("[CLIENT] Spieler \"" + parameters[1] + "\" nicht gefunden", new ColorRGBA(255, 0, 0));
                        return false;
                    }
                }
                else if (parameters.Length == 3)
                {
                    if (!Players.Contains(parameters[1]))
                    {
                        AddToShown("[CLIENT] Spieler \"" + parameters[1] + "\" nicht gefunden", new ColorRGBA(255, 0, 0));
                        return false;
                    }
                    else if (!Players.Contains(parameters[2]))
                    {
                        AddToShown("[CLIENT] Spieler \"" + parameters[2] + "\" nicht gefunden", new ColorRGBA(255, 0, 0));
                        return false;
                    }
                }
                else if (parameters.Length == 5)
                {
                    if (!Players.Contains(parameters[1]))
                    {
                        AddToShown("[CLIENT] Spieler \"" + parameters[1] + "\" nicht gefunden", new ColorRGBA(255, 0, 0));
                        return false;
                    }
                    float X, Y, Z;
                    if (!float.TryParse(parameters[2], out X) || !float.TryParse(parameters[3], out Y) || !float.TryParse(parameters[4], out Z))
                    {
                        AddToShown("[CLIENT] X, Y, Z-Koordinaten wurden falsch angegeben.", new ColorRGBA(255, 0, 0));
                        return false;
                    }
                }
                else
                {
                    AddToShown("[CLIENT] Usage: /tp <player> [<target>] [<X> <Y> <Z>]", new ColorRGBA(255, 0, 0));
                }
            }

            return true;
        }
    }
}