using System;
using System.Collections.Generic;
using System.Text;
using Mono.Data.Sqlite;
using Network.Types;
using Network.Savings;
using Network;

namespace GMP_Server.DB
{
    public class WorldSave
    {
        public WorldSave()
        {
            Program.World.MobInterInsert += new Network.Savings.World.MobInterEventHandler(mobInterInserted);
            Program.World.MobInterUpdate += new Network.Savings.World.MobInterEventHandler(mobInterUpdate);
            Program.World.MobInterRemove += new Network.Savings.World.MobInterEventHandler(mobInterDeleted);


            Program.World.ItemsInsert += new World.ItemsEventHandler(itemsInserted);
            Program.World.ItemsRemove += new World.ItemsEventHandler(itemsRemoved);


            Program.World.ContainerInsert += new World.ContainerEventHandler(MobContainerInserted);
            Program.World.ContainerRemove += new World.ContainerEventHandler(MobContainerRemoved);

            Program.World.ContainerItemInsert += new World.ContainerItemEventHandler(MobContainerItemInserted);
            Program.World.ContainerItemRemove += new World.ContainerItemEventHandler(MobContainerItemRemoved);
            Program.World.ContainerItemUpdate += new World.ContainerItemEventHandler(MobContainerItemUpdated);
        }

        

        public void LoadWorld()
        {
            LoadWorld_MovInter();
            LoadWorld_Items();
            LoadWorld_Containers();
        }

        public void LoadWorld_MovInter()
        {
            String select = "SELECT * FROM `world_mobinteract` ";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = select;
            SqliteDataReader sdr = command.ExecuteReader();

            if (!sdr.HasRows)
            {
                command.Dispose();
                sdr.Close();
                sdr.Dispose();
                return;
            }

            while (sdr.Read())
            {
                WorldMobInter wmi = new WorldMobInter()
                {
                    name = Convert.ToString(sdr["name"]),
                    vobType = Convert.ToInt32(sdr["vobtype"]),
                    pos = new float[] { Convert.ToSingle(sdr["posx"]), Convert.ToSingle(sdr["posy"]), Convert.ToSingle(sdr["posz"]), },
                    world = Player.getMap(Convert.ToString(sdr["world"])),
                    triggered = Convert.ToBoolean(sdr["triggered"])
                };
                Program.World.mobInter.Add(wmi);
            }
            command.Dispose();
            sdr.Close();
            sdr.Dispose();
        }

        public void LoadWorld_Items()
        {
            String select = "SELECT * FROM `world_items` ";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = select;
            SqliteDataReader sdr = command.ExecuteReader();

            if (!sdr.HasRows)
            {
                command.Dispose();
                sdr.Close();
                sdr.Dispose();
                return;
            }

            while (sdr.Read())
            {
                WorldItems wi = new WorldItems()
                {
                    itm = new item() { code = Convert.ToString(sdr["iteminstance"]), Amount = Convert.ToInt32(sdr["amount"]) },
                    pos = new float[] { Convert.ToSingle(sdr["posx"]), Convert.ToSingle(sdr["posy"]), Convert.ToSingle(sdr["posz"]), },
                    world = Player.getMap(Convert.ToString(sdr["world"]))
                };
                Program.World.Items.Add(wi);
            }
            command.Dispose();
            sdr.Close();
            sdr.Dispose();
        }

        public void LoadWorld_Containers()
        {
            String select = "SELECT * FROM `world_container` ";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = select;
            SqliteDataReader sdr = command.ExecuteReader();

            if (!sdr.HasRows)
            {
                command.Dispose();
                sdr.Close();
                sdr.Dispose();
                return;
            }

            while (sdr.Read())
            {
                WorldContainer wc = new WorldContainer()
                {
                    name = Convert.ToString(sdr["name"]),
                    pos = new float[] { Convert.ToSingle(sdr["posx"]), Convert.ToSingle(sdr["posy"]), Convert.ToSingle(sdr["posz"]), },
                    world = Player.getMap(Convert.ToString(sdr["world"]))
                };

                //Items herausholen...
                String selectITM = "SELECT * FROM `world_container_items` WHERE `containerid`=@id ";
                SqliteCommand commandITM = new SqliteCommand(Program.sqlite.connection);
                commandITM.CommandText = selectITM;

                commandITM.Parameters.AddWithValue("@id", sdr["id"]);
                SqliteDataReader sdrITM = commandITM.ExecuteReader();

                if (!sdrITM.HasRows)
                {
                    commandITM.Dispose();
                    sdrITM.Close();
                    sdrITM.Dispose();
                    continue;
                }

                while (sdrITM.Read())
                {
                    item itm = new item() { code = Convert.ToString(sdrITM["item"]), Amount = Convert.ToInt32(sdrITM["amount"]) };
                    wc.itemList.Add(itm);
                }

                commandITM.Dispose();
                sdrITM.Close();
                sdrITM.Dispose();

                Program.World.container.Add( wc );
            }
            command.Dispose();
            sdr.Close();
            sdr.Dispose();
        }


