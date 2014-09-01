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
        protected byte lines = 0;
        protected String font = "FONT_DEFAULT.TGA";

        protected ColorRGBA m_ActiveRowColor = ColorRGBA.Red;
        protected ColorRGBA m_InactiveRowColor = ColorRGBA.White;



        public List(Player player, byte lines, Vec2i pos, Vec2i size)
            : this(lines, "FONT_DEFAULT.TGA", ColorRGBA.Red, ColorRGBA.White, null, pos, size, null, player.ID, true)
        {

        }

        public List(Player player, byte lines, String font, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor, Vec2i pos, Vec2i size, Texture parent)
            : this(lines, font, aActiveRowColor, aInactiveRowColor, null, pos, size, parent, player.ID, true)
        {

        }

        public List(Player player, byte lines, String font, Vec2i pos, Vec2i size, Texture parent)
            : this(lines, font, ColorRGBA.Red, ColorRGBA.White, null, pos, size, parent, player.ID, true)
        {

        }

        public List(Player player, byte lines, String font, String texture, Vec2i pos, Vec2i size, Texture parent)
            : this(lines, font, ColorRGBA.Red, ColorRGBA.White, texture, pos, size, parent, player.ID, true)
        {

        }

        public List(Player player, byte lines, String font, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor, String texture, Vec2i pos, Vec2i size, Texture parent)
            : this(lines, font, aActiveRowColor, aInactiveRowColor, texture, pos, size, parent, player.ID, true)
        {
            
        }

        protected List(byte lines, String font, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor, String tex, Vec2i pos, Vec2i size, Texture parent, int singleUserID, bool useCreate)
            : base(tex, pos, size, parent, true, singleUserID, GUIEvents.None, false)
        {
            this.lines = lines;
            m_ActiveRowColor = aActiveRowColor;
            m_InactiveRowColor = aInactiveRowColor;

            if(useCreate)
                create(-1);

        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateList);

            stream.Write(this.id);
            stream.Write(this.position);
            stream.Write(this.size);
            stream.Write(tex);
            stream.Write(ParentID);


            stream.Write(lines);
            stream.Write(font);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC);
            }
        }


        #region Text

        public ListText addText(String text)
        {
            return addText(text, m_ActiveRowColor, m_InactiveRowColor);
        }

        public ListText addText(String text, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
        {
            ListText lt = new ListText(this, text, aActiveRowColor, aInactiveRowColor);

            rows.Add(lt);

            return lt;
        }
        #endregion

        #region Button
        public ListButton addButton(String text)
        {
            return addButton(text, m_ActiveRowColor, m_InactiveRowColor);
        }
        public ListButton addButton(String text, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
        {
            ListButton lt = new ListButton(this, text, aActiveRowColor, aInactiveRowColor);

            rows.Add(lt);

            return lt;
        }
        #endregion

        #region TextBox
        public ListTextBox addTextBox()
        {
            return addTextBox("", "", m_ActiveRowColor, m_InactiveRowColor);
        }

        public ListTextBox addTextBox(String hardText)
        {
            return addTextBox(hardText, "", m_ActiveRowColor, m_InactiveRowColor);
        }
        public ListTextBox addTextBox(String hardText, String text)
        {
            return addTextBox(hardText, text, m_ActiveRowColor, m_InactiveRowColor);
        }
        public ListTextBox addTextBox(String hardText, String text, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
        {
            ListTextBox lt = new ListTextBox(this, hardText, text, aActiveRowColor, aInactiveRowColor);

            rows.Add(lt);

            return lt;
        }
        #endregion

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
