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
    public class Cursor : Texture
    {
        private static Cursor cursor = null;
        public static Cursor getCursor()
        {
            if (cursor == null)
                cursor = new Cursor();

            return cursor;
        }


        public Cursor()
            : this("Cursor.tga")
        { }

        public Cursor(Vec2i pos)
            : this(pos.X, pos.Y)
        { }

        public Cursor(int x, int y)
            : this("Cursor.tga", x, y, 150, 200)
        { }

        public Cursor(String tex)
            : this(tex, 0, 0, 150, 200)
        { }

        public Cursor(String tex, int x, int y, int width, int height)
            : this(tex, x, y, width, height, null, false, 0)
        { }

        public Cursor(String tex, int x, int y, int width, int height, Texture parent)
            : this(tex, x, y, width, height, parent, false, 0)
        { }

        public Cursor(String tex, int x, int y, int width, int height, Texture parent, bool isSingleUser, int singleUserID)
            : this(tex, new Vec2i(x, y), new Vec2i(width, height), parent, isSingleUser, singleUserID)
        { }

        public Cursor(String tex, Vec2i pos, Vec2i size, Texture parent, bool isSingleUser, int singleUserID)
            : base(tex, pos, size, parent, isSingleUser, singleUserID, GUIEvents.None, false)
        {
            create(-1);
        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateCursor);

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
    }
}
