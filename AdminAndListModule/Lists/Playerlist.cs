using System;
using System.Collections.Generic;
using System.Text;
using Gothic.mClasses;
using WinApi;
using Gothic.mClasses.Elements;
using Injection;
using Network;
using ListModule.Messages;
using Gothic.zClasses;
using Gothic.zTypes;
using GMP.Network.Messages;
using GMP.Modules;
using GMP.Helper;
using Modules;

namespace AdminAndListModule.Lists
{
    public class Playerlist : ManagedListBox
    {
        public Playerlist(Process process)
            : base(process)
        {

        }

        public void Open()
        {
            InputHooked.deaktivateFullControl(process);
            Program.chatBox.tb.Inputenabled = false;
            Enable();
            SetStandard();
            
        }

        public void SetStandard()
        {
            Data.Clear();
            if (Loader.sOptions.AdminAvailable)
            {
                mEButton adminButton = new mEButton();
                adminButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Administration");
                adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(AdminList);
                Data.Add(adminButton);
            }

            if (Loader.sOptions.PlayerListAvailable || AdminMessage.isAdmin)
            {
                foreach (Player player in StaticVars.playerlist)
                {
                    mEButton playerButton = new mEButton();
                    playerButton.Data = player;
                    playerButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(PlayerFeatures);
                    Data.Add(playerButton);
                }
            }
            ActiveID = 0;
        }

        public void Close()
        {
            InputHooked.activateFullControl(process);
            Disable();
            Program.chatBox.tb.Inputenabled = true;
        }

        mETextBox username;
        mETextBox password;
        public void AdminList(object sender, mEButton.ButtonPressedEventArg args)
        {
            //Adminliste neu machen!
            if (AdminMessage.isAdmin)
            {
                Data.Clear();
                mEButton backButton = null;

                if ((AdminMessage.permission & 4096) == 4096 && lastSpawnedNPCInstance != null)
                {
                    spawnNPC = new mETextBox();
                    spawnNPC.hardText = StaticVars.Languages.getValue(Program.PrimLangList, "NPC")+": ";
                    spawnNPC.Data = lastSpawnedNPCInstance;

                    backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Spawn")+": " + lastSpawnedNPCInstance;
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SpawnNPC_EXECUTE);
                    Data.Add(backButton);
                }

                if ((AdminMessage.permission & 4096) == 4096)
                {
                    backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Spawn_NPC");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SpawnNPC);
                    Data.Add(backButton);
                }

                if ((AdminMessage.permission & 4096) == 4096 && Loader.spawnFunctions != null && Loader.spawnFunctions.Length != 0)
                {
                    backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Spawn_Function");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SpawnNPCFunction);
                    Data.Add(backButton);
                }

                if ((AdminMessage.permission & 4096) == 4096)
                {
                    backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Remove_ALL_NPC");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(RemoveAllNPC);
                    Data.Add(backButton);
                }

                
                backButton = new mEButton();
                backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Set_Time");
                backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTime_B);
                backButton.actionID = 16;
                Data.Add(backButton);
                

