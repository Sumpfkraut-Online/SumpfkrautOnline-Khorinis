using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Scripting.Objects.Character
{
    public class Player : NPCProto
    {
        internal Player(WorldObjects.Character.Player ivob)
            : base(ivob)
        {

        }

        public static Player getHero()
        {
            if (WorldObjects.Character.Player.Hero == null)
                return null;

            return (Player)WorldObjects.Character.Player.Hero.ScriptingInstance;
            
        }
        
    }
}
