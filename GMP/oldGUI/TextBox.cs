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
    public class TextBox : View
    {

        zCView thisView;
        textBox tB;

        Texture parent;
        String font = "";

        ColorRGBA color = ColorRGBA.White;

        public TextBox(int id, String text, String font, Vec2i position, Texture parent, ColorRGBA color, int resetKey, int startKey, int sendKey)
            : base(id, position)
        {
            
            this.parent = parent;

            //Creation:
            Process process = Process.ThisProcess();

            thisView = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            setFont(font);

            tB = new textBox(thisView, process);
            tB.setText(text);
            tB.resetKey = resetKey;
            tB.startWritingKey = startKey;
            tB.sendKey = sendKey;

            tB.vt.PosX = this.position.X;
            tB.vt.PosY = this.position.Y;
            tB.SendInput += new EventHandler<EventArgs>(tbSended);


            tB.Inputenabled = false;


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

            stream.Write(tB.getText());

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            tB.setText("");
            this.tB.KeyDisable();
        }

        public void startWriting()
        {
            this.tB.KeyEnable();
        }

        public void stopWriting()
        {
            this.tB.KeyDisable();
        }

        public void callSendText()
        {
            tbSended(this, new EventArgs());

        }


        public void setResetKey(int resetKey)
        {
            tB.resetKey = resetKey;
        }

        public void setSendKey(int sendKey)
        {
            tB.sendKey = sendKey;
        }

        public void setEnableKey(int enableKey)
        {
            tB.startWritingKey = enableKey;
        }

        private void createText()
        {
            String text = tB.getText();
            Process process = Process.ThisProcess();
            zString str = zString.Create(process, text);
            tB.vt = thisView.CreateText(position.X, position.Y, str);
            
            str.Dispose();

            tB.vt.Timed = 0;
            tB.vt.Timer = -1;
        }

        public void setColor(ColorRGBA color)
        {
            this.color.set(color);


            if (tB == null)
                return;

            tB.vt.Color.R = (byte)this.color.R;
            tB.vt.Color.G = (byte)this.color.G;
            tB.vt.Color.B = (byte)this.color.B;
            tB.vt.Color.A = (byte)this.color.A;
        }

        public void setText(String tex)
        {
            if (tB.vt == null)
                return;
            tB.setText(tex);
        }

        public void setFont(String font)
        {
            if (font == null)
                return;
            String oldfont = this.font;
            this.font = font;

            if (oldfont.Trim().ToUpper() == font.Trim().ToUpper())
                return;




            Process process = Process.ThisProcess();
            zString str = zString.Create(process, this.font.Trim().ToUpper());
            thisView.SetFont(str);
            str.Dispose();

            if (tB != null && tB.vt != null)
            {
                tB.vt.Timed = 1;
                tB.vt.Timer = 0;
                createText();
                setColor(this.color);
            }
        }

        public override void setPosition(Vec2i pos)
        {
            this.position.set(pos);

            tB.vt.PosX = position.X;
            tB.vt.PosY = position.Y;
        }

        public override void hide()
        {
            if (!isShown)
                return;
            Process process = Process.ThisProcess();

            tB.Inputenabled = false;
            tB.KeyDisable();

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

            tB.Inputenabled = true;

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
            tB.Delete();
        }

    }
}
