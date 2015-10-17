using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Instances
{
    static class VobInstances
    {
        // wird in Zukunft von Datenbank übernommen
        public static void Init()
        {
            NPCInstance npc = new NPCInstance(1, "_MALE");
            npc.Visual = "HUMANS.MDS";
            npc.BodyMesh = "HUM_BODY_NAKED0";
            NPCInstance.Table.Add(npc);

            npc = new NPCInstance(2, "_FEMALE");
            npc.Visual = "HUMANS.MDS";
            npc.BodyMesh = "HUM_BODY_BABE0";
            NPCInstance.Table.Add(npc);

            npc = new NPCInstance("Scavenger");
            npc.Visual = "Scavenger.mds";
            npc.BodyMesh = "Sca_Body";
            NPCInstance.Table.Add(npc);

            npc = new NPCInstance("Mud");
            npc.Name = "Mud";
            npc.Visual = "HUMANS.MDS";
            npc.BodyMesh = "HUM_BODY_NAKED0";
            npc.BodyTex = 1;
            npc.HeadMesh = "Hum_Head_Pony";
            npc.HeadTex = 22;
            npc.BodyHeight = 100;
            npc.BodyWidth = 100;
            npc.Fatness = 20;
            npc.Voice = Enumeration.HumVoice.Andre;
            NPCInstance.Table.Add(npc);

            NPCInstance.Table.NetUpdate();




            ItemInstance item;


            item = new ItemInstance("itfo_apple");
            item.Name = "Apfel";
            
            item.Type = Enumeration.ItemType.Food_Small;

            item.Visual = "ITFO_APPLE.3DS";
            item.Material = Enumeration.ItemMaterial.Leather;

            item.Text[0] = "Lebensenergie wiederherstellen:";
            item.Count[0] = 3;

            item.Text[2] = "Ein frischer Apfel";
            item.Text[3] = "knackig und saftig";
            ItemInstance.Table.Add(item);



            item = new ItemInstance("itfo_bread");
            item.Name = "Brot";
            item.Visual = "ITFO_BREAD.3DS";
            ItemInstance.Table.Add(item);

            item = new ItemInstance("itfo_cheese");
            item.Name = "Käse";
            item.Visual = "ITFO_CHEESE.3DS";
            ItemInstance.Table.Add(item);

            item = new ItemInstance("itfo_bacon");
            item.Name = "Schinken";
            item.Visual = "ITFO_BACON.3DS";
            ItemInstance.Table.Add(item);

            item = new ItemInstance("itfo_fish");
            item.Name = "Fisch";
            item.Visual = "ITFO_FISH.3DS";
            ItemInstance.Table.Add(item);

            item = new ItemInstance("itfo_stew");
            item.Name = "Eintopf";
            item.Visual = "ITFO_STEW.3DS";
            ItemInstance.Table.Add(item);

            item = new ItemInstance("itfo_beer");
            item.Name = "Bier";
            item.Visual = "ITFO_BEER.3DS";
            ItemInstance.Table.Add(item);

            item = new ItemInstance("itmw_wolfszahn");
            item.Name = "Wolfszahn";
            item.Gender = Enumeration.Gender.Masculine;
            item.Type = Enumeration.ItemType.Sword_1H;
            item.Material = Enumeration.ItemMaterial.Metal;
            item.Visual = "ItMw_020_1h_Sword_short_04.3DS";
            item.Text[1] = "Schaden:"; item.Count[0] = 25;
            item.Text[2] = "Benötigte Stärke:"; item.Count[1] = 20;
            item.Text[3] = "Einhand";
            ItemInstance.Table.Add(item);

            ItemInstance.Table.NetUpdate();
        }

        static Random random = new Random();
        static string RandomString(int len)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[len];

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }
    }
}
