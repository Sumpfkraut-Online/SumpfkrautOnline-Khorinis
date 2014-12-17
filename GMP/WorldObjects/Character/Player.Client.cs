using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Character
{
    internal partial class Player
    {
        protected static Player _player;
        public static Player Hero { get { return _player; } set { _player = value; } }


        protected bool isPlayer = false;

        public Player(bool isPlayer, String name)
        {
            this.isPlayer = isPlayer;
            this.Name = name;
        }

        public bool IsPlayer { get { return isPlayer; } set { isPlayer = value; } }
    }
}
