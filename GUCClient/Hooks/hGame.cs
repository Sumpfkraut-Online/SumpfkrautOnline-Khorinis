using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace GUC.Client.Hooks
{
    public class hGame
    {
        public static Int32 ExitGame(String message)
        {
            try
            {
                Program.client.Disconnect();
                
                zCOption.GetOption(Program.Process).getSection("INTERNAL").getEntry("gameAbnormalExit").VarValue.Set("0");
                zCOption.GetOption(Program.Process).Save("Gothic.INI");
                CGameManager.GameManager(Program.Process).ExitGameVar = 1;
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', ex.ToString(), 0, "hGame.cs", 0);
            }
            return 0;
        }
    }
}
