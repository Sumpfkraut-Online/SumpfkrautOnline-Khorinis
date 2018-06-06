using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.GUI;
using GUC.Scripts.Sumpfkraut.VobSystem.Instances;

namespace GUC.Scripts.Arena.GameModes.TDM
{
    static class RespawnNote
    {
        static GUCVisualText textVis;

        static RespawnNote()
        {
            textVis = new GUCVisual().CreateTextCenterX("Warte auf Respawn...", GUCView.GetScreenSize().Y - 100);
            textVis.Font = GUCView.Fonts.Menu;
        }

        static void Update(long now)
        {
            if (TDMMode.IsActive && NPCInst.Hero != null && NPCInst.Hero.IsDead)
            {
                textVis.Parent.Show();
            }
            else
            {
                textVis.Parent.Hide();
            }
        }

        public static void Toggle(bool activate)
        {
            if (activate)
                GUCScripts.OnUpdate += Update;
            else
                GUCScripts.OnUpdate -= Update;
        }
    }
}