        #region MobContainer

        void MobContainerInserted(object sender, EventArgs e, string name, float[] pos, String world)
        {
            world = Player.getMap(world);
            String insert = "INSERT INTO `world_container` (`id`, `name`, `posx`, `posy`, `posz`, `world` ,`opened`)";
            insert += "VALUES (NULL, @name, @posx, @posy, @posz, @world, @opened);";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = insert;

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@posx", pos[0]);
            command.Parameters.AddWithValue("@posy", pos[1]);
            command.Parameters.AddWithValue("@posz", pos[2]);
            command.Parameters.AddWithValue("@world", world);
            command.Parameters.AddWithValue("@opened", 1);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        void MobContainerRemoved(object sender, EventArgs e, string name, float[] pos, String world)
        {
            world = Player.getMap(world);
            int id = getMobContainerID(name, pos, world);
            if (id == -1)
            {
                Console.WriteLine("mobContainer Remove: Mob nicht in DB gefunden!");
                return;
            }

            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = "DELETE FROM `world_container` WHERE `id`=@id ";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            command.Dispose();

        }

        void MobContainerItemInserted(object sender, EventArgs e, String item, int amount, string name, float[] pos, String world)
        {
            world = Player.getMap(world);
            int id = getMobContainerID(name, pos, world);
            if (id == -1)
            {
                Console.WriteLine("mobContainer Item-Insert: Mob nicht in DB gefunden!");
                return;
            }
            String insert = "INSERT INTO `world_container_items` (`id`, `containerid`, `item`, `amount`)";
            insert += "VALUES (NULL, @containerid, @item, @amount);";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = insert;

            command.Parameters.AddWithValue("@containerid", id);
            command.Parameters.AddWithValue("@item", item);
            command.Parameters.AddWithValue("@amount", amount);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        void MobContainerItemRemoved(object sender, EventArgs e, String item, int amount, string name, float[] pos, String world)
        {
            world = Player.getMap(world);
            int id = getMobContainerID(name, pos, world);
            if (id == -1)
            {
                Console.WriteLine("mobContainer Item-Insert: Mob nicht in DB gefunden!");
                return;
            }

            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = "DELETE FROM `world_container_items` WHERE `containerid`=@id AND `item`=@item";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@item", item);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        void MobContainerItemUpdated(object sender, EventArgs e, String item, int amount, string name, float[] pos, String world)
        {
            world = Player.getMap(world);
            int id = getMobContainerID(name, pos, world);
            if (id == -1)
            {
                Console.WriteLine("mobContainer Item-Insert: Mob nicht in DB gefunden!");
                return;
            }

            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = "UPDATE `world_container_items` SET `amount`=@amount WHERE `containerid`=@id AND `item`=@item ";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@item", item);
            command.Parameters.AddWithValue("@amount", Convert.ToInt32(amount));
            command.ExecuteNonQuery();

            command.Dispose();
        }

        public int getMobContainerID(string name, float[] pos, String world)
        {
            world = Player.getMap(world);
            //String select = "SELECT id, posx, posy, posz FROM `world_mobinteract` WHERE `vobType`=@vobtype AND name=@name AND world=@world AND posx BETWEEN @posxmin AND @posxmax AND posy BETWEEN @posymin AND @posymax AND posz BETWEEN @poszmin AND @poszmax";
            String select = "SELECT id, posx, posy, posz FROM `world_container` WHERE name=@name AND world=@world";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = select;

            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@world", world);
            //command.Parameters.AddWithValue("@posxmin", Convert.ToInt32(pos[0]) - 1);
            //command.Parameters.AddWithValue("@posxmax", Convert.ToInt32(pos[0]) + 1);
            //command.Parameters.AddWithValue("@posymin", Convert.ToInt32(pos[1]) - 1);
            //command.Parameters.AddWithValue("@posymax", Convert.ToInt32(pos[1]) + 1);
            //command.Parameters.AddWithValue("@poszmin", Convert.ToInt32(pos[2]) - 1);
            //command.Parameters.AddWithValue("@poszmax", Convert.ToInt32(pos[2]) + 1);

            SqliteDataReader sdr = command.ExecuteReader();

            if (!sdr.HasRows)
            {
                command.Dispose();
                sdr.Close();
                sdr.Dispose();
                return -1;
            }

            int id = -1;
            float lastDistance = 0;

            while (sdr.Read())
            {
                float[] realPos = new float[] { Convert.ToSingle(sdr["posx"]), Convert.ToSingle(sdr["posy"]), Convert.ToSingle(sdr["posz"]) };
                float distance = new Vec3f(realPos).getDistance((Vec3f)pos);

                if (id == -1 || distance < lastDistance)
                {
                    id = Convert.ToInt32(sdr["id"]);
                    lastDistance = distance;
                }

            }

            sdr.Close();
            sdr.Dispose();
            command.Dispose();

            return id;
        }

        #endregion

        #region Items
        void itemsInserted(object sender, EventArgs e, string name, int amount, float[] pos, String world)
        {
            world = Player.getMap(world);
            String insert = "INSERT INTO `world_items` (`id`, `iteminstance`, `amount`, `posx`, `posy`, `posz`, `world`)";
            insert += "VALUES (NULL, @iteminstance, @amount, @posx, @posy, @posz, @world);";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = insert;

            command.Parameters.AddWithValue("@iteminstance", name);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@posx", pos[0]);
            command.Parameters.AddWithValue("@posy", pos[1]);
            command.Parameters.AddWithValue("@posz", pos[2]);
            command.Parameters.AddWithValue("@world", world);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        void itemsRemoved(object sender, EventArgs e, string name, int amount, float[] pos, String world)
        {
            world = Player.getMap(world);
            int id = getItemID(name, amount, pos, world);
            if (id == -1)
            {
                Console.WriteLine("Items-Removed: Mob nicht in DB gefunden!");
                return;
            }

            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = "DELETE FROM `world_items` WHERE `id`=@id ";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            command.Dispose();
        }

        int getItemID(string name, int amount, float[] pos, String world)
        {
            world = Player.getMap(world);
            //String select = "SELECT id, posx, posy, posz FROM `world_items` WHERE `iteminstance`=@iteminstance AND amount=@amount AND world=@world AND posx BETWEEN @posxmin AND @posxmax AND posy BETWEEN @posymin AND @posymax AND posz BETWEEN @poszmin AND @poszmax";
            //SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            //command.CommandText = select;

            //command.Parameters.AddWithValue("@iteminstance", name);
            //command.Parameters.AddWithValue("@amount", amount);
            //command.Parameters.AddWithValue("@world", world);
            //command.Parameters.AddWithValue("@posxmin", Convert.ToInt32(pos[0]) - 1);
            //command.Parameters.AddWithValue("@posxmax", Convert.ToInt32(pos[0]) + 1);
            //command.Parameters.AddWithValue("@posymin", Convert.ToInt32(pos[1]) - 1);
            //command.Parameters.AddWithValue("@posymax", Convert.ToInt32(pos[1]) + 1);
            //command.Parameters.AddWithValue("@poszmin", Convert.ToInt32(pos[2]) - 1);
            //command.Parameters.AddWithValue("@poszmax", Convert.ToInt32(pos[2]) + 1);
            String select = "SELECT id, posx, posy, posz FROM `world_items` WHERE `iteminstance`=@iteminstance AND amount=@amount AND world=@world ";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = select;

            command.Parameters.AddWithValue("@iteminstance", name);
            command.Parameters.AddWithValue("@amount", amount);
            command.Parameters.AddWithValue("@world", world);

            SqliteDataReader sdr = command.ExecuteReader();

            if (!sdr.HasRows)
            {
                Console.WriteLine("getItemID: Not IN DB: "+name + " " + amount + " "+ world);
                command.Dispose();
                sdr.Close();
                sdr.Dispose();
                return -1;
            }

            int id = -1;
            float lastDistance = 0;

            while (sdr.Read())
            {
                float[] realPos = new float[] { Convert.ToSingle(sdr["posx"]), Convert.ToSingle(sdr["posy"]), Convert.ToSingle(sdr["posz"]) };
                float distance = new Vec3f(realPos).getDistance((Vec3f)pos);

                if (id == -1 || distance < lastDistance)
                {
                    id = Convert.ToInt32(sdr["id"]);
                    lastDistance = distance;
                }

            }

            sdr.Close();
            sdr.Dispose();
            command.Dispose();
            return id;
        }
        #endregion

        #region MobInteract

        public int getMobID(int vobtype, string name, float[] pos, String world)
        {
            world = Player.getMap(world);
            //String select = "SELECT id, posx, posy, posz FROM `world_mobinteract` WHERE `vobType`=@vobtype AND name=@name AND world=@world AND posx BETWEEN @posxmin AND @posxmax AND posy BETWEEN @posymin AND @posymax AND posz BETWEEN @poszmin AND @poszmax";
            String select = "SELECT id, posx, posy, posz FROM `world_mobinteract` WHERE `vobType`=@vobtype AND name=@name AND world=@world";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = select;

            command.Parameters.AddWithValue("@vobtype", vobtype);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@world", world);
            //command.Parameters.AddWithValue("@posxmin", Convert.ToInt32(pos[0]) - 1);
            //command.Parameters.AddWithValue("@posxmax", Convert.ToInt32(pos[0]) + 1);
            //command.Parameters.AddWithValue("@posymin", Convert.ToInt32(pos[1]) - 1);
            //command.Parameters.AddWithValue("@posymax", Convert.ToInt32(pos[1]) + 1);
            //command.Parameters.AddWithValue("@poszmin", Convert.ToInt32(pos[2]) - 1);
            //command.Parameters.AddWithValue("@poszmax", Convert.ToInt32(pos[2]) + 1);

            SqliteDataReader sdr = command.ExecuteReader();

            if (!sdr.HasRows)
            {
                command.Dispose();
                sdr.Close();
                sdr.Dispose();
                return -1;
            }

            int id = -1;
            float lastDistance = 0;

            while (sdr.Read())
            {
                float[] realPos = new float[]{ Convert.ToSingle(sdr["posx"]), Convert.ToSingle(sdr["posy"]), Convert.ToSingle(sdr["posz"]) };
                float distance = new Vec3f(realPos).getDistance((Vec3f)pos);    

                if (id == -1 || distance < lastDistance)
                {
                    id = Convert.ToInt32(sdr["id"]);
                    lastDistance = distance;
                }

            }

            sdr.Close();
            sdr.Dispose();
            command.Dispose();

            return id;
        }

        void mobInterInserted(object sender, EventArgs args, int vobtype, string name, float[] pos, String world, bool triggered)
        {
            world = Player.getMap(world);
            String insert = "INSERT INTO `world_mobinteract` (`id`, `vobType`, `name`, `posx`, `posy`, `posz`, `world`, `triggered`)";
            insert += "VALUES (NULL, @vobtype, @name, @posx, @posy, @posz, @world, @triggered);";
            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = insert;

            command.Parameters.AddWithValue("@vobtype", vobtype);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@posx", pos[0]);
            command.Parameters.AddWithValue("@posy", pos[1]);
            command.Parameters.AddWithValue("@posz", pos[2]);
            command.Parameters.AddWithValue("@world", world);
            command.Parameters.AddWithValue("@triggered", triggered);

            command.ExecuteNonQuery();
            command.Dispose();
        }

        void mobInterUpdate(object sender, EventArgs args, int vobtype, string name, float[] pos, String world, bool triggered)
        {
            world = Player.getMap(world);
            int id = getMobID(vobtype, name, pos, world);
            if (id == -1)
            {
                Console.WriteLine("mobInterUpdate: Mob nicht in DB gefunden!");
                return;
            }

            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = "UPDATE `world_mobinteract` SET `triggered`=@triggered WHERE `id`=@id ";
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@triggered", Convert.ToInt32(triggered));
            command.ExecuteNonQuery();

            command.Dispose();
        }

        void mobInterDeleted(object sender, EventArgs args, int vobtype, string name, float[] pos, String world, bool triggered)
        {
            world = Player.getMap(world);
            int id = getMobID(vobtype, name, pos, world);
            if (id == -1)
            {
                Console.WriteLine("mobInter Remove: Mob nicht in DB gefunden!");
                return;
            }

            SqliteCommand command = new SqliteCommand(Program.sqlite.connection);
            command.CommandText = "DELETE FROM `world_mobinteract` WHERE `id`=@id ";
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();

            command.Dispose();

        }
        #endregion
    }

}
