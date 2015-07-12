using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Server;
using GUC.Server.Network;
using GUC.Types;

namespace GUC.WorldObjects.Character
{
    internal partial class NPC
    {

        public GUC.Server.Network.WorldCell cell = null;

        public bool isPlayer { get { return client != null; } }
        public Client client;
        public Client npcController;

        public int[] protection = new int[8];
        public DamageTypes damageType = DamageTypes.DAM_EDGE;
        public int totalDamage = 0;
        public int[] damages = new int[8];

        public Server.Scripting.Objects.Character.NPC ScriptingNPC
        {
            get { return (Server.Scripting.Objects.Character.NPC)ScriptingVob; }
            set { ScriptingVob = value; }
        }
        
        public override float[] Position
        {
            get { return pos; }
            set
            {
                if (value != null && value.Length == 3)
                {
                    pos = value;
                }
                else
                {
                    pos[0] = 0;
                    pos[1] = 0;
                    pos[2] = 0;
                }
                GUC.Server.Network.WorldCell.UpdatePosition(this, true);
            }
        }

        public void ReadPosition(BitStream stream)
        {
            stream.Read(pos);
            GUC.Server.Network.WorldCell.UpdatePosition(this, false);
        }

        protected override VobSendFlags getSendInfo()
        {
             return base.getSendInfo();
        }

        public override VobSendFlags Write(BitStream stream)
        {
            VobSendFlags sendInfo = base.Write(stream);

            stream.Write(Name);
            stream.Write(BodyMesh);
            stream.Write(BodyTex);
            stream.Write(SkinColor);
            stream.Write(HeadMesh);
            stream.Write(HeadTex);
            stream.Write(TeethTex);

            stream.Write(this.Attributes);
            stream.Write(this.TalentSkills);
            stream.Write(this.TalentValues);
            stream.Write(this.Hitchances);

            stream.Write(this.Scale);
            stream.Write(this.Fatness);


            stream.Write(itemList.Count);
            foreach (Item it in itemList)
            {
                stream.Write(it.ID);
            }

            stream.Write(EquippedList.Count);
            foreach (Item it in EquippedList)
            {
                stream.Write(it.ID);
            }

            if (Armor == null)
                stream.Write(0);
            else
                stream.Write(Armor.ID);

            if (Weapon == null)
                stream.Write(0);
            else
                stream.Write(Weapon.ID);

            if (RangeWeapon == null)
                stream.Write(0);
            else
                stream.Write(RangeWeapon.ID);

            if (this.ActiveSpell == null)
                stream.Write(0);
            else
                stream.Write(this.ActiveSpell.ID);

            stream.Write(Overlays.Count);
            foreach (String str in Overlays)
            {
                stream.Write(str);
            }

            stream.Write(this.IsInvisible);
            stream.Write(this.hideName);

            stream.Write(this.weaponMode);

            return sendInfo;
        }



        internal void SendToAreaPlayers(BitStream stream, PacketPriority pp, PacketReliability pr)
        {
            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, 8000);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.Map).PlayerPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Character.Player> mobs = sWorld.getWorld(this.Map).PlayerPositionList[key];
                foreach (WorldObjects.Character.Player m in mobs)
                {
                    if (m == this)
                        continue;
                    float mD = (m.Position - this.Position).Length;

                    if (mD <= 8000)
                    {
                        using (RakNetGUID guid = m.GUID)
                        {
                            Program.server.ServerInterface.Send(stream, pp, pr, (char)0, guid, false);
                        }
                    }
                }
            }
            
        }

        internal void SendToAreaPlayersAndPlayer(BitStream stream, PacketPriority pp, PacketReliability pr)
        {
            uint[] keys = WorldObjects.World.getImportantKeysByPosition(this.Position.Data, 8000);

            foreach (uint key in keys)
            {
                if (!sWorld.getWorld(this.Map).PlayerPositionList.ContainsKey(key))
                    continue;

                List<WorldObjects.Character.Player> mobs = sWorld.getWorld(this.Map).PlayerPositionList[key];
                foreach (WorldObjects.Character.Player m in mobs)
                {
                    float mD = (m.Position - this.Position).Length;

                    if (mD <= 8000)
                    {
                        using (RakNetGUID guid = m.GUID)
                        {
                            Program.server.ServerInterface.Send(stream, pp, pr, (char)0, guid, false);
                        }
                    }
                }
            }

        }




        public static long lastNearNPCListUpdate = 0;
        public static void sUpdateNPCList(long now)
        {
            if (lastNearNPCListUpdate + 10000 * 60 > now)
                return;
            foreach (Player player in sWorld.PlayerList)
            {
                if (!player.IsSpawned)
                    continue;
                player.updateNearNPCList();
            }
            lastNearNPCListUpdate = now;
        }

        public List<NPC> NearNPCList = new List<NPC>();
        
        public void updateNearNPCList()
        {
            List<NPC> nlCopy = new List<NPC>(NearNPCList);
            GUC.Server.Scripting.Objects.Character.NPCProto[] playerList =
                this.ScriptingNPC.getNearNPC(8000);

            foreach (GUC.Server.Scripting.Objects.Character.NPCProto proto in playerList)
            {
                if (NearNPCList.Contains(proto.proto))
                {
                    nlCopy.Remove(proto.proto);
                    continue;
                }

                if ((proto.Position - this.Position).Length >= 7500)//Adds >= 75meters, Remove >= 80 meters
                {
                    continue;
                }

                NearNPCList.Add(proto.proto);

                //TODO: Send SpawnMessage to Player
                BitStream stream = Program.server.SendBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkID.NPCEnableMessage);
                stream.Write(proto.ID);
                stream.Write(proto.Position);
                stream.Write(true);
                using(RakNetGUID guid = GUID){
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }

            foreach (NPC proto in nlCopy)
            {
                NearNPCList.Remove(proto);

                if (proto is Player /*&& !((Player)proto).IsConnected*/)
                    continue;
                
                //Todo: Send DespawnMessage to Player!
                BitStream stream = Program.server.SendBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkID.NPCEnableMessage);
                stream.Write(proto.ID);
                stream.Write(proto.Position);
                stream.Write(false);
                using (RakNetGUID guid = GUID)
                {
                    Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }

        }

    }
}
