using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.zClasses;
using WinApi;

using GUC.Network;
using GUC.Types;
using Gothic.zTypes;
using GUC.Hooks;
using GUC.Enumeration;

namespace GUC.WorldObjects.Character
{
    internal partial class NPCProto
    {
        public List<String> AnimationList = new List<string>();



        partial void addItemToContainer(Item item)
        {
            if (this.Address == 0)
                return;
            Process process = Process.ThisProcess();
            oCItem it = oCObjectFactory.GetFactory(process).CreateItem("ITGUC_" + item.ItemInstance.ID);
            it.Amount = item.Amount;
            oCNpc npc = new oCNpc(Process.ThisProcess(), Address);
            npc.PutInInv(it);

            item.Address = it.Address;
            sWorld.SpawnedVobDict.Add(item.Address, item);
            
        }

        protected oCNpc gNPC { get { return new oCNpc(Process.ThisProcess(), Address); } }


        public override VobSendFlags Read(RakNet.BitStream stream)
        {
            VobSendFlags sendInfo =  base.Read(stream);

            stream.Read(out name);
            stream.Read(out bodyMesh);
            stream.Read(out bodyTex);
            stream.Read(out skinColor);
            stream.Read(out headMesh);
            stream.Read(out headTex);
            stream.Read(out teethTex);

            stream.Read(out this.attributes, this.Attributes.Length);
            stream.Read(out this.talentSkills, this.TalentSkills.Length);
            stream.Read(out this.talentValues, this.TalentValues.Length);
            stream.Read(out this.hitchances, this.Hitchances.Length);

            int itemCount = 0;
            stream.Read(out itemCount);
            for (int i = 0; i < itemCount; i++)
            {
                int itemID = 0;
                stream.Read(out itemID);

                Item it = (Item)sWorld.VobDict[itemID];
                addItem(it);
            }

            itemCount = 0;
            stream.Read(out itemCount);
            for (int i = 0; i < itemCount; i++)
            {
                int itemID = 0;
                stream.Read(out itemID);

                Item it = (Item)sWorld.VobDict[itemID];
                EquippedList.Add(it);
            }

            int armorID = 0, weaponID = 0, rangeWeaponID = 0;
            stream.Read(out armorID);
            stream.Read(out weaponID);
            stream.Read(out rangeWeaponID);
            if(armorID > 0)
                Armor = (Item)sWorld.VobDict[armorID];
            if (weaponID > 0)
                Weapon = (Item)sWorld.VobDict[weaponID];
            if (rangeWeaponID > 0)
                RangeWeapon = (Item)sWorld.VobDict[rangeWeaponID];

            int overlayCount = 0;
            stream.Read(out overlayCount);
            for (int i = 0; i < overlayCount; i++)
            {
                String overlay = "";
                stream.Read(out overlay);
                Overlays.Add(overlay);
            }

            stream.Read(out this.IsInvisible);
            stream.Read(out this.hideName);

            return sendInfo;
        }

        public override void Despawn()
        {
            if (!IsSpawned || this.Address == 0)
                return;

            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, this.Address);
            oCGame.Game(process).GetSpawnManager().DeleteNPC(npc);
            this.Address = 0;

            sWorld.SpawnedVobDict.Remove(npc.Address);
            
            foreach(Item it in itemList){
                sWorld.SpawnedVobDict.Remove(it.Address);
                it.Address = 0;
            }
        }

