using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

namespace GUC.Hooks
{
    public class hGame
    {
        public static Int32 ExitGame(String message)
        {
            try
            {
                Process Process = Process.ThisProcess();

                if (Scripting.Events.OnExitGame != null)
                    Scripting.Events.OnExitGame(Process);


                Program.client.Disconnect();
                
                zCOption.GetOption(Process).getSection("INTERNAL").getEntry("gameAbnormalExit").VarValue.Set("0");
                zCOption.GetOption(Process).Save("Gothic.INI");
                CGameManager.GameManager(Process).ExitGameVar = 1;

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hGame.cs", 0);
            }
            return 0;
        }
    }
}
