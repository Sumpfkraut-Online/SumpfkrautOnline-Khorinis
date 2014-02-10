using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Modules;
using GMP_Server;
using AccountServerModule.Messages;
using GMP_Server.Net.Message;
using Network;
using AccountServerModule.SqlLite;

namespace AccountServerModule
{
    public class Loader : UpdateModule
    {
        static bool started;
        public static long lastUpdate;
        public override void update(Network.Module module)
        {
            if (!started)
            {
                Program.server.messageListener.Add((byte)0xdf, new AccountMessage());
                Program.server.messageListener.Add((byte)0xde, new StartMessage());
                DisconnectedMessage.Disconnected += new EventHandler<DisconnectedMessage.DisconnectedEventArgs>(AccountMessage.Disconnected);
                started = true;
            }


            
            if (lastUpdate + 10000 * 1000 < DateTime.Now.Ticks)
            {

                foreach (Character chara in AccountMessage.characterList)
                {
                    if (chara.saveStarted)
                    {
                        Player pl = Player.getPlayerByGuid(chara.mGUID, Program.playerList);
                        chara.hp = pl.lastHP;
                        chara.hp_max = pl.lastHP_Max;
                        chara.mp = pl.lastMP;
                        chara.mp_max = pl.lastMP_Max; 
                        chara.str = pl.lastStr;
                        chara.dex = pl.lastDex;
                        chara.posX = pl.pos[0];
                        chara.posY = pl.pos[1];
                        chara.posZ = pl.pos[2];
                        chara.world = Player.getMap(pl.actualMap);

                        chara.hitChances = pl.lastHitChances;
                        chara.talents = pl.lastTalentSkills;
                        chara.itemList = pl.itemList;
                        chara.SaveCharacter();
                    }
                }

                lastUpdate = DateTime.Now.Ticks;
            }
        }
    }
}
