#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace GUC.Server.Scripts.Web.Sites
{
    public class Accounts : Site
    {
        public override string HTMLName
        {
            get { return "accounts.html"; }
        }

        public override string TitleName
        {
            get { return "Accounts"; }
        }

        public override Permissions Permissions
        {
            get { return Web.Permissions.ShowAccounts; }
        }

        

        public override bool load(StringBuilder sb, Permissions permissions, HttpListenerContext context)
        {
            if (!base.load(sb, permissions, context))
                return false;

            if (!permissions.HasFlag(Permissions))
            {
                AppendNoRights(sb);
                return true;
            }


            Actions.GetAccountListAction gpla = new Actions.GetAccountListAction();
            ActionTimer.get().addAction(gpla);

            while (!gpla.IsFinished)
                Thread.Sleep(50);



            sb.AppendLine("<h2>Accounts</h2>");
            sb.AppendLine("\t\t<div class=\"content\">");
            sb.AppendLine("\t\t\t<table>");

            foreach(Actions.GetAccountListAction.Account acc in gpla.List){
                sb.AppendLine("\t\t\t\t<tr>");
                sb.AppendLine("\t\t\t\t\t<td>");
                sb.AppendLine("\t\t\t\t\t\t"+acc.accountID);
                sb.AppendLine("\t\t\t\t\t</td>");
                sb.AppendLine("\t\t\t\t\t<td>");
                sb.AppendLine("\t\t\t\t\t\t" + acc.name);
                sb.AppendLine("\t\t\t\t\t</td>");
                if (permissions.HasFlag(Permissions.Accounts_EDIT))
                {
                    sb.AppendLine("\t\t\t\t\t<td>");
                    sb.AppendLine("\t\t\t\t\t\t" + "<a href=\"#\" >[Bearbeiten]</a>");
                    sb.AppendLine("\t\t\t\t\t</td>");
                }
                if (permissions.HasFlag(Permissions.Accounts_Log))
                {
                    sb.AppendLine("\t\t\t\t\t<td>");
                    sb.AppendLine("\t\t\t\t\t\t" + "<a href=\"accounts_Log.html?accID="+acc.accountID+"\" >[Logs]</a>");
                    sb.AppendLine("\t\t\t\t\t</td>");
                }
                if (permissions.HasFlag(Permissions.Accounts_Delete))
                {
                    sb.AppendLine("\t\t\t\t\t<td>");
                    sb.AppendLine("\t\t\t\t\t\t" + "<a href=\"#\" >[Löschen]</a>");
                    sb.AppendLine("\t\t\t\t\t</td>");
                }
                if (permissions.HasFlag(Permissions.Accounts_Active))
                {
                    sb.AppendLine("\t\t\t\t\t<td>");
                    sb.AppendLine("\t\t\t\t\t\t" + "<a href=\"#\" >[Deaktivieren]</a>");
                    sb.AppendLine("\t\t\t\t\t</td>");
                }
                sb.AppendLine("\t\t\t\t</tr>");
            }

            sb.AppendLine("\t\t\t</table>");
            sb.AppendLine("\t\t</div>");


            return true;
        }
    }
}

#endif