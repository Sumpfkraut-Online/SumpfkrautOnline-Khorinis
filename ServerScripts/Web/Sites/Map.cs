using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;

namespace GUC.Server.Scripts.Web.Sites
{
    public class Map : Site
    {
        public override string HTMLName
        {
            get { return "map.html"; }
        }

        public override string TitleName
        {
            get { return "Karte"; }
        }

        public override Permissions Permissions
        {
            get { return Web.Permissions.ShowUserMap; }
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


            Actions.GetPlayerListAction gpla = new Actions.GetPlayerListAction();
            ActionTimer.get().addAction(gpla);

            while (!gpla.IsFinished)
                Thread.Sleep(50);



            int l = -28000, t = 50500, r = 95500, b = -42500;
            int w = 1024, h = 768;

            sb.AppendLine("<h2>Karte</h2>");
            sb.AppendLine("\t\t<div class=\"content\">");
            sb.AppendLine("\t\t<div style=\"position:relative;\">");
            sb.AppendLine("\t\t\t<img src=\"/images/Map_NewWorld.jpg\" /><br>");




            foreach (GUC.Server.Scripts.Web.Actions.GetPlayerListAction.NPCCOPYED npc in gpla.List)
            {
                int _maxWidth = r - l;
                int _maxHeight = b - t;
                float _realWidth = npc.position.X - l;
                float _realHeight = npc.position.Z - t;

                int _posX = (int)(_realWidth / _maxWidth * w);
                int _posY = (int)(_realHeight / _maxHeight * h);

                if (_posY < 0)
                    _posY = 0;

                String color = "green";
                if (npc.IsPlayer)
                    color = "red";

                sb.AppendLine("\t\t<div style=\"position:absolute; width: 5px; height: 5px; background: " + color + "; top: " + _posY + "px; left: " + _posX + "px;\"> </div>");
            }
            sb.AppendLine("\t\t</div>");
            sb.AppendLine("\t\t</div>");

            return true;
        }
    }
}
