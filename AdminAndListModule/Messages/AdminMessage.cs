using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Injection;
using RakNet;
using Gothic.zClasses;
using Gothic.zTypes;
using WinApi;
using GMP.Helper;
using Network;
using GMP.Modules;
using Gothic.mClasses;

namespace ListModule.Messages
{
    public class AdminMessage : Message
    {
        public static bool isAdmin;
        public static int permission;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="client"></param>
        /// <param name="type"></param>
        /// <param name="user">Benutername oder Daten</param>
        /// <param name="password"></param>
        /// <param name="otherPlayerID"></param>
        public void Write(RakNet.BitStream stream, Client client, byte type, string user, String password, int otherPlayerID)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xFD);
            stream.Write(Program.Player.id);
            stream.Write(Program.Player.guidStr);

            

            if (type == 0 && user.Trim().Length > 0)//User vorhanden, also nicht nur Masterpassword
                type = 1;

            stream.Write(type);

            if (type == 0)
            {
                stream.Write(password);
            }
            else if (type == 1)
            {
                stream.Write(user);
                stream.Write(password);
            }
            else
            {
                stream.Write(otherPlayerID);
                stream.Write(user);
            }
            client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

        }

        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, Client client)
        {
            byte type; bool login;
            int senderid;
            String guid;
            stream.Read(out senderid);
            stream.Read(out guid);
            stream.Read(out type);


            if (type == 0)
            {
                stream.Read(out login);
                if (!login)
                {
                    Process process = Process.ThisProcess();
                    //Fehlermeldung Ausgeben!
                    External_Helper.Print(process, "Login fehlgeschlagen!");

                }
                else
                {
                    bool fullAdmin;
                    stream.Read(out fullAdmin);
                    isAdmin = true;
                    if (fullAdmin)
                    {
                        permission = 0xfffff;
                    }
                    else
                    {
                        int perm;
                        stream.Read(out perm);
                        permission = perm;
                    }


                    Process process = Process.ThisProcess();
                    //Fehlermeldung Ausgeben!
                    External_Helper.Print(process, "Login erfolgreich!");


                }
            }
            else
            {
                Process process = Process.ThisProcess();
                int playerid; String param;
                stream.Read(out playerid); stream.Read(out param);
                if (type == 6)//Teleport
                {
                    Player adminPlayer = Player.getPlayer(senderid, StaticVars.playerlist);
                    oCNpc npc = oCNpc.Player(process);

                    if (!Player.isSameMap(Program.Player.actualMap,  adminPlayer.actualMap))
                    {
                        zString targetlevel = zString.Create(process, Player.getMap(adminPlayer.actualMap));
                        oCGame.Game(process).ChangeLevel(targetlevel, targetlevel);
                        targetlevel.Dispose();
                    }

                    npc.TrafoObjToWorld.set(3, adminPlayer.pos[0]);
                    npc.TrafoObjToWorld.set(7, adminPlayer.pos[1]);
                    npc.TrafoObjToWorld.set(11, adminPlayer.pos[2]);


                    External_Helper.Print(process, "Du wurdest teleportiert!");
                }
                else if (type == 15)//Fatness
                {
                    oCNpc npc = oCNpc.Player(process);
                    try
                    {
                        npc.SetFatness(Convert.ToSingle(param));
                    }catch(Exception ex){}
                }
                else if (type == 14)//Scale
                {
                    oCNpc npc = oCNpc.Player(process);
                    String[] scales = param.Split(';');

                    try
                    {
                        zVec3 vec = zVec3.Create(process);
                        vec.X = Convert.ToSingle(scales[0]);
                        vec.Y = Convert.ToSingle(scales[1]);
                        vec.Z = Convert.ToSingle(scales[2]);
                        npc.SetModelScale(vec);
                        vec.Dispose();
                    }
                    catch (Exception ex) { }
                }
                else if (type == 8) //Unsterblichkeit
                {
                    Player pl = Player.getPlayer(playerid, StaticVars.playerlist);
                    

                    bool b = false;
                    try
                    {
                        if (Convert.ToInt32(param) == 1) b = true;
                    }
                    catch (Exception ex) { param = "0"; }

                    pl.isImmortal = b;
                    if (pl.isSpawned && pl.isImmortal)
                        new oCNpc(process, pl.NPCAddress).Flags |= (int)oCNpc.NPC_Flags.Immortal;
                    else if (pl.isSpawned && !pl.isImmortal)
                        new oCNpc(process, pl.NPCAddress).Flags &= (int)~oCNpc.NPC_Flags.Immortal;
                }
                else if (type == 7) //Unsichtbarkeit
                {
                    Player pl = Player.getPlayer(playerid, StaticVars.playerlist);


                    bool b = false;
                    try
                    {
                        if (Convert.ToInt32(param) == 1) b = true;
                    }
                    catch (Exception ex) { param = "0"; }

                    pl.isInvisible = b;
                    if (pl.isSpawned && pl.isInvisible)
                        new oCNpc(process, pl.NPCAddress).setShowVisual(true);
                    else if (pl.isSpawned && !pl.isInvisible)
                        new oCNpc(process, pl.NPCAddress).setShowVisual(false);
                }
                else if (type == 10) //Freeze
                {
                    Player pl = Player.getPlayer(playerid, StaticVars.playerlist);


                    bool b = false;
                    try
                    {
                        if (Convert.ToInt32(param) == 1) b = true;
                    }
                    catch (Exception ex) { param = "0"; }

                    pl.isFreeze = b;
                    if (pl.isPlayer && pl.isFreeze)
                        InputHooked.deaktivateFullControl(process);
                    else if (pl.isPlayer && !pl.isFreeze)
                        InputHooked.activateFullControl(process);
                }
                else if (type == 4) //mute
                {
                    Player pl = Player.getPlayer(playerid, StaticVars.playerlist);


                    bool b = false;
                    try
                    {
                        if (Convert.ToInt32(param) == 1) b = true;
                    }
                    catch (Exception ex) { param = "0"; }

                    pl.isMuted = b;
                    if (pl.isPlayer && pl.isMuted)
                        Program.chatBox.addRow(0, "Du wurdest gemutet! Der Chat wurde deaktiviert!");
                    else if (pl.isPlayer)
                        Program.chatBox.addRow(0, "Du bist nicht mehr gemutet! Der Chat wurde aktiviert!");
                }
            }
        }
    }
}
