using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Enumeration;
using GUC.WorldObjects.Character;
using Gothic.zClasses;
using WinApi;
using GUC.States;
using GUC.WorldObjects;
using System.IO;
using Gothic.zTypes;
using GUC.Types;
using System.Management;
using System.Security.Cryptography;
using GUC.Network;

namespace GUC.Sumpfkraut.Login
{
    class LoginMessage : IMessage
    {
        public static LoginMessage GetMsg()
        {
            if (!Program.client.messageListener.ContainsKey((byte)NetworkID.LoginMessage))
            {
                Program.client.messageListener.Add((byte)NetworkID.LoginMessage, new LoginMessage());
            }
            return (LoginMessage)Program.client.messageListener[(byte)NetworkID.LoginMessage];
        }

        //Only for networking
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
        }

        public event EventHandler OnFailed;

        public delegate void OnLoginHandler(List<CharInfo> chars);
        public event OnLoginHandler OnLogin;

        public void Login()
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.LoginMessage);
            stream.Write((byte)LoginMessageType.GetChars);

            stream.Write(StartupState.clientOptions.name);
            stream.Write(StartupState.clientOptions.password);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void Register()
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.LoginMessage);
            stream.Write((byte)LoginMessageType.CreateAcc);

            stream.Write(StartupState.clientOptions.name);
            stream.Write(StartupState.clientOptions.password);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void CreateNewCharacter(CharInfo ci)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.LoginMessage);
            stream.Write((byte)LoginMessageType.CreateChar);

            stream.Write(StartupState.clientOptions.name);
            stream.Write(StartupState.clientOptions.password);

            stream.Write(ci.SlotNum);
            stream.Write(ci.Name);
            stream.Write(ci.BodyMesh);
            stream.Write(ci.BodyTex);
            stream.Write(ci.HeadMesh);
            stream.Write(ci.HeadTex);
            stream.Write(ci.Fatness);
            stream.Write(ci.BodyHeight);
            stream.Write(ci.BodyWidth);
            stream.Write(ci.Voice);
            stream.Write(ci.FormerClass);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void StartInWorld(int id)
        {
            BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetworkID.LoginMessage);
            stream.Write((byte)LoginMessageType.LoginChar);

            stream.Write(StartupState.clientOptions.name);
            stream.Write(StartupState.clientOptions.password);

            stream.Write((byte)id);

            Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }

        public void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type;
            stream.Read(out type);

            if (type == (byte)LoginMessageType.Failed) //Something failed
            {
                if (OnFailed != null)
                    OnFailed(null, null);
            }
            else if (type == (byte)LoginMessageType.GetChars)
            {
                byte num;
                stream.Read(out num);

                List<CharInfo> chars = new List<CharInfo>();
                for (int i = 0; i < num; i++)
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
                    stream.Read(out ci.FormerClass);

                    chars.Add(ci);
                }

                if (OnLogin != null)
                    OnLogin(chars);
            }
            else if (type == (byte)LoginMessageType.LoginChar) //we're spawning as a character in the world
            {
                Player pl = new Player(true, StartupState.clientOptions.name);
                pl.Address = oCNpc.Player(Process.ThisProcess()).Address;
                pl.IsSpawned = true;
                pl.Position = (Vec3f)oCNpc.Player(Process.ThisProcess()).GetPositionArray();

                Player.Hero = pl;

                using (BitStream stream2 = new BitStream())
                {
                    Zip.Decompress(stream, stream2);
                    stream = stream2;

                    int id = 0;
                    stream.Read(out id);

                    Player.Hero.ID = id;
                    sWorld.addVob(Player.Hero);

                    int day = 0;
                    byte hour = 0, minute = 0;
                    stream.Read(out day);
                    stream.Read(out hour);
                    stream.Read(out minute);

                    sWorld.Day = day;
                    sWorld.Hour = hour;
                    sWorld.Minute = minute;

                    byte wt = 0, starthour = 0, startminute = 0, endhour = 0, endminute;

                    stream.Read(out wt);
                    stream.Read(out starthour);
                    stream.Read(out startminute);
                    stream.Read(out endhour);
                    stream.Read(out endminute);

                    sWorld.WeatherType = wt;
                    sWorld.StartRainHour = starthour;
                    sWorld.StartRainMinute = startminute;
                    sWorld.EndRainHour = endhour;
                    sWorld.EndRainMinute = endminute;

                    short spellCount = 0;
                    stream.Read(out spellCount);
                    for (int i = 0; i < spellCount; i++)
                    {
                        Spell spell = new Spell();
                        spell.Read(stream);
                        Spell.addItemInstance(spell);
                    }

                    short itemInstancesCount = 0;
                    stream.Read(out itemInstancesCount);
                    for (int i = 0; i < itemInstancesCount; i++)
                    {
                        ItemInstance ii = new ItemInstance();
                        ii.Read(stream);


                        ItemInstance.addItemInstance(ii);
                    }
                    CreateItems();

                    //ItemList:
                    int iLC = 0;
                    stream.Read(out iLC);
                    for (int i = 0; i < iLC; i++)
                    {
                        Item item = new Item();
                        item.Read(stream);
                        sWorld.addVob(item);
                    }

                    //Vob-List:
                    int vLC = 0;
                    stream.Read(out vLC);
                    for (int i = 0; i < vLC; i++)
                    {
                        int vobType = 0;
                        stream.Read(out vobType);
                        Vob vob = Vob.createVob((VobType)vobType);
                        vob.Read(stream);
                        sWorld.addVob(vob);
                    }

                    //NPC-List:
                    int nLC = 0;
                    stream.Read(out nLC);
                    for (int i = 0; i < nLC; i++)
                    {
                        NPC npc = new NPC();
                        npc.Read(stream);
                        sWorld.addVob(npc);
                    }

                    //Player-List:
                    int pLC = 0;
                    stream.Read(out pLC);
                    for (int i = 0; i < pLC; i++)
                    {
                        Player player = new Player(false, "");
                        player.Read(stream);

                        if (player.ID == id)
                            continue;
                        sWorld.addVob(player);
                    }

                    //WorldSpawnList:
                    int worldListCount = 0;
                    stream.Read(out worldListCount);
                    for (int i = 0; i < worldListCount; i++)
                    {

                        World w = new World();
                        w.Read(stream);

                        sWorld.WorldDict.Add(w.Map, w);

                    }
                }
            }
        }

        protected static void CreateItems()
        {
            //Creation of Items:
            StringBuilder sb = new StringBuilder();
            foreach (ItemInstance ii in ItemInstance.ItemInstanceList)
            {
                sb.Append(ii.getDeadalusScript());
                sb.AppendLine("");
                sb.AppendLine("");
            }

            String itemListFileName = StartupState.getRandomScriptString(".d");
            File.WriteAllText(itemListFileName, sb.ToString());

            zString gStr = zString.Create(Process.ThisProcess(), itemListFileName);
            zCParser.getParser(Process.ThisProcess()).ParseFile(gStr);
            gStr.Dispose();
        }
    }
}
