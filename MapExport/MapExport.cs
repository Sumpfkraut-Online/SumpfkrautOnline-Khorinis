using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MapExport.ZenArchive;
using Mono.Data.Sqlite;
using Network;

namespace MapExport
{
    public partial class MapExport : Form
    {
        public int Percent = 0;
        public MapExport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox1.Text = openFileDialog1.FileName;
        }
        List<cVob> items = new List<cVob>();
        List<cVob> container = new List<cVob>();
        private void button2_Click(object sender, EventArgs e)
        {
            items.Clear();
            container.Clear();

            cZenArchive archive = new cZenArchive();
            archive.ImpExp = this;
            archive.Filename = textBox1.Text;
            archive.load(textBox1.Text);

            foreach (cVob vob in archive.AllVobs)
            {
                if (((String)vob.Eigenschaften["object_type"]).Trim().ToLower() == "oCItem:zCVob".ToLower())
                    items.Add(vob);
                if (((String)vob.Eigenschaften["object_type"]).Trim().ToLower() == "oCMobContainer:oCMobInter:oCMOB:zCVob".ToLower()
                    && ((String)vob.Eigenschaften["contains"]).Trim() != "")
                    container.Add(vob);
            }

            label1.Text = "Gefundene Truhen: "+container.Count;
            label2.Text = "Gefundene Items: " + items.Count;

            button3.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GMP_Server.DB.SQLLite sqlite = new GMP_Server.DB.SQLLite();
            sqlite.connection.ConnectionString = "Data Source=" + textBox2.Text;


            sqlite.Open();

            SqliteCommand command = new SqliteCommand(sqlite.connection);
            command.CommandText = "DELETE FROM `world_items` WHERE `world`=@world ";
            command.Parameters.AddWithValue("@world", Player.getMap(textBox3.Text));
            command.ExecuteNonQuery();

            command.Dispose();

            //Container:
            command = new SqliteCommand(sqlite.connection);
            command.CommandText = "SELECT * FROM `world_container`  WHERE `world`=@world";
            command.Parameters.AddWithValue("@world", Player.getMap(textBox3.Text));
            SqliteDataReader sdr = command.ExecuteReader();

            while (sdr.Read())
            {
                //Items herausholen...
                SqliteCommand commandITM = new SqliteCommand(sqlite.connection);
                commandITM.CommandText = "DELETE FROM `world_container_items` WHERE `containerid`=@id ";
                commandITM.Parameters.AddWithValue("@id", sdr["id"]);

                commandITM.ExecuteNonQuery();
                commandITM.Dispose();
            }
            command.Dispose();
            sdr.Close();
            sdr.Dispose();

            command = new SqliteCommand(sqlite.connection);
            command.CommandText = "DELETE FROM `world_container` WHERE `world`=@world ";
            command.Parameters.AddWithValue("@world", Player.getMap(textBox3.Text));
            command.ExecuteNonQuery();

            command.Dispose();



            String insert = "INSERT INTO `world_items` (`id`, `iteminstance`, `amount`, `posx`, `posy`, `posz`, `world`)";
            insert += "VALUES (NULL, @iteminstance, @amount, @posx, @posy, @posz, @world);";
            command = new SqliteCommand(sqlite.connection);
            command.CommandText = insert;

            foreach (cVob item in items)
            {
                String pos = (String)item.Eigenschaften["trafoOSToWSPos"];
                pos = pos.Replace('.', ',');
                string[] posA = pos.Split(' ');
                float[] posF = new float[] { Convert.ToSingle(posA[0]), Convert.ToSingle(posA[1]), Convert.ToSingle(posA[2]) };
                command.Parameters.AddWithValue("@iteminstance", (String)item.Eigenschaften["itemInstance"]);
                command.Parameters.AddWithValue("@amount", 1);
                command.Parameters.AddWithValue("@posx", posF[0]);
                command.Parameters.AddWithValue("@posy", posF[1]);
                command.Parameters.AddWithValue("@posz", posF[2]);
                command.Parameters.AddWithValue("@world", Player.getMap(textBox3.Text));

                command.ExecuteNonQuery();
            }
            command.Dispose();


            insert = "INSERT INTO `world_container` (`id`, `name`, `posx`, `posy`, `posz`, `world` ,`opened`)";
            insert += "VALUES (NULL, @name, @posx, @posy, @posz, @world, @opened);";
            command = new SqliteCommand(sqlite.connection);
            command.CommandText = insert;

            foreach (cVob cont in container)
            {
                String pos = (String)cont.Eigenschaften["trafoOSToWSPos"];
                pos = pos.Replace('.', ',');
                string[] posA = pos.Split(' ');
                float[] posF = new float[] { Convert.ToSingle(posA[0]), Convert.ToSingle(posA[1]), Convert.ToSingle(posA[2]) };
                

                command.Parameters.AddWithValue("@name", cont.Eigenschaften["vobName"]);
                command.Parameters.AddWithValue("@posx", posF[0]);
                command.Parameters.AddWithValue("@posy", posF[1]);
                command.Parameters.AddWithValue("@posz", posF[2]);
                command.Parameters.AddWithValue("@world", Player.getMap(textBox3.Text));
                command.Parameters.AddWithValue("@opened", 0);

                command.ExecuteNonQuery();

                SqliteCommand IDCmd = new SqliteCommand("Select last_insert_rowid();", sqlite.connection);
                int lastid = Convert.ToInt32(IDCmd.ExecuteScalar());
                IDCmd.Dispose();

                String[] contains = ((String)cont.Eigenschaften["contains"]).Split(new char[]{',', ';'});
                
                foreach(String itm in contains)
                {
                    int amount = 1;
                    String code = itm;
                    if(itm.IndexOf(":") != -1)
                    {
                        string[] itemVal = itm.Split(':');
                        code = itemVal[0];
                        amount = Convert.ToInt32(itemVal[1]);
                    }

                    SqliteCommand command2 = new SqliteCommand(sqlite.connection);
                    command2.CommandText = "INSERT INTO `world_container_items` (`id`, `containerid`, `item`, `amount`) VALUES (NULL, @containerid, @item, @amount);";

                    command2.Parameters.AddWithValue("@containerid", lastid);
                    command2.Parameters.AddWithValue("@item", code);
                    command2.Parameters.AddWithValue("@amount", amount);

                    command2.ExecuteNonQuery();
                    command2.Dispose();
                }
                
            }
            command.Dispose();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            textBox2.Text = openFileDialog1.FileName;
        }
    }
}
