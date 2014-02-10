using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GMP.Net.Messages;
using Injection;
using GMP;
using RakNet;
using GMP.Modules;
using Gothic.zTypes;
using Network;
using GMP.Helper;

namespace Gothic.mClasses
{
    public class Chatbox : InputReceiver
    {
        listBox lb;
        public textBox tb;
        zCView view;

        byte actualType = 0;

        public byte maxType = 4;
        List<listBox.Row> rowsAll = new List<listBox.Row>();
        List<listBox.Row> rowsPrivate = new List<listBox.Row>();
        List<listBox.Row> rowsDistance = new List<listBox.Row>();
        List<listBox.Row> rowsFriends = new List<listBox.Row>();
        List<listBox.Row> rowsGuilds = new List<listBox.Row>();

        zCViewText viewChatType = null;

        public String lastWhispered = "";

        public void delete()
        {
            if (viewChatType != null)
            {
                viewChatType.Timed = 1;
                viewChatType.Timer = 0;
            }

            if (lb != null)
                lb.delete();
            if (view != null)
            {
                zCView.GetStartscreen(Process.ThisProcess()).RemoveItem(view);
                view.Dispose();
            }

            if (tb != null)
                tb.Delete();
            InputHooked.receivers.Remove(this);
        }


        private bool isShown = true;
        public void hide()
        {
            if (!isShown)
                return;
            InputHooked.receivers.Remove(this);
            InputHooked.receivers.Remove(tb);
            zCView.GetStartscreen(Process.ThisProcess()).RemoveItem(view);

            isShown = false;
        }

        public void show()
        {
            if (isShown)
                return;
            InputHooked.receivers.Add(this);
            InputHooked.receivers.Add(tb);
            zCView.GetStartscreen(Process.ThisProcess()).InsertItem(view, 0);

            isShown = true;
        }

        public Chatbox(Process process)
        {
            view = zCView.Create(process, 0, 0, 0x2000, 7000);
            zCView.GetStartscreen(process).InsertItem(view, 0);

            isShown = true;


            lb = new listBox(process, 10, view);
            
            tb = new textBox(view, process);
            tb.vt.PosY = lb.getBottom();

            Inputenabled = true;
            tb.SendInput += new EventHandler<EventArgs>(add);
            InputHooked.receivers.Add(this);


            //Chattype anzeige
            zString strType = zString.Create(process, "Normal-Chat");
            viewChatType = view.CreateText(0, 0, strType);
            strType.Dispose();

            change((byte)255);
        }

        public void addRow(byte type, String str)
        {
            if (type == actualType)
                lb.addRow(str);
            else if (type == 0)
                rowsAll.Add(new listBox.Row(str));
            else if (type == 1)
                rowsPrivate.Add(new listBox.Row(str));
            else if (type == 2)
                rowsDistance.Add(new listBox.Row(str));
            else if (type == 3)
                rowsFriends.Add(new listBox.Row(str));
            else if (type == 4)
                rowsGuilds.Add(new listBox.Row(str));
        }

        public void add(object obj, EventArgs args)
        {
            if (tb.getText().Trim() == "")
                return;

            if (StaticVars.serverConfig.chatOptions.BlockOptions.blockInWater && NPCHelper.isInWater(Program.Player)
                || StaticVars.serverConfig.chatOptions.BlockOptions.blockWhenDead && NPCHelper.isDead(Program.Player)
                || StaticVars.serverConfig.chatOptions.BlockOptions.blockWhenSleep && NPCHelper.isInMagicSleep(Program.Player)
                || StaticVars.serverConfig.chatOptions.BlockOptions.blockWhenUnconscious && NPCHelper.isUnconscious(Program.Player))
            {
                tb.setText("");
                return;
            }
            if (actualType == 1)
            {
                if (tb.getText().StartsWith("/"))
                {
                    foreach (Player pl in StaticVars.playerlist)
                    {
                        if (tb.getText().StartsWith("/" + pl.name + " "))
                        {
                            lastWhispered = pl.name;
                            break;
                        }
                    }
                }
                else
                {
                    tb.getText().Insert(0, "/" + lastWhispered);
                }
            }

            if(Program.client != null && GMP.Modules.StaticVars.Ingame && Program.client.isConnected)
                new ChatMessage(this).Write(Program.client.sentBitStream, Program.client,actualType, tb.getText());
            //lb.addRow(tb.getText());
            tb.setText("");
        }


        public void KeyReleased(int key)
        {
            if (!Inputenabled)
                return;

            if (key == Program.clientOptions.keyToogleChat)
            {
                InputHooked.activateFullControl(Process.ThisProcess());
            }
        }
        public void wheelChanged(int steps) { }

        public bool Inputenabled { get; set; }
        public void KeyPressed(int key)
        {
            if (!Inputenabled)
                return;

            if (key == Program.clientOptions.keyToogleChat)
            {
                InputHooked.deaktivateFullControl(Process.ThisProcess());
                byte id = (byte)(actualType + 1);
                change(id);
            }
        }


        public void change(byte aT)
        {
            if (aT > 4)
            {
                change(0);
                return;
            }
            if (aT < 0)
            {
                change(4);
                return;
            }

            if (aT == 0 && !StaticVars.serverConfig.chatOptions.All)
            {
                change((byte)(aT+1));
                return;
            }
            if (aT == 1 && !StaticVars.serverConfig.chatOptions.Private)
            {
                change((byte)(aT + 1));
                return;
            }
            if (aT == 2 && !StaticVars.serverConfig.chatOptions.Distance)
            {
                change((byte)(aT + 1));
                return;
            }
            if (aT == 3 && !StaticVars.serverConfig.chatOptions.Friends)
            {
                change((byte)(aT + 1));
                return;
            }
            if (aT == 4 && !StaticVars.serverConfig.chatOptions.Guild)
            {
                change((byte)(aT + 1));
                return;
            }

            if (actualType == 0)
            {
                rowsAll = lb.rows;
            }
            else if (actualType == 1)
            {
                rowsPrivate = lb.rows;
            }
            else if (actualType == 2)
            {
                rowsDistance = lb.rows;
            }
            else if (actualType == 3)
            {
                rowsFriends = lb.rows;
            }
            else if (actualType == 4)
            {
                rowsGuilds = lb.rows;
            }

            if (aT == 0)
            {
                viewChatType.Text.Set("Normal-Chat");
                lb.rows = rowsAll;
            }
            else if (aT == 1)
            {
                viewChatType.Text.Set("Fluster-Chat mit:" + lastWhispered);
                lb.rows = rowsPrivate;
            }
            else if (aT == 2)
            {
                viewChatType.Text.Set("Umgebungs-Chat");
                lb.rows = rowsDistance;
            }
            else if (aT == 3)
            {
                viewChatType.Text.Set("Party-Chat");
                lb.rows = rowsFriends;
            }
            else if (aT == 4)
            {
                viewChatType.Text.Set("Gilden-Chat");
                lb.rows = rowsGuilds;
            }


            lb.UpdateRows();
            actualType = aT ;
        }
    }
}
