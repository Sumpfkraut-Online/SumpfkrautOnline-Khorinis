using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using GUC.Types;
using WinApi;
using Gothic.zTypes;
using GUC.Enumeration;
using Gothic.mClasses;
using RakNet;
using GUC.WorldObjects.Character;
using GUC.timer;

namespace GUC.GUI
{
    public class Texture : View, InputReceiver
    {
        protected zCView view;

        protected Vec2i size = new Vec2i();
        protected Texture parent;

        protected GUIEvents enabledEvents;
        protected bool hover = false;
        protected Timer updateTimer;



        public Texture(int id, String tex, Vec2i position, Vec2i size, Texture parent, GUIEvents evts)
            : base(id, position)
        {
            this.enabledEvents = evts;
            this.parent = parent;
            this.size.set(size);
            

            Process process = Process.ThisProcess();
            view = zCView.Create(process, position.X, position.Y, position.X + this.size.X, position.Y+this.size.Y);

            if (tex != null && tex != "")
            {
                zString text = zString.Create(process, tex);
                view.InsertBack(text);
                text.Dispose();
            }

            updateTimer = new Timer(200);
            updateTimer.OnTick += timerTick;
        }

        public zCView getView()
        {
            return view;
        }

        public void setTexture(String tex)
        {
            if (tex == null || tex == "")
                return;
            Process process = Process.ThisProcess();
            zString text = zString.Create(process, tex);
            view.InsertBack(text);
            text.Dispose();
        }

        public void setSize(Vec2i size)
        {
            this.size.set(size.X, size.Y);
            view.SetSize(size.X, size.Y);
        }

        public override void setPosition(Vec2i pos)
        {
            this.position.set(pos);
            view.SetPos(this.position.X, this.position.Y);
            
        }

        public override void hide()
        {
            if (!isShown)
                return;
            Process process = Process.ThisProcess();

            if (parent == null)
                zCView.GetStartscreen(process).RemoveItem(this.view);
            else
                parent.getView().RemoveItem(this.view);

            if (this.enabledEvents.HasFlag(GUIEvents.LeftClicked) || this.enabledEvents.HasFlag(GUIEvents.RightClicked))
            {
                InputHooked.receivers.Remove(this);
            }
            if (this.enabledEvents.HasFlag(GUIEvents.Hover))
            {
                updateTimer.End();
            }

            //zCView.GetStartscreen(process).RemoveItem(this.view);
            isShown = false;

        }
        public override void show()
        {
            if (isShown)
                return;
            Process process = Process.ThisProcess();

            //zCView.GetStartscreen(process).InsertItem(this.view, 0);

            if (parent == null)
                zCView.GetStartscreen(process).InsertItem(this.view, 0);
            else
                parent.getView().InsertItem(this.view, 0);

            if (this.enabledEvents.HasFlag(GUIEvents.LeftClicked) || this.enabledEvents.HasFlag(GUIEvents.RightClicked))
            {
                InputHooked.receivers.Add(this);
            }
            if (this.enabledEvents.HasFlag(GUIEvents.Hover))
            {
                updateTimer.Start();
            }

            isShown = true;
        }


        public override void Destroy()
        {
            hide();
            this.view.Dispose();
        }


        public void timerTick()
        {
            if (this.enabledEvents.HasFlag(GUIEvents.Hover))
            {
                if (this.position.X < Gothic.mClasses.Cursor.CursorX() && this.position.X + this.size.X > Gothic.mClasses.Cursor.CursorX()
                    && this.position.Y < Gothic.mClasses.Cursor.CursorY() && this.position.Y + this.size.Y > Gothic.mClasses.Cursor.CursorY())
                {
                    if (!hover)
                    {
                        RakNet.BitStream stream = Program.client.sentBitStream;
                        stream.Reset();
                        stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                        stream.Write((byte)NetworkIDS.GuiMessage);
                        stream.Write((byte)GuiMessageType.GuiEvent);
                        stream.Write(Player.Hero.ID);
                        stream.Write(this.id);

                        stream.Write((int)GUIEvents.Hover);
                        stream.Write(true);

                        Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                        hover = true;
                    }
                }
                else
                {
                    if (hover)
                    {
                        RakNet.BitStream stream = Program.client.sentBitStream;
                        stream.Reset();
                        stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                        stream.Write((byte)NetworkIDS.GuiMessage);
                        stream.Write((byte)GuiMessageType.GuiEvent);
                        stream.Write(Player.Hero.ID);
                        stream.Write(this.id);

                        stream.Write((int)GUIEvents.Hover);
                        stream.Write(false);

                        Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                        hover = false;
                    }
                }
            }
        }

        public void KeyReleased(int key)
        {

        }

        public void KeyPressed(int key)
        {
            if (key == (int)VirtualKeys.LeftButton && this.enabledEvents.HasFlag(GUIEvents.LeftClicked)
                || key == (int)VirtualKeys.RightButton && this.enabledEvents.HasFlag(GUIEvents.RightClicked))
            {
                if (this.position.X < Gothic.mClasses.Cursor.CursorX() && this.position.X + this.size.X > Gothic.mClasses.Cursor.CursorX()
                    && this.position.Y < Gothic.mClasses.Cursor.CursorY() && this.position.Y + this.size.Y > Gothic.mClasses.Cursor.CursorY())
                {
                    RakNet.BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.GuiMessage);
                    stream.Write((byte)GuiMessageType.GuiEvent);
                    stream.Write(Player.Hero.ID);
                    stream.Write(this.id);

                    
                    int evt = (key == (int)VirtualKeys.LeftButton) ? (int)GUIEvents.LeftClicked : (int)GUIEvents.RightClicked;
                    stream.Write(evt);

                    Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                }
            }
        }

        public void wheelChanged(int steps)
        {

        }

    }
}
