using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WinApi;
using WinApi.Kernel;
using System.IO;
using Gothic_Untold_Chapter.Forms;
using WinApi.FileFormat;
using GUC.Options;
using GMPStarter.Options;

namespace GMPStarter
{
    public partial class Form1 : Form
    {
        public ClientOptions co;
        public FavoritesOptions fo;

        List<StatusMessage> statusMessageList = new List<StatusMessage>();
        
        public Form1()
        {
            InitializeComponent();

            StarterFunctions.InitDefaultFolders();

            co = StarterFunctions.getClientOptions();
            fo = StarterFunctions.getFavoritesOptions();

            foreach (FavoritesOptions.cFavorite fav in fo.Favorites)
            {
                addFavoriteToFavList(fav);
                StatusMessage sm = new StatusMessage(fav.ip, (ushort)fav.port);
                sm.start();
                statusMessageList.Add(sm);
            }


            tB_Nickname.Text = co.name;
            tB_IP.Text = co.ip;
            tB_Port.Text = co.port+"";
            tB_Password.Text = co.password;

            textBox5.Text = co.fps + "";
            checkBox2.Checked = co.startWindowed;

            if (co.loglevel >= 0)
                zLogLevel.Value = co.loglevel;

            this.timer1.Start();
        }
        
        public void mBStart_Click(object sender, EventArgs e)
        {

            String nickname = tB_Nickname.Text.Trim();
            String ip = tB_IP.Text.Trim();
            ushort port = getPort();
            String password = tB_Password.Text.Trim();
            int logLevel = zLogLevel.Value;
            int fps = -1;
            bool StartWindowed = checkBox2.Checked;
            Int32.TryParse(textBox5.Text.Trim(), out fps);


            FilesLoader fl = new FilesLoader(co, nickname, ip, port, password, logLevel, fps, StartWindowed);
            fl.ShowDialog(this);

            //try
            //{
            //    StarterFunctions.StartGothic(co, nickname, ip, port, password, logLevel, fps, StartWindowed);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An Error occurred while starting GUC: "+ex.ToString());
            //}
                
        }

        public ushort getPort()
        {
            ushort port = 0;
            UInt16.TryParse(tB_Port.Text.Trim(), out port);

            return port;
        }

        UpdateLoader2 uL;
        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!co.autoupdate)
                return;
            uL = new UpdateLoader2(this);
            uL.ShowDialog();
        }

        private void bFavAdd_Click(object sender, EventArgs e)
        {
            
            String ip = tB_IP.Text.Trim();
            ushort port = getPort();

            if(StarterFunctions.ContainsFavorite(fo, ip, port))
                return;

            FavoritesOptions.cFavorite fav = StarterFunctions.addFavorite(fo, ip, port);
            addFavoriteToFavList(fav);

            StatusMessage sm = new StatusMessage(fav.ip, (ushort)fav.port);
            sm.start();
            statusMessageList.Add(sm);
        }

        public void addFavoriteToFavList(FavoritesOptions.cFavorite fav)
        {
            DGV_Fav.Rows.Add("", fav.ip, fav.port, "0", "-1", ""); 
        }

        private void DGV_Fav_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.tB_IP.Text = Convert.ToString(DGV_Fav.SelectedRows[0].Cells["fav_serverip"].Value);
            this.tB_Port.Text = Convert.ToString(DGV_Fav.SelectedRows[0].Cells["fav_serverport"].Value);
        }

        private void bFavRemove_Click(object sender, EventArgs e)
        {
            String ip = tB_IP.Text.Trim();
            ushort port = getPort();

            if (!StarterFunctions.ContainsFavorite(fo, ip, port))
                return;

            StarterFunctions.removeFavorite(fo, ip, port);
            foreach (DataGridViewRow row in DGV_Fav.Rows)
            {
                bool t1 = ip.Equals(row.Cells["fav_serverip"].Value);
                bool t2 = port == Convert.ToUInt16(row.Cells["fav_serverport"].Value);
                if (ip.Equals(row.Cells["fav_serverip"].Value) && port == (ushort)Convert.ToUInt16(row.Cells["fav_serverport"].Value))
                {
                    DGV_Fav.Rows.Remove(row);
                    break;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            foreach (StatusMessage sm in this.statusMessageList.ToArray())
            {
                if (sm.isLoaded)
                {
                    foreach (DataGridViewRow row in DGV_Fav.Rows)
                    {
                        if (sm.IP.Equals(row.Cells["fav_serverip"].Value) && sm.Port == (ushort)Convert.ToUInt16(row.Cells["fav_serverport"].Value))
                        {
                            row.Cells["fav_servername"].Value = sm.ServerName;
                            row.Cells["fav_serverlanguage"].Value = sm.ServerLanguage;
                            row.Cells["fav_serverslots"].Value = sm.PlayerCount+"/"+sm.MaxSlots;
                            row.Cells["fav_serverping"].Value = sm.Ping;
                            break;
                        }
                    }
                    this.statusMessageList.Remove(sm);
                }
            }

            if (this.statusMessageList.Count == 0)
            {
                //timer1.Enabled = false;
                //timer1.Stop();
            }
        }

        private void zLogLevel_Scroll(object sender, EventArgs e)
        {
            label7.Text = ""+zLogLevel.Value;
        }

        
    }
}
