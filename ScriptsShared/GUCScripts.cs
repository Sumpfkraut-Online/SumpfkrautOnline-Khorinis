using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.Networking;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public bool OnClientConnection(Network.GameClient client)
        {
            new TFFA.TFFAClient(client); //Networking.ScriptClient(client);
            return true;
        }

        public Animations.AniJob CreateAniJob()
        {
            return new ScriptAniJob().BaseAniJob;
        }

        public Animations.Animation CreateAnimation()
        {
            return new ScriptAni().BaseAni;
        }

        public Animations.Overlay CreateOverlay()
        {
            return new ScriptOverlay().BaseOverlay;
        }

        public Models.Model CreateModel()
        {
            return new ModelDef().BaseDef;
        }

        public WorldObjects.World CreateWorld()
        {
            return new WorldInst().BaseWorld;
        }

        public WorldObjects.BaseVob CreateVob(VobTypes type)
        {
            BaseVobInst vob;
            switch (type)
            {
                case VobTypes.Vob:
                    vob = new VobInst();
                    break;
                case VobTypes.Item:
                    vob = new ItemInst();
                    break;
                case VobTypes.NPC:
                    vob = new NPCInst();
                    break;
                case VobTypes.Projectile:
                    vob = new ProjInst();
                    break;
                default:
                    throw new Exception("Unsupported VobType: " + type);
            }
            return vob.BaseInst;
        }


        public WorldObjects.Instances.BaseVobInstance CreateInstance(VobTypes type)
        {
            BaseVobDef def;
            switch (type)
            {
                case VobTypes.Vob:
                    def = new VobDef();
                    break;
                case VobTypes.NPC:
                    def = new NPCDef();
                    break;
                case VobTypes.Item:
                    def = new ItemDef();
                    break;
                case VobTypes.Projectile:
                    def = new ProjDef();
                    break;
                default:
                    throw new Exception("Unsupported VobType: " + type);
            }

            return def.BaseDef;
        }
    }
}
