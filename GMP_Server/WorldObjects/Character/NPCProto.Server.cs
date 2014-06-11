using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RakNet;
using GUC.Network;
using GUC.Enumeration;
using GUC.Server;

namespace GUC.WorldObjects.Character
{
    internal abstract partial class NPCProto
    {
        public int[] protection = new int[8];
        public DamageType damageType = DamageType.DAM_EDGE;
        public int totalDamage = 0;
        public int[] damages = new int[8];



        public NPCProto()
            : base()
        {
            this.type = (int)VobTypes.Npc;
            this.visual = "HUMANS.MDS";

            this.Attributes[(int)NPCAttributeFlags.ATR_HITPOINTS] = 1;
            this.Attributes[(int)NPCAttributeFlags.ATR_HITPOINTS_MAX] = 1;
        }

        public virtual ulong Guid { get { return 0; } }
        public virtual RakNet.RakNetGUID GUID { get { return new RakNet.RakNetGUID(0); } }


        public Server.Scripting.Objects.Character.NPCProto ScriptingNPC
        {
            get { return (Server.Scripting.Objects.Character.NPCProto)ScriptingVob; }
            set { ScriptingVob = value; }
        }


        protected override VobSendFlags getSendInfo()
        {
            VobSendFlags b = base.getSendInfo();
            
            

            return b;
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



        public void SendToAreaPlayers(BitStream stream, PacketPriority pp, PacketReliability pr)
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
                            Program.server.server.Send(stream, pp, pr, (char)0, guid, false);
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

        public List<NPCProto> NearNPCList = new List<NPCProto>();
        
        public void updateNearNPCList()
        {
            List<NPCProto> nlCopy = new List<NPCProto>(NearNPCList);
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
                BitStream stream = Program.server.sendBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.NPCEnableMessage);
                stream.Write(proto.ID);
                stream.Write(proto.Position);
                stream.Write(true);
                using(RakNetGUID guid = GUID){
                    Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }

            foreach (NPCProto proto in nlCopy)
            {
                NearNPCList.Remove(proto);

                if (proto is Player && !((Player)proto).IsConnected)
                    continue;
                
                //Todo: Send DespawnMessage to Player!
                BitStream stream = Program.server.sendBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkIDS.NPCEnableMessage);
                stream.Write(proto.ID);
                stream.Write(proto.Position);
                stream.Write(false);
                using (RakNetGUID guid = GUID)
                {
                    Program.server.server.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, guid, false);
                }
            }

        }

    }
}
