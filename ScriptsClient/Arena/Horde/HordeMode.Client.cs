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
            SetPhase((HordePhase)stream.ReadByte());
        }

        public static void ReadStartMessage(PacketReader stream)
        {
            string name = stream.ReadString();
            activeDef = HordeDef.GetDef(name);
        }

        public static void ReadPhaseMessage(PacketReader stream)
        {
            SetPhase((HordePhase)stream.ReadByte());
        }

        static void SetPhase(HordePhase phase)
        {
            string screenMsg;
            switch (phase)
            {
                case HordePhase.Fight:
                    screenMsg = "Fight!";
                    break;
                case HordePhase.Victory:
                    screenMsg = "Sieg!";
                    HordeBoardScreen.Instance.Open();
                    break;
                case HordePhase.Lost:
                    screenMsg = "Niederlage!";
                    HordeBoardScreen.Instance.Open();
                    break;
                default: return;
            }

            Phase = phase;
            ScreenScrollText.AddText(screenMsg, GUI.GUCView.Fonts.Menu);
            OnPhaseChange?.Invoke(phase);
        }
    }
}
