using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;
using GUC.WorldObjects;
using GUC.Enumeration;
using RakNet;
using GUC.WorldObjects.Character;
using GUC.WorldObjects.Mobs;

namespace GUC.Hooks
{
    public class hMobInter
    {
        public static Int32 OnTrigger(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            try
            {
                zCVob str = new zCVob(process, process.ReadInt(address + 4));
                zCVob str2 = new zCVob(process, process.ReadInt(address + 8));
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));

                if (oCNpc.Player(process).Address == str2.Address)
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Trigger: " + mobInter.State + " | StateNum: " + mobInter.StateNum + " | " + mobInter.Rewind, 0, "Program.cs", 0);

                    if (!sWorld.SpawnedVobDict.ContainsKey(mobInter.Address))
                        return 0;
                    MobInter mI = (MobInter)sWorld.SpawnedVobDict[mobInter.Address];
                    mI.State = 1;

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.MobInterMessage);
                    stream.Write((byte)MobInterNetworkFlags.OnTrigger);
                    stream.Write(Player.Hero.ID);
                    stream.Write(mI.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hMobInter.cs", 0);
            }
            return 0;
        }

        public static Int32 OnUnTrigger(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();
            try
            {
                zCVob str = new zCVob(process, process.ReadInt(address + 4));
                zCVob str2 = new zCVob(process, process.ReadInt(address + 8));
                oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));

                if (oCNpc.Player(process).Address == str2.Address)
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "UnTrigger: " + mobInter.State + " | StateNum: " + mobInter.StateNum + " | " + mobInter.Rewind, 0, "Program.cs", 0);

                    if (!sWorld.SpawnedVobDict.ContainsKey(mobInter.Address))
                        return 0;
                    MobInter mI = (MobInter)sWorld.SpawnedVobDict[mobInter.Address];
                    mI.State = 0;

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.MobInterMessage);
                    stream.Write((byte)MobInterNetworkFlags.OnUnTrigger);
                    stream.Write(Player.Hero.ID);
                    stream.Write(mI.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hMobInter.cs", 0);
            }
            return 0;
        }



        public static Int32 StartInteraction(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            try
            {
                oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
                if (oCNpc.Player(process).Address == npc.Address)
                {
                    oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));

                    if (!sWorld.SpawnedVobDict.ContainsKey(mobInter.Address))
                        return 0;
                    MobInter mI = (MobInter)sWorld.SpawnedVobDict[mobInter.Address];

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.MobInterMessage);
                    stream.Write((byte)MobInterNetworkFlags.StartInteraction);
                    stream.Write(Player.Hero.ID);
                    stream.Write(mI.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hMobInter.cs", 0);
            }
            return 0;
        }

        public static Int32 StopInteraction(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            try
            {
                oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));

                if (oCNpc.Player(process).Address == npc.Address)
                {
                    oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));

                    if (!sWorld.SpawnedVobDict.ContainsKey(mobInter.Address))
                        return 0;
                    MobInter mI = (MobInter)sWorld.SpawnedVobDict[mobInter.Address];

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.MobInterMessage);
                    stream.Write((byte)MobInterNetworkFlags.StopInteraction);
                    stream.Write(Player.Hero.ID);
                    stream.Write(mI.ID);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hMobInter.cs", 0);
            }
            return 0;
        }




        public static Int32 PickLock(String message)
        {
            int address = Convert.ToInt32(message);
            Process process = Process.ThisProcess();

            try
            {
                oCNpc npc = new oCNpc(process, process.ReadInt(address + 4));
                int ch = process.ReadInt(address + 8);

                if (oCNpc.Player(process).Address == npc.Address)
                {
                    oCMobInter mobInter = new oCMobInter(process, process.ReadInt(address));

                    if (!sWorld.SpawnedVobDict.ContainsKey(mobInter.Address))
                        return 0;
                    MobInter mI = (MobInter)sWorld.SpawnedVobDict[mobInter.Address];

                    BitStream stream = Program.client.sentBitStream;
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)NetworkIDS.MobInterMessage);
                    stream.Write((byte)MobInterNetworkFlags.PickLock);
                    stream.Write(Player.Hero.ID);
                    stream.Write(mI.ID);
                    stream.Write((char)ch);
                    Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                }
            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', ex.ToString(), 0, "hMobInter.cs", 0);
            }
            return 0;
        }

    }
}
