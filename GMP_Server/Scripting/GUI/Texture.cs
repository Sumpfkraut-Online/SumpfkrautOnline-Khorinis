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
    public class Texture : View
    {
        protected String tex = null;
        protected Vec2i size = new Vec2i();

        protected GUIEvents enabledEvents = GUIEvents.None;

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
            : this(tex, pos, size, parent, isSingleUser, singleUserID, GUIEvents.None, true)
        { }






        public Texture(String tex, GUIEvents evts)
            : this(tex, 0, 0, 0x2000, 0x2000, evts)
        { }

        public Texture(String tex, int x, int y, int width, int height, GUIEvents evts)
            : this(tex, x, y, width, height, null, false, 0, evts)
        { }

        public Texture(String tex, int x, int y, int width, int height, Texture parent, GUIEvents evts)
            : this(tex, x, y, width, height, parent, false, 0, evts)
        { }

        public Texture(String tex, int x, int y, int width, int height, Texture parent, bool isSingleUser, int singleUserID, GUIEvents evts)
            : this(tex, new Vec2i(x, y), new Vec2i(width, height), parent, isSingleUser, singleUserID, evts)
        { }

        public Texture(String tex, Vec2i pos, Vec2i size, Texture parent, bool isSingleUser, int singleUserID, GUIEvents evts)
            : this(tex, pos, size, parent, isSingleUser, singleUserID, evts, true)
        { }

        protected Texture(String tex, Vec2i pos, Vec2i size, Texture parent, bool isSingleUser, int singleUserID, GUIEvents evts, bool useCreate)
            : base(pos, isSingleUser, singleUserID, parent)
        {
            if (tex == null)
                tex = "";
            this.tex = tex;
            this.size.set(size);

            this.enabledEvents = evts;

            if(useCreate)
                create(-1);

        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateTexture);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(this.size);
            stream.Write(tex);
            stream.Write(ParentID);

            stream.Write((int)enabledEvents);

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

            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
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
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.SetSize);

            stream.Write(this.ID);
            stream.Write(this.size);

            sendStream(0, stream);
        }


        #region Events

        #region LeftClick
        public event Events.TextureEventHandler OnLeftClick;
        internal void iOnLeftClick(Texture sender, Player player)
        {
            if (OnLeftClick != null)
            {
                OnLeftClick(sender, player);
            }
        }

        public static event Events.TextureEventHandler sOnLeftClick;
        internal static void isOnLeftClick(Texture sender, Player player)
        {
            sender.iOnLeftClick(sender, player);
            if (sOnLeftClick != null)
            {
                sOnLeftClick(sender, player);
            }
        }
        #endregion

        #region RightClick
        public event Events.TextureEventHandler OnRightClick;
        internal void iOnRightClick(Texture sender, Player player)
        {
            if (OnRightClick != null)
            {
                OnRightClick(sender, player);
            }
        }

        public static event Events.TextureEventHandler sOnRightClick;
        internal static void isOnRightClick(Texture sender, Player player)
        {
            sender.iOnRightClick(sender, player);
            if (sOnRightClick != null)
            {
                sOnRightClick(sender, player);
            }
        }
        #endregion

        #region Hover
        public event Events.TextureHoverEventHandler OnHover;
        internal void iOnHover(Texture sender, Player player, bool hover)
        {
            if (OnHover != null)
            {
                OnHover(sender, player, hover);
            }
        }

        public static event Events.TextureHoverEventHandler sOnHover;
        internal static void isOnHover(Texture sender, Player player, bool hover)
        {
            sender.iOnHover(sender, player, hover);
            if (sOnHover != null)
            {
                sOnHover(sender, player, hover);
            }
        }
        #endregion

        #endregion
    }
}
