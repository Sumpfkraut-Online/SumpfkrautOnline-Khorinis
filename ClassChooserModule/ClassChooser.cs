using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using Gothic.zClasses;
using WinApi;
using Gothic.mClasses;
using Network;
using ClassChooserServerModule;
using Gothic.zTypes;
using Injection;
using Modules;

namespace ClassChooserModule
{
    public class ClassChooser : StartState
    {
        bool started = false;
        bool started2 = false;
        Button left;
        Button right;
        Button contiueButton;

        Module module;
        public static GothicClasses classes;
        int classID;
        zCView mainview;
        zCViewText name;
        zCViewText desc;
        public override void Update(Network.Module module)
        {
            Process process = Process.ThisProcess();
            if (!started)
            {
                this.module = module;
                //zCInput_Win32.GetInput(process).SetDeviceEnabled(2, 0);

                InputHooked.deaktivateFullControl(process);

                LoadingScreen.Show(process);
                Program.client.messageListener.Add(0xff, new ClassMessage());
                new ClassMessage().Write(Program.client.sentBitStream, Program.client);
                started = true;
            }

            if (classes != null && !started2)
            {
                LoadingScreen.Hide(process);

                mainview = zCView.Create(process, 0, 0, 8000, 2000);
                zCView.GetScreen(process).InsertItem(mainview, 0);
                zString str = zString.Create(process, "");
                name = mainview.CreateText(100, 100, str);
                desc = mainview.CreateText(100, 100+name.Font.GetFontY()*30, str);
                str.Dispose();


                left = new Button(process, "");
                left.setSize(600, 600);
                left.setPos(655, 3787);
                left.setTexture("button_pfeil_l.tga");
                left.ButtonPressed += new EventHandler<EventArgs>(leftRightButton);
                right = new Button(process, "");
                right.setSize(600, 600);
                right.setPos(6902, 3787);
                right.setTexture("button_pfeil_r.tga");
                right.ButtonPressed += new EventHandler<EventArgs>(leftRightButton);
                contiueButton = new Button(process, "Weiter");
                contiueButton.setPos(5725, 7123);
                contiueButton.ButtonPressed += new EventHandler<EventArgs>(continueButton);

                left.Show();
                right.Show();
                contiueButton.Show();

                Cursor.ToTop(process);
                started2 = true;

                classID = 0;
                showClass(classID);
            }
        }

        public void leftRightButton(object obj, EventArgs args)
        {
            if (obj == right)
                classID++;
            if (obj == left)
                classID--;
            if (classID < 0)
                classID = classes.classes.Count-1;
            if (classID >= classes.classes.Count)
                classID = 0;


            showClass(classID);
        }



        public void showClass(int id)
        {
            Process process = Process.ThisProcess();
            GothicClass cls = classes.classes[id];

            name.Text.Set("Name: "+cls.name);
            desc.Text.Set("Desc: "+cls.description);

            SetClass(cls);
            gc = cls;
        }

