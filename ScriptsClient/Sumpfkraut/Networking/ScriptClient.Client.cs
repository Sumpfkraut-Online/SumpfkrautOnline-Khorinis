using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;
using GUC.Utilities;

namespace GUC.Scripts.Sumpfkraut.Networking
{
    public partial class ScriptClient : ExtendedObject, GameClient.IScriptClient
    {
        public static ScriptClient Client { get { return (ScriptClient)GameClient.Client.ScriptObject; } }

        public static PacketWriter GetScriptMessageStream()
        {
            return GameClient.GetScriptMessageStream();
        }

        public static void SendScriptMessage(PacketWriter stream, NetPriority priority, NetReliability reliability)
        {
            GameClient.SendScriptMessage(stream, priority, reliability);
        }

        public virtual void ReadScriptMessage(PacketReader stream)
        {
        }

        public virtual void ReadScriptVobMessage(PacketReader stream, WorldObjects.BaseVob vob)
        {
            ((BaseVobInst)vob.ScriptObject).OnReadScriptVobMsg(stream);
        }

        partial void pSetControl(NPCInst npc)
        {
            Menus.PlayerInventory.Menu.Close();
        }

        partial void pSetToSpectator(WorldInst world, Vec3f pos, Angles ang)
        {
            Menus.PlayerInventory.Menu.Close();
        }
    }
}
