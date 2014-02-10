using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using Network;

namespace GMP_Server.Scripting
{
    public class Text : View
    {
        String text = null;

        int colorR = 255;
        int colorG = 255;
        int colorB = 255;
        int colorA = 255;

        String font = null;

        public Text(String tex, String font, int x, int y, Texture parent, bool singleUser, int singleUserID)
            : base(x, y, singleUser, singleUserID, parent)
        {
            this.text = tex;
            this.font = font;
            this.x = x;
            this.y = y;


            create(-1);
        }

        public Text(String tex, int x, int y)
            : this(tex, "Font_Old_20_White.TGA", x, y, null, false, 0)
        {}

        public Text(String text)
            : this(text, "Font_Old_20_White.TGA", 0, 0, null, false, 0)
        { }

        public Text(String tex, String font, int x, int y)
            : this(tex,font, x, y, null, false, 0)
        {
        }

        public Text(String tex, String font, int x, int y, Texture parent)
            : this(tex, font, x, y, parent, false, 0)
        {}

        protected override void create(int to)
        {
            int id = 0;
            if (parent != null)
                id = parent.getID();

            if (isSingleUser)
                to = singleUserID;

            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, to, (byte)CommandoType.TextCreate, new string[] { text, font }, new int[] { this.id, x, y, id, colorR, colorG, colorB, colorA }, null);

            if (!isSingleUser && AllShown && to != -1)
            {
                show(new Player(Program.playerDict[to]));
            }
        }

        public void setText(String text)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextSet, new string[] { text }, new int[] { this.id }, null);
        }

        public void setColor(int r, int g, int b, int a)
        {
            colorR = r;
            colorG = g;
            colorB = b;
            colorA = a;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextSetColor, null, new int[] { this.id, r, g, b, a }, null);
        }

        public void setColor(Player pl, int r, int g, int b, int a)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.TextSetColor, null, new int[] { this.id, r, g, b, a }, null);
        }

        public void setFont(String font)
        {
            this.font = font;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextSetFont, new string[] { font }, new int[] { this.id }, null);
        }

    }
}
