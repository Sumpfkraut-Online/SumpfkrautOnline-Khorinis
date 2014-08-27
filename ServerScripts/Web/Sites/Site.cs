#if SSM_WEB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace GUC.Server.Scripts.Web.Sites
{
    public abstract class Site
    {
        public abstract String HTMLName { get; }
        public abstract String TitleName { get; }
        public abstract Permissions Permissions { get;}

        public virtual bool ShowInNavi { get { return true; } }

        public virtual bool load(StringBuilder sb, Permissions permissions, HttpListenerContext context)
        {
            if (!context.Request.Url.LocalPath.EndsWith(HTMLName))
                return false;


            return true;
        }

        public virtual void AppendNoRights(StringBuilder sb)
        {
            sb.AppendLine("Du hast nicht die nötigen Zugangsrechte!");
        }

        public virtual void AppendNavi(StringBuilder sb, Permissions permissions)
        {

            if (permissions.HasFlag(Permissions))
                sb.AppendLine("\t\t\t\t<li><a href=\"" + HTMLName + "\">" + TitleName + "</a></li>");
        }
    }
}
#endif