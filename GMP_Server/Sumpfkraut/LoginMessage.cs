using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Server;
using GUC.Server.Network;
using GUC.Enumeration;

namespace GUC.Server.Sumpfkraut
{
    public class LoginMessage : IMessage
    {
        //only for networking
        public class CharInfo
        {
            public int SlotNum;
            public string Name;
            public int BodyMesh;
            public int BodyTex;
            public int HeadMesh;
            public int HeadTex;
            public float Fatness;
            public float BodyHeight;
            public float BodyWidth;
            public int Voice;
            public int FormerClass;

            public float posx;
            public float posy;
            public float posz;
        }

        public delegate bool OnCreateAccountHandler(string accName, string accPW);
        public event OnCreateAccountHandler OnCreateAccount;

        public delegate bool OnCreateCharacterHandler(string accName, string accPW, CharInfo ci);
        public event OnCreateCharacterHandler OnCreateCharacter;

        public delegate bool OnGetCharactersHandler(string accName, string accPW, ref List<CharInfo> chars);
        public event OnGetCharactersHandler OnGetCharacters;

        static LoginMessage msg;
        public static void Init(OnCreateAccountHandler CreateAccount, OnCreateCharacterHandler CreateCharacter, OnGetCharactersHandler GetCharacters)
        {
            if (!Program.server.MessageListener.ContainsKey((byte)NetworkID.LoginMessage))
            {
                Program.server.MessageListener.Add((byte)NetworkID.LoginMessage, new LoginMessage());
            }
            msg = (LoginMessage)Program.server.MessageListener[(byte)NetworkID.LoginMessage];
            msg.OnCreateAccount = CreateAccount;
            msg.OnCreateCharacter = CreateCharacter;
            msg.OnGetCharacters = GetCharacters;
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, GUC.Server.Network.Server server)
        {
            byte type;
            stream.Read(out type);

            string accName;
            string accPW;
            stream.Read(out accName);
            stream.Read(out accPW);

            if (type == (byte)LoginMessageType.GetChars)
            {
                List<CharInfo> chars = new List<CharInfo>();
                if (OnGetCharacters(accName, accPW, ref chars))
                {
                    SendCharList(packet.guid, chars);
                }
                else
                {
                    SendFailed(packet.guid);
                }
            }
            else if (type == (byte)LoginMessageType.CreateAcc)
            {
                if (OnCreateAccount(accName, accPW))
                {
                    SendCharList(packet.guid, new List<CharInfo>());
                }
                else
                {
                    SendFailed(packet.guid);
                }
            }
            else if (type == (byte)LoginMessageType.CreateChar)
            {
                CharInfo ci = new CharInfo();

                stream.Read(out ci.SlotNum);
                stream.Read(out ci.Name);

                stream.Read(out ci.BodyMesh);
                stream.Read(out ci.BodyTex);
                stream.Read(out ci.HeadMesh);
                stream.Read(out ci.HeadTex);
                stream.Read(out ci.Fatness);
                stream.Read(out ci.BodyHeight);
                stream.Read(out ci.BodyWidth);
                stream.Read(out ci.Voice);
                stream.Read(out ci.FormerClass);
                
                if (OnCreateCharacter(accName, accPW, ci))
                {
                    StartInWorld(ci, packet.guid);
                }
                else
                {
                    SendFailed(packet.guid);
                }
            }
            else if (type == (byte)LoginMessageType.LoginChar)
            {
                byte id;
                stream.Read(out id);

                List<CharInfo> chars = new List<CharInfo>();
                if (OnGetCharacters(accName, accPW, ref chars))
                {
                    StartInWorld(chars.Find(o => o.SlotNum == id), packet.guid);
                }
                else
                {
                    SendFailed(packet.guid);
                }
            }
        }

