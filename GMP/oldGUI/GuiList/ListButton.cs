using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.Types;

namespace GUC.GUI.GuiList
{
    public class ListButton : ListText
    {
        public class ButtonPressedEventArg : EventArgs
        {

            public VirtualKey Key;
            public Object Data;
            public List list;
        }
        public event EventHandler<ButtonPressedEventArg> ButtonPressed;



        public ListButton(int id, String text, List list, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
            : base(id, text, list, aActiveRowColor, aInactiveRowColor)
        {

        }

        protected override void updateActive(VirtualKey key)
        {
            base.updateActive(key);

            if (key == VirtualKey.Return)
            {
                //ButtonPressed(this, new ButtonPressedEventArg() { Key = key, Data = this, list = this.mList });


                RakNet.BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkID.GuiMessage);
                stream.Write((byte)GuiMessageType.GuiEvent);
                stream.Write(Player.Hero.ID);
                stream.Write(this.id);


                //int evt = (key == (int)VirtualKeys.LeftButton) ? (int)GUIEvents.LeftClicked : (int)GUIEvents.RightClicked;
                stream.Write((int)GUIEvents.ListButtonClicked);

                Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            }
        }
    }
}
