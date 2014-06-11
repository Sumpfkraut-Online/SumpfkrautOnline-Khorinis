using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Web;
using System.IO;
using GUC.Enumeration;

namespace GUC.Server.Scripts.Web.Sites
{
    public class Accounts_Log : Site
    {
        public override string HTMLName
        {
            get { return "accounts_Log.html"; }
        }

        public override string TitleName
        {
            get { return "Account Log"; }
        }

        public override Permissions Permissions
        {
            get { return Web.Permissions.Accounts_Log; }
        }

        public override bool ShowInNavi
        {
            get
            {
                return false;
            }
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

            int accID = 0;
            //if (context.Request.HttpMethod.ToUpper().Trim() == "GET")
            //{
            String query=  new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd();
            
            var nameValuePairs = HttpUtility.ParseQueryString(query, context.Request.ContentEncoding);

            Int32.TryParse(context.Request.QueryString.Get("accID"), out accID);
            //}


            sb.AppendLine("<h2>Account Log</h2>");
            sb.AppendLine("\t\t<div class=\"content\">");

            if (accID != 0)
            {
                Actions.GetAccountLogAction gpla = new Actions.GetAccountLogAction(accID);
                ActionTimer.get().addAction(gpla);

                while (!gpla.IsFinished)
                    Thread.Sleep(50);

                sb.AppendLine("<h3>Log für: "+gpla.Name+"</h3>");

                sb.AppendLine("\t\t\t<table>");
                foreach (Actions.GetAccountLogAction.AccountLogChar alc
                    in gpla.CharStatList)
                {
                    sb.AppendLine("\t\t\t\t<tr>");
                    sb.AppendLine("\t\t\t\t\t<td>");
                    sb.AppendLine("\t\t\t\t\t\t" + new DateTime(alc.time).ToShortTimeString() + " " + new DateTime(alc.time).ToShortDateString());
                    sb.AppendLine("\t\t\t\t\t</td>");
                    sb.AppendLine("\t\t\t\t\t<td>");
                    String type = "";
                    if (alc.type >= (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.HitChances)
                        type = ""+(NPCTalents)(alc.type - (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.HitChances);
                    else if (alc.type >= (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.TalentSkillStart)
                        type = "" + (NPCTalents)(alc.type - (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.TalentSkillStart);
                    else if (alc.type >= (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.TalentValuesStart)
                        type = "" + (NPCTalents)(alc.type - (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.TalentValuesStart);
                    else if (alc.type >= (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.AttributeStart)
                        type = "" + (NPCAttributeFlags)(alc.type - (long)GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.CharStat.AttributeStart);
                    type += " | " + alc.type;
                    sb.AppendLine("\t\t\t\t\t\t" + type);
                    sb.AppendLine("\t\t\t\t\t</td>");
                    sb.AppendLine("\t\t\t\t\t<td>");
                    sb.AppendLine("\t\t\t\t\t\t" + alc.value);
                    sb.AppendLine("\t\t\t\t\t</td>");
                    sb.AppendLine("\t\t\t\t</tr>");
                }
                sb.AppendLine("\t\t\t</table>");

            }
            else
            {
                sb.AppendLine("Account wurde nicht gefunden!");
            }

            



            


            sb.AppendLine("\t\t</div>");


            return true;
        }
    }
}
