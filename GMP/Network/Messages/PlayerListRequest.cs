using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;
using Injection;
using WinApi;
using Gothic.zClasses;
using Gothic.zTypes;
using GMP.Logger;
using GMP.Modules;
using GMP.Helper;
using GMP.Network.Messages;
using GMP.Injection.Synch;

namespace GMP.Net.Messages
{
    public class PlayerListRequest : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            int count = 0;


            int day; int hour, minute;
            stream.Read(out day);
            stream.Read(out hour);
            stream.Read(out minute);

            TimeMessage.setTime(day, hour, minute);

            stream.Read(out count);

            for (int i = 0; i < count; i++)
            {
                int id = 0;
                String guid = "";
                String name = "";
                String map = "";
                String instance = "";
                String weaponmode = "";
                byte weaponModeType;
                float[] pos = new float[3];
                int hp, hpmax, str, dex, aniID, aniValue;
                bool isImmortal, isInvisible, isFreeze;

                stream.Read(out id);
                stream.Read(out guid);
                stream.Read(out name);

                Player pl = Player.getPlayerByGuid(guid, StaticVars.playerlist);
                if (pl != null && !pl.isPlayer)
                    continue;
                if (pl == null)
                {
                    pl = new Player(guid);
                    pl.id = id;
                    StaticVars.AllPlayerDict.Add(pl.id, pl);
                    StaticVars.PlayerDict.Add(pl.id, pl);
                    Program.playerList.Add(pl);
                    Program.playerList.Sort(new Player.PlayerComparer());
                    StaticVars.playerlist.Add(pl);
                }

                stream.Read(out map);
                stream.Read(out instance);
                stream.Read(out hp); stream.Read(out hpmax); stream.Read(out str); stream.Read(out dex);

                stream.Read(out aniID);
                stream.Read(out aniValue);

                stream.Read(out weaponModeType);
                stream.Read(out weaponmode);

                for (int iTal = 0; iTal < pl.lastTalentSkills.Length; iTal++)
                    stream.Read(out pl.lastTalentSkills[iTal]);
                for (int iTal = 0; iTal < pl.lastTalentValues.Length; iTal++)
                    stream.Read(out pl.lastTalentValues[iTal]);
                for (int iHit = 1; iHit < 5; iHit++)
                    stream.Read(out pl.lastHitChances[iHit - 1]);

                int itemcount; stream.Read(out itemcount);
                pl.itemList = new List<item>();

                for (int iItems = 0; iItems < itemcount; iItems++)
                {
                    string code; int amount;
                    stream.Read(out code);
                    stream.Read(out amount);
                    pl.itemList.Add(new item() { code = code, Amount = amount });
                }


                stream.Read(out pos[0]);
                stream.Read(out pos[1]);
                stream.Read(out pos[2]);
                stream.Read(out isImmortal);
                stream.Read(out isInvisible);
                stream.Read(out isFreeze);
                
                pl.name = name;
                pl.actualMap = Player.getMap(map);
                pl.pos = pos;

                pl.lastHP = hp;
                pl.lastHP_Max = hpmax;
                pl.lastDex = dex;
                pl.lastStr = str;
                
                pl.id = id;
                pl.instance = instance;
                pl.guidStr = guid;

                pl.lastAniID = aniID;
                pl.lastAniValue = aniValue;

                pl.isImmortal = isImmortal;
                pl.isInvisible = isInvisible;
                pl.isFreeze = isFreeze;

                Process Process = Process.ThisProcess();

                if (pl.isPlayer)
                    NPCHelper.MovePlayer(pl);
                //else if (pl.actualMap == Program.Player.actualMap)
                //    NPCHelper.SpawnPlayer(pl,true);

                //if (pl.actualMap == Program.Player.actualMap)
                //    NPCHelper.SetStandards(pl);
            }

            int npcAmount = 0;
            stream.Read(out npcAmount);

