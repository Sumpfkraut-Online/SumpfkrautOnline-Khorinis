using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using RakNet;
using Network;
using Gothic.zClasses;
using WinApi;
using Injection;
using Gothic.zTypes;
using GMP.Helper;
using GMP.Modules;
using GMP.Net.Messages;
using Gothic.mClasses;

namespace GMP.Network.Messages
{
    public class CommandoMessage : Message
    {
        static Dictionary<Int32, GUC.GUI.View> viewList = new Dictionary<int, GUC.GUI.View>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="client"></param>
        /// <param name="playerid">-1 für sentToAll</param>
        /// <param name="type"></param>
        /// <param name="arguments1"></param>
        /// <param name="arguments2"></param>
        /// <param name="arguments3"></param>
        public void Write(RakNet.BitStream stream, Client client, int playerid, byte type, String[] arguments1, int[] arguments2, float[] arguments3)
        {
            //CommandoMessage
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.CommandoMessage);
            stream.Write((byte)type);

            if (playerid < 0)
            {
                stream.Write((byte)CommandoFlags.sentToAll);
            }
            else
            {
                stream.Write((byte)CommandoFlags.sentToPlayer);
                stream.Write(playerid);
            }
            
            CommandoArgumentsFlags argFlag = CommandoArgumentsFlags.None;
            if ((arguments1 != null && arguments1.Length != 0))
                argFlag |= CommandoArgumentsFlags.Arg1;
            if ((arguments2 != null && arguments2.Length != 0))
                argFlag |= CommandoArgumentsFlags.Arg2;
            if ((arguments3 != null && arguments3.Length != 0))
                argFlag |= CommandoArgumentsFlags.Arg3;

            stream.Write((byte)argFlag);
            if ((argFlag & CommandoArgumentsFlags.Arg1) == CommandoArgumentsFlags.Arg1)
            {
                stream.Write(arguments1.Length);
                foreach (String str in arguments1)
                    stream.Write(str);
            }
            if ((argFlag & CommandoArgumentsFlags.Arg2) == CommandoArgumentsFlags.Arg2)
            {
                stream.Write(arguments2.Length);
                foreach (int str in arguments2)
                    stream.Write(str);
            }
            if ((argFlag & CommandoArgumentsFlags.Arg3) == CommandoArgumentsFlags.Arg3)
            {
                stream.Write(arguments3.Length);
                foreach (float str in arguments3)
                    stream.Write(str);
            }
            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);


