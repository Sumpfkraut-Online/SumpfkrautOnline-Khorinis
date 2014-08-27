using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Types;
using GUC.Network;

namespace GUC.Server.Scripting.GUI.GuiList
{
    public class ListButton : ListRow
    {
        internal ListButton(List aParent, String aText, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
            : base(aParent, aText, aActiveRowColor, aInactiveRowColor)
        {
            create(-1);
        }

        protected override void create(int to)
        {
            BitStream stream = Program.server.sendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.GuiMessage);
            stream.Write((byte)GuiMessageType.CreateListButton);

            stream.Write(this.id);
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


        #region Click
        public event Events.ListButtonEventHandler OnClick;
        internal void iOnClick(ListButton sender, Player player)
        {
            if (OnClick != null)
            {
                OnClick(sender, player);
            }
        }

        public static event Events.ListButtonEventHandler sOnClick;
        internal static void isOnClick(ListButton sender, Player player)
        {
            sender.iOnClick(sender, player);
            if (sOnClick != null)
            {
                sOnClick(sender, player);
            }
        }
        #endregion


    }
}
