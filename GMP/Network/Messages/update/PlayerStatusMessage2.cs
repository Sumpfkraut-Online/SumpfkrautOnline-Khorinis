using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using WinApi;
using Gothic.zClasses;
using RakNet;
using Network;
using Injection;
using GMP.Net.Messages;
using Gothic.zTypes;
using GMP.Modules;
using System.Text.RegularExpressions;

namespace GMP.Network.Messages
{
    public class PlayerStatusMessage2 : Message
    {
        static int npcid;
        static long lastUpdate;

        /// <summary>
        /// Schreibt aktuelle Spieler und NPC Informationen
        /// Nur von Menschen
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="client"></param>
        public override void Write(RakNet.BitStream stream, Client client)
        {

            WritePlayerMessage(Program.Player, stream, client);
            for (int i = npcid; i < npcid + 5; i++)
            {
                if (i >= StaticVars.npcControlList.Count)
                    break;
                WritePlayerMessage(StaticVars.npcControlList[i].npcPlayer, stream, client);
            }
            npcid += 5;
            if (StaticVars.npcControlList.Count <= npcid)
            {
                npcid = 0;
                lastUpdate = DateTime.Now.Ticks;
            }
        }


        private void WritePlayerMessage(Player pl, RakNet.BitStream stream, Client client)
        {
            if (pl == null)
                return;

            Process process = Process.ThisProcess();
            oCNpc player = new oCNpc(process, pl.NPCAddress);

            if (player == null || player.Address == 0 || player.VobType != zCVob.VobTypes.Npc ||
                (player.IsMonster() == 1))
                return;
            

            PSM_SentTypes psmSentTypes = PSM_SentTypes.None;
            //Aussehen
            if (!pl.isNPC && player.IsHuman() == 1)
            {
                psmSentTypes |= PSM_SentTypes.appearanceSent;
            }

            



            //Equipment
            String[] equipment = new String[3] { player.GetEquippedArmor().ObjectName.Value.Trim().ToUpper(), player.GetEquippedMeleeWeapon().ObjectName.Value.Trim().ToUpper(), player.GetEquippedRangedWeapon().ObjectName.Value.Trim().ToUpper() };


            for (int i = 0; i < equipment.Length; i++)
            {
                Regex reg = new Regex("/[^a-zA-Z\\-_0-9]/");
                equipment[i] = reg.Replace(equipment[i], "");
            }

            if (equipment[0] != null && equipment[0].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.EquipmentSent_1;
            if (equipment[1] != null && equipment[1].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.EquipmentSent_2;
            if (equipment[2] != null && equipment[2].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.EquipmentSent_3;

            //Slots
            String[] slots = new String[9];
            for (int i = 0; i < slots.Length; i++)
                slots[i] = "";

            if(player.GetSlotItem(oCNpc.SLOT_RIGHTHAND(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_RIGHTHAND(process)).ObjectName.Address != 0)
                slots[0] = player.GetSlotItem(oCNpc.SLOT_RIGHTHAND(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_LEFTHAND(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_LEFTHAND(process)).ObjectName.Address != 0)
                slots[1] = player.GetSlotItem(oCNpc.SLOT_LEFTHAND(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_SWORD(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_SWORD(process)).ObjectName.Address != 0)
                slots[2] = player.GetSlotItem(oCNpc.SLOT_SWORD(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_LONGSWORD(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_LONGSWORD(process)).ObjectName.Address != 0)
                slots[3] = player.GetSlotItem(oCNpc.SLOT_LONGSWORD(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_BOW(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_BOW(process)).ObjectName.Address != 0)
                slots[4] = player.GetSlotItem(oCNpc.SLOT_BOW(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_CROSSBOW(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_CROSSBOW(process)).ObjectName.Address != 0)
                slots[5] = player.GetSlotItem(oCNpc.SLOT_CROSSBOW(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_TORSO(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_TORSO(process)).ObjectName.Address != 0)
                slots[6] = player.GetSlotItem(oCNpc.SLOT_TORSO(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_HELMET(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_HELMET(process)).ObjectName.Address != 0)
                slots[7] = player.GetSlotItem(oCNpc.SLOT_HELMET(process)).ObjectName.Value.Trim().ToUpper();
            if (player.GetSlotItem(oCNpc.SLOT_SHIELD(process)).Address != 0 && player.GetSlotItem(oCNpc.SLOT_SHIELD(process)).ObjectName.Address != 0)
                slots[8] = player.GetSlotItem(oCNpc.SLOT_SHIELD(process)).ObjectName.Value.Trim().ToUpper();

            for (int i = 0; i < slots.Length; i++)
            {
                Regex reg = new Regex("/[^a-zA-Z\\-_0-9]/");
                slots[i] = reg.Replace(slots[i], "");
            }

            //if (slots[0] != null && slots[0].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_1;
            //if (slots[1] != null && slots[1].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_2;
            //if (slots[2] != null && slots[2].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_3;
            //if (slots[3] != null && slots[3].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_4;
            //if (slots[4] != null && slots[4].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_5;
            //if (slots[5] != null && slots[5].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_6;
            //if (slots[6] != null && slots[6].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_7;
            //if (slots[7] != null && slots[7].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_8;
            //if (slots[8] != null && slots[8].Trim().Length != 0)
                psmSentTypes |= PSM_SentTypes.Slot_9;
            

            //Magie
            oCMag_Book magBook = player.MagBook;

            int spellid = player.GetActiveSpellNr();
            int key = magBook.GetKeyByID(spellid);
            String activeSpell = "";
            if (key < magBook.SpellItems.Size && key != -1)
            {
                activeSpell = magBook.SpellItems.get(key).ObjectName.Value;
                if (activeSpell != null && activeSpell.Trim().Length != 0)
                {
                    psmSentTypes |= PSM_SentTypes.Magic;
                }
                
            }

            psmSentTypes = psmSentTypes | PSM_SentTypes.Scale_FatNess;

            //Anfang schreiben
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.PlayerStatusMessage2);
            stream.Write(pl.id);
            stream.Write((int)psmSentTypes);


            
            for (int i = 0; i < equipment.Length; i++)
                if (equipment[i] == null)
                    equipment[i] = "";


            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_1) == PSM_SentTypes.EquipmentSent_1)
                stream.Write(equipment[0]);
            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_2) == PSM_SentTypes.EquipmentSent_2)
                stream.Write(equipment[1]);
            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_3) == PSM_SentTypes.EquipmentSent_3)
                stream.Write(equipment[2]);

            //SlotItems
            if ((psmSentTypes & PSM_SentTypes.Slot_1) == PSM_SentTypes.Slot_1)
                stream.Write(slots[0]);
            if ((psmSentTypes & PSM_SentTypes.Slot_2) == PSM_SentTypes.Slot_2)
                stream.Write(slots[1]);
            if ((psmSentTypes & PSM_SentTypes.Slot_3) == PSM_SentTypes.Slot_3)
                stream.Write(slots[2]);
            if ((psmSentTypes & PSM_SentTypes.Slot_4) == PSM_SentTypes.Slot_4)
                stream.Write(slots[3]);
            if ((psmSentTypes & PSM_SentTypes.Slot_5) == PSM_SentTypes.Slot_5)
                stream.Write(slots[4]);
            if ((psmSentTypes & PSM_SentTypes.Slot_6) == PSM_SentTypes.Slot_6)
                stream.Write((slots[5]));
            if ((psmSentTypes & PSM_SentTypes.Slot_7) == PSM_SentTypes.Slot_7)
                stream.Write((slots[6]));
            if ((psmSentTypes & PSM_SentTypes.Slot_8) == PSM_SentTypes.Slot_8)
                stream.Write((slots[7]));
            if ((psmSentTypes & PSM_SentTypes.Slot_9) == PSM_SentTypes.Slot_9)
                stream.Write((slots[8]));

            //Magie
            if ((psmSentTypes & PSM_SentTypes.Magic) == PSM_SentTypes.Magic)
                stream.Write(activeSpell);
            //oCMag_Book magBook = player.MagBook;

            //int spellid = player.GetActiveSpellNr();
            //int key = magBook.GetKeyByID(spellid);
            //if (key < magBook.SpellItems.Size && key != -1)
            //    stream.Write(magBook.SpellItems.get(key).ObjectName.Value);
            //else
            //    stream.Write("");

            //stream.Write(magBook.SpellItems.Size);
            //for (int i = 0; i < magBook.SpellItems.Size; i++)
            //    stream.Write(magBook.SpellItems.get(i).ObjectName.Value.Trim().ToLower());

            //stream.Write(pl.actualMap);
            //stream.Write(player.ObjectName.Value);


            //Aussehen
            if ((psmSentTypes & PSM_SentTypes.appearanceSent) == PSM_SentTypes.appearanceSent)
            {
                stream.Write((int)player.BodyTex);
                stream.Write(player.BodyVisualString.Value);
                stream.Write((int)player.HeadTex);
                stream.Write(player.HeadVisualString.Value);
                stream.Write(player.Voice);

                stream.Write(player.ActiveOverlays.Size);
                for (int i = 0; i < player.ActiveOverlays.Size; i++)
                    stream.Write(player.ActiveOverlays.get(i, 20).Value.Trim().ToUpper());
            }

            
            if ((psmSentTypes & PSM_SentTypes.Scale_FatNess) == PSM_SentTypes.Scale_FatNess)
            {
                stream.Write(player.Fatness);
                stream.Write(player.Scale.X);
                stream.Write(player.Scale.Y);
                stream.Write(player.Scale.Z);
            }


            client.client.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.UNRELIABLE_SEQUENCED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        public override void Read(BitStream stream, Packet packet, Client client)
        {
            int id = 0;
            String[] equipment = new String[3];
            
            int psmSentTypes_Int;
            PSM_SentTypes psmSentTypes;


            stream.Read(out id);

            stream.Read(out psmSentTypes_Int);
            psmSentTypes = (PSM_SentTypes)psmSentTypes_Int;

            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_1) == PSM_SentTypes.EquipmentSent_1)
                stream.Read(out equipment[0]);
            else
                equipment[0] = "";

            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_2) == PSM_SentTypes.EquipmentSent_2)
                stream.Read(out equipment[1]);
            else
                equipment[1] = "";

            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_3) == PSM_SentTypes.EquipmentSent_3)
                stream.Read(out equipment[2]);
            else
                equipment[2] = "";
            

