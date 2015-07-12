using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using Gothic.zClasses;
using RakNet;
using GUC.Enumeration;

namespace GUC.Client.Hooks
{
    public class hMagBook
    {
        public static Int32 Spell_Invest(String message)
        {
            Process Process = Program.Process;
            try
            {
               /* int address = Convert.ToInt32(message);

                oCMag_Book magBook = new oCMag_Book(Process, Process.ReadInt(address));
                if (oCNpc.Player(Process).MagBook.Address == magBook.Address)
                {
                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkID.SpellInvestMessage);
                    stream.Write(Player.Hero.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                    oCSpell spell = oCNpc.Player(Process).MagBook.GetSelectedSpell();
                    //zERROR.GetZErr(Process).Report(2, 'G', "Spell Invest: "+spell.Caster.ObjectName+"; "+spell.NPC.ObjectName+"; "+spell.Target.ObjectName, 0, "Program.cs", 0);
                }*/
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process).Report(2, 'G', "Exception: " + ex.Message + " " + ex.StackTrace + " " + ex.Source, 0, "Program.cs", 0);
            }
            return 0;
        }
    }
}
