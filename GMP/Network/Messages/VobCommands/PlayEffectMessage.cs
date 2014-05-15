using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace GUC.Network.Messages.VobCommands
{
    class PlayEffectMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int vobID = 0, targetID = 0, effectLevel = 0, damage = 0, damageType = 0;
            bool isProjectile = false;
            String effect = "";

            stream.Read(out vobID);
            stream.Read(out effect);
            stream.Read(out targetID);
            stream.Read(out effectLevel);
            stream.Read(out damage);
            stream.Read(out damageType);
            stream.Read(out isProjectile);

            
            Vob vob = null;
            sWorld.VobDict.TryGetValue(vobID, out vob);
            if (vob == null)
                throw new Exception("Vob was not found: "+vobID);

            Vob targetVob = null;
            if (vobID != 0)
            {
                sWorld.VobDict.TryGetValue(targetID, out targetVob);
                if (targetVob == null)
                    throw new Exception("Target-Vob was not found: " + vobID);
            }

            if (vob.Address == 0 || !vob.IsSpawned)
                return;
            Process process = Process.ThisProcess();
            zCVob gVob = new zCVob(process, vob.Address);
            zCVob gTargetVob = null;

            if (targetVob != null && targetVob.Address != 0 && targetVob.IsSpawned)
                gTargetVob = new zCVob(process, targetVob.Address);
            else
                gTargetVob = new zCVob(process, 0);

            
            using (zString str = zString.Create(process, effect))
                oCVisualFX.CreateAndPlay(process, str, gVob, gTargetVob, effectLevel, damage, damageType, isProjectile ? 1 : 0);
        }
    }
}
