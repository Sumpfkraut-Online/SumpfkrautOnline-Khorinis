using System;
using System.Collections.Generic;
using System.Text;
using WinApi;
using Gothic.zClasses;
using Injection;
using Network;
using Gothic.zTypes;
using GMP.Modules;
using GMP.Injection.Synch;

namespace GMP.Helper
{
    public class NPCHelper
    {
        public static float GetDistance(Player pl1, Player pl2)
        {
            float[] distVec = new float[] { pl2.pos[0] - pl1.pos[0], pl2.pos[1] - pl1.pos[1], pl2.pos[2] - pl1.pos[2] };
            float distanceToPlayer = distVec[0] * distVec[0] + distVec[1] * distVec[1] + distVec[2] * distVec[2];
            distanceToPlayer = (float)Math.Sqrt(distanceToPlayer * distanceToPlayer);

            distanceToPlayer /= 10000;
            return distanceToPlayer;
        }

        public static void SetRespawnPoint(Object spawnpoint)
        {
            Process process = Process.ThisProcess();

            if (spawnpoint == null)
                return;
            if (spawnpoint.GetType() == typeof(String))
            {
                String wp = (String)spawnpoint;

                float[] pos = oCGame.Game(process).World.WayNet.getWaypointPosition(wp);
                if (pos != null)
                {
                    oCNpc.Player(process).TrafoObjToWorld.set(3, pos[0]);
                    oCNpc.Player(process).TrafoObjToWorld.set(7, pos[1]);
                    oCNpc.Player(process).TrafoObjToWorld.set(11, pos[2]);
                    if (pos.Length == 6)
                    {
                        oCNpc.Player(process).TrafoObjToWorld.set(2, pos[3]);
                        oCNpc.Player(process).TrafoObjToWorld.set(6, pos[4]);
                        oCNpc.Player(process).TrafoObjToWorld.set(10, pos[5]);
                    }

                    Program.Player.pos = new float[] { pos[0], pos[1], pos[2] };
                }
                else
                {
                    zERROR.GetZErr(process).Report(2, 'G', "Waypoint not found:" + wp, 0, "Program.cs", 0);
                }
            }
            else
            {
                Vec3 pos = (Vec3)spawnpoint;
                oCNpc.Player(process).TrafoObjToWorld.set(3, pos.x);
                oCNpc.Player(process).TrafoObjToWorld.set(7, pos.y);
                oCNpc.Player(process).TrafoObjToWorld.set(11, pos.z);

                Program.Player.pos = new float[] { pos.x, pos.y, pos.z };
            }
        }

        public static NPC getControllerByNPC(int address)
        {
            Process process = Process.ThisProcess();
            if (new oCNpc(process, address).VobType != zCVob.VobTypes.Npc)
                return null;
            foreach (NPC npc in StaticVars.npcControlList)
            {
                if (address == npc.npcPlayer.NPCAddress)
                {
                    return npc;
                }
            }
            return null;
        }


        public static bool NpcIsDown(Player pl)
        {
            if (!pl.isSpawned)
                return false;
            if (isUnconscious(pl) || isDead(pl) || isInMagicSleep(pl))
                return true;

            return false;
        }

        public static bool isUnconscious(Player pl)
        {
            if (!pl.isSpawned)
                return false;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            //ZS_Unconscious
            zString str = zString.Create(process, "ZS_Unconscious");
            int index = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            if (npc.IsAIState(index) == 1)
                return true;

            return false;
        }

        public static bool isDead(Player pl)
        {
            if (!pl.isSpawned)
                return false;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            if (npc.HP == 0)
                return true;

            return false;
        }

        public static bool isInMagicSleep(Player pl)
        {
            if (!pl.isSpawned)
                return false;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);

            //ZS_MagicSleep
            zString str = zString.Create(process, "ZS_MagicSleep");
            int index = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            if (npc.IsAIState(index) == 1)
                return true;

            return false;
        }

        public static bool isInWater(Player pl)
        {
            
            if (!pl.isSpawned)
                return false;
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            
            if (npc.AniCtrl.GetWalkModeString().Value.Trim().ToLower().Length == 0)
                return true;


            return false;
        }


        public static void setFriend(Player pl)
        {
            //setImmortal(pl);
            if (pl.isFriend == 1)
                new oCNpc(Process.ThisProcess(), pl.NPCAddress).Flags |= (int)oCNpc.NPC_Flags.Friend;
            else
                new oCNpc(Process.ThisProcess(), pl.NPCAddress).Flags &= (int)~oCNpc.NPC_Flags.Friend;
        }

        public static void setImmortal(Player pl)
        {
            if (!pl.isSpawned)
                return;

            if (pl.isFriend == 1 || pl.isImmortal)
            {
                new oCNpc(Process.ThisProcess(), pl.NPCAddress).Flags |= (int)oCNpc.NPC_Flags.Immortal;
            }
            else
            {
                new oCNpc(Process.ThisProcess(), pl.NPCAddress).Flags &= (int)~oCNpc.NPC_Flags.Immortal;
            }
        }


