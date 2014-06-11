#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Web
{
    public enum Permissions
    {
        ShowUserList = 1 << 0,
        ShowUserMap = 1 << 1,
        ShowLog = 1 << 2,
        ShowChat = 1 << 3,
        ShowMessages = 1 << 4,
        ShowAccounts = 1 << 5,
        Accounts_EDIT =  ShowAccounts | 1 << 6,
        Accounts_Delete = ShowAccounts | 1 << 7,
        Accounts_Active = ShowAccounts | 1 << 8,
        Accounts_Log = ShowAccounts | 1 << 9,


        All = ShowUserList | ShowUserMap | ShowLog | ShowChat | ShowMessages | 
            ShowAccounts | Accounts_EDIT | Accounts_Delete | Accounts_Active | Accounts_Log

    }

}

#endif