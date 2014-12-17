using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using GUC.Enumeration;
using Gothic.zClasses;
using RakNet;
using GUC.WorldObjects.Character;
using GUC.Types;

namespace GUC.GUI.GuiList
{
    public class ListTextBox : ListRow
    {
        public String hardText = "";
        public bool Password;


        public ListTextBox(int id, String hardText, String text, List list, ColorRGBA aActiveRowColor, ColorRGBA aInactiveRowColor)
            : base(id, text, list, aActiveRowColor, aInactiveRowColor)
        {
            this.hardText = hardText;
        }

        public override void setControl(zCViewText text)
        {
            if (text == null)
            {
                isActive = false;
                mTextView = null;
                return;
            }
            mTextView = text;
            isActive = true;

            mTextView.Text.Set(hardText + mText);

        }

        public override bool IsInputActive
        {
            get
            {
                return base.IsInputActive;
            }
            set
            {
                base.IsInputActive = value;
                if(isInputActive)
                    InputHooked.deaktivateFullControl(WinApi.Process.ThisProcess());
                else
                    InputHooked.activateFullControl(WinApi.Process.ThisProcess());
            }
        }

        protected override void updateActive(VirtualKeys key)
        {
            if (key == VirtualKeys.Up)
                mList.ActiveID--;
            if (key == VirtualKeys.Down || key == VirtualKeys.Return)
                mList.ActiveID++;

            if (key == VirtualKeys.Up || key == VirtualKeys.Down || key == VirtualKeys.Return)
            {
                RakNet.BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.GuiMessage);
                stream.Write((byte)GuiMessageType.GuiEvent);
                stream.Write(Player.Hero.ID);
                stream.Write(this.id);
                stream.Write((int)GUIEvents.ListTextBoxSend);
                stream.Write(mText);

                Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
                return;
            }


            if ((int)key == 8)
            {
                if (mText.Length == 0)
                    return;
                mText = mText.Substring(0, mText.Length - 1);
                mTextView.Text.Set(hardText + mText);
                return;
            }

            String keyVal = Convert.ToString((char)key);
            keyVal = Gothic.mClasses.textBox.GetCharsFromKeys((WinApi.User.Enumeration.VirtualKeys)key, InputHooked.IsPressed((int)VirtualKeys.Shift), InputHooked.IsPressed((int)VirtualKeys.Control) && InputHooked.IsPressed((int)VirtualKeys.Menu));

            if (keyVal != "")
            {
                mText += keyVal;
                mTextView.Text.Add(keyVal);
            }
        }
    }
}
