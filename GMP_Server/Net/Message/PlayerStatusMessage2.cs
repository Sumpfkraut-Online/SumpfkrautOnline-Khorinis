using System;
using System.Collections.Generic;
using System.Text;
using Network;
using RakNet;

namespace GMP_Server.Net.Message
{
    public class PlayerStatusMessage2 : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            RakNet.BitStream stream_2 = new BitStream();
            stream_2.Reset();
            stream_2.Write(packet.data, packet.length);
            stream_2.IgnoreBytes(2);
            


            int player_id = 0;
            int psmSentTypesInt = 0;
            stream_2.Read(out player_id);
            stream_2.Read(out psmSentTypesInt);

            Player pl = Program.playerDict[player_id];
            PSM_SentTypes psmSentTypes = (PSM_SentTypes)psmSentTypesInt;

            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_1) == PSM_SentTypes.EquipmentSent_1)
            {
                String oldEquipped = pl.equippedArmor;
                stream_2.Read(out pl.equippedArmor);
                Program.scriptManager.OnArmorChanged(new Scripting.Player(pl), oldEquipped, pl.equippedArmor);
            }
            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_2) == PSM_SentTypes.EquipmentSent_2)
            {
                String oldEquipped = pl.equippedWeapon;
                stream_2.Read(out pl.equippedWeapon);
                Program.scriptManager.OnWeaponChanged(new Scripting.Player(pl), oldEquipped, pl.equippedWeapon);
            }
            if ((psmSentTypes & PSM_SentTypes.EquipmentSent_3) == PSM_SentTypes.EquipmentSent_3)
            {
                String oldEquipped = pl.equippedRangeWeapon;
                stream_2.Read(out pl.equippedRangeWeapon);
                Program.scriptManager.OnRangeWeaponChanged(new Scripting.Player(pl), oldEquipped, pl.equippedRangeWeapon);
            }



            if ((psmSentTypes & PSM_SentTypes.Slot_1) == PSM_SentTypes.Slot_1)
            {
                String oldSlot = pl.slots[0];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[0] = newSlot;
                Program.scriptManager.OnRightHandSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[0]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_2) == PSM_SentTypes.Slot_2)
            {
                String oldSlot = pl.slots[1];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[1] = newSlot;
                Program.scriptManager.OnLeftHandSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[1]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_3) == PSM_SentTypes.Slot_3)
            {
                String oldSlot = pl.slots[2];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[2] = newSlot;
                Program.scriptManager.OnSwordSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[2]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_4) == PSM_SentTypes.Slot_4)
            {
                String oldSlot = pl.slots[3];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[3] = newSlot;
                Program.scriptManager.OnLongSwordSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[3]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_5) == PSM_SentTypes.Slot_5)
            {
                String oldSlot = pl.slots[4];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[4] = newSlot;
                Program.scriptManager.OnBowSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[4]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_6) == PSM_SentTypes.Slot_6)
            {
                String oldSlot = pl.slots[5];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[5] = newSlot;
                Program.scriptManager.OnCrossBowSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[5]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_7) == PSM_SentTypes.Slot_7)
            {
                String oldSlot = pl.slots[6];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[6] = newSlot;
                Program.scriptManager.OnTorsoSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[6]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_8) == PSM_SentTypes.Slot_8)
            {
                String oldSlot = pl.slots[7];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[7] = newSlot;
                Program.scriptManager.OnHelmetSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[7]);
            }
            if ((psmSentTypes & PSM_SentTypes.Slot_9) == PSM_SentTypes.Slot_9)
            {
                String oldSlot = pl.slots[8];
                String newSlot = "";
                stream_2.Read(out newSlot);
                pl.slots[8] = newSlot;
                Program.scriptManager.OnShieldSlotChanged(new Scripting.Player(pl), oldSlot, pl.slots[8]);
            }
            if ((psmSentTypes & PSM_SentTypes.Magic) == PSM_SentTypes.Magic)
                stream_2.Read(out pl.activeSpell);

            //int spellCount = 0;
            //stream.Read(out spellCount);
            //pl.spells = new String[spellCount];
            //for (int i = 0; i < spellCount; i++)
            //{
            //    pl.spells[i] = "";
            //    stream.Read(out pl.spells[i]);
            //}

            //stream.Read(out pl.actualMap);
            //pl.actualMap = Player.getMap(pl.actualMap);
            //stream.Read(out pl.instance);
            if ((psmSentTypes & PSM_SentTypes.appearanceSent) == PSM_SentTypes.appearanceSent)
            {
                stream_2.Read(out pl.bodyTex);
                stream_2.Read(out pl.BodyVisual);
                stream_2.Read(out pl.headTex);
                stream_2.Read(out pl.HeadVisual);
                stream_2.Read(out pl.voice);
            }

            if ((psmSentTypes & PSM_SentTypes.Scale_FatNess) == PSM_SentTypes.Scale_FatNess)
            {
                stream_2.Read(out pl.fatness);
                stream_2.Read(out pl.scale[0]);
                stream_2.Read(out pl.scale[1]);
                stream_2.Read(out pl.scale[2]);
            }


            stream_2.Reset();
            stream_2.Dispose();

            server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.UNRELIABLE_SEQUENCED, (char)0, packet.systemAddress, true);

            //int id = 0;

            //String[] equipment = new string[3];
            //String[] slotitems = new string[9];
            ////String world = "";
            ////String instance = "";
            //int voice;
            //int psmSentTypes_Int;
            //PSM_SentTypes psmSentTypes;

            //stream.Read(out id);

            //stream.Read(out psmSentTypes_Int);
            //psmSentTypes = (PSM_SentTypes)psmSentTypes_Int;

            //if ((psmSentTypes & PSM_SentTypes.EquipmentSent_1) == PSM_SentTypes.EquipmentSent_1)
            //    stream.Read(out equipment[0]);
            //else
            //    equipment[0] = "";

            //if ((psmSentTypes & PSM_SentTypes.EquipmentSent_2) == PSM_SentTypes.EquipmentSent_2)
            //    stream.Read(out equipment[1]);
            //else
            //    equipment[1] = "";

            //if ((psmSentTypes & PSM_SentTypes.EquipmentSent_3) == PSM_SentTypes.EquipmentSent_3)
            //    stream.Read(out equipment[2]);
            //else
            //    equipment[2] = "";


            //String[] slotItems = new String[9];
            //for (int i = 0; i < 9; i++)
            //    slotItems[i] = "";

            //if ((psmSentTypes & PSM_SentTypes.Slot_1) == PSM_SentTypes.Slot_1)
            //    stream.Read(out slotItems[0]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_2) == PSM_SentTypes.Slot_2)
            //    stream.Read(out slotItems[1]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_3) == PSM_SentTypes.Slot_3)
            //    stream.Read(out slotItems[2]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_4) == PSM_SentTypes.Slot_4)
            //    stream.Read(out slotItems[3]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_5) == PSM_SentTypes.Slot_5)
            //    stream.Read(out slotItems[4]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_6) == PSM_SentTypes.Slot_6)
            //    stream.Read(out slotItems[5]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_7) == PSM_SentTypes.Slot_7)
            //    stream.Read(out slotItems[6]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_8) == PSM_SentTypes.Slot_8)
            //    stream.Read(out slotItems[7]);
            //if ((psmSentTypes & PSM_SentTypes.Slot_9) == PSM_SentTypes.Slot_9)
            //    stream.Read(out slotItems[8]);



            //String activeSpell = "";
            //if ((psmSentTypes & PSM_SentTypes.Magic) == PSM_SentTypes.Magic)
            //    stream.Read(out activeSpell);
            ////int spellCount = 0;
            ////stream.Read(out spellCount);
            ////String[] spells = new String[spellCount];
            ////for (int i = 0; i < spellCount; i++)
            ////    stream.Read(out spells[i]);

            ////stream.Read(out world);
            ////stream.Read(out instance);

            //int bodyTex, headTex;
            //String BodyVisual, HeadVisual;
            //int overlaySize = 0;

            //stream.Read(out bodyTex);
            //stream.Read(out BodyVisual);
            //stream.Read(out headTex);
            //stream.Read(out HeadVisual);
            //stream.Read(out voice);

            //stream.Read(out overlaySize);
            //String[] overlayAnim = new String[overlaySize];
            //for (int i = 0; i < overlaySize; i++)
            //    stream.Read(out overlayAnim[i]);

            //Player pl = Player.getPlayerSort(id, Program.playerList);

            ////pl.actualMap = world.Trim().ToUpper();
            ////pl.instance = instance;

            //stream.Reset();
            //stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            //stream.Write((byte)NetWorkIDS.PlayerStatusMessage2);
            //stream.Write(id);

            //stream.Write(equipment[0]);
            //stream.Write(equipment[1]);
            //stream.Write(equipment[2]);

            //for (int i = 0; i < 9; i++)
            //    stream.Write(slotitems[i]);

            //stream.Write(activeSpell);
            //stream.Write(spellCount);
            //for (int i = 0; i < spellCount; i++)
            //    stream.Write(spells[i]);

            ////stream.Write(world);
            ////stream.Write(instance);

            //stream.Write(bodyTex);
            //stream.Write(BodyVisual);
            //stream.Write(headTex);
            //stream.Write(HeadVisual);
            //stream.Write(voice);

            //stream.Write(overlaySize);
            //for (int i = 0; i < overlaySize; i++)
            //    stream.Write(overlayAnim[i]);

            
        }
    }
}
