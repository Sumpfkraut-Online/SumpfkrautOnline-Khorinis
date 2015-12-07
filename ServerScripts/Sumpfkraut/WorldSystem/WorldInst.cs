using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.WorldObjects;

namespace GUC.Server.Scripts.Sumpfkraut.WorldSystem
{
    public class WorldInst
    {

        protected int id = -1;
        public int getID () { return this.id; }
        public void setID (int id) { this.id = id; }

        private string name = "";
        public string getName () { return this.name; }
        public void setName (string name) { this.name = name; }

        private World world;
        public World getWorld () { return this.world; }
        public void setWorld (World world) { this.world = world; }



        public WorldInst ()
            : this(-1, WorldHandler.defaultWorldName)
        { }

        public WorldInst (int id, string name)
        {
            this.id = id;
            this.name = name;
            //this.world = World.getWorld(this.name);
            this.world = Network.Server.GetWorld("newworld");
            if (this.world == null)
            {
                Log.Logger.logError("WorldInst (constructor): There is no world named" + this.name 
                    + ", although there should be! Many systematic errors are to be exprected.");

            }
        }

    }
}
