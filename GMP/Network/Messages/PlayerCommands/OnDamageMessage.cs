using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zStruct;
using Gothic.zClasses;
using GUC.WorldObjects;
using RakNet;
using Gothic.zTypes;
using GUC.Types;
using GUC.Network;
using GUC.Enumeration;
using WinApi;
using GUC.WorldObjects.Character;

namespace GUC.Network.Messages.PlayerCommands
{
    class OnDamageMessage : IMessage
    {
        public static void Write(oSDamageDescriptor oDD, oCNpc victim)
        {
            if (!sWorld.SpawnedVobDict.ContainsKey(victim.Address))
                throw new Exception("Victim: "+victim.Address+" "+victim.Name.Value+" was not found!");

            Vob vicProto = sWorld.SpawnedVobDict[victim.Address];
            Vob attProto = null;

            Vob weaponVob = null;
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.OnDamageMessage);



            zVec3 locHit = oDD.LocationHit;
            zVec3 dirFly = oDD.DirectionFly;
            zCVob attacker = oDD.AttackerNPC;
            int weaponMode = oDD.ModeWeapon;
            int spellID = oDD.SpellID;
            zString weapon = oDD.Weapon.ObjectName;

            byte sendFlags = 0;
            if (locHit.Address != 0)
                sendFlags |= 1;
            if (dirFly.Address != 0)
                sendFlags |= 2;
            if (attacker.Address != 0)
            {
                if (!sWorld.SpawnedVobDict.ContainsKey(attacker.Address))
                    throw new Exception("Attacker: " + attacker.Address + " " + attacker.ObjectName.Value + " was not found!");
                attProto = sWorld.SpawnedVobDict[attacker.Address];
                
                sendFlags |= 4;
            }
            if (weaponMode != 0)
                sendFlags |= 8;
            if (spellID > 100)
                sendFlags |= 16;
            if (oDD.Weapon.Address != 0)
            {
                if (!sWorld.SpawnedVobDict.ContainsKey(oDD.Weapon.Address))
                    throw new Exception("Weapon: " + oDD.Weapon.Address + " " + oDD.Weapon.ObjectName.Value + " was not found!");
                weaponVob = sWorld.SpawnedVobDict[oDD.Weapon.Address];
                sendFlags |= 32;
            }
            if (oDD.DamageType == oSDamageDescriptor.DamageTypes.DAM_FALL)
                sendFlags |= 64;

            stream.Write(vicProto.ID);
            stream.Write((byte)oDD.ModeDamage);
            stream.Write(sendFlags);

            if (locHit.Address != 0)
                stream.Write(new Vec3f(locHit.X, locHit.Y, locHit.Z));
            if (dirFly.Address != 0)
                stream.Write(new Vec3f(dirFly.X, dirFly.Y, dirFly.Z));
            if (attacker.Address != 0)
                stream.Write(attProto.ID);
            if (weaponMode != 0)
                stream.Write(weaponMode);
            if (spellID > 100)
                stream.Write(spellID);
            if (oDD.Weapon.Address != 0)
                stream.Write(weaponVob.ID);
            if ((sendFlags & 64) == 64)
                stream.Write(victim.HumanAI.FallDownDistanceY);
            
            
            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void Read(BitStream stream, Packet packet, Client client)
        {
            Vec3f locHit = null, flydir = null;
            int victim = 0, attacker = 0, weaponMode = 0, spellID = 0, weapon = 0;
            byte sendFlags, damageMode;

            stream.Read(out victim);
            stream.Read(out damageMode);
            stream.Read(out sendFlags);

            if ((sendFlags &= 1) == 1)
                stream.Read(out locHit);
            if ((sendFlags &= 2) == 2)
                stream.Read(out flydir);
            if ((sendFlags &= 4) == 4)
                stream.Read(out attacker);
            if ((sendFlags &= 8) == 8)
                stream.Read(out weaponMode);
            if ((sendFlags &= 16) == 16)
                stream.Read(out spellID);
            if ((sendFlags &= 32) == 32)
                stream.Read(out weapon);


            Process Process = Process.ThisProcess();

            oCNpc victimNPC = null, attackerNPC = null;
            oCItem weaponItem = null;
            if (victim != 0)
                victimNPC = new oCNpc(Process, sWorld.VobDict[victim].Address);
            if (attacker != 0)
                attackerNPC = new oCNpc(Process, sWorld.VobDict[attacker].Address);
            if(weapon != 0)
                weaponItem = new oCItem(Process, sWorld.VobDict[weapon].Address);
            
            IntPtr ptr = Process.Alloc(500);
            byte[] arr = new byte[500];
            Process.Write(arr, ptr.ToInt32());

            oSDamageDescriptor oDD = new oSDamageDescriptor(Process, ptr.ToInt32());
            oDD.AttackerNPC = attackerNPC;
            oDD.AttackerVob = attackerNPC;
            oDD.Weapon = weaponItem;
            oDD.ModeDamage = damageMode;
            oDD.ModeWeapon = weaponMode;


            victimNPC.OnDamage(oDD);


            Process.Free(ptr, 500);
        }
    }
}
