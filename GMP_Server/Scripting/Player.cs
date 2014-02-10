using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using GMP_Server.Net.Message;
using GMP_Server.Helper;

namespace GMP_Server.Scripting
{
    public class Player
    {
        private Network.Player player;

        public Player(Network.Player player)
        {
            this.player = player;
        }

        public static Player GetPlayer(int id)
        {
            if (!Program.playerDict.ContainsKey(id))
                return null;
            return new Player(Program.playerDict[id]);
        }

        public static Player CreateNPC(string instance, float[] pos, String wp, String world)
        {
            Program.idCount += 1;
            int npcid = Program.idCount;


            Network.Player pl = new Network.Player("NPC");

            pl.id = npcid;
            pl.actualMap = world.Trim().ToUpper();
            pl.instance = instance;
            pl.isSpawned = true;
            pl.pos = pos;
            pl.isNPC = true;

            NPC npc = new NPC();
            npc.isStatic = true;
            npc.npcPlayer = pl;
            npc.controller = null;
            npc.spawn = pos;
            
            
            pl.NPC = npc;
            new StaticNPCMessage().Write(Program.server.receiveBitStream, Program.server, npc);
            //NPC npc = SpawnNPCMessage.createNPC(instance, pos, world, wp, Program.server.receiveBitStream, Program.server);
            return new Player(npc.npcPlayer);
        }

        public int getID()
        {
            return player.id;
        }

        public String getInstance()
        {
            return player.instance;
        }

        public String getName()
        {
            return player.name;
        }

        public Object getPlayerData(String str)
        {
            if (!player.variableDataList.ContainsKey(str))
                return null;
            return player.variableDataList[str];
        }

        public void setPlayerData(String str, Object data)
        {
            if (player.variableDataList.ContainsKey(str))
                player.variableDataList.Remove(str);
            player.variableDataList.Add(str, data);
        }

        public void ban()
        {
            Kick.ban(player);
        }

        public void kick()
        {
            Kick.kick(this.player);
        }

        public MobInteract GetMobInteract()
        {
            return player.mobInteract;
        }

