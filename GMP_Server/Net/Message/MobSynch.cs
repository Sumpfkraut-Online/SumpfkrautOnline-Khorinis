using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using Network;

namespace GMP_Server.Net.Message
{
    public class MobSynch : Message
    {
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Server server)
        {
            int id = 0;
            byte type = 0;
            int vobtype = 0;
            String name = "";
            float[] pos = new float[3];
            String x; String y;


            stream.Read(out id);
            stream.Read(out type);
            stream.Read(out vobtype);
            stream.Read(out name);
            stream.Read(out x);
            stream.Read(out y);
            for (int i = 0; i < 3;i++ )
                stream.Read(out pos[i]);

            if (type == 1)
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);
                //Console.WriteLine("StartInteract: " + pl.name + " Triggered-Vob: " + name + " in World: " + pl.actualMap + "test: " + x + " 2:" + y);

                Program.scriptManager.OnStartInteraction(new Scripting.Player(pl), vobtype, name, pos);
            }
            else if (type == 2)
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);
                //Console.WriteLine("StopInteract: " + pl.name + " Triggered-Vob: " + name + " in World: " + pl.actualMap + "test: " + x + " 2:" + y);

                Program.scriptManager.OnStopInteraction(new Scripting.Player(pl), vobtype, name, pos);
            }
            else if (type == 4)
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);

                int ch = 0;
                try
                {
                    ch = Convert.ToInt32(x);
                }
                catch (Exception ex) { Console.WriteLine("Mobsynch, Picklocking problem..."); }
                Program.scriptManager.OnPickLock(new Scripting.Player(pl), vobtype, name, pos, ch);

                return;
            }
            else if (type == 5)
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);
                Program.scriptManager.OnOpenContainer(new Scripting.Player(pl), vobtype, name, pos);
            }
            else if (type == 6)
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);

                bool hasKey = false;
                try
                {
                    int cf = Convert.ToInt32(x);
                    if (cf == 1)
                        hasKey = true;
                }
                catch (Exception ex) { Console.WriteLine("Mobsynch, hasKey problem..."); }
                Program.scriptManager.OnCloseContainer(new Scripting.Player(pl), vobtype, name, pos, hasKey);
            }
            else if (type == 11)//OnTrigger
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);
                Program.World.TriggerMobInter(vobtype, name, pos, pl.actualMap, true);
                //Console.WriteLine("OnTrigger: "+pl.name+" Triggered-Vob: "+name+" in World: "+pl.actualMap+ "test: "+x+" 2:"+y);

                Program.scriptManager.OnTrigger(new Scripting.Player(pl), vobtype, name, pos);

            }
            else if (type == 12)//OnUnTrigger
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);
                Program.World.TriggerMobInter(vobtype, name, pos, pl.actualMap, false);
                Program.scriptManager.OnUnTrigger(new Scripting.Player(pl), vobtype, name, pos);
                //Console.WriteLine("OnUnTrigger: " + pl.name + " Triggered-Vob: " + name + " in World: " + pl.actualMap);
            }
            else if (type == 13)//Remove Item
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);
                pl.InsertItem(new item() { code = x, Amount = Convert.ToInt32(y) });

                Program.World.RemoveContainerItem(x, Convert.ToInt32(y), name, pos, pl.actualMap);

                Program.scriptManager.OnRemoveItemToContainer(new Scripting.Player(pl), vobtype, name, pos, x, Convert.ToInt32(y));
                //Console.WriteLine("Item Remove from Container: " + name + " Item:" + x + " Amount:" + y + " Player-Map: " + pl.actualMap);
            }
            else if (type == 14)//Insert Item
            {
                Player pl = Player.getPlayerSort(id, Program.playerList);
                pl.RemoveItem(new item() { code = x, Amount = Convert.ToInt32(y) });

                Program.World.InsertContainerItem(x, Convert.ToInt32(y), name, pos, pl.actualMap);

                Program.scriptManager.OnInsertItemToContainer(new Scripting.Player(pl), vobtype, name, pos, x, Convert.ToInt32(y));
                //Console.WriteLine("Item insert into Container: "+name+" Item:"+x+" Amount:"+y+" Player-Map: " +pl.actualMap);
            }
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.MobSynch);
            stream.Write(id);
            stream.Write(type);
            stream.Write(vobtype);
            stream.Write(name);
            stream.Write(x);
            stream.Write(y);
            for (int i = 0; i < 3; i++)
                stream.Write(pos[i]);

            server.server.Send(stream, PacketPriority.LOW_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, true);
        }
    }
}
