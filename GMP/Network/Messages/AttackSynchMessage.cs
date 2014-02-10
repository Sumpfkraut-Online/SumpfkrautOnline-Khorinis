using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;
using Injection;
using Gothic.zClasses;
using WinApi;
using GMP.Injection.Hooks;
using Gothic.zStruct;
using RakNet;
using Gothic.zTypes;
using GMP.Modules;

namespace GMP.Network.Messages
{
    public class AttackSynchMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int id1; int id2; byte modeDamage; byte modeWeapon;
            byte item;

            stream.Read(out id1);
            stream.Read(out id2);
            stream.Read(out modeDamage);
            stream.Read(out modeWeapon);
            stream.Read(out item);

            Player victim = StaticVars.AllPlayerDict[id1];
            if (victim == null || !victim.isSpawned || victim.NPCAddress == 0)
                return;
            Player attacker = StaticVars.AllPlayerDict[id2];
            if (attacker == null || !attacker.isSpawned || attacker.NPCAddress == 0)
                return;

            Process Process = Process.ThisProcess();
            oCNpc attackerNPC = new oCNpc(Process, 0);
            if(attacker != null)
                attackerNPC = new oCNpc(Process, attacker.NPCAddress);

            oCItem items = new oCItem(Process, 0);
            if (item == 1 && attackerNPC.Address != 0)
                items = attackerNPC.GetEquippedMeleeWeapon();
            else if (item == 2 && attackerNPC.Address != 0)
                items = attackerNPC.GetEquippedRangedWeapon();


            IntPtr ptr = Process.Alloc(500);
            byte[] arr = new byte[500];
            Process.Write(arr, ptr.ToInt32());
            oSDamageDescriptor oDD = new oSDamageDescriptor(Process, ptr.ToInt32());
            oDD.AttackerNPC = attackerNPC;
            oDD.AttackerVob = attackerNPC;
            oDD.Weapon = items;
            oDD.ModeDamage = modeDamage;
            oDD.ModeWeapon = modeWeapon;

            //Zum Sichergehen, other und self setzen...
            zString strOther = zString.Create(Process, "OTHER");
            zCParser.getParser(Process).SetInstance(strOther, attackerNPC.Address);
            strOther.Dispose();

            zString strSELF = zString.Create(Process, "SELF");
            zCParser.getParser(Process).SetInstance(strSELF, victim.NPCAddress);
            strSELF.Dispose();

            AI.onDamageAvailable = true;
            AI.SendOnDamage = false;
            new oCNpc(Process, victim.NPCAddress).OnDamage(oDD);
            Process.Free(ptr, 500);
        }

        public void Write(RakNet.BitStream stream, Client client, int id1, int id2, byte modeDamage, byte modeWeapon,
            byte item, String weapon)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.AttackSynchMessage);
            stream.Write(id1);
            stream.Write(id2);
            stream.Write(modeDamage);
            stream.Write(modeWeapon);
            stream.Write(item);
            stream.Write(weapon);
            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.AttackSynchMessage))
                StaticVars.sStats[(int)NetWorkIDS.AttackSynchMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.AttackSynchMessage] += 1;
        }
    }
}
