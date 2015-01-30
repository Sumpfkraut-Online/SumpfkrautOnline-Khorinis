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
using Gothic.zStruct;
using GUC.timer;

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


            Vec3f vec = new Vec3f();
            stream.Read(out vec);
            Scale = vec;

            stream.Read(out Fatness);


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

            int activeSpellIDid = 0;
            stream.Read(out activeSpellIDid);
            if (activeSpellIDid > 0)
            {
                this.ActiveSpell = (Item)sWorld.VobDict[activeSpellIDid];
            }


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

            stream.Read(out this.weaponMode);

            return sendInfo;
        }

        public override void Despawn()
        {
            if (!IsSpawned || this.Address == 0)
                return;

            Process process = Process.ThisProcess();

            oCNpc npc = new oCNpc(process, this.Address);
            using (zVec3 vec = npc.GetPosition())
            {
                Position = (Vec3f)(new float[]{vec.X, vec.Y, vec.Z});
            }
            npc.HP = 0;
            npc.SetPosition(-1000, -100000, -100000);

            

            int WeaponMode = weaponMode;
            setWeaponMode(0);
            weaponMode = WeaponMode;

            oCGame.Game(process).World.DisableVob(npc);
            oCGame.Game(process).GetSpawnManager().DeleteNPC(npc);

            //oCGame.Game(process).GetSpawnManager().DeleteNPC(npc);//Bei verwandlung wird bedmempointer aufgerufen :/
            //new DespawnTimer(this.Address);
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
            
            this.Address = npc.Address;
            sWorld.SpawnedVobDict.Add(npc.Address, this);

            if (npc.MagBook == null || npc.MagBook.Address == 0)
            {
                npc.MagBook = oCMag_Book.Create(process);
                npc.MagBook.SetOwner(npc);
            }


            setHideNames(this.hideName);

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
            


            //npc.HumanAI = new oCAiHuman(process, 0);
            //npc.AniCtrl = new oCAniCtrl_Human(process, 0);




            

            this.setDirection(direction);
            //Enable(Position);

            

            foreach (Item it in itemList)
                this.addItemToContainer(it);
            foreach (Item it in EquippedList)
            {
                if (!it.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_ARMOR) && !it.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_FF) && !it.ItemInstance.MainFlags.HasFlag(MainFlags.ITEM_KAT_NF))
                {
                    hNpc.blockSendEquip = true;
                    npc.Equip(new oCItem(process, it.Address));
                }
            }
            //if (this.Armor != null)
            //    setArmor(this.Armor);
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

            

            setScale(this.Scale);
            setFatness(this.Fatness);
            setWeaponMode(this.WeaponMode);

            if (enabled)
                Enable(this.Position);

            this.setInvisible(this.IsInvisible);
        }

        public bool enabled = false;

        public void Disable()
        {
            enabled = false;
            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);

                hNpc.blockSendUnEquip = true;
                if (this.Armor != null)
                    npc.UnequipItem(new oCItem(process, this.Armor.Address));
                npc.Disable();

                
            }
        }

        public void Enable(Vec3f pos)
        {
            enabled = true;
            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);

                
                npc.Enable(pos.X, pos.Y, pos.Z);
                setWeaponMode(weaponMode);
                setArmor(armor);

                for (int i = 0; i < Slots.Length; i++)
                {
                    if (Slots[i] != null)
                    {
                        setSlotItem(i, Slots[i]);
                    }
                }
            }
        }

        public void setHideNames(bool hidenames)
        {
            this.hideName = hidenames;

            if(this.Address != 0){
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);
                if (this.hideName)
                    npc.Name.Set("");
                else
                    npc.Name.Set(this.Name);
            }

        }

        public void setInvisible(bool invisible)
        {
            this.IsInvisible = invisible;

            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);
                npc.setShowVisual(!invisible);
            }
        }


        public void RemoveWeapon()
        {
            if (this.Address == 0)
                return;

            Process process = Process.ThisProcess();

            zString str = zString.Create(process, "MOD_RemoveWeapon");
            int id = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            zCParser.getParser(process).SetInstance(zString.Create(process, "SELF"), this.Address);
            

            zCParser.CallFunc(process, new CallValue[] {
                    new IntArg(zCParser.getParser(process).Address),
                    new IntArg(id)
                });
        }

        public void Output(String output)
        {
            if (this.Address == 0)
                return;

            Process process = Process.ThisProcess();

            zString str = zString.Create(process, "guc_string_helper");
            zCPar_Symbol sym = zCParser.getParser(process).GetSymbol(str);
            str.Dispose();

            str = zString.Create(process, output);
            sym.SetValue(str, 0);
            str.Dispose();


            str = zString.Create(process, "MOD_Output");
            int id = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            zCParser.getParser(process).SetInstance(zString.Create(process, "SELF"), this.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "OTHER"), this.Address);

            zCParser.CallFunc(process, new CallValue[] {
                    new IntArg(zCParser.getParser(process).Address),
                    new IntArg(id)
                });
        }

        public void OutputSVM_Overlay(String output)
        {
            if (this.Address == 0)
                return;

            Process process = Process.ThisProcess();

            zString str = zString.Create(process, "guc_string_helper");
            zCPar_Symbol sym = zCParser.getParser(process).GetSymbol(str);
            str.Dispose();

            str = zString.Create(process, output);
            sym.SetValue(str, 0);
            str.Dispose();


            str = zString.Create(process, "MOD_OutputSVM_Overlay");
            int id = zCParser.getParser(process).GetIndex(str);
            str.Dispose();

            zCParser.getParser(process).SetInstance(zString.Create(process, "SELF"), this.Address);
            zCParser.getParser(process).SetInstance(zString.Create(process, "OTHER"), this.Address);

            zCParser.CallFunc(process, new CallValue[] {
                    new IntArg(zCParser.getParser(process).Address),
                    new IntArg(id)
                });
        }

        public void setWeaponMode(int wpMode)
        {
            int oldWeaponMode = this.weaponMode;

            this.weaponMode = wpMode;

            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);

                npc.SetWeaponMode(this.weaponMode);
                npc.SetWeaponMode2(this.weaponMode);

                if (this.weaponMode == 7 && oldWeaponMode != 7)
                {
                    int spellID = npc.MagBook.GetKeyByItem(new oCItem(process, ActiveSpell.Address));
                    npc.MagBook.SpellNr = spellID - 1;
                    npc.MagBook.Open(0);
                }
                else if (oldWeaponMode == 7 && this.weaponMode != 7)
                {
                    npc.MagBook.Close(1);
                }
                
                
            }
        }

        public void setActiveSpell(Item spell)
        {
            if (ActiveSpell == spell)
                return;

            Item oldSpell = ActiveSpell;
            ActiveSpell = spell;

            if(this.Address == 0)
                return;

            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, this.Address);
            if (this.weaponMode == 7)
            {
                npc.MagBook.Close(1);
                int spellID = npc.MagBook.GetKeyByItem(new oCItem(process, ActiveSpell.Address));
                npc.MagBook.SpellNr = spellID - 1;
                npc.MagBook.Open(0);
            }
        }


        public void setScale(Vec3f scale)
        {
            this.Scale = scale;

            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);

                zVec3 v = zVec3.Create(process);
                v.X = scale.X;
                v.Y = scale.Y;
                v.Z = scale.Z;
                npc.SetModelScale(v);
                v.Dispose();
            }
        }

        public void setFatness(float fatness)
        {
            this.Fatness = fatness;

            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);

                npc.SetFatness(this.Fatness);
            }
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
                    hNpc.blockSendUnEquip = true;
                    npc.UnequipItem(npc.GetEquippedArmor());
                }
                else
                {
                    if (Armor.Address == 0)
                        throw new Exception("Armor Adress can't be null if player using it is spawned!");
                    hNpc.blockSendEquip = true;
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
                    hNpc.blockSendUnEquip = true;
                    npc.UnequipItem(npc.GetEquippedRangedWeapon());
                }
                else
                {
                    if (RangeWeapon.Address == 0)
                        throw new Exception("RangeWeapon Adress can't be null if player using it is spawned!");
                    hNpc.blockSendEquip = true;
                    npc.EquipFarWeapon(new oCItem(process, RangeWeapon.Address));
                }
            }
        }


        public void setSlotItem(int slot, Item item)
        {
            //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Set Slot Item 1: " + Slots[slot] + "; NewItem: "+item + " ", 0, "NPCProto.Client.cs", 0);
            Item oldItem = Slots[slot];
            Slots[slot] = item;

            //if (oldItem == item)
            //    return;

            if (this.Address != 0)
            {
                Process process = Process.ThisProcess();
                oCNpc npc = new oCNpc(process, this.Address);

                if (Slots[slot] == null || oldItem != null)
                {
                    zString slotString = oCNpc.getSlotString(process, slot);
                    oCItem oldITem = npc.GetSlotItem(slotString);
                    if(oldITem.Address != 0)
                        npc.RemoveFromSlot(slotString, oldITem.Instanz, oldITem.Amount);
                }

                if(Slots[slot] != null)
                {
                    if (Slots[slot].Address == 0)
                        throw new Exception("Adress can't be null if player using it is spawned!");
                    //zERROR.GetZErr(Process.ThisProcess()).Report(2, 'G', "Set Slot Item: "+Slots[slot]+" "+Slots[slot].ItemInstance.Name, 0, "NPCProto.Client.cs", 0);
                    npc.PutInSlot(oCNpc.getSlotString(process, slot), new oCItem(process, Slots[slot].Address), 1);
                }
            }
        }


        public void SetVisual(String aVisual, String aBodyMesh, String aHeadMesh, int aBodyTex, int aSkinColor, int aHeadTex, int aTeethTex)
        {
            String _visual = aVisual.ToUpper().Trim();
            bool sameVisual = _visual == this.Visual;

            this.Visual = _visual;
            this.BodyMesh = aBodyMesh;
            this.BodyTex = aBodyTex;
            this.SkinColor = aSkinColor;
            this.HeadMesh = aHeadMesh;
            this.HeadTex = aHeadTex;
            this.TeethTex = aTeethTex;

            if (this.Address == 0)
                return;
            
            Process process = Process.ThisProcess();
            oCNpc npc = new oCNpc(process, this.Address);
            
            if (sameVisual)
            {
                npc.SetAdditionalVisuals(BodyMesh, bodyTex, skinColor, HeadMesh, headTex, teethTex, -1);
            }
            else
            {
                npc.ClearOverlays();
                this.Overlays.Clear();


                npc.CloseSpellBook(true);
                npc.SetWeaponMode2(0);
                npc.AniCtrl.SearchStandAni();


                npc.Shrink();

                npc.MDSName.Set(_visual);
                npc.SetAdditionalVisuals(BodyMesh, bodyTex, skinColor, HeadMesh, headTex, teethTex, -1);


                if (this == Player.Hero)
                    npc.UnShrink();
                else
                    npc.AvoidShrink(1000);



                this.setWeaponMode(this.WeaponMode);

                //00AB1F28
                
            }
        }



    }
}
