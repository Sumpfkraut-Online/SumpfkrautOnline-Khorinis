using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network.Messages.NpcCommands;

namespace GUC.Hooks
{
    public class hModelAni
    {
        
        public static Int32 oCStartAnim_ModelAnim(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            int err = 0;
            try
            {
                err = 1;
                zCModel thisModel = new zCModel(process, process.ReadInt(address));
                err = 2;
                int ownerAddress = thisModel.Owner.Address;
                err = 3;
                if (!sWorld.SpawnedVobDict.ContainsKey(ownerAddress))
                    return 0;
                Vob v = sWorld.SpawnedVobDict[ownerAddress];
                if (!(v is NPCProto))
                    return 0;

                NPCProto player = (NPCProto)v;
                if (player == null)
                    return 0;

                if (player != Player.Hero)
                    return 0;

                zCModelAni modelAni = new zCModelAni(process, process.ReadInt(address + 4));
                if (modelAni == null || modelAni.Address == 0)
                    return 0;

                

                int aniID = modelAni.GetAniID();
                //String name = modelAni.AniName.Value.Trim();//Works
                if (player.Animation == aniID)
                    return 0;

                player.Animation = (short)aniID;
                AnimationUpdateMessage.Write(player);

                
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', err+"Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "Program.cs", 0);
            }


            return 0;
        }
    }
}
