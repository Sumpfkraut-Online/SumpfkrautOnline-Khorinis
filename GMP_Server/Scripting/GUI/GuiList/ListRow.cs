using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Types;

namespace GUC.Server.Scripting.GUI.GuiList
{
    public abstract class ListRow : View
    {
        protected String m_Text;
        protected List m_Parent;

        protected ColorRGBA m_ActiveRowColor = ColorRGBA.Red;
        protected ColorRGBA m_InactiveRowColor = ColorRGBA.White;

        internal ListRow(List aParent, String aText, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
            : base(0, 0, aParent.getIsSingleUser(), aParent.getSingleUserID(), aParent)
        {
            m_Text = aText;
            m_Parent = aParent;

            m_ActiveRowColor = aActiveRowColor;
            m_InactiveRowColor = aInactiveRowColor;
        }

        //protected override void create(int to)
        //{
        //    BitStream stream = Program.server.sendBitStream;
        //    stream.Reset();
        //    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
        //    stream.Write((byte)NetworkIDS.GuiMessage);
        //    stream.Write((byte)GuiMessageType.CreateText);

        //    stream.Write(this.id);
        //    stream.Write(this.m_Text);
        //    stream.Write(this.m_Parent.ID);

        //    sendStream(to, stream);


        //    if (!isSingleUser && allShown && to != -1)
        //    {
        //        show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC);
        //    }
        //}


        public override void destroy()
        {
            base.destroy();
        }

        public override void setPosition(Types.Vec2i pos)
        {
            throw new NotImplementedException("This function does not work for list elements");
        }

        public override void show(int plID)
        {
            throw new NotImplementedException("This function does not work for list elements");
        }

        public override void hide(int plID)
        {
            throw new NotImplementedException("This function does not work for list elements");
        }

        
    }
}
