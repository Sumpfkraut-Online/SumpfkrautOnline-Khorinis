using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Server;
using GUC.Server.Network;
using GUC.Enumeration;



namespace GUC.Server.Sumpfkraut
{
    public class AnimationMenuMessage : IMessage
    {

        public AnimationMenuMessage()
        {
        }

        public static void Init()
        {
            if (!Program.server.MessageListener.ContainsKey((byte)NetworkID.AnimationMenuMessage))
            {
                Program.server.MessageListener.Add((byte)NetworkID.AnimationMenuMessage, new AnimationMenuMessage());
            }
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, GUC.Server.Network.Server server)
        {
            string animation = null;
            Player from = null;
            int mode = 1;
            try
            {
                int playerID = 0;
                stream.Read(out playerID);
                from = (Player)sWorld.VobDict[playerID];

                stream.Read(out animation);
                // mode: 1:Animation wird gestartet. 0: gestoppt.
                stream.Read(out mode);
            }
            catch { }
            finally
            {

                Scripting.Objects.Character.Player pl = (Scripting.Objects.Character.Player)from.ScriptingNPC;
                if (mode == 1)
                    pl.playAnimation(animation);
                else if (mode == 0)
                    pl.stopAnimation(animation);

            }
        }
    }
}
