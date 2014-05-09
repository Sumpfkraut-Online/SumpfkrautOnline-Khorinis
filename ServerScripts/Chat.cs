using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using GUC.Server.Log;
using GUC.Server.Scripting.Listener;
using GUC.Server.Scripting;
using GUC.Server.Scripting.Objects;
using GUC.Server.Scripting.Objects.Character;
using GUC.Server.Scripting.Objects.Mob;
using GUC.Enumeration;
using GUC.Server.Scripting.GUI;
using GUC.Types;

using GUC.Server.Scripts.AI;
using GUC.Server.Scripts.AI.Waypoints;
using GUC.Server.Scripts.AI.NPC_Def.Monster;

namespace GUC.Server.Scripts
{
	public class Chat
	{
		protected MessagesBox mB;
		protected NPC lastNPC = null;
		protected Player lastPlayer = null;
		public void Init()
		{
			Console.WriteLine("############### Initalise Chatsystem ######################");

			TextBox tB = new TextBox("", "Font_Old_20_White.TGA", 0, 0x800, 0x0D, 0x54, 0x1B);
			tB.show();

			tB.TextSended += new Events.TextBoxMessageEventHandler(textBoxMessageReceived);

			mB = new MessagesBox("Font_Old_20_White.TGA", 8, 0, 0);
			mB.show();

			Player.playerSpawns += new Events.PlayerEventHandler(spawn);
			Player.playerDisconnects += new Events.PlayerEventHandler(disconnect);
            

            MobInter.OnStartInteractions += new Events.MobInterEventHandler(startInteract);
            MobInter.OnStopInteractions += new Events.MobInterEventHandler(stopInteract);

            MobInter.OnTriggers += new Events.MobInterEventHandler(trigger);
            MobInter.OnUnTriggers += new Events.MobInterEventHandler(untrigger);



            lastNPC = new NPC("Test");
            lastNPC.Spawn("NEWWORLD\\NEWWORLD.ZEN", new Vec3f(0, 0, 0), new Vec3f(0, 0, 0));

            lastNPC.addItem(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 12);
            lastNPC.addItem(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 11);
            lastNPC.addItem(ItemInstance.getItemInstance("ITMW_1H_VLK_MACE"), 1);

            lastNPC.InitNPCAI();

            lastNPC.getAI().DailyRoutine = tagesablauf;
		}

		public void spawn(Player player) {
			mB.addLine(255, 255, 255, 255, player.Name+" betritt das Spiel");
            Logger.log(Logger.LOG_INFO, player.Name + " betritt das Spiel");

		}

		public void disconnect(Player player) {
			mB.addLine(255, 255, 255, 255, player.Name+" verlässt das Spiel");
            Logger.log(Logger.LOG_INFO, player.Name + " verlässt das Spiel");
		}


        public void tagesablauf(NPCProto proto)
        {
            if (proto.RTN_ACTIVE(0, 0, 23, 59))
            {
                proto.AI_GOTOWP("NW_TO_PASS_02");
                proto.AI_ALIGNTOWP("NW_TO_PASS_02");
                proto.AI_PLAYANIMATION("S_LGUARD");
            }
        }

