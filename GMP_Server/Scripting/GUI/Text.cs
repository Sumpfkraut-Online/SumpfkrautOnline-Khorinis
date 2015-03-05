using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Types;
using GUC.Enumeration;
using RakNet;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.GUI
{
    public class Text : View
    {
        protected Dictionary<int, ColorRGBA> colorDict = new Dictionary<int, ColorRGBA>();
        protected String text = null;

        protected String font = null;

        //For use with TextBox
        internal Text(String tex, String font, Vec2i position, Texture parent, bool singleUser, int singleUserID, bool useCreate)
            : base(position, singleUser, singleUserID, parent)
        {
            colorDict.Add(0, ColorRGBA.White);

            this.text = tex;
            this.font = font;

            if(useCreate)
                create(-1);
        }

        public Text(String tex, String font, Vec2i position, Texture parent, bool singleUser, int singleUserID)
            : this(tex, font, position, parent, singleUser, singleUserID, true)
        { }

        public Text(String tex, String font, int x, int y, Texture parent, bool singleUser, int singleUserID)
            : this(tex, font, new Vec2i(x, y), parent, singleUser, singleUserID)
        { }

        public Text(String tex, int x, int y)
            : this(tex, "FONT_DEFAULT.TGA", x, y, null, false, 0)
        { }

        public Text(String tex, Vec2i position)
            : this(tex, "FONT_DEFAULT.TGA", position, null, false, 0)
        { }

        public Text(String text)
            : this(text, "FONT_DEFAULT.TGA", 0, 0, null, false, 0)
        { }

        public Text(String tex, String font, int x, int y)
            : this(tex, font, x, y, null, false, 0)
        { }

        public Text(String tex, String font, Vec2i position)
            : this(tex, font, position, null, false, 0)
        { }

        public Text(String tex, String font, int x, int y, Texture parent)
            : this(tex, font, x, y, parent, false, 0)
        { }

        public Text(String tex, String font, Vec2i position, Texture parent)
            : this(tex, font, position, parent, false, 0)
        { }

        protected override void create(int to)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateText);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(this.text);
            stream.Write(this.font);
            stream.Write(this.getColor(to));
            stream.Write(ParentID);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC);
            }
        }

        public ColorRGBA getColor()
        {
            return colorDict[0];
        }

        public ColorRGBA getColor(int playerID)
        {
            if (playerID <= 0)
                return colorDict[0];
            if (colorDict.ContainsKey(playerID))
                return colorDict[playerID];
            return colorDict[0];
        }

        public void setText(String text)
        {
            this.text = text;


            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.SetText);

            stream.Write(this.id);
            stream.Write(this.text);

            sendStream(0, stream);
        }

        public void setColor(int playerID, byte r, byte g, byte b, byte a)
        {
            if(playerID < 0)
                playerID = 0;

            if(!this.colorDict.ContainsKey(playerID))
                this.colorDict.Add(playerID, new ColorRGBA(r, g, b, a));
            else
                this.colorDict[playerID].set(r, g, b, a);

            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.SetTextColor);

            stream.Write(this.id);
            stream.Write(this.getColor(playerID));

            sendStream(playerID, stream);
        }

        public void setColor(byte r, byte g, byte b, byte a)
        {
            this.setColor(0, r, g, b, a);
        }

        public void setColor(Player pl, byte r, byte g, byte b, byte a)
        {
            setColor(pl.ID, r, g, b, a);
        }

        public virtual void setFont(String font)
        {
            if (font == null)
                throw new ArgumentException("Font can't be null!");
            this.font = font;


            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.SetTextFont);

            stream.Write(this.id);
            stream.Write(this.font);

            sendStream(0, stream);
        }
    }
}
