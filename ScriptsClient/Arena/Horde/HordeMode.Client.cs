using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    static partial class HordeMode
    {
        public static void ReadGameInfo(PacketReader stream)
        {
            string name = stream.ReadString();
            activeDef = HordeDef.GetDef(name);
            activeSectionIndex = stream.ReadByte();
        }

        public static void ReadStartMessage(PacketReader stream)
        {
            string name = stream.ReadString();
            activeDef = HordeDef.GetDef(name);
            activeSectionIndex = 0;
        }

        public static void ReadPhaseMessage(PacketReader stream)
        {
            Phase = (HordePhase)stream.ReadByte();
            activeSectionIndex = stream.ReadByte();

            Sumpfkraut.Menus.ScreenScrollText.AddText(Phase.ToString() + " " + activeSectionIndex, GUI.GUCView.Fonts.Menu);
        }


    }
}
