using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Network;
using GUC.WorldObjects;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.Types;

namespace GUC.Scripts.TFFA
{
    public partial class TFFAClient : GameClient.IScriptClient
    {
        public PlayerClass Class = PlayerClass.None;
        public Team Team = Team.Spec;
        public string Name = "Spieler";

        GameClient baseClient;
        public GameClient BaseClient { get { return this.baseClient; } }

        public NPCInst Character { get { return this.baseClient.Character == null ? null : (NPCInst)this.baseClient.Character.ScriptObject; } }

        partial void pConstruct();
        public TFFAClient(GameClient client)
        {
            this.baseClient = client;
            client.ScriptObject = this;
            pConstruct();
        }

        public void SetControl(NPC npc)
        {
            this.SetControl((NPCInst)npc.ScriptObject);
        }

        public void SetControl(NPCInst npc)
        {
            this.baseClient.SetControl(npc.BaseInst);
        }

        public void OnReadProperties(PacketReader stream)
        {
        }

        public void OnWriteProperties(PacketWriter stream)
        {
        }

        public void SetToSpectator(World world, Vec3f pos, Vec3f dir)
        {
            this.SetToSpectator((WorldInst)world.ScriptObject, pos, dir);
        }

        partial void pSetToSpectator(WorldInst world, Vec3f pos, Vec3f dir);
        public void SetToSpectator(WorldInst world, Vec3f pos, Vec3f dir)
        {
            this.baseClient.SetToSpectate(world.BaseWorld, pos, dir);
            pSetToSpectator(world, pos, dir);
        }
    }
}
