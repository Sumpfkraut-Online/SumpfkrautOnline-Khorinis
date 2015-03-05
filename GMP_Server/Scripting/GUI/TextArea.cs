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
    public class TextArea : Text
    {
        Vec2i size = new Vec2i(0x2000 ,0x2000);
        int sendButton = 0;
        int startWritingButton = 0;
        int resetButton = 0;

        public TextArea(String tex, int x, int y)
            : this(tex, "FONT_DEFAULT.TGA", x, y, 0x2000, 0x2000, 0, 0, 0, null, false, 0)
        { }

        public TextArea(int x, int y, int width, int height)
            : this("", "FONT_DEFAULT.TGA", x, y, width, height, 0, 0, 0, null, false, 0)
        { }

        public TextArea(String tex, String font, int x, int y)
            : this(tex, font, x, y, 0x2000, 0x2000, 0, 0, 0, null, false, 0)
        { }

        public TextArea(String tex, String font, int x, int y, int width, int height)
            : this(tex, font, x, y, width, height, 0, 0, 0, null, false, 0)
        { }

        public TextArea(String font, int x, int y, int width, int height)
            : this("", font, x, y, width, height, 0, 0, 0, null, false, 0)
        { }

        public TextArea(String font, int x, int y, int width, int height, int sendButton, int startWritingButton, int resetButton)
            : this("", font, x, y, width, height, sendButton, startWritingButton, resetButton, null, false, 0)
        { }

        public TextArea(int x, int y, int width, int height, int sendButton, int startWritingButton, int resetButton)
            : this("", "FONT_DEFAULT.TGA", x, y, width, height, sendButton, startWritingButton, resetButton, null, false, 0)
        { }

        public TextArea(String tex, String font, int x, int y, int width, int height, int sendButton, int startWritingButton, int resetButton)
            : this(tex, font, x, y, width, height, sendButton, startWritingButton, resetButton, null, false, 0)
        { }

        public TextArea(String tex, String font, int x, int y, int width, int height, int sendButton, int startWritingButton, int resetButton, Texture parent)
            : this(tex, font, x, y, width, height, sendButton, startWritingButton, resetButton, parent, false, 0)
        { }

        public TextArea(String tex, String font, int x, int y, int width, int height, int sendButton, int startWritingButton, int resetButton, Texture parent, bool singleUser, int singleUserID)
            : this(tex, font, new Vec2i(x, y), new Vec2i(width, height), sendButton, startWritingButton, resetButton, parent, singleUser, singleUserID)
        { }

        public TextArea(String tex, String font, Vec2i position, Vec2i size, int sendButton, int startWritingButton, int resetButton, Texture parent, bool singleUser, int singleUserID)
            : base(tex, font ,position, parent, singleUser, singleUserID, false)
        {
            this.sendButton = sendButton;
            this.startWritingButton = startWritingButton;
            this.resetButton = resetButton;

            this.size = size;

            create(-1);
        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateTextArea);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(this.size);
            stream.Write(this.text);
            stream.Write(this.font);
            stream.Write(this.getColor(to));
            stream.Write(this.sendButton);
            stream.Write(this.startWritingButton);
            stream.Write(this.resetButton);
            stream.Write(ParentID);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC);
            }
        }

        public override void setFont(string font)
        {
            throw new NotImplementedException("Set Font does not work with TextArea!");
        }

        public void StartWriting(Player pl)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.TextBoxStartWriting);

            stream.Write(this.id);

            sendStream(pl.ID, stream);
        }
        public void StopWriting(Player pl)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.TextBoxStopWriting);

            stream.Write(this.id);

            sendStream(pl.ID, stream);
        }
        public void CallSendText(Player pl)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.TextBoxCallSend);

            stream.Write(this.id);

            sendStream(pl.ID, stream);
        }

        public void setStartWritingKey(int key)
        {
            this.startWritingButton = key;


            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.TextBoxSetStartWritingKey);

            stream.Write(this.id);
            stream.Write(this.startWritingButton);

            sendStream(0, stream);
        }

        public void setResetKey(int key)
        {
            this.resetButton = key;


            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.TextBoxSetResetKey);

            stream.Write(this.id);
            stream.Write(this.resetButton);

            sendStream(0, stream);
        }

        public void setSendKey(int key)
        {
            this.sendButton = key;


            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.TextBoxSetSendKey);

            stream.Write(this.id);
            stream.Write(this.sendButton);

            sendStream(0, stream);
        }




        #region Events
        public event GUC.Server.Scripting.Events.TextBoxMessageEventHandler TextSended;
        internal void OnTextSended(TextBox tex, Player player, String message)
        {
            if (TextSended != null)
                TextSended(tex, player, message);
        }

        #endregion
        #region Static Events:

        public static event Events.TextBoxMessageEventHandler TextSends;

        internal static void OnTextSends(TextBox tex, Player player, String message)
        {
            tex.OnTextSended(tex, player, message);
            if (TextSends != null)
                TextSends(tex, player, message);
        }

        #endregion

    }
}
