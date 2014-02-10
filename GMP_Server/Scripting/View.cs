using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using Network;

namespace GMP_Server.Scripting
{
    public abstract class View
    {
        protected static int currID = 0;
        protected int id;
        protected int x = 0;
        protected int y = 0;

        protected bool AllShown = false;

        protected bool isSingleUser = false;
        protected int singleUserID = 0;

        protected static Dictionary<Int32, View> allViewDic = new Dictionary<int, View>();

        protected View parent;


        public static View getView(int id)
        {
            return allViewDic[id];
        }

        public View(int x, int y, bool isSingleUser, int singleUserID, View parent)
        {
            currID++;
            id = currID;
            this.x = x;
            this.y = y;
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
            return this.isSingleUser? this.singleUserID : -1;
        }

        public bool getIsSingleUser()
        {
            return this.isSingleUser;
        }

        public int getSingleUserID()
        {
            return this.id;
        }

        public void setPosition(int x, int y)
        {
            if(isSingleUser)
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, singleUserID, (byte)CommandoType.ViewSetPosition, null, new int[] { this.id, x, y }, null);
            else
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.ViewSetPosition, null, new int[] { this.id, x, y }, null);
        }

        public void hide()
        {
            if(isSingleUser)
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, singleUserID, (byte)CommandoType.ViewHide, null, new int[] { this.id }, null);
            else
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.ViewHide, null, new int[] { this.id }, null);
            AllShown = false;
        }

        public void hide(Player pl)
        {
            if(isSingleUser)
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, singleUserID, (byte)CommandoType.ViewHide, null, new int[] { this.id }, null);
            else
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.ViewHide, null, new int[] { this.id }, null);
        }

        public void show()
        {
            if (isSingleUser)
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, singleUserID, (byte)CommandoType.ViewShow, null, new int[] { this.id }, null);
            else
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.ViewShow, null, new int[] { this.id }, null);
            AllShown = true;
        }

        public void show(Player pl)
        {
            if(isSingleUser)
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, singleUserID, (byte)CommandoType.ViewShow, null, new int[] { this.id }, null);
            else
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, pl.getID(), (byte)CommandoType.ViewShow, null, new int[] { this.id }, null);
        }


        public void Destroy()
        {
            if (isSingleUser)
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, singleUserID, (byte)CommandoType.ViewDestroy, null, new int[] { this.id }, null);
            else
                new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.ViewDestroy, null, new int[] { this.id }, null);
            allViewDic.Remove(this.id);
        }


        /// <summary>
        /// For Internal Use Only!
        /// This Function will be called if a new Player connects
        /// </summary>
        /// <param name="pl"></param>
        public static void SendToPlayer(Player pl)
        {
            
            foreach (KeyValuePair<int, View> v in allViewDic)
            {
                if (v.Value.isSingleUser)
                    continue;
                v.Value.create(pl.getID());
            }
        }
    }
}
