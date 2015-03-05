using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.GUI;

namespace GUC.Server.Scripts.Test
{
    public class Text3DTest
    {
        public static void Init()
        {
            Player.sOnPlayerSpawns += spawnPlayer;
        }
        public static void spawnPlayer(Player player)
        {
            Text3D text3d = new Text3D(player, 2000, Types.ColorRGBA.Red, player.World, new Types.Vec3f(0, 100, 0));
            text3d.addRow("Test1", 10000);
            text3d.addRow("Test2", 12000);
            text3d.addRow("Test3", 14000);
            text3d.addRow("Test4", 16000);
            text3d.addRow("Test5", 18000);
            text3d.show();

            PlayerText playerText = new PlayerText(player);
            playerText.addRow("Okay", 80000);
            playerText.addRow("Okay2", 80000);
            playerText.addRow("Okay3", 80000);
            playerText.addRow("Okay4", 80000);
            playerText.addRow("Okay5", 80000);
            playerText.addRow("Okay6", 80000);
            playerText.addRow("Okay7", 80000);
            playerText.addRow("Okay8", 80000);
            playerText.addRow("Okay9", 80000);
            playerText.addRow("Okay10", 80000);
            playerText.addRow("Okay11", 80000);
            playerText.show();
        }
    }
}
