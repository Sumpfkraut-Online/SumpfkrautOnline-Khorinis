using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using WinApi;
using Gothic.zClasses;

namespace GUC.Network.Messages.CameraCommands
{
    class CamToVobMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0;
            stream.Read(out vobID);

            if (vobID == 0 || !sWorld.VobDict.ContainsKey(vobID))
                throw new Exception("Vob not found!");
            Vob vob = sWorld.VobDict[vobID];

            if (vob.Address == 0)
                return;

            Process process = Process.ThisProcess();
            oCGame.Game(process).AICamera.SetTarget(new zCVob(process, vob.Address));


            CamToPlayerFront.destroyPlayerVob();
        }
    }
}
