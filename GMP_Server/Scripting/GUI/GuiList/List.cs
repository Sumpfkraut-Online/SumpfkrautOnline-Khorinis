using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Types;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripting.GUI.GuiList
{
    public class List : Texture
    {
        protected List<ListRow> rows = new List<ListRow>();


        public List(Player player, String texture, Vec2i pos, Vec2i size, Texture parent)
            : this(texture, pos, size, parent, player.ID, true)
        {

        }

        protected List(String tex, Vec2i pos, Vec2i size, Texture parent, int singleUserID, bool useCreate)
            : base(tex, pos, size, parent, true, singleUserID, GUIEvents.None, false)
        {


            if(useCreate)
                create(-1);

        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateList);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(this.size);
            stream.Write(tex);
            stream.Write(ParentID);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC);
            }
        }


        public ListText addText(String text)
        {
            throw new NotImplementedException("");
        }

        public ListButton addButton(String text)
        {
            throw new NotImplementedException("");
        }

        public ListTextBox addTextBox(String text)
        {
            throw new NotImplementedException("");
        }

        public void remove(ListRow element)
        {
            rows.Remove(element);
            element.destroy();
        }


        public override void destroy()
        {
            //Destroy all childs:
            foreach(View row in rows){
                row.destroy();
            }


            base.destroy();
        }
        
    }
}