		public void textBoxMessageReceived(TextBox tb, Player pl, String message) {
			if(message.Trim().Length == 0)
				return;
			message = message.Trim();
			String upperMessage = message.ToUpper();
			if(upperMessage.StartsWith("/TTNMI")) {
				pl.Position = pl.getNearestMobInteract().Position;
				
			}else if(upperMessage.StartsWith("/RAIN")) {
                World.setRainTime(World.WeatherType.Rain, 12, 0, 11, 59);

            }
            else if (upperMessage.StartsWith("/SPAWN "))
            {
                String second = upperMessage.Substring("/SPAWN ".Length).Trim().ToLower();

                WayPoint wp = AI.AISystem.WayNets[@"NEWWORLD\NEWWORLD.ZEN"].getNearestWaypoint(pl.Position);
                NPC npc = null;
                if(second == "goblin")
                    npc = new Young_Gobbo_Green();
                else if (second == "wolf")
                    npc = new YoungWolf();
                else if (second == "bloodfly")
                    npc = new Bloodfly();
                else if (second == "waran")
                    npc = new Waran();
                else if (second == "keiler")
                    npc = new Keiler();
                if(npc != null)
                    npc.Spawn(@"NEWWORLD\NEWWORLD.ZEN", wp.Position, null);

            }
            else if (upperMessage.StartsWith("/GOTO"))
            {
                pl.Position = lastNPC.Position;
            }
            else if (upperMessage.StartsWith("/EXITGAME"))
            {
                pl.exitGame();
            }
            else if (upperMessage.StartsWith("/SETTIME "))
            {

            }
            else if (upperMessage.StartsWith("/drawSize"))
            {

            }
            else if (upperMessage.StartsWith("/EQUIPITEM"))
            {
                foreach (Item item in pl.getItemList())
                {
                    if (item.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF))
                    {
                        pl.Equip(item);
                    }
                }
            }
            else if (upperMessage.StartsWith("/UNEQUIPITEM"))
            {
                pl.UnEquip(pl.EquippedWeapon);
            }
            else if (upperMessage.StartsWith("/SNOW"))
            {
                World.setRainTime(World.WeatherType.Snow, 12, 0, 11, 59);
            }
            else if (upperMessage.StartsWith("/GMP"))
            {
                for (int i = 0; i < (int)NPCAttributeFlags.ATR_MAX; i++)
                {
                    pl.setAttribute((NPCAttributeFlags)i, 100 + i);
                }

                for (int i = (int)NPCTalents.H1; i < (int)NPCTalents.MaxTalents; i++)
                {
                    pl.setTalentSkills((NPCTalents)i, 100 + i);
                    pl.setTalentValues((NPCTalents)i, 100 + i);
                }

                for (int i = (int)NPCTalents.H1; i <= (int)NPCTalents.CrossBow; i++)
                {
                    pl.setHitchances((NPCTalents)i, 100 + i);
                }
            }
            else if (upperMessage.StartsWith("/SPAWNMOBDOOR"))
            {
                MobDoor v = new MobDoor("DOOR_WOODEN.MDS");
                v.Spawn(pl.Map, new Vec3f(0, -160, 0), new Vec3f(0, 0, 1));
            }
            else if (upperMessage.StartsWith("/SPAWNMOBBED"))
            {
                MobBed v = new MobBed("BEDHIGH_NW_MASTER_01.ASC");
                v.Spawn(pl.Map, new Vec3f(0, -160, 0), new Vec3f(0, 0, 1));
            }
            else if (upperMessage.StartsWith("/SPAWNMOBINTER"))
            {
                MobInter v = new MobInter("BSFIRE_OC.MDS", ItemInstance.getItemInstance("ITMISWORDRAW"));
                v.Spawn(pl.Map, new Vec3f(0, -160, 0), new Vec3f(0, 0, 1));
            }
            else if (upperMessage.StartsWith("/SPAWNVOB"))
            {
                Vob v = new Vob("dt_bookshelf_v1.3DS", true, true);
                v.Spawn(pl.Map, new Vec3f(0, 0, 0), new Vec3f(0, 0, 1));
            }
            else if (upperMessage.StartsWith("/SPAWNITEM "))
            {
                pl.World.addItem(ItemInstance.getItemInstance(upperMessage.Substring("/SPAWNITEM ".Length).Trim()), 1, pl.Position);
            }
            else if (upperMessage.StartsWith("/GIVEITEM "))
            {
                pl.addItem(ItemInstance.getItemInstance(upperMessage.Substring("/GIVEITEM ".Length).Trim()), 1);
            }
            else if (upperMessage.StartsWith("/SPAWNNPC"))
            {
                lastNPC = new NPC("Test");
                lastNPC.Spawn(pl.Map, new Vec3f(0, 0, 0), new Vec3f(0, 0, 0));

                lastNPC.addItem(ItemInstance.getItemInstance("ITAT_SHEEPFUR"), 12);
                lastNPC.addItem(ItemInstance.getItemInstance("ITAT_WOLFFUR"), 11);
                lastNPC.addItem(ItemInstance.getItemInstance("ITMW_1H_VLK_MACE"), 1);

                lastNPC.InitNPCAI();

                lastPlayer = pl;
            }
            else if (upperMessage.StartsWith("/GOTOWP"))
            {
                lastNPC.AI_GOTOWP("NW_TO_PASS_02");
            }
            else if (upperMessage.StartsWith("/SA "))
            {
                lastNPC.playAnimation(message.Substring("/SA ".Length).Trim());
            }
            else if (upperMessage.StartsWith("/STA "))
            {
                lastNPC.stopAnimation(message.Substring("/STA ".Length).Trim());
            }
            else if (upperMessage.StartsWith("/CHNPC"))
            {
                lastNPC.setVisual("Orc.mds", "Orc_BodyElite", 0, 0, "Orc_HeadWarrior", 0, 0);
            }
            else if (message == "/kill")
            {
                pl.HP = 0;
            }
            else if (message.StartsWith("/uncon "))
            {
                pl.dropUnconscious(Convert.ToSingle(message.Substring("/uncon ".Length).Trim()));
            }
            else if (message.StartsWith("/setHP "))
            {
                pl.HP = Convert.ToInt32(message.Substring("/setHP ".Length).Trim());
            }
            else if (message == "/revive")
            {
                pl.revive();
            }
            else if (message == "/freeze")
            {
                pl.freeze();
            }
            else if (message == "/unfreeze")
            {
                pl.unfreeze();
            }
            else if (message.StartsWith("/startAnim "))
            {
                pl.playAnimation(message.Substring("/startAnim ".Length).Trim());
            }
            else if (message.StartsWith("/stopAnim "))
            {
                pl.stopAnimation(message.Substring("/stopAnim ".Length).Trim());
            }
            else if (message.StartsWith("/alli"))
            {
                pl.setVisual("Alligator.mds", "KRO_BODY", 0, 0, "", 0, 0);
            }
            else if (message.StartsWith("/orcelite"))
            {
                pl.setVisual("Orc.mds", "Orc_BodyElite", 0, 0, "Orc_HeadWarrior", 0, 0);
            }
            else if (message.StartsWith("/orcelite"))
            {
                pl.setVisual("Razor.mds", "Raz_Body", 0, 0, "", 0, 0);
            }
            else if (message.StartsWith("/human"))
            {
                pl.setVisual("HUMANS.mds", "hum_body_Naked0", 9, 0, "Hum_Head_Pony", 0, 0);
            }
            else
            {
                mB.addLine(255, 255, 255, 255, pl.Name + ": " + message);
            }
		}



        public void startInteract(MobInter sender, NPCProto npc)
        {
            Logger.log(Logger.LOG_INFO, npc.Name+" starts Interaction "+sender);
        }

        public void stopInteract(MobInter sender, NPCProto npc)
        {
            Logger.log(Logger.LOG_INFO, npc.Name + " stop Interaction " + sender);
        }

        public void trigger(MobInter sender, NPCProto npc)
        {
            Logger.log(Logger.LOG_INFO, npc.Name + " triggers " + sender);
        }

        public void untrigger(MobInter sender, NPCProto npc)
        {
            Logger.log(Logger.LOG_INFO, npc.Name + " untriggers " + sender);
        }



    }
}
