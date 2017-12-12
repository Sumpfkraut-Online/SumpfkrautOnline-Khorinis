using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.Menus;

namespace GUC.Scripts.Arena
{
    static partial class HordeMode
    {
        public static void ReadGameInfo(PacketReader stream)
        {
            string name = stream.ReadString();
            activeDef = HordeDef.GetDef(name);
            activeSectionIndex = stream.ReadByte();
            SetPhase((HordePhase)stream.ReadByte());
        }

        public static void ReadStartMessage(PacketReader stream)
        {
            string name = stream.ReadString();
            activeDef = HordeDef.GetDef(name);
            activeSectionIndex = 0;
        }

        public static void ReadPhaseMessage(PacketReader stream)
        {
            activeSectionIndex = stream.ReadByte();
            SetPhase((HordePhase)stream.ReadByte());
        }

        static void SetPhase(HordePhase phase)
        {
            Phase = phase;
            if (phase == HordePhase.Intermission)
            {
                ScreenScrollText.AddText(ActiveSection.FinishedMessage, GUI.GUCView.Fonts.Menu);
            }
            else
            {
                ScreenScrollText.AddText("Fight!", GUI.GUCView.Fonts.Menu);
            }
        }
    }
}
