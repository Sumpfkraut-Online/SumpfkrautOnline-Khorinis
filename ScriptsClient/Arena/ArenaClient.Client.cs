using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Utilities;

namespace GUC.Scripts.Arena
{
    partial class ArenaClient
    {
        new public static ArenaClient Client { get { return (ArenaClient)ScriptClient.Client; } }

        public static void SendCharCreationMessage(CharCreationInfo info)
        {
            var stream = GetScriptMessageStream();
            stream.Write((byte)ScriptMessages.CharCreation);
            info.Write(stream);
            SendScriptMessage(stream, PktPriority.Low, PktReliability.Reliable);
        }

        static LockTimer requestTime = new LockTimer(500);
        public static void SendDuelRequest(NPCInst target)
        {
            if (requestTime.IsReady)
            {
                var stream = GetScriptMessageStream();
                stream.Write((byte)ScriptMessages.DuelRequest);
                stream.Write((ushort)target.ID);
                SendScriptMessage(stream, PktPriority.Low, PktReliability.Reliable);
            }
        }

        public override void ReadScriptMessage(PacketReader stream)
        {
            ScriptMessages id = (ScriptMessages)stream.ReadByte();
            switch (id)
            {
                case ScriptMessages.ScreenMessage:
                    string msg = stream.ReadString();
                    var color = stream.ReadColorRGBA();
                    Log.Logger.Log(msg);
                    break;
            }
        }
    }
}
