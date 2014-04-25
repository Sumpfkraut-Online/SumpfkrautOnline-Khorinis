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
    public class Button : Texture
    {
        protected String text;

        public Button()
            : this("")
        { }
        public Button(String text)
            : this(text, "buttons_03.tga")
        { }

        public Button(String text, Vec2i pos)
            : this(text, pos.X, pos.Y)
        { }

        public Button(String text, int x, int y)
            : this(text, "buttons_03.tga", x, y, 1700, 600)
        { }

        public Button(String text, String tex)
            : this(text, tex, 0, 0, 1700, 600)
        { }

        public Button(String text, String tex, int x, int y, int width, int height)
            : this(text, tex, x, y, width, height, null, false, 0)
        { }

        public Button(String text, String tex, int x, int y, int width, int height, Texture parent)
            : this(text, tex, x, y, width, height, parent, false, 0)
        { }

        public Button(String text, String tex, int x, int y, int width, int height, Texture parent, bool isSingleUser, int singleUserID)
            : this(text, tex, new Vec2i(x, y), new Vec2i(width, height), parent, isSingleUser, singleUserID)
        { }

        public Button(String text, String tex, Vec2i pos, Vec2i size, Texture parent, bool isSingleUser, int singleUserID)
            : base(tex, pos, size, parent, isSingleUser, singleUserID, false)
        {
            this.text = text;
            create(-1);
        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateButton);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(this.size);
            stream.Write(this.text);
            stream.Write(tex);
            stream.Write(ParentID);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show( (GUC.Server.Scripting.Objects.Character.Player) ((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC );
            }
        }




        #region Events
        public event GUC.Server.Scripting.Events.ButtonEventHandler Pressed;
        internal void OnButtonPressed(Button sender, Player player)
        {
            if (Pressed != null)
                Pressed(sender, player);
        }

        #endregion
        #region Static Events:

        public static event Events.ButtonEventHandler ButtonPressed;

        internal static void OnButtonPress(Button sender, Player player)
        {
            sender.OnButtonPressed(sender, player);
            if (ButtonPressed != null)
                ButtonPressed(sender, player);
        }

        #endregion


    }
}
