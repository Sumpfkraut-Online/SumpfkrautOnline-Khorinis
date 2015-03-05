using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using RakNet;
using GUC.WorldObjects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;
using GUC.Network;

namespace GUC.Server.Scripting.GUI.GuiList
{
    public class ListTextBox : ListRow
    {
        protected String m_HardText;
        internal ListTextBox(List aParent, String aHardText, String aText, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
            : base(aParent, aText, aActiveRowColor, aInactiveRowColor)
        {
            this.m_HardText = aHardText;
            create(-1);
        }


        protected override void create(int to)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateListTextBox);

            stream.Write(this.id);
            stream.Write(this.m_HardText);
            stream.Write(this.m_Text);
            stream.Write(this.m_Parent.ID);
            stream.Write(m_ActiveRowColor);
            stream.Write(m_InactiveRowColor);

            sendStream(to, stream);


            if (!isSingleUser && allShown && to != -1)
            {
                show((GUC.Server.Scripting.Objects.Character.Player)((GUC.WorldObjects.Character.NPCProto)sWorld.VobDict[to]).ScriptingNPC);
            }
        }

        #region SendText
        public event Events.ListTextboxEventHandler OnTextSend;
        internal void iOnTextSend(ListTextBox sender, Player player, String text)
        {
            if (OnTextSend != null)
            {
                OnTextSend(sender, player, text);
            }
        }

        public static event Events.ListTextboxEventHandler sOnTextSend;
        internal static void isOnTextSend(ListTextBox sender, Player player, String text)
        {
            sender.iOnTextSend(sender, player, text);
            if (sOnTextSend != null)
            {
                sOnTextSend(sender, player, text);
            }
        }
        #endregion
    }
}
