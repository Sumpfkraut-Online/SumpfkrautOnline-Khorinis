using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Network.Messages.Connection;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;
using GUC.Network;
using Gothic.zClasses;
using WinApi;
using GUC.Types;
using GUC.WorldObjects;
using GUC.Network.Messages.NpcCommands;
using Gothic.mClasses;
using WinApi.User.Enumeration;
using Gothic.zTypes;
using Gothic.zStruct;
using GUC.Network.Messages.PlayerCommands;

namespace GUC.States
{
    class GameState : AbstractState
    {
        protected long lastPlayerPosUpdate = 0;
        protected long lastNPCPosUpdate = 0;

        protected PlayerKeyMessage pkm;
        public override void Init()
        {
            if (_init)
                return;

            Process process = Process.ThisProcess();
            if (oCNpc.Player(process).MagBook.Address == 0)
            {
                oCMag_Book magBook = oCMag_Book.Create(process);
                oCNpc.Player(process).MagBook = magBook;

                magBook.SetOwner(oCNpc.Player(process));
            }

            NPCSpawnMessage.Write();

            StealContainer sc = new StealContainer(Process.ThisProcess());
            sc.Enable();

            pkm = new PlayerKeyMessage();
            pkm.Init();


            

            _init = true;
        }

        static zString SoundStr = null;
        static zTSound3DParams SoundParam = null;
        static int soundInt = 0;
        public override void update()
        {
            Process process = Process.ThisProcess();

            long now = DateTime.Now.Ticks;

            if (Program.newWorld)
            {
                sWorld.getWorld(oCGame.Game(process).World.WorldFileName.Value).SpawnWorld();
                Program.newWorld = false;
            }


            if (InputHooked.IsPressed(process, (int)VirtualKeys.Numpad5))
            {
                if (SoundStr == null)
                {
                    SoundStr = zString.Create(process, "DIA_DRAGONTALK_MAIN_4_20_04.WAV");
                    SoundParam = zTSound3DParams.Create(process);

                    soundInt = zCSndSys_MSS.SoundSystem(process).PlaySound3D(SoundStr, oCNpc.Player(process).Enemy, soundInt, SoundParam);
                }
                process.Write(process.ReadInt(SoundParam.Address) + 0x158, SoundParam.Address); 
                //+0x158
                int si2 = zCSndSys_MSS.SoundSystem(process).UpdateSound3D(ref soundInt, SoundParam);
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Play-Sound: " + soundInt + ", " + si2, 0, "Program.cs", 0);
            }

            if (InputHooked.IsPressed(process, (int)VirtualKeys.Numpad7))
            {
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Mag-Book-: " + oCNpc.Player(process).Enemy.MagBook.SpellItems.Size, 0, "Program.cs", 0);
                
            }


            if (lastPlayerPosUpdate + 10000 * 200 < now )
            {
                SendPlayerPosition(process, Player.Hero);
                NPCUpdateMessage.Write(Player.Hero);

                //foreach (NPC iNPC in Player.Hero.NPCControlledList)
                //{
                //    SendPlayerPosition(process, iNPC);
                //    NPCUpdateMessage.Write(iNPC);
                //}
                lastPlayerPosUpdate = now;
            }

            if (lastNPCPosUpdate + 10000 * 500 < now)
            {
                //SendPlayerPosition(process, Player.Hero);
                //NPCUpdateMessage.Write(Player.Hero);

                foreach (NPC iNPC in Player.Hero.NPCControlledList)
                {
                    SendPlayerPosition(process, iNPC);
                    NPCUpdateMessage.Write(iNPC);
                }
                lastNPCPosUpdate = now;
            }

            pkm.update();

            Program.client.Update();
        }

        protected void SendPlayerPosition(Process process, NPCProto proto)
        {
            if (proto.Address != 0)
            {
                oCNpc npc = new oCNpc(process, proto.Address);
                proto.Position = (Vec3f)npc.TrafoObjToWorld.getPosition();
                proto.Direction = (Vec3f)npc.TrafoObjToWorld.getDirection();
            }
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkIDS.SetVobPosDirMessage);
            stream.Write(proto.ID);

            stream.Write(proto.Position);
            stream.Write(proto.Direction);

            Program.client.client.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

    }
}
