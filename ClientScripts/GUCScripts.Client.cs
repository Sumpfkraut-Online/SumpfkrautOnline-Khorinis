using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using GUC.Log;
using GUC.Scripting;
using System.Reflection;
using System.IO;
using GUC.Client.Scripts.Sumpfkraut.Menus;
using GUC.Client.Scripts.Sumpfkraut.Menus.MainMenus;
using GUC.Client.Scripts.Sumpfkraut;
using GUC.Network;
using GUC.Enumeration;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;
using GUC.Scripts.Sumpfkraut.WorldSystem;

namespace GUC.Scripts
{
    public partial class GUCScripts : ScriptInterface
    {
        public static bool Ingame = false;

        public GUCScripts()
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
            Logger.Log("SumpfkrautOnline ClientScripts loaded!");
        }

        static Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Substring(0, args.Name.IndexOf(','));

            Assembly asm = Assembly.LoadFrom(Path.GetFullPath("System\\Multiplayer\\UntoldChapters\\SumpfkrautOnline\\" + name + ".dll"));
            if (asm == null)
            {
                asm = Assembly.LoadFrom(Path.GetFullPath("Multiplayer\\UntoldChapters\\SumpfkrautOnline\\" + name + ".dll"));
            }
            return asm;
        }

        public void Update(long ticks)
        {
            GUCMenu.UpdateMenus(ticks);
        }

        public void StartOutgame()
        {
            InputControl.Init();
            MainMenu.Menu.Open();
            Logger.Log("Outgame started.");
        }

        public void StartIngame()
        {
            InputControl.Init();
            MainMenu.CloseActiveMenus();
            Ingame = true;
            Logger.Log("Ingame started.");
        }

        public void OnCreateInstanceMsg(VobTypes type, PacketReader stream)
        {
            BaseVobDef def;
            switch (type)
            {
                case VobTypes.Vob:
                    def = new VobDef(stream);
                    break;
                case VobTypes.NPC:
                    def = new NPCDef(stream);
                    break;
                case VobTypes.Item:
                    def = new ItemDef(stream);
                    break;
                default:
                    Logger.LogError("Unknown VobDef-Type! " + type);
                    return;
            }

            def.Create();
        }

        public void OnDeleteInstanceMsg(WorldObjects.Instances.BaseVobInstance instance)
        {
            ((BaseVobDef)instance.ScriptObject).Delete();
        }

        public void OnSpawnVobMsg(VobTypes type, PacketReader stream)
        {
            BaseVobInst inst;
            switch (type)
            {
                case VobTypes.Vob:
                    inst = new VobInst(stream);
                    break;
                case VobTypes.NPC:
                    inst = new NPCInst(stream);
                    break;
                case VobTypes.Item:
                    inst = new ItemInst(stream);
                    break;
                default:
                    Logger.LogError("Unknown VobDef-Type! " + type);
                    return;
            }
            inst.Spawn(WorldInst.Current);
        }

        public void OnDespawnVobMsg(WorldObjects.BaseVob vob)
        {
            vob.Despawn();
        }

        public void OnLoadWorldMsg(out WorldObjects.World world, PacketReader stream)
        {
            WorldInst w = new WorldInst(stream);
            w.Load();
            world = w.BaseWorld;
            WorldInst.Current = w;
        }
    }
}
