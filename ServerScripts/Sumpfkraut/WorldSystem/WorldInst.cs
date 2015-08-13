using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripting.Objects;

namespace GUC.Server.Scripts.Sumpfkraut.WorldSystem
{
    class WorldInst
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
        {
            // hardcoded values at first --> wehn using more worlds, change this
            this.id = 0;
            this.name = @"NEWWORLD\NEWWORLD.ZEN";
            this.world = World.getWorld(this.name);
            if (this.world == null)
            {
                Log.Logger.logError("WorldInst (constructor): There is no world named" + this.name 
                    + ", although there should be! Many systematic errors are to be exprected.");

            }
        }

    }
}
