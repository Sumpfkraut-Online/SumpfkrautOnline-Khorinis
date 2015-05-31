using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using RakNet;
using Gothic.zStruct;
using WinApi;
using Gothic.zClasses;
using GUC.WorldObjects.Character;
using Gothic.zTypes;
using GUC.Enumeration;

namespace GUC.Sumpfkraut.Ingame
{
    class VoiceMessage : IMessage
    {
        Process proc;

        zTSound3DParams param;
        zCSndSys_MSS ss;

        public VoiceMessage()
        {
            proc = Process.ThisProcess();

            param = zTSound3DParams.Create(proc);
            //param.Ambient3D = true;    
            //param.Volume = 1f;
            //param.Radius = -1f;

            ss = zCSndSys_MSS.SoundSystem(proc);
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {

        }

        public void SendVoice(int num)
        {

        }

        private void PlayVoice(Player pl, int num)
        {
            oCNpc npc = new oCNpc(proc, pl.Address);
            String vcmd = String.Format("SVM_{0}_{1}.WAV",7 /*npc.Voice*/,((VoiceCommands)num).ToString());
            using (zString z = zString.Create(proc,vcmd))
            {
                ss.PlaySound3D(z, npc, 0, param);
            }
        }
    }
}