        public void Revive()
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.Revive, null, new int[] { player.id }, null);
        }

        public void setInstance(String instance)
        {
            
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.SetInstance, new string[]{instance}, new int[] { player.id }, null);
        }

        public void setVoice(int voice)
        {
            player.voice = voice;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetVoice, null, new int[] { player.id, voice }, null);
        }

        public void say(String svm)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.OutputSVM, new string[]{svm}, new int[] { player.id }, null);
        }

        public void setAdditionalVisual(int headtex, String headVisual, int bodyTex, String bodyVisual)
        {
            player.headTex = headtex;
            player.bodyTex = bodyTex;
            player.HeadVisual = headVisual;
            player.BodyVisual = bodyVisual;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetPlayerAdditionalVisual, new String[]{headVisual, bodyVisual}, new int[] { player.id, headtex, bodyTex }, null);
        }

        public void setFatness(int fatness)
        {
            player.fatness = fatness;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetPlayerFatness, null, new int[] { player.id, fatness }, null);
        }

        public void setScale(float x, float y, float z)
        {
            player.scale[0] = x;
            player.scale[1] = y;
            player.scale[2] = z;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetPlayerScale, null, new int[] { player.id }, new float[]{x, y, z});
        }



        public void Freeze()
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.FreezePlayer, null, new int[] { 1 }, null);
        }

        public void UnFreeze()
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.FreezePlayer, null, new int[] { 0 }, null);
        }

        public int HasItem(String code)
        {
            foreach(item itm in player.itemList){
                if (itm.code.Trim().ToLower() == code.Trim().ToLower())
                {
                    return itm.Amount;
                }
            }

            return 0;
        }


        public void GiveItem(String code, int amount)
        {
            if (code == null || amount == 0)
                return;
            player.InsertItem(new item() { code = code, Amount =amount });
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.GiveItems, new String[]{code}, new int[] { player.id, amount }, null);
        }

        public void ChangeWorld(String world)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.ChangeWorld, new String[]{ world }, null, null);
        }

        public void Equip(String item)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.Equip, new String[] { item }, null, null);
        }

        public void EquipArmor(String item)
        {
            player.equippedArmor = item;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.EquipArmor, new String[] { item }, null, null);
        }

        public void EquipWeapon(String item)
        {
            player.equippedWeapon = item;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.EquipWeapon, new String[] { item }, null, null);
        }

        public void EquipRangeWeapon(String item)
        {
            player.equippedRangeWeapon = item;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, player.id, (byte)CommandoType.EquipRangeWeapon, new String[] { item }, null, null);
        }

        public void setPosition(float x, float y, float z)
        {
            this.setPosition(new float[]{x,y,z});
        }

        public void setPosition(float[] pos){
            if (pos.Length != 3)
            {
                return;
            }
            player.pos = pos;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetPosition, null, new int[] { player.id }, pos);
        }

        public void setPositionFast(float x, float y, float z)
        {
            this.setPositionFast(new float[] { x, y, z });
        }
        
        public void setPositionFast(float[] pos)
        {
            if (pos.Length != 3)
            {
                return;
            }

            
            player.pos = pos;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetPosition, null, new int[] { player.id }, pos, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED);
        }

        public void PlayAnimation(String anim)
        {
            if (anim == null)
                return;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.PlayAnimation, new String[]{anim}, new int[] { player.id }, null);
        }

        public void StopAnimation(String anim)
        {
            if (anim == null)
                return;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.StopAnimation, new String[] { anim }, new int[] { player.id }, null);
        }

        public float[] getPosition()
        {
            return player.pos;
        }

        public void setDirection(float[] dir)
        {
            if (dir.Length != 3)
            {
                return;
            }
            player.dir = dir;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetDirection, null, new int[] { player.id }, dir);
        }

        public void setDirectionFast(float[] dir)
        {
            if (dir.Length != 3)
            {
                return;
            }
            player.dir = dir;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetDirection, null, new int[] { player.id }, dir, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.UNRELIABLE_SEQUENCED);
        }

        public void setAngle(float angle)
        {
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetAngle, null, new int[] { player.id }, new float[]{ angle });
        }

        public void setDirection(float x, float y, float z)
        {
            this.setDirection(new float[] { x, y, z });
        }

        public float[] getDirection()
        {
            return player.dir;
        }

        public int getHealth()
        {
            return player.lastHP;
        }

        public void setHealth(int hp)
        {
            player.lastHP = hp;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetHP, null, new int[] { player.id, player.lastHP }, null);
        }

        public int getMaxHealth()
        {
            return player.lastHP_Max;
        }

        public void setMaxHealth(int hp)
        {
            player.lastHP_Max = hp;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetMaxHP, null, new int[] { player.id, player.lastHP_Max }, null);
        }

        public int getMana()
        {
            return player.lastMP;
        }

        public void setMana(int mp)
        {
            player.lastMP = mp;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetMana, null, new int[] { player.id, player.lastMP }, null);
        }

        public int getMaxMana()
        {
            return player.lastMP_Max;
        }

        public void setMaxMana(int mp)
        {
            player.lastMP_Max = mp;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetMaxMana, null, new int[] { player.id, player.lastMP_Max }, null);
        }

        public int getStrength()
        {
            return player.lastStr;
        }

        public void setStrength(int str)
        {
            player.lastStr = str;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetStrength, null, new int[] { player.id, player.lastStr }, null);
        }

        public int getDexterity()
        {
            return player.lastStr;
        }

        public void setDexterity(int dex)
        {
            player.lastDex = dex;
            new CommandoMessage().Write(Program.server.receiveBitStream, Program.server, -1, (byte)CommandoType.SetDexterity, null, new int[] { player.id, player.lastDex }, null);
        }
    }
}