        public override void Spawn(String map, Vec3f position, Vec3f direction)
        {
            this.Map = map;
            this.Position = position;
            this.Direction = direction;

            spawned = true;

            if (this.Address != 0)
                return;

            if (this.Map != Player.Hero.Map)
                return;
            Process process = Process.ThisProcess();
            oCNpc npc = oCObjectFactory.GetFactory(process).CreateNPC("OTHERS_NPC");

            if(hideName)
                npc.Name.Set("");
            else
                npc.Name.Set(Name);

            for (int i = 0; i < this.Attributes.Length; i++)
                npc.setAttributes((byte)i, this.Attributes[i]);

            for (int i = 0; i < this.TalentSkills.Length; i++)
                npc.SetTalentSkill(i, this.TalentSkills[i]);
            for (int i = 0; i < this.TalentValues.Length; i++)
                npc.SetTalentValue(i, this.TalentValues[i]);
            for (int i = 0; i < this.Hitchances.Length; i++)
                npc.SetHitChances(i, this.Hitchances[i]);

            npc.SetVisual(this.Visual);
            npc.SetAdditionalVisuals(BodyMesh, bodyTex, skinColor, HeadMesh, headTex, teethTex, -1);
            npc.SetWeaponMode(this.WeaponMode);
            npc.SetWeaponMode2(this.WeaponMode);

            npc.HumanAI = new oCAiHuman(process, 0);
            npc.AniCtrl = new oCAniCtrl_Human(process, 0);






            this.setDirection(direction);
            npc.Enable(Position.X, Position.Y, Position.Z);
            

            this.Address = npc.Address;
            sWorld.SpawnedVobDict.Add(npc.Address, this);

            foreach (Item it in itemList)
                this.addItemToContainer(it);
            foreach (Item it in EquippedList)
                if( !it.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR) && !it.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF) && !it.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF))
                    npc.Equip(new oCItem(process, it.Address));

            if (this.Armor != null)
                setArmor(this.Armor);
            if (this.Weapon != null)
                setWeapon(this.Weapon);
            if (this.RangeWeapon != null)
                setRangeWeapon(this.RangeWeapon);

            for (int i = 0; i < Slots.Length; i++)
            {
                if (Slots[i] != null)
                {
                    setSlotItem(i, Slots[i]);
                }
            }

            foreach (String act in Overlays)
            {
                zString overlayStr = zString.Create(process, act);
                npc.ApplyOverlay(overlayStr);
                overlayStr.Dispose();
            }

            npc.setShowVisual(!this.IsInvisible);

        }


        public void setArmor(Item armor)
        {
            Armor = armor;

            if (this.Address != 0)
            {
                
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);
                if (Armor == null)
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "UnequipArmor: ", 0, "Client.cs", 0);
                    npc.UnequipItem(npc.GetEquippedArmor());
                }
                else
                {
                    if (Armor.Address == 0)
                        throw new Exception("Armor Adress can't be null if player using it is spawned!");
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "EquipArmor: ", 0, "Client.cs", 0);
                    npc.EquipArmor(new oCItem(process, Armor.Address));
                }
            }

            
        }


        public void setWeapon(Item weapon)
        {
            Weapon = weapon;

            if (this.Address != 0)
            {
                
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);
                if (Weapon == null)
                {
                    hNpc.blockSendUnEquip = true;
                    npc.UnequipItem(npc.GetEquippedMeleeWeapon());
                }
                else
                {
                    if (Weapon.Address == 0)
                        throw new Exception("Weapon Adress can't be null if player using it is spawned!");
                    hNpc.blockSendEquip = true;
                    npc.EquipWeapon(new oCItem(process, Weapon.Address));
                }
            }
        }

        public void setRangeWeapon(Item rangeWeapon)
        {
            RangeWeapon = rangeWeapon;

            if (this.Address != 0)
            {

                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);
                if (RangeWeapon == null)
                {
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Unequip RangeWeapon: ", 0, "Client.cs", 0);
                    npc.UnequipItem(npc.GetEquippedRangedWeapon());
                }
                else
                {
                    if (RangeWeapon.Address == 0)
                        throw new Exception("RangeWeapon Adress can't be null if player using it is spawned!");
                    zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Equip RangeWeapon: ", 0, "Client.cs", 0);
                    npc.EquipFarWeapon(new oCItem(process, RangeWeapon.Address));
                }
            }
        }

        public void setWeaponMode(int weaponMode)
        {
            WeaponMode = weaponMode;

            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);
                npc.SetWeaponMode(weaponMode);
            }
        }

        public void setSlotItem(int slot, Item item)
        {
            Slots[slot] = item;

            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);
                if (Slots[slot] == null)
                {
                    zString slotString = oCNpc.getSlotString(process, slot);
                    oCItem oldITem = npc.GetSlotItem(slotString);
                    if(oldITem.Address != 0)
                        npc.RemoveFromSlot(slotString, oldITem.Instanz, oldITem.Amount);
                }
                else
                {
                    if (Slots[slot].Address == 0)
                        throw new Exception("RangeWeapon Adress can't be null if player using it is spawned!");
                    npc.PutInSlot(oCNpc.getSlotString(process, slot), new oCItem(process, Slots[slot].Address), 1);
                }
            }
        }
    }
}
