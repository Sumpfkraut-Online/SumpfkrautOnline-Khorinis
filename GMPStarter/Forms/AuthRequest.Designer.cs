namespace GMPStarter.Forms
{
    partial class AuthRequest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tBUsername = new System.Windows.Forms.TextBox();
            this.tBPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bLogin = new System.Windows.Forms.Button();
            this.bBreak = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // tBUsername
            // 
            this.tBUsername.Location = new System.Drawing.Point(95, 40);
            this.tBUsername.Name = "tBUsername";
            this.tBUsername.Size = new System.Drawing.Size(261, 20);
            this.tBUsername.TabIndex = 2;
            // 
            // tBPassword
            // 
            this.tBPassword.Location = new System.Drawing.Point(95, 67);
            this.tBPassword.Name = "tBPassword";
            this.tBPassword.Size = new System.Drawing.Size(261, 20);
            this.tBPassword.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(207, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Der Update benötigt eine Authentifizierung";
            // 
            // bLogin
            // 
            this.bLogin.Location = new System.Drawing.Point(200, 93);
            this.bLogin.Name = "bLogin";
            this.bLogin.Size = new System.Drawing.Size(75, 23);
            this.bLogin.TabIndex = 5;
            this.bLogin.Text = "Anmelden";
            this.bLogin.UseVisualStyleBackColor = true;
            this.bLogin.Click += new System.EventHandler(this.bLogin_Click);
            // 
            // bBreak
            // 
            this.bBreak.Location = new System.Drawing.Point(281, 93);
            this.bBreak.Name = "bBreak";
            this.bBreak.Size = new System.Drawing.Size(75, 23);
            this.bBreak.TabIndex = 6;
            this.bBreak.Text = "Abbrechen";
            this.bBreak.UseVisualStyleBackColor = true;
            this.bBreak.Click += new System.EventHandler(this.bBreak_Click);
            // 
            // AuthRequest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 125);
            this.Controls.Add(this.bBreak);
            this.Controls.Add(this.bLogin);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tBPassword);
            this.Controls.Add(this.tBUsername);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "AuthRequest";
            this.Text = "AuthRequest";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tBUsername;
        private System.Windows.Forms.TextBox tBPassword;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bLogin;
        private System.Windows.Forms.Button bBreak;
    }
}