using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using Network;

namespace GMP_Server.Scripting
{
    public class Texture : View
    {
        String tex = null;
        int width = 0;
        int height = 0;

        public Texture(String tex)
            : this(tex, 0, 0, 0x2000, 0x2000)
        {}

        public Texture(String tex, int x, int y, int width, int height)
            : this(tex, x, y, width, height,null, false, 0)
        {}

        public Texture(String tex, int x, int y, int width, int height, Texture parent)
            : this(tex, x, y, width, height, parent, false, 0)
        { }

        public Texture(String tex, int x, int y, int width, int height, Texture parent, bool isSingleUser, int singleUserID)
            : base(x, y, isSingleUser, singleUserID, parent)
        {
            this.tex = tex;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            

            create(-1);
            
        }

        protected override void create(int to)
        {
            if (isSingleUser)
                to = singleUserID;
            int id = 0;
            if (parent != null)
                id = parent.getID();

            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, to, (byte)CommandoType.TextureCreate, new string[] { tex }, new int[] { this.id, x, y, width, height, id }, null);

            if (!isSingleUser && AllShown && to != -1)
            {
                show(new Player(Program.playerDict[to]));
            }
        }

        public void setTexture(String tex)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextureTex, new string[] { tex }, new int[] { this.id }, null);
        }

        public void setSize(int x, int y)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, getSendToID(), (byte)CommandoType.TextureSize, null, new int[] { this.id, x, y }, null);
        }

    }
}
