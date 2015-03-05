using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Enumeration;
using GUC.Types;
using GUC.Network;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages
{
    class GuiMessage : IMessage
    {
        protected Dictionary<int, View> viewList = new Dictionary<int, View>();

        protected View getViewFromList(int id)
        {
            if (!viewList.ContainsKey(id))
                throw new Exception("The key: " + id + " was not found in the viewlist!");
            return viewList[id];
        }

        protected Texture getTextureParentFromList(int id)
        {
            if (!viewList.ContainsKey(id))
                return null;
            View v = viewList[id];
            if (v is Texture)
                return (Texture)v;
            throw new Exception("Parent was not a texture! :"+id+" "+v);
        }

        protected GUI.GuiList.List getListParentFromList(int id)
        {
            if (!viewList.ContainsKey(id))
                return null;
            View v = viewList[id];
            if (v is GUI.GuiList.List)
                return (GUI.GuiList.List)v;
            throw new Exception("Parent was not a GUI.GuiList.List! :" + id + " " + v);
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type = 0;
            stream.Read(out type);
            GuiMessageType gmT = (GuiMessageType)type;

            int viewID = 0;

            if (gmT == GuiMessageType.Show)
            {
                stream.Read(out viewID);
                View v = getViewFromList(viewID);

                v.show();
            }
            else if (gmT == GuiMessageType.Hide)
            {
                stream.Read(out viewID);
                View v = getViewFromList(viewID);

                v.hide();
            }
            else if (gmT == GuiMessageType.SetPosition)
            {
                Vec2i position;
                stream.Read(out viewID);
                stream.Read(out position);
                View v = getViewFromList(viewID);

                v.setPosition(position);
            }
            else if (gmT == GuiMessageType.Destroy)
            {
                stream.Read(out viewID);
                View v = getViewFromList(viewID);

                v.Destroy();
            }//Creation:
            else if (gmT == GuiMessageType.CreateTexture)
            {
                Vec2i position, size;
                string texture;
                int parentID = 0;
                int guiEvents = 0;

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out size);
                stream.Read(out texture);
                stream.Read(out parentID);
                stream.Read(out guiEvents);

                Texture tex = new Texture(viewID, texture, position, size, getTextureParentFromList(parentID), (GUIEvents)guiEvents);
                viewList.Add(viewID, tex);
            }
            else if (gmT == GuiMessageType.CreateText)
            {
                Vec2i position;
                string text, font;
                ColorRGBA color;
                int parentID = 0;

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out text);
                stream.Read(out font);
                stream.Read(out color);
                stream.Read(out parentID);

                Text textView = new Text(viewID, text, font, position, getTextureParentFromList(parentID), color);
                viewList.Add(viewID, textView);
            }
            else if (gmT == GuiMessageType.CreateTextBox)
            {
                Vec2i position;
                string text, font;
                ColorRGBA color;
                int parentID = 0, sendKey, startWriteKey, resetKey;

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out text);
                stream.Read(out font);
                stream.Read(out color);
                stream.Read(out sendKey);
                stream.Read(out startWriteKey);
                stream.Read(out resetKey);
                stream.Read(out parentID);

                TextBox textView = new TextBox(viewID, text, font, position, getTextureParentFromList(parentID), color, resetKey, startWriteKey, sendKey);
                viewList.Add(viewID, textView);
            }
            else if (gmT == GuiMessageType.CreateTextArea)
            {
                Vec2i position, size;
                string text, font;
                ColorRGBA color;
                int parentID = 0, sendKey, startWriteKey, resetKey;

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out size);
                stream.Read(out text);
                stream.Read(out font);
                stream.Read(out color);
                stream.Read(out sendKey);
                stream.Read(out startWriteKey);
                stream.Read(out resetKey);
                stream.Read(out parentID);

                TextArea textView = new TextArea(viewID, text, font, position, size, getTextureParentFromList(parentID), color, resetKey, startWriteKey, sendKey);
                viewList.Add(viewID, textView);
            }
            else if (gmT == GuiMessageType.CreateMessageBox)
            {
                Vec2i position;
                int parentID = 0;
                String font;
                byte lines;

                int scrollUp, scrollDown, resetScroll;
                bool resetOnNewMessage;

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out parentID);
                stream.Read(out font);
                stream.Read(out lines);

                stream.Read(out scrollUp);
                stream.Read(out scrollDown);
                stream.Read(out resetScroll);
                stream.Read(out resetOnNewMessage);

                MessageBox messageBox = new MessageBox(viewID, lines, font, position, getTextureParentFromList(parentID), scrollUp, scrollDown, resetScroll, resetOnNewMessage);
                viewList.Add(viewID, messageBox);
            }
            else if (gmT == GuiMessageType.CreateCursor)
            {
                Vec2i position, size;
                string texture;
                int parentID = 0;

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out size);
                stream.Read(out texture);
                stream.Read(out parentID);

                

                Cursor tex = new Cursor(viewID, texture, position, size);
                viewList.Add(viewID, tex);

            }
            else if (gmT == GuiMessageType.CreateButton)
            {
                Vec2i position, size;
                string texture, text, font;
                int parentID = 0;
                ColorRGBA color;
                int guiEvents = 0;

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out size);
                stream.Read(out text);
                stream.Read(out texture);
                stream.Read(out font);
                stream.Read(out color);
                
               
                stream.Read(out parentID);
                stream.Read(out guiEvents);


                Button tex = new Button(viewID, text, texture, font, color, position, size, getTextureParentFromList(parentID), (GUIEvents)guiEvents);
                viewList.Add(viewID, tex);

            }
            else if (gmT == GuiMessageType.CreateList)
            {
                Vec2i position, size;
                string texture, font;
                int parentID = 0;
                int guiEvents = 0;
                byte lines = 0;
                

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out size);
                stream.Read(out texture);
                stream.Read(out parentID);
                //stream.Read(out guiEvents);
                stream.Read(out lines);
                stream.Read(out font);

                GUI.GuiList.List tex = new GUI.GuiList.List(viewID, lines, font, position, size, texture, getTextureParentFromList(parentID), (GUIEvents)guiEvents);
                viewList.Add(viewID, tex);
            }
            else if (gmT == GuiMessageType.CreateListText)
            {
                String text = "";
                int parentID = 0;
                ColorRGBA activeColor, inactiveColor;


                stream.Read(out viewID);
                stream.Read(out text);
                stream.Read(out parentID);
                stream.Read(out activeColor);
                stream.Read(out inactiveColor);
                

                GUI.GuiList.List list = getListParentFromList(parentID);
                GUI.GuiList.ListText listelement = new GUI.GuiList.ListText(viewID, text, list, activeColor, inactiveColor);
                list.addRow(listelement);
                viewList.Add(viewID, listelement);
            }
            else if (gmT == GuiMessageType.CreateListButton)
            {
                String text = "";
                int parentID = 0;
                ColorRGBA activeColor, inactiveColor;

                stream.Read(out viewID);
                stream.Read(out text);
                stream.Read(out parentID);

                stream.Read(out activeColor);
                stream.Read(out inactiveColor);


                GUI.GuiList.List list = getListParentFromList(parentID);
                GUI.GuiList.ListButton listelement = new GUI.GuiList.ListButton(viewID, text, list, activeColor, inactiveColor);
                list.addRow(listelement);
                viewList.Add(viewID, listelement);
            }
            else if (gmT == GuiMessageType.CreateListTextBox)
            {
                String text = "";
                String hardText = "";
                int parentID = 0;
                ColorRGBA activeColor, inactiveColor;


                stream.Read(out viewID);

                stream.Read(out hardText);
                stream.Read(out text);
                
                stream.Read(out parentID);

                stream.Read(out activeColor);
                stream.Read(out inactiveColor);


                GUI.GuiList.List list = getListParentFromList(parentID);
                GUI.GuiList.ListTextBox listelement = new GUI.GuiList.ListTextBox(viewID, hardText, text, list, activeColor, inactiveColor);
                list.addRow(listelement);
                viewList.Add(viewID, listelement);
            }
            else if (gmT == GuiMessageType.CreateText3D)
            {
                Vec3f position;
                float maxDistance;
                int parentID = 0;
                String world = "";

                stream.Read(out viewID);
                stream.Read(out position);
                stream.Read(out world);
                stream.Read(out maxDistance);
                stream.Read(out parentID);

                int rowCount = 0;
                stream.Read(out rowCount);

                Text3D textView = new Text3D(viewID, world, maxDistance, position);
                for (int i = 0; i < rowCount; i++)
                {
                    String text = "";
                    ColorRGBA color = ColorRGBA.White;
                    long time = 0, blendTime;

                    stream.Read(out text);
                    stream.Read(out color);
                    stream.Read(out time);
                    stream.Read(out blendTime);

                    textView.addRow(new Text3D.Text3DRow() { Text = text, Color = color, Time = time, BlendTime = blendTime });
                }

                
                viewList.Add(viewID, textView);
            }
            else if (gmT == GuiMessageType.CreateTextPlayer)
            {
                int playerID = 0;
                float maxDistance;
                int parentID = 0;
                

                stream.Read(out viewID);
                stream.Read(out playerID);
                stream.Read(out maxDistance);
                stream.Read(out parentID);

                int rowCount = 0;
                stream.Read(out rowCount);

                Vob vob = null;
                if (!sWorld.VobDict.TryGetValue(playerID, out vob))
                    throw new Exception("Player id:" +playerID +" was not found!");
                if (!(vob is Player))
                    throw new Exception("Vob is not a Player: "+playerID+" "+vob);

                PlayerText textView = new PlayerText(viewID, (Player)vob, maxDistance);
                for (int i = 0; i < rowCount; i++)
                {
                    String text = "";
                    ColorRGBA color = ColorRGBA.White;
                    long time = 0, blendTime;

                    stream.Read(out text);
                    stream.Read(out color);
                    stream.Read(out time);
                    stream.Read(out blendTime);

                    textView.addRow(new Text3D.Text3DRow() { Text = text, Color = color, Time = time, BlendTime = blendTime });
                }


                viewList.Add(viewID, textView);
            }
            else if (gmT == GuiMessageType.Text3DAddRow)
            {
                String text = "";
                ColorRGBA color = ColorRGBA.White;
                long time = 0, blendTime = 0;

                stream.Read(out viewID);
                stream.Read(out text);
                stream.Read(out color);
                stream.Read(out time);
                stream.Read(out blendTime);

                View v = getViewFromList(viewID);

                if (!(v is Text3D))
                    throw new Exception("Text3DAddRow works only with a Text3D!: " + v);

                Text3D t3d = (Text3D)v;
                t3d.addRow(new Text3D.Text3DRow() { Text = text, Color = color, Time = time, BlendTime = blendTime });
            }
            else if (gmT == GuiMessageType.Text3DClear)
            {
                stream.Read(out viewID);
                View v = getViewFromList(viewID);

                if (!(v is Text3D))
                    throw new Exception("Text3DAddRow works only with a Text3D!: " + v);

                Text3D t3d = (Text3D)v;
                t3d.Clear();
            }
            else if (gmT == GuiMessageType.Text3DPosition)
            {
                Vec3f position = new Vec3f();
                stream.Read(out viewID);
                stream.Read(out position);
                View v = getViewFromList(viewID);

                if (!(v is Text3D))
                    throw new Exception("Text3DAddRow works only with a Text3D!: " + v);

                Text3D t3d = (Text3D)v;
                t3d.setPosition(position);
            }
                //Texture:
            else if (gmT == GuiMessageType.SetTexture)
            {
                String texture = "";

                stream.Read(out viewID);
                stream.Read(out texture);
                View v = getViewFromList(viewID);

                if (!(v is Texture) && !(v is Button))
                    throw new Exception("SetTexture works only with a texture or buttons!: "+v);
                Texture tex = (Texture)v;
                tex.setTexture(texture);
            }
            else if (gmT == GuiMessageType.SetSize)
            {
                Vec2i position;

                stream.Read(out viewID);
                stream.Read(out position);
                View v = getViewFromList(viewID);

                if (!(v is Texture) && !(v is Button))
                    throw new Exception("SetSize works only with a texture or buttons!: " + v);
                Texture tex = (Texture)v;
                tex.setSize(position);
            }//Text&TextBox:
            else if (gmT == GuiMessageType.SetText)
            {
                String text = "";

                stream.Read(out viewID);
                stream.Read(out text);
                View v = getViewFromList(viewID);

                if (!(v is Text) && !(v is TextBox) && !(v is TextArea) && !(v is Button))
                    throw new Exception("SetText works only with a text and textbox!: " + v);

                if (v is Text)
                {
                    Text tex = (Text)v;
                    tex.setText(text);
                }
                else if(v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.setText(text);
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.setText(text);
                }
                else if (v is Button)
                {
                    Button button = (Button)v;
                    button.setText(text);
                }
            }
            else if (gmT == GuiMessageType.SetTextFont)
            {
                String font = "";

                stream.Read(out viewID);
                stream.Read(out font);
                View v = getViewFromList(viewID);

                if (!(v is Text) && !(v is TextBox))
                    throw new Exception("SetFont works only with a text and textbox!: " + v);

                if (v is Text)
                {
                    Text tex = (Text)v;
                    tex.setFont(font);
                }
                else if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.setFont(font);
                }
            }
            else if (gmT == GuiMessageType.SetTextColor)
            {
                ColorRGBA color;

                stream.Read(out viewID);
                stream.Read(out color);
                View v = getViewFromList(viewID);

                if (!(v is Text) && !(v is TextBox) && !(v is TextArea))
                    throw new Exception("SetColor works only with a text and textbox!: " + v);

                if (v is Text)
                {
                    Text tex = (Text)v;
                    tex.setColor(color);
                }
                else if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.setColor(color);
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.setColor(color);
                }
            }//TextBox!
            else if (gmT == GuiMessageType.TextBoxStartWriting)
            {
                stream.Read(out viewID);
                View v = getViewFromList(viewID);

                if (!(v is TextBox) && !(v is TextArea))
                    throw new Exception("TextBoxStartWriting works only with a textbox!: " + v);

                if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.startWriting();
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.startWriting();
                }
            }
            else if (gmT == GuiMessageType.TextBoxStopWriting)
            {
                stream.Read(out viewID);
                View v = getViewFromList(viewID);

                if (!(v is TextBox) && !(v is TextArea))
                    throw new Exception("TextBoxStartWriting works only with a textbox!: " + v);

                if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.stopWriting();
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.stopWriting();
                }
            }
            else if (gmT == GuiMessageType.TextBoxCallSend)
            {
                stream.Read(out viewID);
                View v = getViewFromList(viewID);

                if (!(v is TextBox) && !(v is TextArea))
                    throw new Exception("TextBoxStartWriting works only with a textbox!: " + v);

                if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.callSendText();
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.callSendText();
                }
            }
            else if (gmT == GuiMessageType.TextBoxSetStartWritingKey)
            {
                int key = 0;
                stream.Read(out viewID);
                stream.Read(out key);
                View v = getViewFromList(viewID);

                if (!(v is TextBox) && !(v is TextArea))
                    throw new Exception("TextBoxStartWriting works only with a textbox!: " + v);

                if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.setEnableKey(key);
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.setEnableKey(key);
                }
            }
            else if (gmT == GuiMessageType.TextBoxSetSendKey)
            {
                int key = 0;
                stream.Read(out viewID);
                stream.Read(out key);
                View v = getViewFromList(viewID);

                if (!(v is TextBox) && !(v is TextArea))
                    throw new Exception("TextBoxStartWriting works only with a textbox!: " + v);

                if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.setSendKey(key);
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.setSendKey(key);
                }
            }
            else if (gmT == GuiMessageType.TextBoxSetResetKey)
            {
                int key = 0;
                stream.Read(out viewID);
                stream.Read(out key);
                View v = getViewFromList(viewID);

                if (!(v is TextBox) && !(v is TextArea))
                    throw new Exception("TextBoxStartWriting works only with a textbox!: " + v);


                if (v is TextBox)
                {
                    TextBox tex = (TextBox)v;
                    tex.setResetKey(key);
                }
                else if (v is TextArea)
                {
                    TextArea tex = (TextArea)v;
                    tex.setResetKey(key);
                }
            }//MessageBox:
            else if (gmT == GuiMessageType.MessageBoxAddLine)
            {
                String message;
                ColorRGBA color;
                stream.Read(out viewID);
                stream.Read(out color);
                stream.Read(out message);
                View v = getViewFromList(viewID);

                if (!(v is MessageBox))
                    throw new Exception("TextBoxStartWriting works only with a textbox!: " + v);

                MessageBox tex = (MessageBox)v;
                tex.addMessage(message, color);
            }
        }
    }
}
