using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Injection;
using Network;
using GMP.Logger;
using Gothic.zClasses;
using WinApi;
using GMP.Modules;
using GMP.Helper;

namespace GMP.Net.Messages
{
    public class NewPlayerMessage : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            if (!StaticVars.Ingame || Program.Player == null)
                return;
            ErrorLog.Log(typeof(NewPlayerMessage), "Read");
            int id = 0;
            String guid = "";
            String name = "";
            String world = "";
            String instance = "";
            int hp, hpmax, str, dex;
            float[] pos = new float[3];
            stream.Read(out id);

            if (Player.getPlayer(id, StaticVars.playerlist) != null)
                return;

            stream.Read(out guid);
            Player pl = new Player(guid);


            stream.Read(out name);
            stream.Read(out world);
            stream.Read(out instance);
            stream.Read(out hp); stream.Read(out hpmax); stream.Read(out str); stream.Read(out dex);

            for (int i = 0; i < pl.lastTalentSkills.Length; i++)
                stream.Read(out pl.lastTalentSkills[i]);
            for (int i = 1; i < 5; i++)
                stream.Read(out pl.lastHitChances[i - 1]);

            stream.Read(out pos[0]);
            stream.Read(out pos[1]);
            stream.Read(out pos[2]);

            //inventar
            int count = 0;
            stream.Read(out count);
            pl.itemList = new List<item>();
            for (int i = 0; i < count; i++)
            {
                string code; int amount;
                stream.Read(out code); stream.Read(out amount);
                pl.itemList.Add(new item() { code = code, Amount = amount });
            }




            pl.id = id;
            pl.actualMap = Player.getMap(world);
            pl.name = name;
            pl.pos = pos;
            pl.lastHP = hp;
            pl.lastHP_Max = hpmax;
            pl.lastStr = str;
            pl.lastDex = dex;

            

            pl.instance = instance;

            StaticVars.AllPlayerDict.Add(pl.id, pl);
            StaticVars.PlayerDict.Add(pl.id, pl);
            Program.playerList.Add(pl);
            Program.playerList.Sort(new Player.PlayerComparer());
            StaticVars.playerlist.Add(pl);

            ErrorLog.Log(typeof(NewPlayerMessage), "Read "+name+" "+guid + " " + id);
            Process Process = Process.ThisProcess();

            if (Program.chatBox != null && StaticVars.serverConfig.ShowConnectionMessages)
            {
                for (byte i = 0; i < Program.chatBox.maxType; i++)
                {
                    if(!StaticVars.serverConfig.HideNames || pl.knowName)
                        Program.chatBox.addRow(i, "New Player connected " + pl.name);
                    else
                        Program.chatBox.addRow(i, "New Player connected ");
                }
            }
            if (!Player.isSameMap(pl.actualMap, Program.Player.actualMap))
            {
                return;
            }

            

            NPCHelper.SpawnPlayer(pl,true);
            NPCHelper.SetStandards(pl);
        }
    }
}
