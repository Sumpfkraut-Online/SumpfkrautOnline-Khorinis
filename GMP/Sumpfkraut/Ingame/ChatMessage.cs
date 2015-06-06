using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using GUC.WorldObjects;
using GUC.Network;
using RakNet;
using Gothic.zClasses;

namespace GUC.Sumpfkraut.Ingame
{
    class ChatMessage : IMessage
    {
        public delegate void ReceiveMessageHandler(ChatTextType type, string sender, string message);
        public ReceiveMessageHandler OnReceiveMessage;

        oCNpc npc = null;

        public void SendMessage(string message)
        {
            Process process = Process.ThisProcess();
            
            /*zCVob vob = oCNpc.Create(process);
            oCNpc npc = new oCNpc(Process.ThisProcess(), vob.Address);
            npc.SetVisual("Shadow.mds");
            npc.SetAdditionalVisuals("Sha_Body", 0, 0, "", 0, 0, 0);
            using (Gothic.zTypes.zString z = Gothic.zTypes.zString.Create(process, "Schattenläufer"))
                npc.SetName(z);

            //npc.SetAI()

            float[] pos = Player.Hero.Pos;

            vob.TrafoObjToWorld.setPosition(pos);
            oCGame.Game(process).World.AddVob(vob);*/

            if (npc == null)
            {
                npc = oCObjectFactory.GetFactory(process).CreateNPC("Shadowbeast");
                npc.SetVisual("SHADOW.MDS");
                npc.SetAdditionalVisuals("Sha_Body", 0, 0, "", 0, 0, 0);

                oCGame.Game(process).World.AddVob(npc);
            }
            npc.GetModel().StartAnimation("T_WARN");

            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.ChatMessage);

            stream.Write(Player.Hero.ID);
            stream.Write(message);

            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

            if (test == null)
            {
                test = new GUI.GUCMenuText("", 10, 10, true);
                test.Show();
            }
        }

        Sumpfkraut.GUI.GUCMenuText test = null;

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type;
            stream.Read(out type);
            ChatTextType ctt = (ChatTextType)type;

            string name = null;
            if (ctt != ChatTextType.RPGlobal && ctt != ChatTextType.RPEvent
                && ctt != ChatTextType.OOCEvent && ctt != ChatTextType._Error && ctt != ChatTextType._Hint)
            {
                int ID;
                stream.Read(out ID);

                oCNpc sender = new oCNpc(Process.ThisProcess(), sWorld.VobDict[ID].Address);
                name = sender.Name.ToString();
            }

            string message;
            stream.Read(out message);

            if (OnReceiveMessage != null)
            {
                OnReceiveMessage(ctt, name, message);
            }
        }
    }
}
