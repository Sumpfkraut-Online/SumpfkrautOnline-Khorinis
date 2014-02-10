using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;
using Gothic.mClasses;
using Injection;
using RakNet;

namespace GUC.GUI
{
    public class TextBox : View
    {

        zCView thisView;
        textBox tB;

        Texture parent;
        String font;

        int colorR = 255;
        int colorG = 255;
        int colorB = 255;
        int colorA = 255;

        public TextBox(int id, String text, String font, int x, int y, Texture parent, int r, int g, int b, int a, int resetKey, int startKey, int sendKey)
            : base(id)
        {
            this.x = x;
            this.y = y;
            this.parent = parent;
            this.font = font;

            //Creation:
            Process process = Process.ThisProcess();
            
            thisView = zCView.Create(Process.ThisProcess(), 0, 0, 0x2000, 0x2000);
            setFont(font);
            tB = new textBox(thisView, process);
            tB.setText(text);
            tB.resetKey = resetKey;
            tB.startWritingKey = startKey;
            tB.sendKey = sendKey;

            tB.vt.PosX = x;
            tB.vt.PosY = y;
            tB.SendInput += new EventHandler<EventArgs>(tbSended);


            tB.Inputenabled = false;


            setColor(r,g,b,a);
        }

        private void tbSended(object obj, EventArgs args)
        {
            RakNet.BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.TextBoxSendMessage);

            stream.Write(Program.Player.id);
            stream.Write(this.id);

            stream.Write(tB.getText());

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            tB.setText("");
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
            tB.vt = thisView.CreateText(x, y, str);
            str.Dispose();

            tB.vt.Timed = 0;
            tB.vt.Timer = -1;
        }

        public void setColor(int r, int g, int b, int a)
        {
            colorR = r;
            colorG = g;
            colorB = b;
            colorA = a;


            if (tB == null)
                return;

            tB.vt.Color.R = (byte)this.colorR;
            tB.vt.Color.G = (byte)this.colorG;
            tB.vt.Color.B = (byte)this.colorB;
            tB.vt.Color.A = (byte)this.colorA;
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
            zString str = zString.Create(process, this.font);
            thisView.SetFont(str);
            str.Dispose();

            if (tB != null && tB.vt != null)
            {
                tB.vt.Timed = 1;
                tB.vt.Timer = 0;
                createText();
                setColor(colorR, colorG, colorB, colorA);
            }
        }
        
        public override void setPosition(int x, int y)
        {
            this.x = x;
            this.y = y;

            tB.vt.PosX = x;
            tB.vt.PosY = y;
        }

        public override void hide()
        {
            if (!isShown)
                return;
            Process process = Process.ThisProcess();

            tB.Inputenabled = false;

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
