#if SSM_WEB
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using System.Web;

namespace GUC.Server.Scripts.Web.Sites
{
    public class Messages : Site
    {
        public override string HTMLName
        {
            get { return "messages.html"; }
        }

        public override string TitleName
        {
            get { return "Nachrichten";  }
        }

        public override Permissions Permissions
        {
            get { return Permissions.ShowMessages; }
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

            sb.AppendLine("\t\t<h2>Nachrichten:</h2><br>");

            if (context.Request.HttpMethod.ToUpper().Trim() == "POST")
            {
                var nameValuePairs = HttpUtility.ParseQueryString(new StreamReader(context.Request.InputStream, context.Request.ContentEncoding).ReadToEnd(), context.Request.ContentEncoding);
                int id = 0;
                if (Int32.TryParse(nameValuePairs.Get("msgID"), out id)
                    && nameValuePairs.Get("sendMsg") != null)
                {
                    String str = nameValuePairs.Get("taMsg");
                    if (str != null & str.Length != 0)
                    {
                        lock (Startup.chat.messageList)
                        {
                            foreach (Chat.SLMessage message in Startup.chat.messageList)
                            {
                                if (message.ID == id)
                                {
                                    message.IsAnswered = true;

                                    Actions.WriteMessageAction gpla = new Actions.WriteMessageAction(message.playerID, str, 255, 0, 0, 255);
                                    ActionTimer.get().addAction(gpla);

                                    while (!gpla.IsFinished)
                                        Thread.Sleep(50);

                                    sb.AppendLine("\t\t<div class=\"content_n\">");
                                    if (gpla.completed)
                                    {
                                        sb.AppendLine("Nachricht gesendet!");
                                    }
                                    else
                                    {
                                        sb.AppendLine("Spieler nicht Online!");
                                    }
                                    sb.AppendLine("\t\t</div>");

                                    break;
                                }
                            }
                        }
                    }
                }
            }

            lock (Startup.chat.messageList)
            {
                foreach (Chat.SLMessage slm in Startup.chat.messageList)
                {
                    sb.AppendLine("\t\t<div class=\"content_n\">");
                    sb.AppendLine("\t\t\t" + slm.name + " (" + slm.playerID + "): " + slm.msg + "<br>");
                    sb.AppendLine("\t\t\t<hr>");
                    sb.AppendLine("\t\t\t<form method=\"post\">");
                    sb.AppendLine("\t\t\t\t<input type=\"hidden\" name=\"msgID\" value=\"" + slm.ID + "\" />");
                    sb.AppendLine("\t\t\t\t<textarea name=\"taMsg\" style=\"width: 100%; height: 100px;\"></textarea>");
                    sb.AppendLine("\t\t\t\t<input style=\"width: 100%;\" name=\"sendMsg\" type=\"submit\" value=\"Abschicken\" >");
                    sb.AppendLine("\t\t\t</form>");
                    sb.AppendLine("\t\t</div>");
                }
            }


            return true;
        }
    }
}

#endif