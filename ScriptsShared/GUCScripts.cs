using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Animations;
using GUC.Network;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.Visuals;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public AniJob CreateAniJob(PacketReader stream)
        {
            return new ScriptAniJob(stream).BaseAniJob;
        }

        public Animation CreateAnimation(PacketReader stream)
        {
            return new ScriptAni(stream).BaseAni;
        }

        public Item CreateInvItem(PacketReader stream)
        {
            return Sumpfkraut.VobSystem.Instances.ItemInst.ReadFromInvMsg(stream).BaseInst;
        }

        public bool OnClientConnection(GameClient client)
        {
            ScriptClient sc = new ScriptClient(client);
            return true;
        }
    }
}
