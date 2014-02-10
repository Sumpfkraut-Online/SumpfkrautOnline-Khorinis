using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using Network;

namespace GMP_Server.Scripting
{
    public class TextBox : View
    {
        String text = null;

        int colorR = 255;
        int colorG = 255;
        int colorB = 255;
        int colorA = 255;

        String font = null;

        int sendButton = 0;
        int startWritingButton = 0;
        int resetButton = 0;

        private List<Listener.ITextBoxListener> textBoxListener = new List<Listener.ITextBoxListener>();

        public TextBox(String tex, String font, int x, int y)
            : this(tex, font, x, y, 0, 0, 0, null, false, 0)
        { }

        public TextBox(String font, int x, int y)
            : this("", font, x, y, 0, 0, 0, null, false, 0)
        { }

        public TextBox(String font, int x, int y, int sendButton, int startWritingButton, int resetButton)
            : this("", font, x, y, sendButton, startWritingButton, resetButton, null, false, 0)
        { }

        public TextBox(int x, int y, int sendButton, int startWritingButton, int resetButton)
            : this("", "Font_Old_20_White.TGA", x, y, sendButton, startWritingButton, resetButton, null, false, 0)
        { }

        public TextBox(String tex, String font, int x, int y, int sendButton, int startWritingButton, int resetButton)
            : this(tex, font, x, y, sendButton, startWritingButton, resetButton, null, false, 0)
        { }

        public TextBox(String tex, String font, int x, int y, int sendButton, int startWritingButton, int resetButton, Texture parent)
            : this(tex, font, x, y, sendButton, startWritingButton, resetButton, parent, false, 0)
        { }

        public TextBox(String tex, String font, int x, int y, int sendButton, int startWritingButton, int resetButton, Texture parent, bool singleUser, int singleUserID)
            : base(x, y, singleUser, singleUserID, parent)
        {
            this.text = tex;
            this.font = font;
            this.x = x;
            this.y = y;

            this.sendButton = sendButton;
            this.startWritingButton = startWritingButton;
            this.resetButton = resetButton;


            create(-1);
        }




        public void addTextBoxListener(Listener.ITextBoxListener kl)
        {
            textBoxListener.Add(kl);
        }

        public void OnTextBoxMessageReceived(TextBox tb, Player pl, String message)
        {
            Listener.ITextBoxListener[] list = textBoxListener.ToArray();
            foreach (Listener.ITextBoxListener listern in list)
            {
                listern.OnMessageReceived(tb, pl, message);
            }
        }

        protected override void create(int to)
        {
            int id = 0;
            if (parent != null)
                id = parent.getID();

            if (isSingleUser)
                to = singleUserID;

            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, to, (byte)CommandoType.TextBoxCreate, new string[] { text, font }, new int[] { this.id, x, y, id, colorR, colorG, colorB, colorA, startWritingButton, resetButton, sendButton }, null);

            if (!isSingleUser && AllShown && to != -1)
            {
                show(new Player(Program.playerDict[to]));
            }
        }

        public void StartWriting(Player pl)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.TextBoxStartWirting, null, new int[] { this.id, 1 }, null);
        }
        public void StopWriting(Player pl)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.TextBoxStartWirting, null, new int[] { this.id, 0 }, null);
        }
        public void CallSendText(Player pl)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.TextBoxCallSend, null, new int[] { this.id }, null);
        }

        public void setStartWritingKey(int key)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextBoxStartKey, null, new int[] { this.id, key }, null);
        }

        public void setResetKey(int key)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextBoxResetKey, null, new int[] { this.id, key }, null);
        }

        public void setSendKey(int key)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextBoxSendKey, null, new int[] { this.id, key }, null);
        }

        public void setText(String text, Player pl)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.TextBoxSet, new string[] { text }, new int[] { this.id }, null);
        }

        public void setText(String text)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextBoxSet, new string[] { text }, new int[] { this.id }, null);
        }

        public void setColor(int r, int g, int b, int a)
        {
            colorR = r;
            colorG = g;
            colorB = b;
            colorA = a;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextBoxColorSet, null, new int[] { this.id, r, g, b, a }, null);
        }

        public void setColor(Player pl, int r, int g, int b, int a)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.TextBoxColorSet, null, new int[] { this.id, r, g, b, a }, null);
        }

        public void setFont(String font)
        {
            this.font = font;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextBoxFontSet, new string[] { font }, new int[] { this.id }, null);
        }

    }
}
