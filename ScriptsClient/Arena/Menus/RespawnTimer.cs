using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena.Menus
{
    static class RespawnTimer
    {
        public static long NextSpawnTime;
        static GUCVisualText textVis;

        static RespawnTimer()
        {
            GUCScripts.OnUpdate += Update;

            textVis = new GUCVisual().CreateTextCenterX("", GUCView.GetScreenSize().Y - 100);
            textVis.Font = GUCView.Fonts.Menu;
        }

        static void Update(long now)
        {
            double total = new TimeSpan(NextSpawnTime - GameTime.Ticks).TotalSeconds;
            if (total < 0)
            {
                NextSpawnTime += 10 * TimeSpan.TicksPerSecond;
                return;
            }

            int seconds = 1 + (int)total;

            bool show = false;
            if (0 < seconds && seconds < 10)
                if (TeamMode.TeamDef != null)
                {
                    if (ArenaClient.Client.ClassDef != null && TeamMode.Phase != TOPhases.None && TeamMode.Phase != TOPhases.Finish)
                    {
                        if (NPCInst.Hero != null)
                        {
                            if (NPCInst.Hero.IsDead)
                                show = true;
                        }
                        else if (ArenaClient.Client.IsSpecating)
                        {
                            show = true;
                        }
                    }
                }
                else // free mode
                {
                    if (NPCInst.Hero != null && NPCInst.Hero.IsDead)
                        show = true;
                }

            if (!show)
            {
                textVis.Parent.Hide();
            }
            else
            {
                textVis.Parent.Show();
                textVis.Text = "Spawn in " + seconds;
            }
        }
    }
}
