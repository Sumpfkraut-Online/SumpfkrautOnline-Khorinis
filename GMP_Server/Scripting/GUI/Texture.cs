using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects;

namespace GUC.Server.Scripting.GUI
{
    public class Texture : View
    {
        protected String tex = null;
        protected Vec2i size = new Vec2i();

        public Texture(String tex)
            : this(tex, 0, 0, 0x2000, 0x2000)
        { }

        public Texture(String tex, int x, int y, int width, int height)
            : this(tex, x, y, width, height, null, false, 0)
        { }

        public Texture(String tex, int x, int y, int width, int height, Texture parent)
            : this(tex, x, y, width, height, parent, false, 0)
        { }

        public Texture(String tex, int x, int y, int width, int height, Texture parent, bool isSingleUser, int singleUserID)
            : this(tex, new Vec2i(x, y), new Vec2i(width, height), parent, isSingleUser, singleUserID)
        { }

        public Texture(String tex, Vec2i pos, Vec2i size, Texture parent, bool isSingleUser, int singleUserID)
            : this(tex, pos, size, parent, isSingleUser, singleUserID, true)
        { }

        protected Texture(String tex, Vec2i pos, Vec2i size, Texture parent, bool isSingleUser, int singleUserID, bool useCreate)
            : base(pos, isSingleUser, singleUserID, parent)
        {
            if (tex == null)
                tex = "";
            this.tex = tex;
            this.size.set(size);

            if(useCreate)
                create(-1);

        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateTexture);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(this.size);
            stream.Write(tex);
            stream.Write(ParentID);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show( (GUC.Server.Scripting.Objects.Character.Player) ((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC );
            }
        }

        public void setTexture(String tex)
        {
            if(tex == null)
                tex = "";
            this.tex = tex;

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.SetTexture);

            stream.Write(this.ID);
            stream.Write(this.tex);

            sendStream(0, stream);
        }

        public void setSize(int x, int y)
        {
            this.setSize(new Vec2i(x, y));
        }

        public void setSize(Vec2i size)
        {
            if (size == null)
                throw new ArgumentNullException("The size can't be null!");
            this.size.set(size);
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.SetSize);

            stream.Write(this.ID);
            stream.Write(this.size);

            sendStream(0, stream);
        }
    }
}
