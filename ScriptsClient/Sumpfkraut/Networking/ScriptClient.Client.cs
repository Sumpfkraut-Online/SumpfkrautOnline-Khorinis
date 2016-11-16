using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public static ScriptClient Client { get { return (ScriptClient)GameClient.Client.ScriptObject; } }
        
        public static PacketWriter GetScriptMessageStream()
        {
            return GameClient.GetScriptMessageStream();
        }

        public static void SendScriptMessage(PacketWriter stream, PktPriority priority, PktReliability reliability)
        {
            GameClient.SendScriptMessage(stream, priority, reliability);
        }

        public virtual void ReadScriptMessage(PacketReader stream)
        {
        }

        public virtual void ReadScriptVobMessage(PacketReader stream, WorldObjects.BaseVob vob)
        {
            NPCInst.Hero.OnReadScriptVobMsg(stream);
        }
    }
}