        public List<String> BodyMDS = new List<String> { "NON", "Humans_Mage.mds", "Humans_Babe.mds", "HumanS_Militia.mds", "HumanS_Relaxed.mds", "Humans_Tired.mds", "Humans_Arrogance.mds" };
        public static void SetClass(GothicClass cls)
        {
            Process process = Process.ThisProcess();
            oCNpc player = oCNpc.Player(process);

            ushort headTex = player.HeadTex;
            ushort bodyTex = player.BodyTex;
            string headVisual = player.HeadVisualString.Value;
            string bodyVisual = player.BodyVisualString.Value;
            zString[] overlayAnims = new zString[player.ActiveOverlays.Size];
            for (int i = 0; i < overlayAnims.Length; i++)
                overlayAnims[i] = player.ActiveOverlays.get(i, 20);


            //String overlay = "";
            //foreach (String ovl in BodyMDS)
            //player.GetOverlay();

            int voice = player.Voice;



            ////Unequippen
            oCItem equipItem = player.GetEquippedMeleeWeapon();
            if (equipItem.Address != 0)
                player.UnequipItem(equipItem);

            equipItem = player.GetEquippedRangedWeapon();
            if (equipItem.Address != 0)
                player.UnequipItem(equipItem);

            equipItem = player.GetEquippedArmor();
            if (equipItem.Address != 0)
                player.UnequipItem(equipItem);


            List<oCItem> itemList = new List<oCItem>();
            zCListSort<oCItem> itemListS = player.Inventory.ItemList;
            do
            {
                itemList.Add(itemListS.Data);
            } while ((itemListS = itemListS.Next).Address != 0);

            foreach (oCItem itm in itemList)
                player.RemoveFromInv(itm, itm.Amount);


            //Instance
            zString instancestr = zString.Create(process, cls.instance);
            int npcid = zCParser.getParser(process).GetIndex(instancestr);
            instancestr.Dispose();
            if (npcid == 0)
            {
                instancestr = zString.Create(process, "PC_HERO");
                npcid = zCParser.getParser(process).GetIndex(instancestr);
                instancestr.Dispose();
            }
            oCNpc newplayer = oCObjectFactory.GetFactory(process).CreateNPC(npcid);
            zVec3 pos = player.GetPosition();
            newplayer.Enable(pos);
            pos.Dispose();

            newplayer.SetAsPlayer();
            oCGame.Game(process).GetSpawnManager().DeleteNPC(player);

            player = newplayer;

            //Aussehen
            player.HeadTex = headTex;
            player.BodyTex = bodyTex;
            player.HeadVisualString.Set(headVisual);
            player.BodyVisualString.Set(bodyVisual);
            player.Voice = voice;

            zString[] ooA = new zString[player.ActiveOverlays.Size];
            for (int i = 0; i < ooA.Length; i++)
                ooA[i] = player.ActiveOverlays.get(i, 20);
            foreach (zString zs in ooA)
                player.RemoveOverlay(zs);

            foreach (zString zs in overlayAnims)
                player.ApplyOverlay(zs);





            //talente auf 0
            for (int i = 1; i < 5; i++)
            {
                player.SetHitChances(i, 0);
            }
            for (int i = 0; i < player.Talents.Size; i++)
            {
                player.SetTalentSkill(i, 0);
            }
            //Overlay von Akrobatik löschen...
            zString acroStr = zString.Create(process, "Humans_Acrobatic.mds");
            player.RemoveOverlay(acroStr);
            acroStr.Dispose();


            //talente setzen!
            foreach (talent tal in cls.talents)
            {
                if (tal.id == (byte)oCNpc.NPC_Talents.H1 || tal.id == (byte)oCNpc.NPC_Talents.H2
                    || tal.id == (byte)oCNpc.NPC_Talents.CrossBow || tal.id == (byte)oCNpc.NPC_Talents.Bow)
                {
                    player.SetFightSkill(tal.id, tal.value);
                }
                else if (tal.id == (byte)oCNpc.NPC_Talents.Pickpocket)
                {
                    player.SetTalentSkill(tal.id, 1);
                    player.SetTalentValue(tal.id, tal.value);
                }
                else
                {
                    player.SetTalentSkill(tal.id, tal.value);
                }
            }


            //Attribute
            player.HP = cls.hp;
            player.HPMax = cls.hp;
            player.MP = cls.mp;
            player.MPMax = cls.mp;
            player.Strength = cls.str;
            player.Dexterity = cls.dex;

            //Equippen
            if (cls.weapon != "")
            {
                zString str = zString.Create(process, cls.weapon);
                if (zCParser.getParser(process).GetIndex(str) != 0)
                {
                    oCItem item = player.PutInInv(str, 1);
                    
                    player.Equip(item);
                }
                str.Dispose();
            }
            if (cls.rangeweapon != "")
            {
                zString str = zString.Create(process, cls.rangeweapon);
                if (zCParser.getParser(process).GetIndex(str) != 0)
                {
                    oCItem item = player.PutInInv(str, 1);
                    
                    player.Equip(item);
                }
                str.Dispose();
            }
            if (cls.armor != "")
            {
                zString str = zString.Create(process, cls.armor);
                if (zCParser.getParser(process).GetIndex(str) != 0)
                {
                    oCItem item = player.PutInInv(str, 1);
                    player.RemoveFromInv(item, item.Amount);
                    player.PutInInv(item);
                    
                    player.Equip(item);
                }
                str.Dispose();
            }

            //Inventar hinzufügen!
            if (Program.Player == null)
            {
                foreach (item it in cls.items)
                {
                    zString str = zString.Create(process, it.code);
                    oCItem item = player.PutInInv(str, it.Amount);
                    player.RemoveFromInv(item, item.Amount);
                    player.PutInInv(item);
                    str.Dispose();
                }
            }
            else
            {
                Program.Player.itemList = cls.items;
                Commando.SetInventory(Program.Player);
            }
        }



        public static GothicClass gc;
        public void continueButton(object obj, EventArgs args)
        {
            
            Next(module);
        }

        public override void Next(Network.Module module)
        {
            Process process = Process.ThisProcess();
            //zCInput_Win32.GetInput(process).SetDeviceEnabled(2, 1);
            left.Hide();
            right.Hide();
            contiueButton.Hide();
            zCView.GetScreen(process).RemoveItem(mainview);
            InputHooked.activateFullControl(process);

            base.Next(module);
        }
    }
}
