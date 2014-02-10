using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using GMP.Net.Messages;
using Injection;
using Gothic.zTypes;
using GMP.Modules;
using Network;
using System.Windows.Forms;
using GMP.Helper;

namespace GMP.Injection.Synch
{
    public class Animation
    {

        public static Dictionary<Player, Dictionary<int, float>> actFrameList = new Dictionary<Player, Dictionary<int, float>>();
        /// <summary>
        /// kA obs überhaupt noch gebraucht wird....
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 SetActFrame(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();


            zCModelAniActive aniActive = new zCModelAniActive(process, process.ReadInt(address));
            if (oCNpc.Player(process).GetModel().GetActiveAni(aniActive.ModelAni.GetAniID()).Address == aniActive.Address)
            {
                if (actFrameList[Program.Player] == null)
                    actFrameList[Program.Player] = new Dictionary<int, float>();
                actFrameList[Program.Player][aniActive.ModelAni.GetAniID()] = process.ReadFloat(address + 4);
                //new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 6, Program.Player, aniActive.ModelAni.GetAniID(), 0, process.ReadFloat(address + 4));
            }
            return 0;
        }



        public static Int32 StopAnisLayerRange(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            int heroAddress = oCNpc.Player(process).Address;
            int ownerAddress = new zCModel(process, process.ReadInt(address)).Owner.Address;

            if (heroAddress == ownerAddress)
            {
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 5, Program.Player, process.ReadInt(address + 4), process.ReadInt(address + 8));
            }
            return 0;
        }

