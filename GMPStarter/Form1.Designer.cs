namespace GMPStarter
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mBStart = new System.Windows.Forms.Button();
            this.tB_Nickname = new System.Windows.Forms.TextBox();
            this.tB_IP = new System.Windows.Forms.TextBox();
            this.tB_Port = new System.Windows.Forms.TextBox();
            this.tB_Password = new System.Windows.Forms.TextBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.zLogLevel = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.bFavRemove = new System.Windows.Forms.Button();
            this.bFavAdd = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.DGV_Fav = new System.Windows.Forms.DataGridView();
            this.fav_servername = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fav_serverip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fav_serverport = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fav_serverslots = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fav_serverping = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.fav_serverlanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.zLogLevel)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Fav)).BeginInit();
            this.SuspendLayout();
            // 
            // mBStart
            // 
            this.mBStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mBStart.Location = new System.Drawing.Point(760, 363);
            this.mBStart.Name = "mBStart";
            this.mBStart.Size = new System.Drawing.Size(182, 40);
            this.mBStart.TabIndex = 0;
            this.mBStart.Text = "Start";
            this.mBStart.UseVisualStyleBackColor = true;
            this.mBStart.Click += new System.EventHandler(this.mBStart_Click);
            // 
            // tB_Nickname
            // 
            this.tB_Nickname.Location = new System.Drawing.Point(74, 69);
            this.tB_Nickname.Name = "tB_Nickname";
            this.tB_Nickname.Size = new System.Drawing.Size(255, 20);
            this.tB_Nickname.TabIndex = 1;
            // 
            // tB_IP
            // 
            this.tB_IP.Location = new System.Drawing.Point(74, 17);
            this.tB_IP.Name = "tB_IP";
            this.tB_IP.Size = new System.Drawing.Size(125, 20);
            this.tB_IP.TabIndex = 2;
            // 
            // tB_Port
            // 
            this.tB_Port.Location = new System.Drawing.Point(269, 17);
            this.tB_Port.Name = "tB_Port";
            this.tB_Port.Size = new System.Drawing.Size(72, 20);
            this.tB_Port.TabIndex = 3;
            // 
            // tB_Password
            // 
            this.tB_Password.Location = new System.Drawing.Point(74, 43);
            this.tB_Password.Name = "tB_Password";
            this.tB_Password.Size = new System.Drawing.Size(267, 20);
            this.tB_Password.TabIndex = 4;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(10, 92);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(95, 17);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "Window-Mode";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // zLogLevel
            // 
            this.zLogLevel.Location = new System.Drawing.Point(90, 19);
            this.zLogLevel.Maximum = 9;
            this.zLogLevel.Minimum = -1;
            this.zLogLevel.Name = "zLogLevel";
            this.zLogLevel.Size = new System.Drawing.Size(104, 45);
            this.zLogLevel.TabIndex = 8;
            this.zLogLevel.Value = -1;
            this.zLogLevel.Scroll += new System.EventHandler(this.zLogLevel_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Nickname:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "IP:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(234, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Port:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(74, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Maximum FPS";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(90, 66);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(46, 20);
            this.textBox5.TabIndex = 14;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBox5);
            this.groupBox1.Controls.Add(this.zLogLevel);
            this.groupBox1.Controls.Add(this.checkBox2);
            this.groupBox1.Location = new System.Drawing.Point(12, 283);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 120);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General Options:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(71, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(13, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Log-Level:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.bFavRemove);
            this.groupBox2.Controls.Add(this.bFavAdd);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.tB_IP);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.tB_Port);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.tB_Password);
            this.groupBox2.Controls.Add(this.tB_Nickname);
            this.groupBox2.Location = new System.Drawing.Point(350, 283);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(404, 120);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server-Options:";
            // 
            // bFavRemove
            // 
            this.bFavRemove.Location = new System.Drawing.Point(206, 91);
            this.bFavRemove.Name = "bFavRemove";
            this.bFavRemove.Size = new System.Drawing.Size(192, 23);
            this.bFavRemove.TabIndex = 14;
            this.bFavRemove.Text = "Remove from favorites";
            this.bFavRemove.UseVisualStyleBackColor = true;
            this.bFavRemove.Click += new System.EventHandler(this.bFavRemove_Click);
            // 
            // bFavAdd
            // 
            this.bFavAdd.Location = new System.Drawing.Point(7, 91);
            this.bFavAdd.Name = "bFavAdd";
            this.bFavAdd.Size = new System.Drawing.Size(192, 23);
            this.bFavAdd.TabIndex = 13;
            this.bFavAdd.Text = "Add to favorites";
            this.bFavAdd.UseVisualStyleBackColor = true;
            this.bFavAdd.Click += new System.EventHandler(this.bFavAdd_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(930, 265);
            this.tabControl1.TabIndex = 18;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.DGV_Fav);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(922, 239);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Favorites";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // DGV_Fav
            // 
            this.DGV_Fav.AllowUserToAddRows = false;
            this.DGV_Fav.AllowUserToDeleteRows = false;
            this.DGV_Fav.AllowUserToOrderColumns = true;
            this.DGV_Fav.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_Fav.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_Fav.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_Fav.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fav_servername,
            this.fav_serverip,
            this.fav_serverport,
            this.fav_serverslots,
            this.fav_serverping,
            this.fav_serverlanguage});
            this.DGV_Fav.Location = new System.Drawing.Point(7, 6);
            this.DGV_Fav.Name = "DGV_Fav";
            this.DGV_Fav.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_Fav.Size = new System.Drawing.Size(909, 226);
            this.DGV_Fav.TabIndex = 1;
            this.DGV_Fav.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DGV_Fav_CellDoubleClick);
            // 
            // fav_servername
            // 
            this.fav_servername.HeaderText = "Servername";
            this.fav_servername.Name = "fav_servername";
            this.fav_servername.ReadOnly = true;
            // 
            // fav_serverip
            // 
            this.fav_serverip.HeaderText = "IP";
            this.fav_serverip.Name = "fav_serverip";
            this.fav_serverip.ReadOnly = true;
            // 
            // fav_serverport
            // 
            this.fav_serverport.FillWeight = 50F;
            this.fav_serverport.HeaderText = "Port";
            this.fav_serverport.Name = "fav_serverport";
            this.fav_serverport.ReadOnly = true;
            // 
            // fav_serverslots
            // 
            this.fav_serverslots.FillWeight = 50F;
            this.fav_serverslots.HeaderText = "Slots";
            this.fav_serverslots.Name = "fav_serverslots";
            this.fav_serverslots.ReadOnly = true;
            // 
            // fav_serverping
            // 
            this.fav_serverping.FillWeight = 50F;
            this.fav_serverping.HeaderText = "Ping";
            this.fav_serverping.Name = "fav_serverping";
            this.fav_serverping.ReadOnly = true;
            // 
            // fav_serverlanguage
            // 
            this.fav_serverlanguage.FillWeight = 50F;
            this.fav_serverlanguage.HeaderText = "Language";
            this.fav_serverlanguage.Name = "fav_serverlanguage";
            this.fav_serverlanguage.ReadOnly = true;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 415);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mBStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.zLogLevel)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_Fav)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button mBStart;
        private System.Windows.Forms.TextBox tB_Nickname;
        private System.Windows.Forms.TextBox tB_Password;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.TrackBar zLogLevel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox5;
        public System.Windows.Forms.TextBox tB_IP;
        public System.Windows.Forms.TextBox tB_Port;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button bFavRemove;
        private System.Windows.Forms.Button bFavAdd;
        private System.Windows.Forms.DataGridView DGV_Fav;
        private System.Windows.Forms.DataGridViewTextBoxColumn fav_servername;
        private System.Windows.Forms.DataGridViewTextBoxColumn fav_serverip;
        private System.Windows.Forms.DataGridViewTextBoxColumn fav_serverport;
        private System.Windows.Forms.DataGridViewTextBoxColumn fav_serverslots;
        private System.Windows.Forms.DataGridViewTextBoxColumn fav_serverping;
        private System.Windows.Forms.DataGridViewTextBoxColumn fav_serverlanguage;
        private System.Windows.Forms.Timer timer1;
    }
}

