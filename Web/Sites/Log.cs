#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace GUC.Server.Scripts.Web.Sites
{
    public class Log : Site
    {
        public override string HTMLName
        {
            get { return "log.html"; }
        }

        public override string TitleName
        {
            get { return "Log";  }
        }

        public override Permissions Permissions
        {
            get { return Permissions.ShowLog; }
        }

        public override bool load(StringBuilder sb, Permissions permissions, System.Net.HttpListenerContext context)
        {
            if (!base.load(sb, permissions, context))
                return false;

            if (!permissions.HasFlag(Permissions))
            {
                AppendNoRights(sb);
                return true;
            }

            sb.AppendLine("<h2>Log:</h2><br>");
            sb.AppendLine("\t\t<div class=\"content\">");
            try
            {
                sb.AppendLine(File.ReadAllText("serverlog.html"));
            }
            catch (Exception ex) { }
            sb.AppendLine("\t\t</div>");


            return true;
        }
    }
}

#endif