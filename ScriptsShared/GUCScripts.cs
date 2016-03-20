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
using GUC.Models;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public AniJob CreateAniJob()
        {
            return new ScriptAniJob().BaseAniJob;
        }

        public Animation CreateAnimation()
        {
            return new ScriptAni().BaseAni;
        }

        public Overlay CreateOverlay()
        {
            return new ScriptOverlay().BaseOverlay;
        }

        public Model CreateModel()
        {
            return new ModelDef().BaseDef;
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
