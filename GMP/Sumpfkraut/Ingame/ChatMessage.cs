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
        }

        public void setDirection(GUC.Types.Vec3f dir, zCVob vob)
        {
            dir = dir.normalise();

            Process process = Process.ThisProcess();

            Types.Vec3f zAxis = dir;
            Types.Vec3f up = new Types.Vec3f(0.0f, 0.0f, 0.0f);

            if (Math.Abs(zAxis.Y) > 0.5)
            {
                if (zAxis.Y > 0)
                    up.Z = -1.0f;
                else
                    up.Z = 1.0f;
            }
            else if (Math.Abs(zAxis.X) < 0.0001 && Math.Abs(zAxis.Y) < 0.0001)
            {
                if (zAxis.Y > -0.0001)
                {
                    up.Y = 1.0f;
                }
                else
                {
                    up.Y = -1.0f;
                }
            }
            else
            {
                up.Y = 1.0f;
            }



            Types.Vec3f xAxis = up.cross(zAxis).normalise();
            Types.Vec3f yAxis = zAxis.cross(xAxis).normalise();

            Matrix4 trafo = vob.TrafoObjToWorld;

            trafo.set(12, 0);
            trafo.set(13, 0);
            trafo.set(14, 0);
            trafo.set(15, 1);

            trafo.set(0, xAxis.X);
            trafo.set(4, xAxis.Y);
            trafo.set(8, xAxis.Z);

            trafo.set(1, yAxis.X);
            trafo.set(5, yAxis.Y);
            trafo.set(9, yAxis.Z);

            trafo.set(2, zAxis.X);
            trafo.set(6, zAxis.Y);
            trafo.set(10, zAxis.Z);


            Gothic.zTypes.zVec3 p = vob.GetPosition();
            vob.SetPositionWorld(p);

            p.Dispose();
        }

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
