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
            npc.Visual = "HUMANS.MDS";
            npc.BodyMesh = "HUM_BODY_NAKED0";
            npc.BodyTex = 1;
            npc.HeadMesh = "Hum_Head_Pony";
            npc.HeadTex = 22;
            npc.BodyHeight = 70;
            npc.BodyWidth = 70;
            npc.Fatness = sbyte.MinValue;
            npc.Voice = 2;

            /*Random rand = random;
            for (int i = 0; i < 1000; i++)
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
