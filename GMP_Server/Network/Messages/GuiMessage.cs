using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Server.Network.Messages
{
    class GuiMessage : IMessage
    {
        protected View getViewFromList(int id)
        {
            if (!View.AllViewDict.ContainsKey(id))
                throw new Exception("The key: " + id + " was not found in the viewlist!");
            return View.AllViewDict[id];
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int playerID = 0;
            byte type = 0;
            stream.Read(out type);
            stream.Read(out playerID);

            Player pl = (Player)sWorld.VobDict[playerID];

            GuiMessageType gmT = (GuiMessageType)type;
            int viewID = 0;
            if (gmT == GuiMessageType.TextBoxCallSend)
            {
                String message;
                stream.Read(out viewID);
                stream.Read(out message);

                View v = getViewFromList(viewID);
                if (!(v is TextBox))
                    throw new Exception("TextBoxCallSend needs a TextBox "+v);
                TextBox tex = (TextBox)v;
                TextBox.OnTextSends(tex, (Scripting.Objects.Character.Player)pl.ScriptingNPC, message);
            }
            else if (gmT == GuiMessageType.ButtonPressed)
            {
                
                stream.Read(out viewID);

                View v = getViewFromList(viewID);
                if (!(v is Button))
                    throw new Exception("ButtonPressed needs a Button " + v);
                Button button = (Button)v;
                Button.OnButtonPress(button, (Scripting.Objects.Character.Player)pl.ScriptingNPC);
            }
            else if (gmT == GuiMessageType.GuiEvent)
            {
                int guiEvent = 0;
                stream.Read(out viewID);
                stream.Read(out guiEvent);

                GUIEvents evt = (GUIEvents)guiEvent;

                View v = getViewFromList(viewID);


                if (evt.HasFlag(GUIEvents.LeftClicked))
                {
                    if ((v is Button))
                    {
                        Button button = (Button)v;
                        Button.OnButtonPress(button, (Scripting.Objects.Character.Player)pl.ScriptingNPC);
                    }
                    Texture tex = (Texture)v;
                    Texture.isOnLeftClick(tex, (Scripting.Objects.Character.Player)pl.ScriptingNPC);
                }
                else if (evt.HasFlag(GUIEvents.RightClicked))
                {
                    Texture.isOnRightClick((Texture)v, (Scripting.Objects.Character.Player)pl.ScriptingNPC);
                }
                else if (evt.HasFlag(GUIEvents.Hover))
                {
                    bool hover = false;
                    stream.Read(out hover);
                    Texture.isOnHover((Texture)v, (Scripting.Objects.Character.Player)pl.ScriptingNPC, hover);
                }
                else if (evt.HasFlag(GUIEvents.ListButtonClicked))
                {
                    Scripting.GUI.GuiList.ListButton.isOnClick((Scripting.GUI.GuiList.ListButton)v, (Scripting.Objects.Character.Player)pl.ScriptingNPC);
                    //Texture.isOnHover((Texture)v, (Scripting.Objects.Character.Player)pl.ScriptingNPC, hover);
                }
                else if (evt.HasFlag(GUIEvents.ListTextBoxSend))
                {
                    String text = "";
                    stream.Read(out text);
                    Scripting.GUI.GuiList.ListTextBox.isOnTextSend((Scripting.GUI.GuiList.ListTextBox)v, (Scripting.Objects.Character.Player)pl.ScriptingNPC, text);
                    //Texture.isOnHover((Texture)v, (Scripting.Objects.Character.Player)pl.ScriptingNPC, hover);
                }
            }
        }
    }
}