            //Slotitems
            String[] slotItems = new String[9];
            for (int i = 0; i < 9; i++)
                slotItems[i] = "";

            if ((psmSentTypes & PSM_SentTypes.Slot_1) == PSM_SentTypes.Slot_1)
                stream.Read(out slotItems[0]);
            if ((psmSentTypes & PSM_SentTypes.Slot_2) == PSM_SentTypes.Slot_2)
                stream.Read(out slotItems[1]);
            if ((psmSentTypes & PSM_SentTypes.Slot_3) == PSM_SentTypes.Slot_3)
                stream.Read(out slotItems[2]);
            if ((psmSentTypes & PSM_SentTypes.Slot_4) == PSM_SentTypes.Slot_4)
                stream.Read(out slotItems[3]);
            if ((psmSentTypes & PSM_SentTypes.Slot_5) == PSM_SentTypes.Slot_5)
                stream.Read(out slotItems[4]);
            if ((psmSentTypes & PSM_SentTypes.Slot_6) == PSM_SentTypes.Slot_6)
                stream.Read(out slotItems[5]);
            if ((psmSentTypes & PSM_SentTypes.Slot_7) == PSM_SentTypes.Slot_7)
                stream.Read(out slotItems[6]);
            if ((psmSentTypes & PSM_SentTypes.Slot_8) == PSM_SentTypes.Slot_8)
                stream.Read(out slotItems[7]);
            if ((psmSentTypes & PSM_SentTypes.Slot_9) == PSM_SentTypes.Slot_9)
                stream.Read(out slotItems[8]);



