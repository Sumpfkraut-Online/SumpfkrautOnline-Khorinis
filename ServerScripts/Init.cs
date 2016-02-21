using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using GUC.Log;
using GUC.Scripting;
using GUC.Server.Network;
using GUC.Network;
using GUC.Scripts.Sumpfkraut.VobSystem.Definitions;

namespace GUC.Scripts
{
    public class Init : ScriptInterface
    {
		public Init()
		{
            Logger.Log("######## Initalise SumpfkrautOnline ServerScripts #########");

            AddSomeDefs();

            Logger.Log("######################## Finished #########################");
		}

        void AddSomeDefs()
        {
            VobDef vobDef = new VobDef("baum");
            vobDef.Visual = "NW_Nature_BigTree_356P.3ds";
            vobDef.Create();

            ItemDef itemDef = new ItemDef("apple");
            itemDef.Name = "Apfel";
            itemDef.Visual = "ItFo_Apple.3ds";
            itemDef.Create();

            NPCDef npcDef = new NPCDef("player");
            npcDef.Name = "Spieler";
            npcDef.Visual = "humans.mds";
            npcDef.BodyMesh = Enumeration.HumBodyMeshs.HUM_BODY_NAKED0.ToString();
            npcDef.BodyTex = (int)Enumeration.HumBodyTexs.G1Hero;
            npcDef.HeadMesh = Enumeration.HumHeadMeshs.HUM_HEAD_PONY.ToString();
            npcDef.HeadTex = (int)Enumeration.HumHeadTexs.Face_N_Player;
            npcDef.Create();
        }

        public void OnReadMenuMsg(Client client, PacketReader steam)
        {
            Logger.Log("Login!");

            
        }

        public void OnReadIngameMsg(Client client, PacketReader steam) { }

        public bool OnClientValidation(Client client)
        {
            return true; // is not banned
        }
    }
}
