using System;
using System.Collections.Generic;
using System.Text;

namespace GUC.WorldObjects.Character
{
    internal partial class Player : NPCProto
    {
        public Dictionary<World, List<String>> knownArea = new Dictionary<World, List<string>>();
        protected bool isSpawned = false;

        public List<NPCProto> NPCControlledList = new List<NPCProto>();

        public Player()
            : base()
        {
            this.VobType = Enumeration.VobTypes.Player;
        }

        public void spawned()
        {
            isSpawned = true;
        }

        public bool IsSpawned { get { return isSpawned; } }
        
        public override void StealItem(Vob other, String item, int amount) { }
        public override void StealItem(Vob other, Item item, int amount) { }
    }
}
