using System;
using System.Collections.Generic;
using System.Text;
using GMP_Server.Net.Message;
using Network;
using GMP_Server;
using GMP_Server.Helper;

namespace ListModuleServer
{
    public class AdminMessage : Message
    {
        public class Admin
        {
            public Player player;
            public User user;
            public bool isAdmin;
            public bool isFullAdmin;
        }

        public class WrongPassword
        {
            public Player player;
            public int tryCount;
        }

        public static List<Admin> adminList = new List<Admin>();
        public static List<WrongPassword> wrongList = new List<WrongPassword>();
        public override void Read(RakNet.BitStream stream, RakNet.Packet packet, GMP_Server.Net.Server server)
        {
            //0xFD
            int id; String guid;

            stream.Read(out id); stream.Read(out guid);

            Player player = Player.getPlayer(id, Program.playList);
            if (player == null)
                return;

            byte type = 0;
            stream.Read(out type);
            if (type == 0)//Login mit Masterpassowrd
            {
                String masterPassword = "";
                stream.Read(out masterPassword);

                if (ListModuleServer.sOptions.MasterPassword.Length == 0 ||
                    ListModuleServer.sOptions.MasterPassword.Trim() != masterPassword.Trim())
                {
                    GMP_Server.Log.Logger.log(GMP_Server.Log.Logger.LOG_INFO, player.guid + " " + player.systemAddress + " Player: " + player.name + " failed to Login. Password:" + masterPassword);
                    WrongPasswordFunc(player, server, packet);
                    WriteLoginMessage(stream, packet, server,id,guid, false, null);
                    return;
                }
                //Password ist korrekt isAdmin setzen
                Admin admin = new Admin();
                admin.player = player;
                admin.isAdmin = true;
                admin.isFullAdmin = true;
                adminList.Add(admin);

                WriteLoginMessage(stream, packet, server, id, guid, true, admin);
                GMP_Server.Log.Logger.log(GMP_Server.Log.Logger.LOG_INFO, player.guid + " " + player.systemAddress + " Player: " + player.name + " Logged in as Master");
                return;
            }
            else if (type == 1)//Login mit Username
            {
                String username, password;
                stream.Read(out username); stream.Read(out password);

                Admin admin = new Admin();
                foreach (User user in ListModuleServer.sOptions.UserList)
                {
                    if (user.name.Trim() == username.Trim() && user.password.Trim() == password.Trim())
                    {
                        admin.user = user;
                        break;
                    }
                }
                if (admin.user == null)
                {
                    GMP_Server.Log.Logger.log(GMP_Server.Log.Logger.LOG_INFO, player.guid + " " + player.systemAddress + " Player: " + player.name + " failed to Login. User: " + username + " Password: " + password);
                    
                    WrongPasswordFunc(player, server, packet);
                    WriteLoginMessage(stream, packet, server, id, guid, false, null);
                    return;
                }
                //Password korrekt und eingeloggt
                admin.player = player;
                admin.isAdmin = true;
                adminList.Add(admin);

                WriteLoginMessage(stream, packet, server, id, guid, true, admin);
                return;

            }
            else//Befehle
            {
                //Ist der Spieler ein Admin?
                bool isadmin = false;
                foreach (Admin admin in adminList)
                {
                    if (admin.player == player)
                    {
                        isadmin = true;
                        break;
                    }
                }

                if (!isadmin)//Darf nicht geschehen! 
                {
                    server.server.AddToBanList(packet.systemAddress.ToString());//Todo: ban?
                    return;
                }

                //Befehl lesen
                int otherplayerid;
                String param;

                stream.Read(out otherplayerid); stream.Read(out param);

                //typen
                if (type == 2 || type == 3 || type == 16)//Nur server
                {
                    if (type == 2)
                    {
                        Player otherPlayer = Player.getPlayer(otherplayerid, Program.playList);
                        if (otherPlayer == null)
                            return;
                        Kick.kick(otherPlayer);

                    }
                    else if (type == 3)
                    {
                        Player otherPlayer = Player.getPlayer(otherplayerid, Program.playList);
                        if (otherPlayer == null)
                            return;
                        Kick.ban(otherPlayer);

                    }
                    else if (type == 16)//SetTime
                    {
                        String[] scales = param.Split(';');

                        try
                        {
                            Program.gTime.time = GothicTime.GetTime(Convert.ToInt32(scales[0]), Convert.ToInt32(scales[1]), Convert.ToInt32(scales[2]));
                            Program.time = Program.gTime.time;
                            new TimeMessage().Write(server.receiveBitStream, server);
                        }
                        catch (Exception ex) { }
                    }
                }
                else if (type == 10  || type == 6 || type == 14 || type == 15)//Nur zum Spieler
                {
                    //Freeze, Teleport, SetFatNess oder SetScale
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)0xFD);
                    stream.Write(id);
                    stream.Write(guid);
                    stream.Write(type);

                    stream.Write(otherplayerid);
                    stream.Write(param);


                    Player otherPlayer = Player.getPlayer(otherplayerid, Program.playList);
                    if (otherPlayer == null)
                        return;

                    GMP_Server.Log.Logger.log(GMP_Server.Log.Logger.LOG_INFO, player.guid + " " + player.systemAddress + " Player: " + player.name + " teleport Player " + otherPlayer.name);
                    
                    
                    server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, otherPlayer.guid, false);

                }
                else if (type == 7 || type == 8 || type == 10 || type == 4)//Zu jedem Spieler
                {
                    //Unsichtbarkeit, unsterblichkeit
                    stream.Reset();
                    stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                    stream.Write((byte)0xFD);
                    stream.Write(id);
                    stream.Write(guid);
                    stream.Write(type);

                    stream.Write(otherplayerid);
                    stream.Write(param);

                    bool b = false;
                    try 
                    { 
                        if (Convert.ToInt32(param) == 1)  b = true;
                    }
                    catch (Exception ex) { param = "0"; }

                    Player otherPlayer = Player.getPlayer(otherplayerid, Program.playList);
                    if (type == 8)
                        otherPlayer.isImmortal = b;
                    else if (type == 7)
                        otherPlayer.isInvisible = b;
                    else if (type == 10)
                        otherPlayer.isFreeze = b;
                    else if (type == 4)
                        otherPlayer.isMuted = b;

                    server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                }
            }
        }



        public static void WrongPasswordFunc(Player player, GMP_Server.Net.Server server, RakNet.Packet packet)
        {
            WrongPassword wp = null;
            foreach (WrongPassword wpT in wrongList)
            {
                if (wpT.player == player)
                {
                    wp = wpT;
                    break;
                }
            }
            if (wp == null)
            {
                wp = new WrongPassword();
                wp.player = player;
                wp.tryCount = 1;
                wrongList.Add(wp);
            }
            else
            {
                wp.tryCount += 1;
                if (wp.tryCount >= ListModuleServer.sOptions.maxTries)
                {
                    GMP_Server.Log.Logger.log(GMP_Server.Log.Logger.LOG_INFO, player.guid + " " + player.systemAddress + " Player: " + player.name + " was banned (Admin)");

                    Kick.ban(player);
                    return;
                }
            }

        }

        public static void WriteLoginMessage(RakNet.BitStream stream, RakNet.Packet packet, GMP_Server.Net.Server server, int id, String guid, bool login, Admin admin)
        {
            stream.Reset();
            stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)0xFD);
            stream.Write(id);
            stream.Write(guid);
            stream.Write((byte)0);//type
            stream.Write(login);

            if (login)
            {
                stream.Write(admin.isFullAdmin);
                if (!admin.isFullAdmin)
                {
                    int permissions = 0xfffffff;
                    if (admin.user.PermName.Trim().Length != 0)
                    {
                        permissions = 0;
                        Permission perm = ListModuleServer.sOptions.GetPermissionByName(admin.user.PermName);
                        if (perm.kick)
                            permissions += 1;
                        if (perm.ban)
                            permissions += 2;
                        if (perm.mute)
                            permissions += 4;
                        if (perm.teleportToPlayer)
                            permissions += 8;
                        if (perm.TeleportPlayer)
                            permissions += 16;
                        if (perm.makeInvisible)
                            permissions += 32;
                        if (perm.makeImmortal)
                            permissions += 64;
                        if (perm.giveItem)
                            permissions += 128;
                        if (perm.freeze)
                            permissions += 256;
                        if (perm.kill)
                            permissions += 512;
                        if (perm.revive)
                            permissions += 1024;
                        if (perm.giveTempRights)
                            permissions += 2048;
                        if (perm.Scale)
                            permissions += 4096;
                        if (perm.SetFatness)
                            permissions += 8192;
                        if (perm.setTalents)
                            permissions += 16384;
                    }
                    stream.Write(permissions);
                }
            }



            server.server.Send(stream, RakNet.PacketPriority.HIGH_PRIORITY, RakNet.PacketReliability.RELIABLE_ORDERED, (char)0, packet.systemAddress, false);



        }
    }
}
