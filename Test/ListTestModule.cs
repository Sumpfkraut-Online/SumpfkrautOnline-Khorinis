using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;

namespace GUC.Server.Scripts.Test
{
    //gittest
    //ouhfasdkfghvakjhgsf

    public class ListTestModule : StartModules.AbstractModule
    {
        public override void start(Player player)
        {
            Scripting.GUI.GuiList.List list = new Scripting.GUI.GuiList.List(player, 5, "FONT_DEFAULT.TGA", "LOG_PAPER.tga", new Types.Vec2i(0, 0), new Types.Vec2i(0x2000, 0x2000), null);
            list.addText("Test1");
            list.addText("Test2");
            list.addText("Test3");

            list.addTextBox("TextBox1", "test");
            list.addTextBox("TextBox2", "test2");

            list.addButton("Button1");
            list.addButton("Button2");
            list.addButton("Button3");
            Scripting.GUI.GuiList.ListButton.sOnClick += onclick;
            Scripting.GUI.GuiList.ListTextBox.sOnTextSend += ontext;
            list.show();

        }

        public void onclick(Scripting.GUI.GuiList.ListButton b, Player player)
        {
            Console.WriteLine("Button clicked: " + b.ID + " " + player.Name);
        }

        public void ontext(Scripting.GUI.GuiList.ListTextBox b, Player player, String text)
        {
            Console.WriteLine("Text sended: " + b.ID + " " + player.Name +" text: "+text);
        }
    }
}