            String activeSpell = "";
            if ((psmSentTypes & PSM_SentTypes.Magic) == PSM_SentTypes.Magic)
                stream.Read(out activeSpell);
            //int spellCount = 0;
            //stream.Read(out spellCount);
            //String[] spells = new String[spellCount];
            //for (int i = 0; i < spellCount; i++)
            //    stream.Read(out spells[i]);



            int bodyTex = 0, headTex = 0;
            String BodyVisual = "", HeadVisual = "";
            int overlaySize = 0;
            int voice = 0;
            String[] overlayAnim = null;

            if ((psmSentTypes & PSM_SentTypes.appearanceSent) == PSM_SentTypes.appearanceSent)
            {
                stream.Read(out bodyTex);
                stream.Read(out BodyVisual);
                stream.Read(out headTex);
                stream.Read(out HeadVisual);
                stream.Read(out voice);

                stream.Read(out overlaySize);
                overlayAnim = new String[overlaySize];
                for (int i = 0; i < overlaySize; i++)
                    stream.Read(out overlayAnim[i]);
            }

            float fatness = 0;
            float[] scale = new float[3];
            if ((psmSentTypes & PSM_SentTypes.Scale_FatNess) == PSM_SentTypes.Scale_FatNess)
            {
                stream.Read(out fatness);
                stream.Read(out scale[0]);
                stream.Read(out scale[1]);
                stream.Read(out scale[2]);
            }




