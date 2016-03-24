using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.Client.WorldObjects;

namespace GUC.Client.Hooks
{
    public class hModelAni
    {

        public static Int32 oCStartAni_ModelInt(String message)
        {
            try
            {
               /* if (Player.Hero == null || !Player.Hero.Spawned)
                    return 0;

                int address = Convert.ToInt32(message);
                zCModel thisModel = new zCModel(Program.Process, Program.Process.ReadInt(address));

                if (thisModel.Owner.Address != Player.Hero.gVob.Address)
                    return 0;

                zCModelAni modelAni = new zCModelAni(Program.Process, Program.Process.ReadInt(address + 4));
                if (modelAni == null || modelAni.Address == 0)
                    return 0;

                int aniID = modelAni.GetAniID();

                if (Player.Hero.TurnAnimation == aniID)
                    return 0;

                Player.Hero.TurnAnimation = (short)aniID;
                Network.Messages.NPCMessage.WriteAnimation(Player.Hero);*/
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Program.Process).Report(2, 'G', "Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "hModelAni.cs", 0);
            }
            return 0;
        }
    }
}
