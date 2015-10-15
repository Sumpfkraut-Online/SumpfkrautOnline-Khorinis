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
            npc.visual = "HUMANS.MDS";
            npc.bodyMesh = "HUM_BODY_NAKED0";
            NPCInstance.Add(npc);

            npc = new NPCInstance(2, "_FEMALE");
            npc.visual = "HUMANS.MDS";
            npc.bodyMesh = "HUM_BODY_BABE0";
            NPCInstance.Add(npc);

            npc = new NPCInstance("Scavenger");
            npc.visual = "Scavenger.mds";
            npc.bodyMesh = "Sca_Body";
            NPCInstance.Add(npc);

            npc = new NPCInstance("Mud");
            npc.name = "Mud";
            npc.visual = "HUMANS.MDS";
            npc.bodyMesh = "HUM_BODY_NAKED0";
            npc.bodyTex = 1;
            npc.headMesh = "Hum_Head_Pony";
            npc.headTex = 22;
            npc.bodyHeight = 100;
            npc.bodyWidth = 100;
            npc.fatness = 20;
            npc.voice = Enumeration.HumVoice.Andre;
            NPCInstance.Add(npc);

            NPCInstance.NetUpdate();




            ItemInstance item;


            item = new ItemInstance("itfo_apple");
            item.type = Enumeration.ItemType.Food_Small;

            item.visual = "ITFO_APPLE.3DS";
            item.material = Enumeration.ItemMaterial.Leather;

            item.description = item.name;
            item.text[0] = "Lebensenergie wiederherstellen:";
            item.count[0] = 3;

            item.text[2] = "Ein frischer Apfel";
            item.text[3] = "knackig und saftig";
            ItemInstance.Add(item);



            item = new ItemInstance("itfo_bread");
            item.name = "Brot";
            item.visual = "ITFO_BREAD.3DS";
            ItemInstance.Add(item);

            item = new ItemInstance("itfo_cheese");
            item.name = "Käse";
            item.visual = "ITFO_CHEESE.3DS";
            ItemInstance.Add(item);

            item = new ItemInstance("itfo_bacon");
            item.name = "Schinken";
            item.visual = "ITFO_BACON.3DS";
            ItemInstance.Add(item);

            item = new ItemInstance("itfo_fish");
            item.name = "Fisch";
            item.visual = "ITFO_FISH.3DS";
            ItemInstance.Add(item);

            item = new ItemInstance("itfo_stew");
            item.name = "Eintopf";
            item.visual = "ITFO_STEW.3DS";
            ItemInstance.Add(item);

            item = new ItemInstance("itfo_beer");
            item.name = "Bier";
            item.visual = "ITFO_BEER.3DS";
            ItemInstance.Add(item);
            
            ItemInstance.NetUpdate();
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
