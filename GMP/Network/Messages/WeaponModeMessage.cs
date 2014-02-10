using System;
using System.Collections.Generic;
using System.Text;
using Injection;
using WinApi;
using Gothic.zClasses;
using Network;
using RakNet;
using Gothic.zTypes;
using System.Windows.Forms;
using GMP.Modules;

namespace GMP.Net.Messages
{
    public class WeaponModeMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            if (!StaticVars.Ingame)
                return;

            int xxx = 0;
            try
            {
                int id = 0;
                byte type = 0;
                String mode;

                stream.Read(out id);
                stream.Read(out type);
                stream.Read(out mode);

                if (Program.Player == null || id == Program.Player.id)
                    return;

                Player pl = StaticVars.AllPlayerDict[id];
                if (pl == null || !pl.isSpawned || mode.Trim().Length == 0)
                    return;

                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, pl.NPCAddress);

                if (pl.NPCAddress == 0)
                    return;

                if (type == 1)
                {
                    zString str = zString.Create(process, mode);
                    npc.SetWeaponMode2(str);
                    str.Dispose();
                }
                else if (type == 2)
                {
                    npc.SetWeaponMode2(Convert.ToInt32(mode));
                }
                else if (type == 3)
                {
                    npc.SetWeaponMode(Convert.ToInt32(mode));
                }
                
            }
            catch (System.Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "WeaponModeMessage.cs", 0);
            }
        }

        public void Write(RakNet.BitStream stream, Client client,byte type, String mode)
        {
            if (mode.Trim() == "")
                return;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.WeaponModeMessage);
            stream.Write(Program.Player.id);
            stream.Write((byte)type);
            stream.Write(mode);

            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.WeaponModeMessage))
                StaticVars.sStats[(int)NetWorkIDS.WeaponModeMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.WeaponModeMessage] += 1;
        }
    }
}
