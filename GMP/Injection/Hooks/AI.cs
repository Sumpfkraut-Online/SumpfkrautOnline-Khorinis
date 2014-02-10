using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using WinApi;
using Network;
using Injection;
using Gothic.zClasses;
using GMP.Network.Messages;
using Gothic.zTypes;
using Gothic.zStruct;

namespace GMP.Injection.Hooks
{
    public class AI
    {
        public static bool SendOnDamage = true;



        public static bool onDamageAvailable;
        static bool dmgOn = true;

        public static bool manageAssessMessages = true;
        public static bool alwaysDisableDamage;


        public static Int32 Output(String message)
        {
            if (!StaticVars.Ingame)
                return 0;

            Process Process = Process.ThisProcess();
            int address = Convert.ToInt32(message);

            oCMsgConversation msgConversation = new oCMsgConversation(Process, Process.ReadInt(address + 4));
            if (msgConversation.Name.Value.Trim().Length == 0)
                return 0;

            oCNpc npc = new oCNpc(Process, Process.ReadInt(address));
            Player npcPlayer = StaticVars.spawnedPlayerDict[npc.Address];
            if (!npcPlayer.isPlayer && !npcPlayer.isNPC)
                return 0;
            if (!npcPlayer.isPlayer && npcPlayer.isNPC && (npcPlayer.NPCList.Count == 0 || npcPlayer.NPCList[0].controller != Program.Player))
                return 0;

            Player argPlayer = StaticVars.spawnedPlayerDict[msgConversation.Vob.Address];

            new OutputSynchMessage().Write(Program.client.sentBitStream, Program.client, npcPlayer, argPlayer, msgConversation.subType, msgConversation.Name.Value);
            
            return 0;
        }

        public static Int32 AssessTalk_S(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            if (StaticVars.serverConfig.NPCAIOnServer)
                return 0;

            Process Process = Process.ThisProcess();
            int address = Convert.ToInt32(message);

            oCNpc npc_sender = new oCNpc(Process, Process.ReadInt(address));//Angesprochener
            oCNpc npc_receiver = new oCNpc(Process, Process.ReadInt(address+4));//Spieler

            Player npcSender = StaticVars.spawnedPlayerDict[npc_sender.Address];
            Player npcReceiver = StaticVars.spawnedPlayerDict[npc_receiver.Address];
            byte[] arr = null;
            if (npcReceiver == null || npcSender == null || !npcReceiver.isPlayer || !npcSender.isNPC || (npcSender.NPCList.Count == 1 && npcSender.NPCList[0].controller == Program.Player))
            {
                //Aktivieren...
                arr = new byte[] { 0x64, 0xA1, 0x00, 0x00, 0x00 };
                Process.Write(arr, Program.AssessTalk_S.oldFuncInNewFunc.ToInt32());//ActivateRtnState
                return 0;

            }
            zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "AssessTalk: "+npc_sender.Name.Value+" | "+npc_receiver.Name.Value, 0, "AI.cs", 66);
            //Process.Write(new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 }, 0x0075C898);//Deaktivieren

            arr = new byte[] { 0x33, 0xC0, 0xC2, 0x04, 0x00 };
            Process.Write(arr, Program.AssessTalk_S.oldFuncInNewFunc.ToInt32());//ActivateRtnState

            new AssessTalkMessage().Write(Program.client.sentBitStream, Program.client, npcSender.id, npcReceiver.id, 0);

            return 0;
        }

        /// <summary>
        /// Todo: oCNpc::AssessPlayer_S wird nur von npc's ausgeführt, bei denen die AiState gesetzt ist.
        /// Eventuell selber berechnen?
        /// Ist wichtig, damit von anderen kontrollierte NPC's korrekt auf Mitspieler reagieren z.b. Wird von der AssessPlayer funktion ebenfalls die DrawWeapon funktion ausgeführt (auf Daedalus-Ebene)
        /// Daher reagieren NPC's nicht korrekt, wenn ein anderer Spieler schon vorher die Waffe gezogen hat
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 AssessPlayer_S(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            if (StaticVars.serverConfig.NPCAIOnServer)
                return 0;

            Process Process = Process.ThisProcess();
            int address = Convert.ToInt32(message);
            oCNpc npc = new oCNpc(Process, Process.ReadInt(address));


            Player npcPlayer = StaticVars.spawnedPlayerDict[npc.Address];
            if (npcPlayer == null || !npcPlayer.isNPC || (npcPlayer.NPCList.Count == 1 && npcPlayer.NPCList[0].controller == Program.Player))//Muss nicht übertragen werden, wenn man eh schon der Controller des NPCs ist
                return 0;

            new AssessPlayerMessage().Write(Program.client.sentBitStream, Program.client, npcPlayer.id);
            return 0;
        }
        
