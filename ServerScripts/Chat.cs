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
using GUC.Server.Scripts.Items;

namespace GUC.Server.Scripts
{
	public class Chat
	{
		protected MessagesBox mB;
		protected NPC lastNPC = null;
		protected Player lastPlayer = null;
		public void Init()
		{
            Logger.log(Logger.LogLevel.INFO, "################### Initalise Chatsystem ##################");
            
			
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
            Console.WriteLine("Spieler: "+player.ID+" "+player.Name);
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

        #region CommandFunctions
        public bool IsCommand(String commString)
        {
            if (commString.StartsWith("/"))
                return true;
            return false;
        }
        public bool IsCommand(String command, String commString)
        {
            if (commString.ToLower().StartsWith("/"+command.ToLower()))
                return true;
            return false;
        }

        public void getParameters(String commString, out String param){
            commString = commString.Trim();
            int indexOf = commString.IndexOf(" ");
            if (indexOf == -1)
            {
                param = null;
                return;
            }

            String parameters = commString.Substring(indexOf + 1).Trim();
            param = parameters;
            
        }

        public void getParameters(String commString, out String param, out int param2)
        {
            String[] parameters = null;
            getParameters(commString, out parameters);

            if (parameters == null || parameters.Length == 0)
            {
                param = null;
                param2 = 0;
                return;
            }
            param = parameters[0];
            if (parameters.Length == 1)
                param2 = 0;
            else
                Int32.TryParse(parameters[1], out param2);
        }

        public void getParameters(String commString, out String[] param)
        {
            String parameterString = null;
            getParameters(commString, out parameterString);

            if (parameterString == null)
            {
                param = null;
                return;
            }
            param = parameterString.Split(new char[]{' '});
        }

        public void getParameters(String commString, out int[] param)
        {
            String[] parameters = null;
            getParameters(commString, out parameters);

            if (parameters == null)
            {
                param = null;
                return;
            }
            param = new int[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                Int32.TryParse(parameters[i], out param[i]);
            }
        }

        #endregion

        public void textBoxMessageReceived(TextBox tb, Player pl, String message) {
			if(message.Trim().Length == 0)
				return;
			message = message.Trim();

            //Wenn es kein Kommando ist, wird die Nachricht direkt ausgegeben!
            if (!IsCommand(message))
            {
                mB.addLine(255, 255, 255, 255, pl.Name + ": " + message);
                return;
            }

            if (IsCommand("giveitem", message))
            {
                String instance = ""; int amount = 0;
                getParameters(message, out instance, out amount);
                if (instance == null || ItemInstance.getItemInstance(instance) == null)
                    return;
                if (amount == 0)
                    amount = 1;
                pl.addItem(ItemInstance.getItemInstance(instance), amount);
            }
            else if (IsCommand("giveSkills", message))
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
                    pl.setTalentSkills((NPCTalents)i, 3);
                    pl.setTalentValues((NPCTalents)i, 3);
                    pl.setHitchances((NPCTalents)i, 100);
                }
            }
            else if (IsCommand("giveAttribute", message))
            {
                int[] arg = null;
                getParameters(message, out arg);
                if (arg.Length != 2)
                    return;
                pl.setAttribute((NPCAttributeFlags)arg[0], arg[1]);
            }
            else if (IsCommand("giveTalent", message))
            {
                int[] arg = null;
                getParameters(message, out arg);
                if (arg.Length != 3)
                    return;
                pl.setTalentSkills((NPCTalents)arg[0], arg[1]);
                pl.setTalentValues((NPCTalents)arg[0], arg[2]);
            }
            else if (IsCommand("giveSpell"))
            {
                pl.addItem(ITSC_SHRINK.get(), 90);
                pl.addItem(ITSC_TRFSHEEP.get(), 90);
            }
            else if (IsCommand("setTime", message))
            {
                int[] arg = null;
                getParameters(message, out arg);
                if (arg.Length != 2)
                    return;
                DayTime.setTime(arg[0], arg[1]);
            }
            else if (IsCommand("toWP", message))
            {
                String wp = null;
                getParameters(message, out wp);
                if (wp == null)
                    return;
                WayPoint wayp = AISystem.getWaypoint(pl.Map, wp);
                if (wayp == null)
                {
                    mB.addLine(pl, 255, 0, 0, 255, "Waypoint was not found!: " + wp);
                    return;
                }
                pl.setPosition(wayp.Position);
            }
            else if (IsCommand("revive", message))
            {
                pl.revive();
                Console.WriteLine("HP: "+pl.HP+"; "+pl.HPMax);
            }
            else if (IsCommand("sppedup", message))
            {
                pl.ApplyOverlay("HUMANS_SPRINT.MDS");
            }
            else
            {
                mB.addLine(pl, 255, 255, 255, 255, "Command was not found: " + message);
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
