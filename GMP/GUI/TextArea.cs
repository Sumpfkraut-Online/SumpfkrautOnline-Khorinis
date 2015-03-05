using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;
using GUC.Types;
using Gothic.mClasses;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Character;

namespace GUC.GUI
{
    public class TextArea : View
    {

        zCView thisView;
        textArea tA;

        Texture parent;
        String font = "";
        ColorRGBA color = ColorRGBA.White;

        public TextArea(int id, String text, String font, Vec2i position, Vec2i size, Texture parent, ColorRGBA color, int resetKey, int startKey, int sendKey)
            : base(id, position)
        {
            
            this.parent = parent;
            this.font = font;
            //Creation:
            Process process = Process.ThisProcess();

            thisView = zCView.Create(Process.ThisProcess(), position.X, position.Y, position.X+size.X, position.Y+size.Y);
            thisView.SetFont(font);

            tA = new textArea(process, thisView, size.Y);
            tA.setText(text);
            tA.resetKey = resetKey;
            tA.startWritingKey = startKey;
            tA.sendKey = sendKey;

            tA.SendInput += new EventHandler<EventArgs>(tbSended);


            tA.Inputenabled = false;


            setColor(color);
        }

        private void tbSended(object obj, EventArgs args)
        {
            RakNet.BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.TextBoxCallSend);

            stream.Write(Player.Hero.ID);
            stream.Write(this.id);

            stream.Write(tA.getText());

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            tA.setText("");
            this.tA.KeyDisable();
        }

        public void startWriting()
        {
            this.tA.KeyEnable();
        }

        public void stopWriting()
        {
            this.tA.KeyDisable();
        }

        public void callSendText()
        {
            tbSended(this, new EventArgs());

        }


        public void setResetKey(int resetKey)
        {
            tA.resetKey = resetKey;
        }

        public void setSendKey(int sendKey)
        {
            tA.sendKey = sendKey;
        }

        public void setEnableKey(int enableKey)
        {
            tA.startWritingKey = enableKey;
        }

        public void setColor(ColorRGBA color)
        {
            this.color.set(color);


            if (tA == null)
                return;

            foreach (zCViewText vt in tA.vt)
            {
                vt.Color.R = (byte)this.color.R;
                vt.Color.G = (byte)this.color.G;
                vt.Color.B = (byte)this.color.B;
                vt.Color.A = (byte)this.color.A;
            }
        }

        public void setText(String tex)
        {
            if (tA.vt == null)
                return;
            tA.setText(tex);
        }

        public override void setPosition(Vec2i pos)
        {
            this.position.set(pos);

            thisView.SetPos(pos.X, pos.Y);
        }

        public override void hide()
        {
            if (!isShown)
                return;
            Process process = Process.ThisProcess();

            tA.Inputenabled = false;
            tA.KeyDisable();

            if (parent == null)
                zCView.GetStartscreen(process).RemoveItem(this.thisView);
            else
                parent.getView().RemoveItem(this.thisView);

            isShown = false;

        }
        public override void show()
        {
            if (isShown)
                return;
            Process process = Process.ThisProcess();

            tA.Inputenabled = true;

            if (parent == null)
                zCView.GetStartscreen(process).InsertItem(this.thisView, 0);
            else
                parent.getView().InsertItem(this.thisView, 0);

            isShown = true;
        }


        public override void Destroy()
        {
            hide();

            thisView.Dispose();
            tA.Delete();
        }

    }
}
