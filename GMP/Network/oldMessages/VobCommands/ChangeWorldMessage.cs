using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using Gothic.zClasses;
using WinApi;
using Gothic.zTypes;

namespace GUC.Network.Messages.VobCommands
{
    class ChangeWorldMessage : IMessage
    {
        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int plID = 0;
            String levelName = "";
            stream.Read(out plID);
            stream.Read(out levelName);

            if (Player.Hero.ID != plID)
            {
                Vob pl = sWorld.VobDict[plID];
                pl.Despawn();
                sWorld.getWorld(levelName).addVob(pl);
                
                pl.Spawn(pl.Map, pl.Position, pl.Direction);

            }
            else
            {
                zString ln = zString.Create(Process.ThisProcess(), levelName);
                oCGame.Game(Process.ThisProcess()).ChangeLevel(ln, ln);
                ln.Dispose();
            }
        }
    }
}
