using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GUC.Scripting;
using GUC.Scripts.Sumpfkraut.VobSystem;
using GUC.Scripts.Sumpfkraut.Visuals;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances.Mobs;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions.Mobs;
using GUC.Scripts.Sumpfkraut.WorldSystem;
using GUC.WorldObjects.VobGuiding;
using GUC.Scripts.Sumpfkraut.AI.GuideCommands;
using GUC.WorldObjects;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        partial void pConstruct();
        public GUCScripts()
        {
            pConstruct();
        }

        public Network.GameClient CreateClient()
        {
            return new Arena.ArenaClient().BaseClient; //new ScriptClient().BaseClient;
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

        public Models.ModelInstance CreateModelInstance()
        {
            return new ModelDef().BaseDef;
        }

        public World CreateWorld()
        {
            return new WorldInst().BaseWorld;
        }

        public GUCBaseVobInst CreateVob(byte type)
        {
            BaseVobInst vob;
            switch ((VobType)type)
            {
                case VobType.Vob:
                    vob = new VobInst();
                    break;
                case VobType.Mob:
                    vob = new MobInst();
                    break;
                case VobType.Item:
                    vob = new ItemInst();
                    break;
                case VobType.NPC:
                    vob = new NPCInst();
                    break;
                case VobType.Projectile:
                    vob = new ProjInst();
                    break;
                default:
                    throw new Exception("Unsupported VobType: " + (VobType)type);
            }
            return vob.BaseInst;
        }


        public WorldObjects.Definitions.GUCBaseVobDef CreateInstance(byte type)
        {
            BaseVobDef def;
            switch ((VobType)type)
            {
                case VobType.Vob:
                    def = new VobDef();
                    break;
                case VobType.Mob:
                    def = new MobDef();
                    break;
                case VobType.NPC:
                    def = new NPCDef();
                    break;
                case VobType.Item:
                    def = new ItemDef();
                    break;
                case VobType.Projectile:
                    def = new ProjDef();
                    break;
                default:
                    throw new Exception("Unsupported VobType: " + (VobType)type);
            }

            return def.BaseDef;
        }

        public GuideCmd CreateGuideCommand(byte type)
        {
            switch ((CommandType)type)
            {
                case CommandType.GoToPos:
                    return new GoToPosCommand();
                case CommandType.GoToVob:
                    return new GoToVobCommand();
                case CommandType.GoToVobLookAt:
                    return new GoToVobLookAtCommand();
                default:
                    throw new Exception("Unsupported guide command type: " + type);
            }
        }
    }
}
