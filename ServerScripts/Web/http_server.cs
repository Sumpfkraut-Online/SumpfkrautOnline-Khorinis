#if SSM_WEB

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Web;
using GUC.Server.Scripts.Web.Actions;
using GUC.Server.Scripts.Web.Sites;

namespace GUC.Server.Scripts.Web
{
    public class http_server
    {
        static HttpListener mListener = null;
        static Dictionary<String, User> mUserDict = new Dictionary<string, User>();

        static Site[] SiteList = new Site[]{new Map(), new Sites.Log(), new Sites.Accounts(), new Sites.Messages(),
            new Accounts_Log()};

        public static void addHTTPUser(String username, String password, Permissions perm)
        {
            mUserDict.Add(username, new User(username, password, perm));
        }

        public static Dictionary<String, User> UserDict { get { return mUserDict; } }

        public static void Init()
        {
            ActionTimer.get();

            //Init UserList:
            addHTTPUser("Rio", "test", Permissions.All);



            //Init Listener:
            mListener = new HttpListener();
            mListener.Prefixes.Add(String.Format("http://*:{0}{1}",
              1234, "/"));

            mListener.Start();
            mListener.AuthenticationSchemes = AuthenticationSchemes.Basic;
            

            mListener.BeginGetContext(new
      AsyncCallback(ContextReceivedCallback), null);


#if SSM_ACCOUNT_LOGGING
            GUC.Server.Scripts.Accounts.Logs.SQLiteLogger.Init();
#endif
        }


        private static void ContextReceivedCallback(IAsyncResult asyncResult)
        {
            try
            {
                HttpListenerContext context;

                // HttpListenerContext abholen
                context = mListener.EndGetContext(asyncResult);

                // neuen thread für eingehende requests starten
                mListener.BeginGetContext(new
                  AsyncCallback(ContextReceivedCallback), null);

                Console.WriteLine("Request für: {0}, {1}",
                  context.Request.Url.LocalPath, context.Request.ContentType);

                if (!checkUser(context))
                    return;

                if (sendCSSData(context))
                    return;
                if (sendImageData(context))
                    return;

                sendData(context);
            }
            catch (Exception ex)
            {
            }
        }

        #region Other-Datas
        private static bool sendCSSData(HttpListenerContext context)
        {
            if(!context.Request.Url.LocalPath.ToLower().Trim().EndsWith(".css"))
                return false;

            String fp = Path.GetFullPath("Web/" + context.Request.Url.LocalPath.ToLower().Trim());
            if (!fp.Contains(Environment.CurrentDirectory) || !File.Exists(fp))
            {
                error(context, 402);
                return true;
            }
            
            byte[] responseBytes = Encoding.UTF8.GetBytes(File.ReadAllText(fp));

            context.Response.ContentType = "text/css";
            context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
            context.Response.Close();

            return true;
        }

        private static bool sendImageData(HttpListenerContext context)
        {
            if (!context.Request.Url.LocalPath.ToLower().Trim().EndsWith(".jpg") && !context.Request.Url.LocalPath.ToLower().Trim().EndsWith(".jpeg") &&
                !context.Request.Url.LocalPath.ToLower().Trim().EndsWith(".png") && !context.Request.Url.LocalPath.ToLower().Trim().EndsWith(".gif"))
                return false;

            String fp = Path.GetFullPath("Web/" + context.Request.Url.LocalPath.ToLower().Trim());
            if (!fp.Contains(Environment.CurrentDirectory) || !File.Exists(fp))
            {
                error(context, 402);
                return true;
            }

            byte[] responseBytes = File.ReadAllBytes(fp);// Encoding.UTF8.GetBytes(File.ReadAllText(fp));

            
            context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
            context.Response.Close();

            return true;
        }

        #endregion

        #region HTML-DATA
        public static void AppendHeader(StringBuilder sb)
        {
            sb.AppendLine("<HTML>");
            sb.AppendLine("\t<HEAD>");
            sb.AppendLine("\t\t<link rel=\"stylesheet\" href=\"/css/base.css\" media=\"screen\">");
            sb.AppendLine("\t\t<meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">");
            sb.AppendLine("\t</HEAD>");
            sb.AppendLine("\t<BODY>");

            
        }

        public static void AppendNavi(StringBuilder sb, Permissions permissions)
        {
            sb.AppendLine("\t\t<div class=\"navi\">");
            sb.AppendLine("\t\t\t<h1><img src=\"/images/guc.png\" /></h1>");
            sb.AppendLine("\t\t\t<ul>");

            foreach (Site s in SiteList)
                if(s.ShowInNavi)
                    s.AppendNavi(sb, permissions);

            sb.AppendLine("\t\t\t</ul>");
            sb.AppendLine("\t\t</div>");
        }

        public static void AppendFooter(StringBuilder sb)
        {
            sb.AppendLine("\t</BODY>");
            sb.AppendLine("</HTML>");
        }

        public static void AppendNoRights(StringBuilder sb)
        {
            sb.AppendLine("Du hast nicht die nötigen Zugangsrechte!");
        }



        #endregion

        private static void sendData(HttpListenerContext context){
            StringBuilder sb = new StringBuilder();
            AppendHeader(sb);
            AppendNavi(sb, mUserDict[context.User.Identity.Name].Permissions);


            foreach (Site site in SiteList)
                if (site.load(sb, mUserDict[context.User.Identity.Name].Permissions, context))
                    break;

            AppendFooter(sb);






            byte[]  responseBytes = Encoding.UTF8.GetBytes(sb.ToString());

            context.Response.OutputStream.Write(responseBytes, 0, responseBytes.Length);
            context.Response.Close();
        }

        private static bool checkUser(HttpListenerContext ctx)
        {
            if (ctx.User == null || ctx.User.Identity == null || !(ctx.User.Identity is HttpListenerBasicIdentity))
            {
                error(ctx, 401);
                return false;
            }
            HttpListenerBasicIdentity client = ctx.User.Identity as HttpListenerBasicIdentity;
            if (mUserDict.ContainsKey(client.Name) && mUserDict[client.Name].Password == client.Password)
            {
                return true;
            }
            error(ctx, 401);
            return false;
        }

        private static void error(HttpListenerContext ctx, int statusCode)
        {
            string description = "";
            byte[] descriptionBytes = Encoding.UTF8.GetBytes(description);

            ctx.Response.StatusCode = statusCode;
            ctx.Response.StatusDescription = description;
            ctx.Response.OutputStream.Write(
              descriptionBytes, 0, descriptionBytes.Length);
            ctx.Response.Close();
        }
    }
}


#endif