using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Network;
using GUC.Types;
using GUC.Server.Sumpfkraut;

namespace GUC.WorldObjects
{
    internal partial class World
    {
        protected List<NPC> playerList = new List<NPC>();
        public List<NPC> PlayerList { get { return playerList; } }

        protected GUC.Server.Scripting.Objects.World scriptingWorld;

        public GUC.Server.Scripting.Objects.World ScriptingWorld
        {
            get
            {
                if (scriptingWorld == null)
                    scriptingWorld = new GUC.Server.Scripting.Objects.World(this);
                return scriptingWorld;
            }
        }

        public virtual void Write(BitStream stream)
        {
            stream.Write(Map);

            stream.Write(itemList.Count);
            foreach (Item item in itemList)
            {
                stream.Write(item.ID);
                stream.Write(item.Position);
                stream.Write(item.Direction);
            }

            stream.Write(this.VobList.Count);
            foreach (Vob vob in VobList)
            {
                stream.Write(vob.ID);
                stream.Write(vob.Position);
                stream.Write(vob.Direction);
            }

            stream.Write(npcList.Count);
            foreach (NPC proto in npcList)
            {
                stream.Write(proto.ID);
                stream.Write(proto.Position);
                stream.Write(proto.Direction);
            }
        }
    }
}
