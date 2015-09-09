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
            NPCInstance npc = new NPCInstance(0, "_MALE");
            npc.Visual = "HUMANS.MDS";
            npc.BodyMesh = "HUM_BODY_NAKED0";

            npc = new NPCInstance(1, "_FEMALE");
            npc.Visual = "HUMANS.MDS";
            npc.BodyMesh = "HUM_BODY_BABE0";

            npc = new NPCInstance("Scavenger");
            npc.Visual = "Scavenger.mds";
            npc.BodyMesh = "Sca_Body";

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
            npc.Voice = 3;

            /*Random rand = random;
            for (int i = 0; i < 10000; i++)
            {
                npc = new NPCInstance("NPC_" + i);
                npc.Name = RandomString(10);
                npc.Visual = RandomString(10);
                npc.BodyMesh = RandomString(10);
                npc.BodyTex = (byte)rand.Next(0, 0xFF);
                npc.HeadMesh = RandomString(10);
                npc.HeadTex = (byte)rand.Next(0, 0xFF);

                npc.BodyWidth = (byte)rand.Next(0, 0xFF);
                npc.BodyHeight = (byte)rand.Next(0, 0xFF);
                npc.Fatness = (sbyte)rand.Next(0, 55);

                npc.Voice = (byte)rand.Next(0, 0xFF);
            }*/

            NPCInstance.NetUpdate();

            ItemInstance item = new ItemInstance("itfo_apple");
            item.Name = "Apfel";

            item.Type = Enumeration.ItemType.Food_Small;

            item.Visual = "ITFO_APPLE.3DS";
            item.Material = Enumeration.ItemMaterial.Leather;

            item.Description = item.Name;
            item.Text[1] = "Lebensenergier wiederherstellen:";
            item.Count[1] = 3;

            item.Text[3] = "Ein frischer Apfel";
            item.Text[4] = "knackig und saftig";
            item.Text[5] = "Gewicht:";
            item.Count[5] = 1;


            item = new ItemInstance("itfo_bread");
            item.Name = "Brot";
            item.Visual = "ITFO_BREAD.3DS";

            item = new ItemInstance("itfo_cheese");
            item.Name = "Käse";
            item.Visual = "ITFO_CHEESE.3DS";

            item = new ItemInstance("itfo_bacon");
            item.Name = "Schinken";
            item.Visual = "ITFO_BACON.3DS";

            item = new ItemInstance("itfo_fish");
            item.Name = "Fisch";
            item.Visual = "ITFO_FISH.3DS";

            item = new ItemInstance("itfo_stew");
            item.Name = "Eintopf";
            item.Visual = "ITFO_STEW.3DS";

            item = new ItemInstance("itfo_beer");
            item.Name = "Bier";
            item.Visual = "ITFO_BEER.3DS";

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
