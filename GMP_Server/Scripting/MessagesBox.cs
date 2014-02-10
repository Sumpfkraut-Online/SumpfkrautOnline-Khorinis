using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using Network;

namespace GMP_Server.Scripting
{
    public class MessagesBox : View
    {
        String font;
        int lines = 5;


        public MessagesBox(String font, int lines, int x, int y)
            : this(font, lines, x, y, null,  false, 0)
        { }
        public MessagesBox(String font, int lines, int x, int y, Texture parent, bool singleUser, int singleUserID)
            : base(x, y, singleUser, singleUserID, parent)
        {
            this.font = font;
            this.lines = lines;
            this.x = x;
            this.y = y;


            create(-1);
        }

        protected override void create(int to)
        {
            int id = 0;
            if (parent != null)
                id = parent.getID();

            if (isSingleUser)
                to = singleUserID;

            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, to, (byte)CommandoType.MessagesBoxCreate, new string[] { font }, new int[] { this.id, x, y, id, this.lines}, null);

            if (!isSingleUser && AllShown && to != -1)
            {
                show(new Player(Program.playerDict[to]));
            }
        }

        public void addLine(byte r, byte g, byte b, byte a, String message)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.MessagesBoxSetLine, new string[] { message }, new int[] { this.id, r, g, b, a }, null);
        }

        public void addLine(Player player, byte r, byte g, byte b, byte a, String message)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.getID(), (byte)CommandoType.MessagesBoxSetLine, new string[] { message }, new int[] { this.id, r, g, b, a }, null);
        }

    }
}