            for (int i = 0; i < npcAmount; i++)
            {
                int id, hp, hpmax, str, dex, aniID, aniValue; String actualmap, instance, weaponmode; float[] pos = new float[3];
                byte type, weaponModeType; float[] spawn = new float[3]; String wp = null;
                stream.Read(out id);
                stream.Read(out actualmap);
                stream.Read(out instance);
                stream.Read(out hp);
                stream.Read(out hpmax);
                stream.Read(out str);
                stream.Read(out dex);
                stream.Read(out aniID);
                stream.Read(out aniValue);
                stream.Read(out weaponModeType);
                stream.Read(out weaponmode);


                //Spawn
                stream.Read(out type);

                if (type == 0 || type == 1)
                {
                    stream.Read(out spawn[0]);
                    stream.Read(out spawn[1]);
                    stream.Read(out spawn[2]);
                }
                else
                {
                    stream.Read(out wp);
                }


                stream.Read(out pos[0]);
                stream.Read(out pos[1]);
                stream.Read(out pos[2]);


                Player pl = new Player("NPC");
                pl.id = id;
                pl.actualMap = Player.getMap(actualmap);
                pl.instance = instance.Trim().ToUpper();
                pl.lastHP = hp;
                pl.lastHP_Max = hpmax;
                pl.lastStr = str;
                pl.pos = pos;
                pl.isNPC = true;
                pl.lastAniID = aniID;
                pl.lastAniValue = aniValue;

                

                NPC npc = new NPC();

                if (type == 0)
                    npc.isStatic = true;
                else if (type == 1)
                    npc.isSummond = true;
                else if (type == 2)
                    npc.isSpawned = true;

                npc.spawn = spawn;
                npc.wp = wp;

                pl.NPC = npc;


                //if (pl.actualMap == Program.Player.actualMap)
                //{
                //    NPCHelper.SpawnPlayer(pl, false);
                //    Process process = Process.ThisProcess();
                //    if (pl.lastHP != -1)
                //        new oCNpc(process, pl.NPCAddress).HP = pl.lastHP;
                //    if (pl.lastHP_Max != -1)
                //        new oCNpc(process, pl.NPCAddress).HPMax = pl.lastHP_Max;
                //    if (pl.lastStr != -1)
                //        new oCNpc(process, pl.NPCAddress).Strength = pl.lastStr;
                //    if (pl.lastDex != -1)
                //        new oCNpc(process, pl.NPCAddress).Dexterity = pl.lastDex;
                //    Animation.startAnimEnabled = true;
                //    new oCNpc(process, pl.NPCAddress).GetModel().StartAni(aniID, aniValue);


                //    if (weaponModeType == 1)
                //    {
                //        zString wm = zString.Create(process, weaponmode);
                //        new oCNpc(process, pl.NPCAddress).SetWeaponMode2(wm);
                //        wm.Dispose();
                //    }
                //    else if (weaponModeType == 2)
                //    {
                //        new oCNpc(process, pl.NPCAddress).SetWeaponMode2(Convert.ToInt32(weaponmode));
                //    }
                //    else if (weaponModeType == 3)
                //    {
                //        new oCNpc(process, pl.NPCAddress).SetWeaponMode(Convert.ToInt32(weaponmode));
                //    }


                //    zVec3 vec = zVec3.Create(process);
                //    vec.X = pl.pos[0]; vec.Y = pl.pos[1]; vec.Z = pl.pos[2];
                //    new oCNpc(process, pl.NPCAddress).NpcStates.InitAIStateDriven(vec);
                //    vec.Dispose();
                //}
                

                npc.npcPlayer = pl;
                npc.npcPlayer.NPCList.Add(npc);
                StaticVars.npcList.Add(npc);

                StaticVars.AllPlayerDict.Add(pl.id, pl);
                Program.playerList.Add(pl);
                Program.playerList.Sort(new Player.PlayerComparer());
            }
            TimeMessage.firstTimeUpdate = false;
            Program.FullLoaded = true;
             
        }

        

        public override void Write(RakNet.BitStream stream, Client client)
        {
            ErrorLog.Log(typeof(PlayerListRequest), "Write");
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.PlayerListRequest);
            stream.Write(Program.Player.id);

            Process process = Process.ThisProcess();
            oCNpc player = oCNpc.Player(process);
            stream.Write(player.HPMax);
            stream.Write(player.Strength);
            stream.Write(player.Dexterity);
            Player nullpl = new Player("");
            for (int i = 0; i < nullpl.lastTalentSkills.Length; i++)
                stream.Write(player.GetTalentSkill(i));
            for (int i = 0; i < nullpl.lastTalentValues.Length; i++)
                stream.Write(player.GetTalentValue(i));
            for (int i = 1; i < 5; i++)
                stream.Write(player.GetHitChances(i));

            stream.Write(player.ObjectName.Value);


            //Inventar setzen
            zCListSort<oCItem> itemList = player.Inventory.ItemList;
            do
            {
                Program.Player.itemList.Add(new item() { code = itemList.Data.ObjectName.Value, Amount = itemList.Data.Amount });
            } while ((itemList = itemList.Next).Address != 0);



            //Inventar senden
            stream.Write(Program.Player.itemList.Count);
            for (int i = 0; i < Program.Player.itemList.Count; i++)
            {
                stream.Write(Program.Player.itemList[i].code);
                stream.Write(Program.Player.itemList[i].Amount);
            }




            //Spawnpunkt berechnen -- Wozu 2 mal??? ConnectionRequest berechnet ihn dochs chon
            //Object spawnObj = StaticVars.serverConfig.GetRandomSpawn();
            //if (spawnObj.GetType() == typeof(Vec3))
            //{
            //    Vec3 spawnpoint = (Vec3)spawnObj;
            //    stream.Write(spawnpoint.x); stream.Write(spawnpoint.y); stream.Write(spawnpoint.z);
            //}
            //else
            //{
            //    zCWaypoint wp = oCGame.Game(process).World.WayNet.GetWaypointByName((String)spawnObj);
            //    zVec3 pos = wp.Position;
            //    stream.Write(pos.X); stream.Write(pos.Y); stream.Write(pos.Z);
            //}
            if (Program.Player.pos != null)
            {
                stream.Write(Program.Player.pos[0]);
                stream.Write(Program.Player.pos[1]);
                stream.Write(Program.Player.pos[2]);
            }
            else
            {
                stream.Write(0.0f);
                stream.Write(0.0f);
                stream.Write(0.0f);
            }



            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.PlayerListRequest))
                StaticVars.sStats[(int)NetWorkIDS.PlayerListRequest] = 0;
            StaticVars.sStats[(int)NetWorkIDS.PlayerListRequest] += 1;
        }
    }
}
