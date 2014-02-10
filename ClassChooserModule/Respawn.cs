using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using Injection;
using Network;
using Gothic.zClasses;
using WinApi;
using System.Windows.Forms;
using Gothic.zTypes;
using GMP.Helper;

namespace ClassChooserModule
{
    public class Respawn : UpdateModule
    {
        public bool loaded;
        public long time;
        public bool died;

      


        public override void update(Network.Module module)
        {
            Process process = Process.ThisProcess();
            if (!loaded && Program.FullLoaded && StaticVars.Ingame && !Program.FirstTime)
            {
                //Neuer SpawnPunkt setzen!
                if (ClassChooser.gc.Spawn.Count != 0)
                {
                    Random rand = new Random();
                    int lang = rand.Next(0, ClassChooser.gc.Spawn.Count);
                    NPCHelper.SetRespawnPoint(ClassChooser.gc.Spawn[lang]);

                }
                loaded = true;
            }

            if (Program.FullLoaded && ClassChooser.classes.respawnTime != -1)
            {
                //HP Kontrollieren!
                
                if (!died && oCNpc.Player(process).HP == 0)
                {
                    time = DateTime.Now.Ticks;
                    died = true;
                }


                if (died && time + ClassChooser.classes.respawnTime * 10000*1000 < DateTime.Now.Ticks)
                {
                    if (oCNpc.Player(process).HP != 0)
                    {
                        died = false;
                        return;
                    }

                    if (!Player.isSameMap(Program.Player.actualMap, StaticVars.serverConfig.World))
                    {
                        zString nowlevel = zString.Create(process, StaticVars.serverConfig.World);
                        oCGame.Game(process).ChangeLevel(nowlevel, nowlevel);
                        nowlevel.Dispose(); 
                    }

                    ClassChooser.SetClass(ClassChooser.gc);
                    died = false;

                    if (ClassChooser.gc.Respawn.Count != 0)
                    {
                        Random rand = new Random();
                        int lang = rand.Next(0, ClassChooser.gc.Respawn.Count);
                        NPCHelper.SetRespawnPoint(ClassChooser.gc.Respawn[lang]);
                    }else if (ClassChooser.gc.Spawn.Count != 0)
                    {
                        Random rand = new Random();
                        int lang = rand.Next(0, ClassChooser.gc.Spawn.Count);
                        NPCHelper.SetRespawnPoint(ClassChooser.gc.Spawn[lang]);
                    }
                    else if (StaticVars.serverConfig.Spawn.Count != 0)
                    {
                        Random rand = new Random();
                        int lang = rand.Next(0, StaticVars.serverConfig.Spawn.Count);
                        NPCHelper.SetRespawnPoint(StaticVars.serverConfig.Spawn[lang]);
                    }

                    

                }
            }
        }
    }
}