        static long time = 0;
        public static Int32 FadeOutAnisLayerRange(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            int heroAddress = oCNpc.Player(process).Address;
            int ownerAddress = new zCModel(process, process.ReadInt(address)).Owner.Address;

            if (heroAddress == ownerAddress)
            {
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 4, Program.Player, process.ReadInt(address + 4), process.ReadInt(address + 8));
                zERROR.GetZErr(process).Report(3, 'G', "Fade out: " + new zCModel(process, process.ReadInt(address)).Owner.ObjectName.Value + " " + process.ReadInt(address + 4) + " " + process.ReadInt(address + 8), 0, "Program.cs", 0);
            }
            else
            {
                long lasttime = (DateTime.Now.Ticks - time)/10000;
                zERROR.GetZErr(process).Report(3, 'G', "Fade out: " + new zCModel(process, process.ReadInt(address)).Owner.ObjectName.Value + " " + process.ReadInt(address + 4) + " " + process.ReadInt(address + 8) + " " + lasttime, 0, "Program.cs", 0);
                time = DateTime.Now.Ticks;
            }
            return 0;
        }

        public static bool startAnimEnabled = false;


        public static Player isPlayerModel(zCModel model )
        {
            Process process = Process.ThisProcess();
            return StaticVars.spawnedPlayerDict[model.Owner.Address];
        }

        public static bool onModelAnim;
        /// <summary>
        /// Diese Funktion blockt und überträgt Animationen. Alle Animationen von NPC's die nicht kontrolliert werden, werden geblockt, außer startAnimEnabled ist gesetzt
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        
        public static Int32 oCStartAnim_ModelAnim(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            
            zCModel thisModel = new zCModel(process, process.ReadInt(address));

            NPC npc = null;
            //int heroAddress = oCNpc.Player(process).Address;
            int ownerAddress = thisModel.Owner.Address;

            Player player = StaticVars.spawnedPlayerDict[ownerAddress];
            if (player == null)
                return 0;
            zCModelAni modelAni = new zCModelAni(process, process.ReadInt(address + 4));
            if (modelAni == null || modelAni.Address == 0)
                return 0;
            int aniID = modelAni.GetAniID();
            player.lastAnimation = (short)aniID;

            //zERROR.GetZErr(process).Report(2, 'G', "Setting ani!", 0, "Program.cs", 0);

            //if (heroAddress != ownerAddress && thisModel.Owner.VobType == Gothic.zClasses.zCVob.VobTypes.Npc)
            //    npc = NPCHelper.getControllerByNPC(ownerAddress);

            //if (heroAddress == ownerAddress)
            //{
            //    byte[] arr = new byte[] { 0x83, 0xEC, 0x24 };
            //    process.Write(arr, Program.StartAnim.oldFuncInNewFunc.ToInt32());


            //    zCModelAni modelAni = new zCModelAni(process, process.ReadInt(address + 4));
            //    int aniID = modelAni.GetAniID();
            //    new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 1, Program.Player, aniID, process.ReadInt(address + 8));
            //}
            //else if (npc != null)
            //{
            //    byte[] arr = new byte[] { 0x83, 0xEC, 0x24 };
            //    process.Write(arr, Program.StartAnim.oldFuncInNewFunc.ToInt32());

            //    zCModelAni modelAni = new zCModelAni(process, process.ReadInt(address + 4));
            //    int aniID = modelAni.GetAniID();
            //    new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 1, npc.npcPlayer, aniID, process.ReadInt(address + 8));

            //}
            //else if (startAnimEnabled)
            //{
            //    byte[] arr = new byte[] { 0x83, 0xEC, 0x24 };
            //    process.Write(arr, Program.StartAnim.oldFuncInNewFunc.ToInt32());

            //    startAnimEnabled = false;
            //}
            //else if (thisModel.Owner.VobType == Gothic.zClasses.zCVob.VobTypes.Npc)
            //{
            //    //Ausschalten wenn es einer der anderen spieler ist
            //    byte[] arr = new byte[] { 0xC2, 0x08, 0x00 };
            //    process.Write(arr, Program.StartAnim.oldFuncInNewFunc.ToInt32());
            //}
            //else
            //{
            //    byte[] arr = new byte[] { 0x83, 0xEC, 0x24 };
            //    process.Write(arr, Program.StartAnim.oldFuncInNewFunc.ToInt32());
            //}
            
            return 0;
        }

        public static Int32 oCStopAnim_ModelAnimAktive(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            NPC npc = null;
            int heroAddress = oCNpc.Player(process).Address;
            zCVob owner = new zCModel(process, process.ReadInt(address)).Owner;
            if (heroAddress != owner.Address && owner.VobType == Gothic.zClasses.zCVob.VobTypes.Npc)
                npc = NPCHelper.getControllerByNPC(owner.Address);

            if (heroAddress == owner.Address)
            {
                zCModelAniActive ani = new zCModelAniActive(process, process.ReadInt(address + 4));
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 2, Program.Player, ani.ModelAni.GetAniID(), 0);
            }
            else if (npc != null)
            {
                zCModelAniActive ani = new zCModelAniActive(process, process.ReadInt(address + 4));
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 2, npc.npcPlayer, ani.ModelAni.GetAniID(), 0);
            }
            return 0;
        }

        public static Int32 oCFadeOut_ModelAnimAktive(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            NPC npc = null;
            int heroAddress = oCNpc.Player(process).Address;
            int ownerAddress = new zCModel(process, process.ReadInt(address)).Owner.Address;
            if (heroAddress != ownerAddress)
                npc = NPCHelper.getControllerByNPC(ownerAddress);

            if (heroAddress == ownerAddress)
            {
                zCModelAniActive ani = new zCModelAniActive(process, process.ReadInt(address + 4));
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 3, Program.Player, ani.ModelAni.GetAniID(), 0);
            }
            else if (npc != null)
            {
                zCModelAniActive ani = new zCModelAniActive(process, process.ReadInt(address + 4));
                new AnimationMessage().Write(Program.client.sentBitStream, Program.client, 3, npc.npcPlayer, ani.ModelAni.GetAniID(), 0);
            }
            return 0;
        }





        /// <summary>
        /// Todo: Eventuell streichen, Overlays werden anders synchronisiert... Eventuell mal durchtesten...
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 oCApplyOverlay(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                zString str = new zString(process, process.ReadInt(address + 4));
                new AnimationOverlayMessage().Write(Program.client.sentBitStream, Program.client, 1, str.Value, 0);
            }
            return 0;
        }

        public static Int32 oCApplyTimedOverlayMDS(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                zString str = new zString(process, process.ReadInt(address + 4));
                new AnimationOverlayMessage().Write(Program.client.sentBitStream, Program.client, 2, str.Value, process.ReadFloat(address+8));
            }
            return 0;
        }

        public static Int32 oCRemoveOverlay(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).Address == process.ReadInt(address))
            {
                zString str = new zString(process, process.ReadInt(address + 4));
                new AnimationOverlayMessage().Write(Program.client.sentBitStream, Program.client, 3, str.Value, 0);
            }
            return 0;
        }



       
    }
}
