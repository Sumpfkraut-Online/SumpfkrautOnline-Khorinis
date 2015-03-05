using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.GUI
{

    public class MessagesBox : View
    {
        protected String font = "";
        byte lines = 5;

        public MessagesBox(String font, byte lines, int x, int y)
            : this(font, lines, x, y, null, false, 0)
        { }

        public MessagesBox(String font, byte lines, int x, int y, Texture parent, bool singleUser, int singleUserID)
            : this(font, lines, new Vec2i(x, y), parent, singleUser, singleUserID)
        {  }

        public MessagesBox(String font, byte lines, Vec2i position, Texture parent, bool singleUser, int singleUserID)
            : base(position, singleUser, singleUserID, parent)
        {
            this.font = font;
            this.lines = lines;


            create(-1);
        }


        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateMessageBox);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(ParentID);
            stream.Write(font);
            stream.Write(lines);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC);
            }
        }

        public void addLine(int plID, ColorRGBA color, String message)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.MessageBoxAddLine);

            stream.Write(this.id);
            stream.Write(color);
            stream.Write(message);

            sendStream(plID, stream);
        }

        public void addLine(Player player, byte r, byte g, byte b, byte a, String message)
        {
            this.addLine(player.ID, new ColorRGBA(r, g, b, a), message);
        }

        public void addLine(byte r, byte g, byte b, byte a, String message)
        {
            this.addLine(0, new ColorRGBA(r, g, b, a), message);
        }

        public void addLine(Player player, ColorRGBA color, String message)
        {
            this.addLine(player.ID, color, message);
        }

        public void addLine(ColorRGBA color, String message)
        {
            this.addLine(0, color, message);
        }

    }
}