                backButton = new mEButton();
                backButton.Data = "Zuruck";
                backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
                Data.Add(backButton);
                ActiveID = 0;
            }
            else
            {
                Data.Clear();
                //2Textboxen

                username = new mETextBox();
                username.hardText = "User: ";
                Data.Add(username);

                password = new mETextBox();
                password.hardText = "Password: ";
                Data.Add(password);

                mEButton adminButton = new mEButton();
                adminButton.Data = "Login";
                adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(LoginButton);
                Data.Add(adminButton);

                adminButton = new mEButton();
                adminButton.Data = "Zuruck";
                adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
                Data.Add(adminButton);

                ActiveID = 0;
            }
        }


        public void RemoveAllNPC(object sender, mEButton.ButtonPressedEventArg args)
        {
            foreach(NPC npc in StaticVars.npcList)
                CommandoMessage.RemoveNPC(npc.npcPlayer.id);
            BackButton(sender, args);
        }


        public void LoginButton(object sender, mEButton.ButtonPressedEventArg args)
        {
            new AdminMessage().Write(Program.client.sentBitStream, Program.client, 0, username.Data.ToString(), password.Data.ToString(), 0);
            SetStandard();
        }

        public void BackButton(object sender, mEButton.ButtonPressedEventArg args)
        {
            SetStandard();
        }

        int selectedPlayer = -1;
        int otherPlayer = -1;

        public void Fluestern(object sender, mEButton.ButtonPressedEventArg args)
        {
            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
            Program.chatBox.lastWhispered = pl.name;
            Program.chatBox.change((byte)1);

            BackButton(sender, args);
        }


        public void FriendButton(object sender, mEButton.ButtonPressedEventArg args)
        {
            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
            if (pl == null)
                return;
            byte type = (byte)((mEButton)sender).actionID;
            pl.isFriend = type;
            
            
            
            NPCHelper.setFriend(pl);

            new FriendMessage().Write(Program.client.sentBitStream, Program.client, pl, type);

            BackButton(sender, args);
        }
        public static List<Player> Friends = new List<Player>();
        public static List<Player> RequestedFriends = new List<Player>();//Du hast angefragt
        public static List<Player> FriendsRequested = new List<Player>();//Er hat angefragt...
        public void PlayerFeatures(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();

            selectedPlayer = ((Player)((mEButton)sender).Data).id;
            Player pl = (Player)((mEButton)sender).Data;

            if (StaticVars.serverConfig.chatOptions.Private && pl != Program.Player)
            {
                mEButton playerButton = new mEButton();
                playerButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Whisper");
                playerButton.actionID = 14;
                playerButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Fluestern);
                Data.Add(playerButton);
            }

            if (StaticVars.serverConfig.AvailableFriends && pl != Program.Player)//Todo Partnerschaften erlaubt durch Serverconfig?
            {

                if (pl.isFriend == 1)//Freund schon in Freundesliste
                {
                    mEButton playerButton = new mEButton();
                    playerButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Friend_Remove");
                    playerButton.actionID = 0;
                    playerButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(FriendButton);
                    Data.Add(playerButton);
                }
                else if (pl.isFriend == 2)//Nichts tun, warten auf annahme (du hast losgeschickt)
                {}
                else if (pl.isFriend == 3)//Freundschaft annehmen, er hat losgeschickt
                {
                    mEButton playerButton = new mEButton();
                    playerButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Friend_Accept");
                    playerButton.actionID = 1;
                    playerButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(FriendButton);
                    Data.Add(playerButton);
                }
                else
                {
                    mEButton playerButton = new mEButton();
                    playerButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Friend_Add");
                    playerButton.actionID = 2;
                    playerButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(FriendButton);
                    Data.Add(playerButton);
                }

                
            }

            if (AdminMessage.isAdmin)
            {

                if ((AdminMessage.permission & 1) == 1)
                {
                    mEButton backButton = new mEButton();
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SentAdmin);
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Kick");
                    backButton.actionID = 2;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 2) == 2)
                {
                    mEButton backButton = new mEButton();
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SentAdmin);
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Ban");
                    backButton.actionID = 3;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 4) == 4)
                {
                    mEButton backButton = new mEButton();
                    if(pl.isMuted)
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Unmute");
                    else
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Mute");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SentAdmin);
                    backButton.actionID = 4;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 8) == 8)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Teleport_To_Player");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Teleport);
                    backButton.actionID = 5;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 16) == 16)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Teleport_Player_To_Me");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SentAdmin);
                    backButton.actionID = 6;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 32) == 32)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Invisible");
                    if (pl.isInvisible)
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Invisible_Off");
                    else
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Invisible_On");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SentAdmin);
                    backButton.actionID = 7;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 64) == 64)
                {
                    mEButton backButton = new mEButton();
                    if(pl.isImmortal)
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Immoral_Off");
                    else
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Immoral_On");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SentAdmin);
                    backButton.actionID = 8;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 128) == 128)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Give_Item");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(GiveItem);
                    backButton.actionID = 9;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 256) == 256)
                {
                    mEButton backButton = new mEButton();
                    if (pl.isFreeze)
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Unfreeze");
                    else
                        backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Freeze");
                    backButton.actionID = 10;
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SentAdmin);
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 512) == 512)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Kill");
                    backButton.actionID = 11;
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Kill);
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 1024) == 1024)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Revive");
                    backButton.actionID = 12;
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Revive);
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 2048) == 2048)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Temp_Mod_Rights");
                    backButton.actionID = 13;
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 4096) == 4096)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Scale");
                    backButton.actionID = 14;
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Scale_B);
                    Data.Add(backButton);
                }
                if ((AdminMessage.permission & 8192) == 8192)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Set_Fatness");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetFatness_B);
                    backButton.actionID = 15;
                    Data.Add(backButton);
                }//16 auch belegt

                if ((AdminMessage.permission & 16384) == 16384)
                {
                    mEButton backButton = new mEButton();
                    backButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Set_Talents");
                    backButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B);
                    backButton.actionID = 17;
                    Data.Add(backButton);
                }
            }




            mEButton bButton = new mEButton();
            bButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Back_Immoral_Off");
            bButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(bButton);
            ActiveID = 0;
        }

        mETextBox Scale_Z;
        public void SetTalents_B(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();
            //2Textboxen
            mEButton talButton = null;

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Onehand");
            talButton.actionID = 1;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Twohand");
            talButton.actionID = 2;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Bow");
            talButton.actionID = 3;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Crossbow");
            talButton.actionID = 4;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Lockpicking");
            talButton.actionID = 5;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Pickpocket_G1");
            talButton.actionID = 6;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Magic");
            talButton.actionID = 7;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Sneak");
            talButton.actionID = 8;
            Data.Add(talButton);


            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Acrobatics");
            talButton.actionID = 11;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Pickpocket_G2");
            talButton.actionID = 12;
            Data.Add(talButton);


            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Forging");
            talButton.actionID = 13;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Runes");
            talButton.actionID = 14;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Alchemy");
            talButton.actionID = 15;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Animal_Trophys");
            talButton.actionID = 16;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTalents_B2);
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Foreign_Languages");
            talButton.actionID = 17;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Back");
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(talButton);

            ActiveID = 0;
        }

        public void SetTalents_B2(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();

            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
            
            int ts = pl.lastTalentSkills[((mEButton)sender).actionID];
            int tv = 0;
            if (pl.isSpawned)
            {
                oCNpc npc = new oCNpc(Process.ThisProcess(), pl.NPCAddress);
                ts = npc.GetTalentSkill(((mEButton)sender).actionID);
                tv = npc.GetTalentValue(((mEButton)sender).actionID);
            }

            mEButton talButton = null;
            
            talButton = new mEButton();
            talButton.Data = ((mEButton)sender).Data;
            Data.Add(talButton);

            iteminstance = new mETextBox();
            iteminstance.hardText = "Skill: ";
            iteminstance.Data = ""+ts;
            Data.Add(iteminstance);

            itemCount = new mETextBox();
            itemCount.hardText = "Value: ";
            iteminstance.Data = "" + tv;
            Data.Add(itemCount);

            talButton = new mEButton();
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Set_Talent");
            talButton.actionID = ((mEButton)sender).actionID;
            Data.Add(talButton);

            talButton = new mEButton();
            talButton.Data = StaticVars.Languages.getValue(Program.PrimLangList, "Back");
            talButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(talButton);

            ActiveID = 0;
        }

        public void SetTalents_Execute(object sender, mEButton.ButtonPressedEventArg args)
        {
            if (iteminstance == null || iteminstance.Data == null || ((String)iteminstance.Data).Length == 0)
                return;
            if (itemCount == null || itemCount.Data == null || ((String)itemCount.Data).Length == 0)
                return;

            Process process = Process.ThisProcess();

            int skill = 0;
            int value = 0;
            try
            {
                skill = Convert.ToInt32(iteminstance.Data);
                value = Convert.ToInt32(itemCount.Data);
            }
            catch (Exception ex) { External_Helper.Print(process, StaticVars.Languages.getValue(Program.PrimLangList, "Wrong_Value")); return; }

            
            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
            Commando.SetTalent(pl, ((mEButton)sender).actionID, value, skill);

            iteminstance = null;
            itemCount = null;

            BackButton(sender, args);
        }


        public void SetTime_B(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();
            //2Textboxen

            iteminstance = new mETextBox();
            iteminstance.hardText = "Tag: ";
            iteminstance.Data = "" + TimeMessage.day;
            Data.Add(iteminstance);

            itemCount = new mETextBox();
            itemCount.hardText = "Stunde: ";
            itemCount.Data = "" + TimeMessage.hour;
            Data.Add(itemCount);

            Scale_Z = new mETextBox();
            Scale_Z.hardText = "Minute: ";
            Scale_Z.Data = ""+TimeMessage.minute;
            Data.Add(Scale_Z);

            mEButton adminButton = new mEButton();
            adminButton.Data = "Bestatigen";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetTime_Execute);
            Data.Add(adminButton);

            adminButton = new mEButton();
            adminButton.Data = "Zuruck";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(adminButton);

            ActiveID = 0;
        }

        public void SetTime_Execute(object sender, mEButton.ButtonPressedEventArg args)
        {
            if (iteminstance == null || iteminstance.Data == null || ((String)iteminstance.Data).Length == 0)
                return;
            if (itemCount == null || itemCount.Data == null || ((String)itemCount.Data).Length == 0)
                return;
            if (Scale_Z == null || Scale_Z.Data == null || ((String)Scale_Z.Data).Length == 0)
                return;

            try
            {
                Convert.ToInt32(iteminstance.Data);
                Convert.ToInt32(itemCount.Data);
                Convert.ToInt32(Scale_Z.Data);
            }
            catch (Exception ex) { External_Helper.Print(process, StaticVars.Languages.getValue(Program.PrimLangList, "Wrong_Value")); return; }


            new AdminMessage().Write(Program.client.sentBitStream, Program.client,
                16, (String)iteminstance.Data + ";" + (String)itemCount.Data + ";" + (String)Scale_Z.Data, "", selectedPlayer);

            iteminstance = null;

            BackButton(sender, args);
        }




        public void Scale_B(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();
            //2Textboxen

            iteminstance = new mETextBox();
            iteminstance.hardText = "X: ";
            Data.Add(iteminstance);

            itemCount = new mETextBox();
            itemCount.hardText = "Y: ";
            Data.Add(itemCount);

            Scale_Z = new mETextBox();
            Scale_Z.hardText = "Z: ";
            Data.Add(Scale_Z);

            mEButton adminButton = new mEButton();
            adminButton.Data = "Bestatigen";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(Scale_Execute);
            Data.Add(adminButton);

            adminButton = new mEButton();
            adminButton.Data = "Zuruck";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(adminButton);

            ActiveID = 0;
        }

        public void Scale_Execute(object sender, mEButton.ButtonPressedEventArg args)
        {
            if (iteminstance == null || iteminstance.Data == null || ((String)iteminstance.Data).Length == 0)
                return;
            if (itemCount == null || itemCount.Data == null || ((String)itemCount.Data).Length == 0)
                return;
            if (Scale_Z == null || Scale_Z.Data == null || ((String)Scale_Z.Data).Length == 0)
                return;

            try
            {
                Convert.ToSingle(iteminstance.Data);
                Convert.ToSingle(itemCount.Data);
                Convert.ToSingle(Scale_Z.Data);
            }
            catch (Exception ex) { External_Helper.Print(process, StaticVars.Languages.getValue(Program.PrimLangList, "Wrong_Value")); return; }


            new AdminMessage().Write(Program.client.sentBitStream, Program.client,
                14, (String)iteminstance.Data + ";" + (String)itemCount.Data + ";" + (String)Scale_Z.Data, "", selectedPlayer);

            iteminstance = null;

            BackButton(sender, args);
        }



        public void SetFatness_B(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();
            //2Textboxen

            iteminstance = new mETextBox();
            iteminstance.hardText = "Fatness: ";
            Data.Add(iteminstance);

            mEButton adminButton = new mEButton();
            adminButton.Data = "Bestatigen";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SetFatness_Execute);
            Data.Add(adminButton);

            adminButton = new mEButton();
            adminButton.Data = "Zuruck";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(adminButton);

            ActiveID = 0;
        }

        public void SetFatness_Execute(object sender, mEButton.ButtonPressedEventArg args)
        {
            if (iteminstance == null || iteminstance.Data == null || ((String)iteminstance.Data).Length == 0)
                return;

            try
            {
                Convert.ToSingle(iteminstance.Data);
            }
            catch (Exception ex) { External_Helper.Print(process, StaticVars.Languages.getValue(Program.PrimLangList, "Wrong_Value")); return; }


            new AdminMessage().Write(Program.client.sentBitStream, Program.client,
                15, (String)iteminstance.Data, "", selectedPlayer);

            iteminstance = null;

            BackButton(sender, args);
        }

        
        public void SentAdmin(object sender, mEButton.ButtonPressedEventArg args)
        {
            String param = "";
            if ((byte)((mEButton)sender).actionID == 8)
            {
                Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
                if (pl.isImmortal)
                {
                    param = "0";
                }
                else
                {
                    param = "1";
                }
            }
            else if ((byte)((mEButton)sender).actionID == 7)
            {
                Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
                if (pl.isInvisible)
                {
                    param = "0";
                }
                else
                {
                    param = "1";
                }
            }
            else if ((byte)((mEButton)sender).actionID == 10)
            {
                Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
                if (pl.isFreeze)
                {
                    param = "0";
                }
                else
                {
                    param = "1";
                }
            }
            else if ((byte)((mEButton)sender).actionID == 4)
            {
                Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
                if (pl.isMuted)
                {
                    param = "0";
                }
                else
                {
                    param = "1";
                }
            }
            new AdminMessage().Write(Program.client.sentBitStream, Program.client,
                (byte)((mEButton)sender).actionID, param, "", selectedPlayer);
            
            BackButton(sender, args);
        }


        public void Teleport(object sender, mEButton.ButtonPressedEventArg args)
        {
            Process process = Process.ThisProcess();
            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);

            oCNpc target = new oCNpc(process, pl.NPCAddress);
            oCNpc hero = oCNpc.Player(process);

            if(!pl.isSpawned)
            {
                zString nowlevel = zString.Create(process, Program.Player.actualMap);
                zString targetlevel = zString.Create(process, pl.actualMap);
                oCGame.Game(process).ChangeLevel(targetlevel, targetlevel);
                nowlevel.Dispose(); targetlevel.Dispose();
            }

            hero.TrafoObjToWorld.set(3, pl.pos[0]);
            hero.TrafoObjToWorld.set(7, pl.pos[1]);
            hero.TrafoObjToWorld.set(11, pl.pos[2]);

            BackButton(sender, args);
        }

        public void Kill(object sender, mEButton.ButtonPressedEventArg args)
        {
            Process process = Process.ThisProcess();
            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);

            oCNpc target = new oCNpc(process, pl.NPCAddress);
            oCNpc hero = oCNpc.Player(process);

            target.HP = 0;
            BackButton(sender, args);
        }

        public void Revive(object sender, mEButton.ButtonPressedEventArg args)
        {
            Process process = Process.ThisProcess();
            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);

            oCNpc target = new oCNpc(process, pl.NPCAddress);
            oCNpc hero = oCNpc.Player(process);

            target.HP = target.HPMax;
            zVec3 pos = target.GetPosition();
            target.ResetPos(pos);
            pos.Dispose();
            BackButton(sender, args);
        }


        mETextBox iteminstance;
        mETextBox itemCount;
        public void GiveItem(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();
            //2Textboxen

            iteminstance = new mETextBox();
            iteminstance.hardText = "Item: ";
            Data.Add(iteminstance);

            itemCount = new mETextBox();
            itemCount.hardText = "Anzahl: ";
            Data.Add(itemCount);

            mEButton adminButton = new mEButton();
            adminButton.Data = "Bestatigen";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(GiveItem_Execute);
            Data.Add(adminButton);

            adminButton = new mEButton();
            adminButton.Data = "Zuruck";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(adminButton);

            ActiveID = 0;
        }


        public void GiveItem_Execute(object sender, mEButton.ButtonPressedEventArg args)
        {
            String item = (String)iteminstance.Data;
            int count = 0;

            

            try
            {
                count = Convert.ToInt32((String)itemCount.Data);
            }
            catch (Exception ex) { }
            

            if (item == null || item.Trim().Length == 0 || count == 0)
                return;
            Process process = Process.ThisProcess();
            Player pl = Player.getPlayer(selectedPlayer, StaticVars.playerlist);
            oCNpc target = new oCNpc(process, pl.NPCAddress);

            zString str = zString.Create(process, item.Trim());
            int i = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            if (i == -1)
            {
                External_Helper.Print(process, "Instance ungueltig: " + item);
                return;
            }

            Commando.GiveItem(pl, item, count);
            


            
            BackButton(sender, args);
        }



        public void SpawnNPCFunction(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();

            mEButton adminButton = null;
            for (int i = 0; i < Loader.spawnFunctions.Length; i++ )
            {
                adminButton = new mEButton();
                adminButton.Data = Loader.spawnFunctions[i].name;
                adminButton.actionID = i;
                adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SpawnNPCFunction_EXECUTE);
                Data.Add(adminButton);
            }


            adminButton = new mEButton();
            adminButton.Data = "Zuruck";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(adminButton);

            ActiveID = 0;
        }

        mETextBox spawnNPC = null;
        public void SpawnNPC(object sender, mEButton.ButtonPressedEventArg args)
        {
            Data.Clear();
            //2Textboxen

            spawnNPC = new mETextBox();
            spawnNPC.hardText = "NPC: ";
            Data.Add(spawnNPC);

            mEButton adminButton = new mEButton();
            adminButton.Data = "Bestatigen";
            adminButton.actionID = (byte)((mEButton)sender).actionID;
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(SpawnNPC_EXECUTE);
            Data.Add(adminButton);

            adminButton = new mEButton();
            adminButton.Data = "Zuruck";
            adminButton.ButtonPressed += new EventHandler<mEButton.ButtonPressedEventArg>(BackButton);
            Data.Add(adminButton);

            ActiveID = 0;
        }

        public static String lastSpawnedNPCInstance = null;
        public void SpawnNPC_EXECUTE(object sender, mEButton.ButtonPressedEventArg args)
        {

            String item = (String)spawnNPC.Data;

            if (item == null || item.Trim().Length == 0 )
                return;

            Process process = Process.ThisProcess();
            zString str = zString.Create(process, item);
            int i = zCParser.getParser(process).GetIndex(str);
            str.Dispose();
            if (i == -1)
            {
                External_Helper.Print(process, "Instance ungueltig: "+item);
                return;
            }

            lastSpawnedNPCInstance = item;
            new StaticNPCMessage().Write(Program.client.sentBitStream, Program.client, item, Program.Player.actualMap,
                Program.Player.pos[0], Program.Player.pos[1], Program.Player.pos[2]);

            BackButton(sender, args);
        }


        public void SpawnNPCFunction_EXECUTE(object sender, mEButton.ButtonPressedEventArg args)
        {
            SpawnFunction spawnFunction = Loader.spawnFunctions[((mEButton)sender).actionID];
            Process process = Process.ThisProcess();


            if (spawnFunction == null)
                return;
            foreach (SpawnContent sc in spawnFunction.Spawns)
            {
                if (sc.instance == null)
                    continue;
                zString str = zString.Create(process, sc.instance);
                int i = zCParser.getParser(process).GetIndex(str);
                str.Dispose();
                if (i == -1)
                {
                    External_Helper.Print(process, "Instance ungueltig: " + sc.instance);
                    return;
                }


                foreach (object spawn in sc.Spawns)
                {

                    if (spawn == null || (spawn.GetType() == typeof(String) && !Player.isSameMap(Program.Player.actualMap,  sc.world)))
                        continue;
                    Vec3 position = new Vec3();

                    if (spawn.GetType() == typeof(String))
                    {
                        zCWaypoint wp = oCGame.Game(process).World.WayNet.GetWaypointByName((String)spawn);
                        if (wp == null)
                            continue;
                        position.x = wp.Position.X;
                        position.y = wp.Position.Y;
                        position.z = wp.Position.Z;
                    }
                    else
                    {
                        position = (Vec3)spawn;
                    }

                    lastSpawnedNPCInstance = sc.instance;
                    new StaticNPCMessage().Write(Program.client.sentBitStream, Program.client, sc.instance, sc.world,
                        position.x, position.y, position.z);

                }
            }
            

            

            BackButton(sender, args);
        }
    }
}