        public static void RemovePlayer(Player pl, Boolean fromList)
        {
            if (pl != null && fromList)
            {
                StaticVars.AllPlayerDict.Remove(pl.id);
                StaticVars.PlayerDict.Remove(pl.id);

                Program.playerList.Remove(pl);
                StaticVars.playerlist.Remove(pl);
            }
            if (pl == null || !pl.isSpawned || pl.isPlayer)
                return;
            pl.isSpawned = false;

            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            if (npc.Address == 0)
                return;
            pl.NPCAddress = 0;
            oCGame.Game(process).GetSpawnManager().DeleteNPC(npc);

            lock (StaticVars.spawnedPlayerList)
            {
                StaticVars.spawnedPlayerList.Remove(pl);
                StaticVars.spawnedPlayerDict.Remove(npc.Address);
            }
        }

        public static void RemoveNPC(NPC npcPL)
        {
            if (npcPL == null)
                return;

            Player pl = npcPL.npcPlayer;
            if (!pl.isNPC || pl == null)
                return;

            if (Program.playerList.Contains(pl))
            {
                StaticVars.AllPlayerDict.Remove(pl.id);
                Program.playerList.Remove(pl);
            }
            if (StaticVars.npcList.Contains(npcPL))
                StaticVars.npcList.Remove(npcPL);
            if (StaticVars.npcControlList.Contains(npcPL))
                StaticVars.npcControlList.Remove(npcPL);
            
            if (!pl.isSpawned)
                return;

            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            if (npc.Address == 0)
                return;
            pl.NPCAddress = 0;
            oCGame.Game(process).GetSpawnManager().DeleteNPC(npc);

            lock (StaticVars.spawnedPlayerList)
            {
                StaticVars.spawnedPlayerList.Remove(pl);
                StaticVars.spawnedPlayerDict.Remove(npc.Address);
            }
        }

        public static void MovePlayer(Player pl)
        {
            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, pl.NPCAddress);
            npc.TrafoObjToWorld.set(3, pl.pos[0]);
            npc.TrafoObjToWorld.set(7, pl.pos[1]);
            npc.TrafoObjToWorld.set(11, pl.pos[2]);
        }

        public static void SpawnPlayer(Player pl, bool setStats)
        {
            if (pl.isSpawned)
                return;

            Process process = Process.ThisProcess();

            zString str = zString.Create(process, pl.instance);

            oCNpc npc = oCObjectFactory.GetFactory(process).CreateNPC(zCParser.getParser(process).GetIndex(str));
            str.Dispose();
            pl.NPCAddress = npc.Address;

            
            //npc.MagBook = oCMag_Book.Create(process);
            //npc.MagBook.SetOwner(npc);
            //zERROR.GetZErr(process).Report(2, 'G', "MagBook created", 0, "Program.cs", 0);
            

            if (!pl.isPlayer)//TODO: Und Kein npc der vom player kontrolliert wird
            {
                npc.MPMax = 100000;
                npc.MP = 100000;
                npc.DiveCTR = 99999.0f;
                npc.DiveTime = 99999.0f;
                npc.FallDownDamage = 0;
            }

            if (pl.isInvisible)
                npc.setShowVisual(false);
            else
                npc.setShowVisual(true);

            if (pl.lastHP != -1)
                npc.HP = pl.lastHP;


            if (setStats)
            {
                npc.HP = pl.lastHP;
                npc.HPMax = pl.lastHP_Max;
                npc.Strength = pl.lastStr;
                npc.Dexterity = pl.lastDex;

                for (int i = 0; i < pl.lastTalentSkills.Length; i++)
                    npc.SetTalentSkill(i, pl.lastTalentSkills[i]);

                for (int i = 1; i < 5; i++)
                    npc.SetHitChances(i, pl.lastHitChances[i - 1]);






                InventoryHelper.RemoveInventory(pl);
                InventoryHelper.AddInventory(pl);
            }

            zVec3 pos = zVec3.Create(process);
            pos.X = pl.pos[0];
            pos.Y = pl.pos[1];
            pos.Z = pl.pos[2];

            pl.isSpawned = true;
            npc.Enable(pos);
            //npc.ResetPos(pos); //TODO: Wozu?
            pos.Dispose();

            if (pl.lastAniID != -1)
            {
                Animation.startAnimEnabled = true;
                npc.GetModel().StartAni(pl.lastAniID, pl.lastAniValue);
            }

            NPCHelper.setImmortal(pl);
            oCRtnManager.GetRtnManager(process).UpdateSingleRoutine(npc);


            
            StaticVars.spawnedPlayerList.Add(pl);
            StaticVars.spawnedPlayerDict.Add(pl.NPCAddress, pl);
            StaticVars.spawnedPlayerList.Sort(new Player.PlayerAddressComparer());

        }

        public static void SetStandards(Player pl)
        {
            
            Process process = Process.ThisProcess();
            oCNpc player = new oCNpc(process, pl.NPCAddress);

            if (StaticVars.serverConfig.OnlyHumanNames && player.IsHuman() == 0)
                return;

            zString STR = null;
            if (StaticVars.serverConfig.HideNames && !pl.knowName)
                STR = zString.Create(process, "");
            else
                STR = zString.Create(process, pl.name);
            player.SetName(STR);
            STR.Dispose();
        }


    }
}