            if (Program.Player == null || id == Program.Player.id)
                return;

            if (!StaticVars.AllPlayerDict.ContainsKey(id))
                return;

            Player pl = StaticVars.AllPlayerDict[id];
            if (pl == null)
                return;

            //pl.actualMap = world.Trim().ToUpper();
            //pl.instance = instance;

            if (!pl.isSpawned || pl.NPCAddress == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc player = new oCNpc(process, pl.NPCAddress);


            PlayerStatusMessage.EquipArmor(player, equipment[0]);
            PlayerStatusMessage.EquipWeapon(player, equipment[1]);
            PlayerStatusMessage.EquipRangeWeapon(player, equipment[2]);

            //SlotItems
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_RIGHTHAND(process), slotItems[0]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_LEFTHAND(process), slotItems[1]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_SWORD(process), slotItems[2]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_LONGSWORD(process), slotItems[3]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_BOW(process), slotItems[4]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_CROSSBOW(process), slotItems[5]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_TORSO(process), slotItems[6]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_HELMET(process), slotItems[7]);
            PlayerStatusMessage.PutInSlot(process, player, oCNpc.SLOT_SHIELD(process), slotItems[8]);

            

            try
            {
                if ((psmSentTypes & PSM_SentTypes.Magic) == PSM_SentTypes.Magic && activeSpell != null && activeSpell.Trim().ToLower() !=
                    player.MagBook.SpellItems.get(player.MagBook.GetKeyByID(player.GetActiveSpellNr())).ObjectName.Value.Trim().ToLower())
                {
                    if (PlayerStatusMessage.GetItemMag(player, activeSpell) == null)
                        PlayerStatusMessage.Equip(player, activeSpell);
                    

                    PlayerStatusMessage.SetActiveSpell(player, activeSpell);
                    player.MagBook.SetShowHandSymbol(1);
                }

            }
            catch (Exception ex)
            {
                zERROR.GetZErr(process).Report(2, 'G', ex.StackTrace, 0, "PlayerStatusMessage", 0);
            }


            if ((psmSentTypes & PSM_SentTypes.appearanceSent) == PSM_SentTypes.appearanceSent)
            {


                List<zString> strList = new List<zString>();
                for (int i = 0; i < player.ActiveOverlays.Size; i++)
                    strList.Add(player.ActiveOverlays.get(i, 20));

                List<String> overlayTempList = new List<string>();
                for (int i = 0; i < overlayAnim.Length; i++)
                    overlayTempList.Add(overlayAnim[i]);

                foreach (String strin in overlayAnim)
                {
                    foreach (zString str in strList)
                    {
                        if (str.Value == strin)
                        {
                            strList.Remove(str);
                            overlayTempList.Remove(strin);
                            break;
                        }
                    }
                }

                foreach (zString str in strList)
                {
                    player.RemoveOverlay(str);
                }

                foreach (String str in overlayTempList)
                {
                    zString zstr = zString.Create(process, str);
                    player.ApplyOverlay(zstr);
                    //zstr.Dispose();
                }




                bool headChanged = false;
                if (player.HeadVisualString.Value != HeadVisual && HeadVisual.Trim().Length != 0)
                {
                    player.HeadVisualString.Set(HeadVisual);
                    headChanged = true;
                }

                if (player.HeadTex != headTex)
                {
                    player.HeadTex = (ushort)headTex;
                    headChanged = true;
                }

                if (headChanged)
                    player.SetHead();


                bool bodyChanged = false;
                if (player.BodyVisualString.Value != BodyVisual && BodyVisual.Trim().Length != 0)
                {
                    player.BodyVisualString.Set(BodyVisual);
                    bodyChanged = true;
                }

                if (player.BodyTex != bodyTex)
                {
                    player.BodyTex = (ushort)bodyTex;
                    bodyChanged = true;
                }

                if (bodyChanged)
                    player.InitModel();


                player.Voice = voice;

            }

            if ((psmSentTypes & PSM_SentTypes.Scale_FatNess) == PSM_SentTypes.Scale_FatNess)
            {
                player.Fatness = fatness;
                player.SetFatness(fatness);

                zVec3 s = zVec3.Create(process);
                s.X = scale[0];
                s.Y = scale[1];
                s.Z = scale[2];
                player.SetModelScale(s);
                s.Dispose();
            }

            //player.Enable(player.GetPosition());
        }
    }
}
