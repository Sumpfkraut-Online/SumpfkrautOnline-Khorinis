#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Web
{
    public class User
    {
        private String mName;
        protected String mPassword;
        protected Permissions mPermissions;

        public User(String name, String password, Permissions permissions)
        {
            mName = name;
            mPassword = password;
            mPermissions = permissions;
        }

        public String Name { get { return mName; } }
        public String Password { get { return mPassword; } }
        public Permissions Permissions { get { return mPermissions; } }
    }
}

#endif