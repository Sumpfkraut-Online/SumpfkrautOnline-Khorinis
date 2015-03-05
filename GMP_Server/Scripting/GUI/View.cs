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
    public abstract class View
    {
        protected static int viewIDS = 0;
        protected static Dictionary<Int32, View> allViewDic = new Dictionary<int, View>();

        static internal Dictionary<Int32, View> AllViewDict { get { return allViewDic; } }

        protected int id;
        protected Vec2i position = new Vec2i();

        protected bool allShown = false;
        protected bool isSingleUser = false;
        protected int singleUserID = 0;

        protected View parent;

        public View(int x, int y, bool isSingleUser, int singleUserID, View parent)
            : this(new Vec2i(x, y), isSingleUser, singleUserID, parent)
        { }

        public View(int x, int y)
            : this(new Vec2i(x, y), false, 0, null)
        { }

        public View(Vec2i pos)
            : this(pos, false, 0, null)
        { }

        public View(int x, int y, View parent)
            : this(new Vec2i(x, y), false, 0, parent)
        { }

        public View(Vec2i pos, View parent)
            : this(pos, false, 0, parent)
        { }



        public View(Vec2i pos, bool isSingleUser, int singleUserID, View parent)
        {
            viewIDS++;
            id = viewIDS;
            this.position.set(pos);
            this.isSingleUser = isSingleUser;
            this.singleUserID = singleUserID;

            this.parent = parent;

            if (this.parent != null && this.parent.getIsSingleUser())
            {
                if (!isSingleUser || singleUserID != this.parent.getSingleUserID())
                {
                    this.parent = null;
                    parent = null;
                }
            }



            allViewDic.Add(id, this);
        }

        protected abstract void create(int to);

        public int getID()
        {
            return id;
        }

        protected int getSendToID()
        {
            return this.isSingleUser ? this.singleUserID : -1;
        }

        public bool getIsSingleUser()
        {
            return this.isSingleUser;
        }

        public int getSingleUserID()
        {
            return this.singleUserID;
        }

        public void setPosition(int x, int y)
        {
            this.setPosition(new Vec2i(x, y));
        }

        public virtual void setPosition(Vec2i pos)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.SetPosition);

            stream.Write(this.id);
            stream.Write(this.position);

            sendStream(0, stream);
        }

        public void show()
        {
            allShown = true;
            show(0);
        }

        public void show(Player player)
        {
            show(player.ID);
        }

        public bool isVisibleByAll()
        {
            return allShown;
        }

        public virtual void show(int plID)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.Show);

            stream.Write(this.id);

            sendStream(plID, stream);
        }

        public int ParentID { get { if (parent == null) return 0; else return parent.ID; } }

        public int ID { get { return this.id; } }


        public void hide()
        {
            allShown = false;
            hide(0);
        }

        public void hide(Player pl)
        {
            hide(pl.ID);
        }

        public virtual void hide(int plID)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.Hide);

            stream.Write(this.id);

            sendStream(plID, stream);
        }

        public virtual void destroy()
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.Destroy);

            stream.Write(this.id);

            sendStream(0, stream);
        }




        protected void sendStream(int to, BitStream stream)
        {
            if (isSingleUser)
            {
                if (singleUserID == 0 || !sWorld.VobDict.ContainsKey(singleUserID))
                    throw new Exception("Single-User-ID was not valid! The User does not exists on the server: "+singleUserID);
                Vob v = sWorld.VobDict[singleUserID];
                if (!(v is GUC.WorldObjects.Character.Player))
                    throw new Exception("PlayerID: "+singleUserID+" was not a Player! "+v);
                GUC.WorldObjects.Character.Player player = (GUC.WorldObjects.Character.Player)v;

                using(RakNetGUID guid = player.GUID)
                    Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
            }
            else
            {
                if (to <= 0)
                    Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                else
                {
                    if (!sWorld.VobDict.ContainsKey(to))
                        throw new Exception("To-User-ID was not valid! The User does not exists on the server: " + to);
                    Vob v = sWorld.VobDict[to];
                    if (!(v is GUC.WorldObjects.Character.Player))
                        throw new Exception("PlayerID: " + to + " was not a Player! " + v);
                    GUC.WorldObjects.Character.Player player = (GUC.WorldObjects.Character.Player)v;

                    using (RakNetGUID guid = player.GUID)
                        Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }
        }


        internal static void SendToPlayer(GUC.WorldObjects.Character.Player pl)
        {

            foreach (KeyValuePair<int, View> v in allViewDic)
            {
                if (v.Value.isSingleUser)
                    continue;
                v.Value.create(pl.ID);
            }
        }
    }
}
