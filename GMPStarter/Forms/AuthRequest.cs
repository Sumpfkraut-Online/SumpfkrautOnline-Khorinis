using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GMPStarter.Forms
{
    public partial class AuthRequest : Form
    {
        public AuthRequest()
        {
            InitializeComponent();
        }

        public String username = null;
        public String password = null;

        public bool loggedIn = false;

        private void bLogin_Click(object sender, EventArgs e)
        {
            username = tBUsername.Text;
            password = tBPassword.Text;
            loggedIn = true;

            this.Close();
        }

        private void bBreak_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