        private void SendCharList(RakNetGUID guid, List<CharInfo> chars)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.LoginMessage);
            stream.Write((byte)LoginMessageType.GetChars);
            stream.Write((byte)chars.Count);
            for (int i = 0; i < chars.Count; i++ )
            {
                stream.Write(chars[i].SlotNum);
                stream.Write(chars[i].Name);
                stream.Write(chars[i].BodyMesh);
                stream.Write(chars[i].BodyTex);
                stream.Write(chars[i].HeadMesh);
                stream.Write(chars[i].HeadTex);
                stream.Write(chars[i].Fatness);
                stream.Write(chars[i].BodyHeight);
                stream.Write(chars[i].BodyWidth);
                stream.Write(chars[i].FormerClass);
            }
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        private void SendFailed(RakNetGUID guid)
        {
            BitStream stream = Program.server.SendBitStream;
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.LoginMessage);
            stream.Write((byte)LoginMessageType.Failed);
            Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
        }

        private void StartInWorld(CharInfo ci, RakNetGUID guid)
        {
            if (ci == null)
                return;

            Player player = new Player(guid, ci.Name);
            player.Position = new Types.Vec3f(0, 0, 0);
            sWorld.addVob(player);

            RakNet.BitStream stream = Program.server.SendBitStream;
            stream.Reset();

            stream.Write(player.ID);

            stream.Write(sWorld.Day);
            stream.Write(sWorld.Hour);
            stream.Write(sWorld.Minute);

            stream.Write(sWorld.WeatherType);
            stream.Write(sWorld.StartRainHour);
            stream.Write(sWorld.StartRainMinute);
            stream.Write(sWorld.EndRainHour);
            stream.Write(sWorld.EndRainMinute);

            //Writing Spells:
            stream.Write((short)Spell.SpellList.Count);
            foreach (Spell spell in Spell.SpellList)
            {
                spell.Write(stream);
            }

            //Writing created item instances:
            stream.Write((short)ItemInstance.ItemInstanceList.Count);
            foreach (ItemInstance ii in ItemInstance.ItemInstanceList)
            {
                ii.Write(stream);
            }

            //ItemList:
            stream.Write(sWorld.ItemList.Count);
            foreach (Item item in sWorld.ItemList)
            {
                item.Write(stream);
            }

            //VobList:
            stream.Write(sWorld.VobList.Count);
            foreach (Vob vob in sWorld.VobList)
            {
                stream.Write((int)vob.VobType);
                vob.Write(stream);
            }

            //NPCList:
            stream.Write(sWorld.NpcList.Count);
            foreach (NPCProto proto in sWorld.NpcList)
            {
                proto.Write(stream);
            }
            //PlayerList:
            stream.Write(sWorld.PlayerList.Count);
            foreach (NPCProto proto in sWorld.PlayerList)
            {
                proto.Write(stream);
            }

            //World-SpawnList:
            stream.Write(sWorld.WorldDict.Count);
            foreach (KeyValuePair<String, World> worldPair in sWorld.WorldDict)
            {
                worldPair.Value.Write(stream);
            }

            //System.IO.File.WriteAllBytes("test.stream", stream.GetData());

            using (BitStream stream2 = new BitStream())
            {
                stream2.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream2.Write((byte)NetworkID.LoginMessage);
                stream2.Write((byte)LoginMessageType.LoginChar);

                GUC.Types.Zip.Compress(stream, 0, stream2);

                Program.server.ServerInterface.Send(stream2, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
            }

            GUC.Server.Network.Messages.VobCommands.CreateVobMessage.Write(player, guid);
            Scripting.Objects.Character.Player.isOnPlayerConnect((GUC.Server.Scripting.Objects.Character.Player)player.ScriptingNPC);

            player.ScriptingNPC.Spawn();
            player.ScriptingNPC.setFatness(ci.Fatness);
            player.ScriptingNPC.setScale(new Types.Vec3f(ci.BodyWidth, ci.BodyHeight, ci.BodyWidth));
            player.ScriptingNPC.setVisual("HUMANS.MDS", ((BodyMesh)ci.BodyMesh).ToString(), ci.BodyTex, 0, ((HeadMesh)ci.HeadMesh).ToString(), ci.HeadTex, 0);
        }
    }
}
