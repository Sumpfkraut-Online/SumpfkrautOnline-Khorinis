using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using Gothic.mClasses;
using WinApi.User.Enumeration;
using Gothic.zTypes;
using WinApi;
using GUC.Enumeration;
using RakNet;
using GUC.WorldObjects.Character;

namespace GUC.GUI
{
    public class Button : Texture, InputReceiver
    {
        protected String text;
        public Button(int id, String text, String tex, Vec2i position, Vec2i size)
            : base(id, tex, position, size)
        {
            this.text = text;

            if (this.text != null && this.text.Trim().Length != 0)
            {
                zString str = zString.Create(Process.ThisProcess(), this.text);
                zColor color = zColor.Create(Process.ThisProcess(), 255, 255, 255, 255);
                view.PrintTimedCXY(str, -1, color);
                str.Dispose();
                color.Dispose();
                
            }
        }

        public override void show()
        {
            base.show();
            InputHooked.receivers.Add(this);

            
        }

        public override void hide()
        {
            base.hide();
            InputHooked.receivers.Remove(this);
        }





        public void KeyReleased(int key)
        {
            
        }

        public void KeyPressed(int key)
        {
            if (key != (int)VirtualKeys.LeftButton)
                return;

            if (this.position.X < Gothic.mClasses.Cursor.CursorX() && this.position.X + this.size.X > Gothic.mClasses.Cursor.CursorX()
                && this.position.Y < Gothic.mClasses.Cursor.CursorY() && this.position.Y + this.size.Y > Gothic.mClasses.Cursor.CursorY())
            {
                RakNet.BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.GuiMessage);
                stream.Write((byte)GuiMessageType.ButtonPressed);

                stream.Write(Player.Hero.ID);
                stream.Write(this.id);

                Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            }
        }

        public void wheelChanged(int steps)
        {
            
        }
    }
}