            if (!StaticVars.sStats.ContainsKey((int)NetWorkIDS.CommandoMessage))
                StaticVars.sStats[(int)NetWorkIDS.CommandoMessage] = 0;
            StaticVars.sStats[(int)NetWorkIDS.CommandoMessage] += 1;
        }


        public override void Read(BitStream stream, Packet packet, Client client)
        {
            byte type = 0; byte commandoFlag = 0; byte argFlag = 0;
            int playerid = -1;
            String[] arguments1 = null; int[] arguments2 = null; float[] arguments3 = null;


            stream.Read(out type);
            stream.Read(out commandoFlag);
            if (commandoFlag == (int)CommandoFlags.sentToPlayer)
                stream.Read(out playerid);
            stream.Read(out argFlag);
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg1) == (byte)CommandoArgumentsFlags.Arg1)
            {
                int argLength = 0;
                stream.Read(out argLength);
                arguments1 = new string[argLength];
                for (int i = 0; i < argLength; i++)
                    stream.Read(out arguments1[i]);
            }
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg2) == (byte)CommandoArgumentsFlags.Arg2)
            {
                int argLength = 0;
                stream.Read(out argLength);
                arguments2 = new int[argLength];
                for (int i = 0; i < argLength; i++)
                    stream.Read(out arguments2[i]);
            }
            if ((argFlag & (byte)CommandoArgumentsFlags.Arg3) == (byte)CommandoArgumentsFlags.Arg3)
            {
                int argLength = 0;
                stream.Read(out argLength);
                arguments3 = new float[argLength];
                for (int i = 0; i < argLength; i++)
                    stream.Read(out arguments3[i]);
            }


            //Daten bearbeiten:
            Process Process = Process.ThisProcess();
            if (type == (byte)CommandoType.SetTalents)
            {
                new oCNpc(Process, Program.Player.NPCAddress).SetTalentSkill(arguments2[0], arguments2[2]);
                new oCNpc(Process, Program.Player.NPCAddress).SetTalentValue(arguments2[0], arguments2[1]);
            }

            if (type == (byte)CommandoType.GiveItems)
            {
                zString str = zString.Create(Process, arguments1[0]);
                int index = zCParser.getParser(Process).GetIndex(str);
                str.Dispose();

                if(index <= 0)
                    return;
                if (!StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    return;

                Player pl = StaticVars.AllPlayerDict[arguments2[0]];
                if (pl == null)
                    return;
                pl.InsertItem(new item() { code = arguments1[0], Amount = arguments2[1] });
                if (pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    oCItem itm = npc.PutInInv(index, arguments2[1]);
                    npc.RemoveFromInv(itm, itm.Amount);
                    npc.PutInInv(itm);
                }
            }

            if (type == (byte)CommandoType.SetInventory)
            {
                if (!StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    return;
                Player pl = StaticVars.AllPlayerDict[arguments2[arguments2.Length - 1]];
                if (pl == null)
                    return;
                pl.itemList.Clear();
                for (int i = 0; i < arguments1.Length; i++)
                {
                    pl.InsertItem(new item() { code = arguments1[i], Amount = arguments2[i] });
                }
                InventoryHelper.RemoveInventory(pl);
                InventoryHelper.AddInventory(pl);
            }

            if (type == (byte)CommandoType.RemoveNPC)
            {
                if (!StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    return;
                Player pl = StaticVars.AllPlayerDict[arguments2[0]];
                if (!pl.isNPC)
                    return;
                NPCHelper.RemoveNPC(pl.NPC);

            }else if(type == (byte)CommandoType.SetPosition)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null)
                    pl.pos = new float[] { arguments3[0], arguments3[1], arguments3[2] };

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    Matrix4 trafo = npc.TrafoObjToWorld;

                    trafo.set(3, pl.pos[0]);
                    trafo.set(7, pl.pos[1]);
                    trafo.set(11, pl.pos[2]);
                }

            }
            else if (type == (byte)CommandoType.SetDirection)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null)
                    pl.dir = new float[] { arguments3[0], arguments3[1], arguments3[2] };

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    PlayerStatusMessage.SetDirection(pl);
                }

            }
            else if (type == (byte)CommandoType.SetAngle)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    PlayerStatusMessage.SetAngle(pl, arguments3[0]);
                }

            }
            else if (type == (byte)CommandoType.SetHP)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null)
                    pl.lastHP = arguments2[1];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.HP = pl.lastHP;
                }

            }
            else if (type == (byte)CommandoType.SetMaxHP)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null)
                    pl.lastHP_Max = arguments2[1];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.HPMax = pl.lastHP_Max;
                }

            }
            else if (type == (byte)CommandoType.SetMana)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null)
                    pl.lastMP = arguments2[1];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.MP = pl.lastMP;
                }

            }
            else if (type == (byte)CommandoType.SetMaxMana)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];
                
                if(pl != null)
                    pl.lastMP_Max = arguments2[1];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.MPMax = pl.lastMP_Max;
                    
                }

            }
            else if (type == (byte)CommandoType.SetStrength)
            {

                if (!StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    return;

                Player pl = StaticVars.AllPlayerDict[arguments2[0]];
                if (pl != null)
                    pl.lastStr = arguments2[1];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.Strength = pl.lastStr;

                }

            }
            else if (type == (byte)CommandoType.SetDexterity)
            {

                if (!StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    return;

                Player pl = StaticVars.AllPlayerDict[arguments2[0]];
                if (pl != null)
                    pl.lastDex = arguments2[1];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.Dexterity = pl.lastDex;
                    
                }

            }
            else if (type == (byte)CommandoType.PlayAnimation)
            {

                Player pl = null;
                if(StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];
                

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    zString str = zString.Create(Process, arguments1[0]);
                    npc.GetModel().StartAnimation(str);
                    npc.GetModel().StartAni(str, 1);
                    str.Dispose();
                }

            }
            else if (type == (byte)CommandoType.StopAnimation)
            {

                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];


                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    zString str = zString.Create(Process, arguments1[0]);
                    npc.GetModel().StopAnimation(str);
                    str.Dispose();
                }

            }
            else if (type == (byte)CommandoType.FreezePlayer)
            {
                if(arguments2[0] == 0)
                    oCNpc.Freeze(Process, false);
                else
                    oCNpc.Freeze(Process, true);
                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Freeze-Command...", 0, "Program.cs", 0);

            }
            else if (type == (byte)CommandoType.Revive)
            {
                Player pl = null;
                if(StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];


                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.ResetPos(npc.GetPosition());
                }
            }
            else if (type == (byte)CommandoType.ChangeWorld)
            {
                zString targetlevel = zString.Create(Process, Player.getMap(arguments1[0]));
                oCGame.Game(Process).ChangeLevel(targetlevel, targetlevel);
                targetlevel.Dispose();
            }
            else if (type == (byte)CommandoType.Equip)
            {
                PlayerStatusMessage.Equip(oCNpc.Player(Process), arguments1[0]);
            }
            else if (type == (byte)CommandoType.EquipArmor)
            {
                PlayerStatusMessage.EquipArmor(oCNpc.Player(Process), arguments1[0]);
            }
            else if (type == (byte)CommandoType.EquipWeapon)
            {
                PlayerStatusMessage.EquipWeapon(oCNpc.Player(Process), arguments1[0]);
            }
            else if (type == (byte)CommandoType.EquipRangeWeapon)
            {
                PlayerStatusMessage.EquipRangeWeapon(oCNpc.Player(Process), arguments1[0]);
            }
            else if (type == (byte)CommandoType.TextureCreate)
            {
                GUC.GUI.Texture texture = new GUC.GUI.Texture(arguments2[0], arguments1[0], arguments2[1], arguments2[2], arguments2[3], arguments2[4]);
                viewList.Add(arguments2[0], texture);

                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Create Texture", 0, "Program.cs", 0);
            }
            else if (type == (byte)CommandoType.TextureTex)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.Texture texture = (GUC.GUI.Texture)viewList[arguments2[0]];
                texture.setTexture(arguments1[0]);
            }
            else if (type == (byte)CommandoType.TextureSize)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.Texture texture = (GUC.GUI.Texture)viewList[arguments2[0]];
                texture.setSize(arguments2[1], arguments2[2]);
            }
            else if (type == (byte)CommandoType.ViewSetPosition)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.View texture = viewList[arguments2[0]];
                texture.setPosition(arguments2[1], arguments2[2]);
            }
            else if (type == (byte)CommandoType.ViewShow)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.View texture = viewList[arguments2[0]];
                texture.show();

                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Show texture", 0, "Program.cs", 0);
            }
            else if (type == (byte)CommandoType.ViewHide)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.View texture = viewList[arguments2[0]];
                texture.show();
            }
            else if (type == (byte)CommandoType.ViewDestroy)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.View texture = viewList[arguments2[0]];
                texture.Destroy();
            }
            else if (type == (byte)CommandoType.TextCreate)
            {
                GUC.GUI.Texture tex = null;
                if(viewList.ContainsKey(arguments2[3]))
                    tex = (GUC.GUI.Texture)viewList[arguments2[3]];

                GUC.GUI.Text texture = new GUC.GUI.Text(arguments2[0], arguments1[0], arguments1[1], arguments2[1], arguments2[2], tex, arguments2[4], arguments2[5], arguments2[6], arguments2[7]);
                viewList.Add(arguments2[0], texture);

                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Create Text with font:" + arguments1[1], 0, "Program.cs", 0);
            }
            else if (type == (byte)CommandoType.TextSet)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.Text texture = (GUC.GUI.Text)viewList[arguments2[0]];
                texture.setText(arguments1[0]);
            }
            else if (type == (byte)CommandoType.TextSetColor)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.Text texture = (GUC.GUI.Text)viewList[arguments2[0]];
                texture.setColor(arguments2[1], arguments2[2], arguments2[3], arguments2[4]);
            }
            else if (type == (byte)CommandoType.TextSetFont)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.Text texture = (GUC.GUI.Text)viewList[arguments2[0]];
                texture.setFont(arguments1[0]);
            }
            else if (type == (byte)CommandoType.EnablePlayerKey)
            {
                if (arguments2[0] == 1)
                {
                    if (Program.playerKeyMessage != null)
                        return;

                    Program.playerKeyMessage = new PlayerKeyMessage();
                }
                else
                {
                    if (Program.playerKeyMessage == null)
                        return;
                    InputHooked.receivers.Remove(Program.playerKeyMessage);
                    Program.playerKeyMessage = null;
                }
            }
            else if (type == (byte)CommandoType.TextBoxCreate)
            {
                GUC.GUI.Texture tex = null;
                if (viewList.ContainsKey(arguments2[3]))
                    tex = (GUC.GUI.Texture)viewList[arguments2[3]];

                GUC.GUI.TextBox texture = new GUC.GUI.TextBox(arguments2[0], arguments1[0], arguments1[1], arguments2[1], arguments2[2], tex, arguments2[4], arguments2[5], arguments2[6], arguments2[7], arguments2[9], arguments2[8], arguments2[10]);
                viewList.Add(arguments2[0], texture);

                zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Create Textbox with font:" + arguments1[1], 0, "Program.cs", 0);
            }
            else if (type == (byte)CommandoType.TextBoxSet)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                texture.setText(arguments1[0]);
            }
            else if (type == (byte)CommandoType.TextBoxColorSet)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                texture.setColor(arguments2[1], arguments2[2], arguments2[3], arguments2[4]);
            }
            else if (type == (byte)CommandoType.TextBoxFontSet)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                texture.setFont(arguments1[0]);
            }
            else if (type == (byte)CommandoType.TextBoxCallSend)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                texture.callSendText();
            }
            else if (type == (byte)CommandoType.TextBoxResetKey)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                texture.setResetKey(arguments2[1]);
            }
            else if (type == (byte)CommandoType.TextBoxSendKey)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                texture.setSendKey(arguments2[1]);
            }
            else if (type == (byte)CommandoType.TextBoxStartKey)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                texture.setEnableKey(arguments2[1]);
            }
            else if (type == (byte)CommandoType.TextBoxStartWirting)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.TextBox texture = (GUC.GUI.TextBox)viewList[arguments2[0]];
                if (arguments2[1] == 1)
                    texture.startWriting();
                else
                    texture.stopWriting();

            }
            else if (type == (byte)CommandoType.EnableChatBox)
            {

                if (arguments2[0] == 1)
                    Program.chatBox.show();
                else
                    Program.chatBox.hide();

            }
            else if (type == (byte)CommandoType.SetPlayerFatness)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                {
                    pl = StaticVars.AllPlayerDict[arguments2[0]];
                    pl.fatness = arguments3[0];
                }

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.Fatness = pl.fatness;
                    npc.SetFatness(pl.fatness);
                    
                }
            }
            else if (type == (byte)CommandoType.SetPlayerAdditionalVisual)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                {
                    pl = StaticVars.AllPlayerDict[arguments2[0]];
                    pl.headTex = arguments2[1];
                    pl.HeadVisual = arguments1[0];

                    pl.bodyTex = arguments2[2];
                    pl.BodyVisual = arguments1[1];

                }

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.BodyTex = (ushort)pl.bodyTex;
                    npc.BodyVisualString.Set(pl.BodyVisual);
                    npc.HeadTex = (ushort)pl.headTex;
                    npc.HeadVisualString.Set(pl.HeadVisual);

                    npc.SetHead();
                    npc.InitModel();

                }
            }
            else if (type == (byte)CommandoType.SetVoice)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                {
                    pl = StaticVars.AllPlayerDict[arguments2[0]];
                    pl.voice = arguments2[1];
                }

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.Voice = pl.voice;
                }
            }
            else if (type == (byte)CommandoType.OutputSVM)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    External_Helper.AI_OutputSVM_Overlay(Process, npc, npc, arguments1[0]);
                }
            }
            else if (type == (byte)CommandoType.SetInstance)
            {
                Player pl = null;
                if (StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    pl = StaticVars.AllPlayerDict[arguments2[0]];

                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    pl.instance = arguments1[0];
                    //NPCHelper.RemovePlayer(pl, false);
                    //NPCHelper.SpawnPlayer(pl, true);
                    //NPCHelper.SetStandards(pl);

                    zString instancestr = zString.Create(Process, pl.instance);
                    int npcid = zCParser.getParser(Process).GetIndex(instancestr);
                    instancestr.Dispose();
                    if (npcid == 0)
                    {
                        instancestr = zString.Create(Process, "PC_HERO");
                        npcid = zCParser.getParser(Process).GetIndex(instancestr);
                        instancestr.Dispose();
                    }


                    oCNpc oldPC = new oCNpc(Process, pl.NPCAddress);
                    oCNpc npc = oCObjectFactory.GetFactory(Process).CreateNPC(npcid);
                   
                    zVec3 p = oldPC.GetPosition();
                    npc.Enable(p);
                    p.Dispose();

                    npc.SetAsPlayer();
                    oCGame.Game(Process).GetSpawnManager().DeleteNPC(oldPC);
                    


                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "set Instance:" + arguments1[0], 0, "Program.cs", 0);
                }
            }
            else if (type == (byte)CommandoType.MessagesBoxCreate)
            {
                GUC.GUI.Texture tex = null;
                if (viewList.ContainsKey(arguments2[3]))
                    tex = (GUC.GUI.Texture)viewList[arguments2[3]];

                GUC.GUI.MessageBox texture = new GUC.GUI.MessageBox(arguments2[0], arguments2[4], arguments1[0], arguments2[1], arguments2[2], tex);
                viewList.Add(arguments2[0], texture);
            }
            else if (type == (byte)CommandoType.MessagesBoxSetLine)
            {
                if (!viewList.ContainsKey(arguments2[0]))
                    return;
                GUC.GUI.MessageBox texture = (GUC.GUI.MessageBox)viewList[arguments2[0]];
                texture.addMessage(arguments1[0], arguments2[1], arguments2[2], arguments2[3], arguments2[4]);
            }
            else if (type == (byte)CommandoType.SetPlayerScale)
            {

                if (!StaticVars.AllPlayerDict.ContainsKey(arguments2[0]))
                    return;

                Player pl = StaticVars.AllPlayerDict[arguments2[0]];
                if (pl != null)
                {
                    pl.scale = new float[] { arguments3[0], arguments3[1], arguments3[2] };
                }
                if (pl != null && pl.NPCAddress != 0 && pl.isSpawned)
                {
                    oCNpc npc = new oCNpc(Process, pl.NPCAddress);
                    npc.Scale.X = pl.scale[0];
                    npc.Scale.Y = pl.scale[1];
                    npc.Scale.Z = pl.scale[2];

                }

            }

            
        }

        public static void SetPlayerPosition(Player pl, float[] position)
        {
            new CommandoMessage().Write(Program.client.sentBitStream, Program.client, -1, (byte)CommandoType.SetPosition, null, new int[] { pl.id }, new float[]{position[0], position[1], position[2]});
        }

        public static void SetTalents(int id, int talentid, int talentvalue, int talentskill)
        {
            new CommandoMessage().Write(Program.client.sentBitStream, Program.client, id, (byte)CommandoType.SetTalents, null, new int[] { talentid, talentvalue, talentskill }, null);
        }

        public static void GiveItem(int id, string itemname, int amount)
        {
            Process Process = Process.ThisProcess();
            zString str = zString.Create(Process, itemname);
            int index = zCParser.getParser(Process).GetIndex(str);
            str.Dispose();

            if (index <= 0)
                return;
            new CommandoMessage().Write(Program.client.sentBitStream, Program.client, -1, (byte)CommandoType.GiveItems, new string[] { itemname }, new int[] { id, amount }, null);
        }

        public static void RemoveNPC(int id)
        {
            new CommandoMessage().Write(Program.client.sentBitStream, Program.client, -1, (byte)CommandoType.RemoveNPC, null, new int[] { id }, null);
        }

        public static void SetInventory(Player pl)
        {
            //InventoryHelper.RemoveInventory(pl);
            //InventoryHelper.AddInventory(pl);

            string[] items = new string[pl.itemList.Count];
            int[] amount = new int[pl.itemList.Count + 1 ];

            amount[amount.Length - 1] = pl.id;
            for (int i = 0; i < pl.itemList.Count; i++)
            {
                items[i] = pl.itemList[i].code;
                amount[i] = pl.itemList[i].Amount;
            }


            new CommandoMessage().Write(Program.client.sentBitStream, Program.client, -1, (byte)CommandoType.GiveItems, items, amount, null);
        }
    }
}