        /// <summary>
        /// Hook wird ausgeführt, wenn der npc irgenteine Art von Schaden erleidet.
        /// Sollen nur die Effekte ausgeführt werden, und das dazugehörige Script (z.b. Wegfliegen, nach Troll-Treffer) wird die HP-Änderung blockiert, da diese durch die HP-Synchronisation gesetzt wird.
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 OnDamage_DD(String message)
        {
            Process Process = Process.ThisProcess();
            if (StaticVars.serverConfig.DamageOnServer)//Schaden wird komplett vom Server berechnet 
            {
                if (dmgOn)
                {
                    Process.VirtualProtect(0x0066CAC9, 10);
                    byte[] arr = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                    Process.Write(arr, 0x0066CAC9);
                    dmgOn = false;
                }
            }
            else if (onDamageAvailable)//Kam von jemand anderem?
            {
                //HP Änderungen ausschalten...
                if (dmgOn)
                {
                    Process.VirtualProtect(0x0066CAC9, 10);
                    byte[] arr = new byte[] { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
                    Process.Write(arr, 0x0066CAC9);
                    dmgOn = false;
                }
                onDamageAvailable = false;
                SendOnDamage = true;
                return 0;
            }
            else if (!dmgOn)
            {
                //HP Änderungen anschalten
                byte[] arr = new byte[] { 0x57, 0x6A, 0x00, 0x8B, 0xCD, 0xE8, 0x8D, 0x34, 0x0C, 0x00 };
                Process.Write(arr, 0x0066CAC9);
                dmgOn = true;
            }
            
            int address = Convert.ToInt32(message);

            if (!SendOnDamage)
            {
                SendOnDamage = true;
                return 0 ;
            }

            oCNpc npc = new oCNpc(Process, Process.ReadInt(address));
            oSDamageDescriptor oDD = new oSDamageDescriptor(Process, Process.ReadInt(address + 4));

            String weapon = "";
            if(oDD.SpellID != 0){

                String t = oDD.AttackerNPC.MagBook.SpellItems.get(oDD.AttackerNPC.MagBook.GetKeyByID(oDD.SpellID)).ObjectName.Value;

                weapon = t;
                //zERROR.GetZErr(Process).Report(2, 'G', "Spell Attack: "+t, 0, "Program.cs", 0);
            }
            //if (oDD.SpellID != 0)//Spells sollen nicht übertragen werden...
            //    return 0;
            
            Player npcPlayer = StaticVars.spawnedPlayerDict[npc.Address];

            
            //if (npcPlayer == null || npcPlayer.isPlayer || (npcPlayer.isNPC && npcPlayer.NPCList[0].controller != null))
            if (npcPlayer == null)
                return 0;

            Player attacker = StaticVars.spawnedPlayerDict[oDD.AttackerNPC.Address];
            if (attacker == null)
                return 0;
            byte modeDamage = (byte)oDD.ModeDamage;
            byte modeWeapon = (byte)oDD.ModeWeapon;
            byte item = 0;

            if (attacker != null && oDD.AttackerNPC.GetEquippedMeleeWeapon().ObjectName.Value.Trim().ToUpper() ==
                oDD.Weapon.ObjectName.Value.Trim().ToUpper())
                item = 1;
            else
                item = 2;

            //else if (attacker != null && oDD.AttackerNPC.GetEquippedRangedWeapon().ObjectName.Value.Trim().ToUpper() ==
            //    oDD.Weapon.ObjectName.Value.Trim().ToUpper())
            //    item = 2;

            if (oDD.SpellID == 0)
            {
                weapon = oDD.Weapon.ObjectName.Value;
            }
            
            int id2 = -1;
            if(attacker != null)
                id2 = attacker.id;
            new AttackSynchMessage().Write(Program.client.sentBitStream, Program.client, npcPlayer.id, id2, modeDamage, modeWeapon, item, weapon);

            return 0;
        }

        public static bool disableFunction = false;
        /// <summary>
        /// Dieser Hook wird durch die die Gothic-Funktionen für z.b. Waffe ziehen, NPC ermorden etc. aufgerufen
        /// arg0 ist der Typ ( In den Daedalus-Scripten ansehbar unter Passive Perceptions), die anderen beiden sind die Beteiligten NPC's (eventuell auch andere vobs).
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 CreatePassivePerception(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            if (StaticVars.serverConfig.NPCAIOnServer)
                return 0;
            Process Process = Process.ThisProcess();
            try
            {
                if (!StaticVars.Ingame)
                    return 0;

                if (disableFunction)
                {
                    disableFunction = false;
                    return 0;
                }

                
                int address = Convert.ToInt32(message);
                oCNpc npc = new oCNpc(Process, Process.ReadInt(address));

                int arg0 = Process.ReadInt(address + 4);
                zCVob arg1 = new zCVob(Process, Process.ReadInt(address + 8));
                zCVob arg2 = new zCVob(Process, Process.ReadInt(address + 12));


                if (!StaticVars.spawnedPlayerDict.ContainsKey(npc.Address))
                    return 0;
                Player npcPlayer = StaticVars.spawnedPlayerDict[npc.Address]; ;
                if (npcPlayer == null)
                    return 0;


                Player arg1Player = null; 
                Player arg2Player = null;

                if(StaticVars.spawnedPlayerDict.ContainsKey(arg1.Address))
                    arg1Player = StaticVars.spawnedPlayerDict[arg1.Address];
                if (StaticVars.spawnedPlayerDict.ContainsKey(arg2.Address))
                    arg2Player = StaticVars.spawnedPlayerDict[arg2.Address];

                int npcID = -1; int arg1ID = -1; int arg2ID = -1;
                if (npcPlayer != null)
                    npcID = npcPlayer.id;
                if (arg1Player != null)
                    arg1ID = arg1Player.id;
                if (arg2Player != null)
                    arg2ID = arg2Player.id;

                new PassivePerceptionMessage().Write(Program.client.sentBitStream, Program.client, arg0, npcID, arg1ID, arg2ID);

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(Process).Report(2, 'G', ex.ToString(), 0, "AI.cs", 0);
            }
            //
            //System.Windows.Forms.MessageBox.Show(arg0 + " | " + npc.ObjectName.Value + " | " + arg1.ObjectName.Value + " | " + arg2.ObjectName.Value);
            return 0;
        }


        /// <summary>
        /// Dieser Hook dient zum Blocken und Entblocken der Funktion. Ist die Funktion für den NPC geblockt (z.b. weil er nicht vom Spieler kontrolliert wird) wird die AiState nicht gesetzt.
        /// Damit sind keine Reaktionen vom NPC zu erwarten
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static Int32 ActivateRtnState(String message)
        {
            if (!StaticVars.Ingame)
                return 0;
            
            Process Process = Process.ThisProcess();
            

            int address = Convert.ToInt32(message);
            oCNpc_States npcstate = new oCNpc_States(Process, Process.ReadInt(address));
            
            NPC controlNPC = null;
            if (!StaticVars.serverConfig.NPCAIOnServer)
            {
                foreach (NPC npc in StaticVars.npcControlList)
                {
                    if (npcstate.Owner.Address == npc.npcPlayer.NPCAddress)
                    {
                        controlNPC = npc;
                        break;
                    }
                }
            }

            if (controlNPC != null && !StaticVars.serverConfig.NPCAIOnServer)
            {
                //Aktivieren
                byte[] arr = new byte[] { 0x64, 0xA1, 0x00 };
                Process.Write(arr, Program.ActivateRtnState.oldFuncInNewFunc.ToInt32());//ActivateRtnState
            }
            else
            {
                //Deaktivieren
                byte[] arr = new byte[] { 0xC2, 0x04, 0x00 };
                Process.Write(arr, Program.ActivateRtnState.oldFuncInNewFunc.ToInt32());//ActivateRtnState
            }

            return 0;
        }
    }
}
