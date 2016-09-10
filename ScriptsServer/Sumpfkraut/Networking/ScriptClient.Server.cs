using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Types;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ScriptObject, GameClient.IScriptClient
    {
        public bool IsAllowedToConnect()
        {
            return true;
        }

        partial void pOnConnect()
        {
            this.SetToSpectator(WorldInst.Current, new Vec3f(), new Vec3f(0, 0, 1));
        }

        public static int GetCount()
        {
            return GameClient.Count;
        }

        public virtual void ReadScriptMessage(PacketReader stream)
        {
        }
    }
}
