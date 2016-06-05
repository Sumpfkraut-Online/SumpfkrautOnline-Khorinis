using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic;

namespace GUC.Scripts.Sumpfkraut.WorldSystem
{
    public partial class WorldInst
    {
        public static WorldInst Current { get { return (WorldInst)WorldObjects.World.Current.ScriptObject; } }

        public void Load()
        {
            GUC.Client.Scripts.Sumpfkraut.Menus.GUCMenu.CloseActiveMenus();
            oCGame.LoadGame(true, "OLDWORLD\\OLDWORLD.ZEN");
        }
    }
}
