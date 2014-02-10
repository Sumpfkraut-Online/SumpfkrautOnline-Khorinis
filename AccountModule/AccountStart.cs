using System;
using System.Collections.Generic;
using System.Text;
using GMP.Modules;
using Network;
using AccountModule.Login;
using WinApi;
using Injection;

namespace AccountModule
{
    public class AccountStart : StartState
    {
        bool started;
        public Module module;
        public override void Update(Module module)
        {
            if (!started)
            {
                Program.client.messageListener.Add((byte)0xdf, new AccountMessage());
                openAccountLogin();
                this.module = module;

                started = true;
            }
        }

        public static LoginBox loginBox;
        public void openAccountLogin()
        {
            Process process = Process.ThisProcess();
            loginBox = new LoginBox(process, this);
            loginBox.Open();
        }

        public void closeAccountLogin()
        {

        }

        public override void Next(Module module)
        {
            loginBox.Close();
            
            base.Next(module);
        }
    }
}
