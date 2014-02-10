using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting
{
    public class ItemEntity
    {
        public enum ItemType
        {
            Inventory,
            World,
            Container
        }

        private Item item = null;
        private int id = -1;

        private static int currID;

        private int positionType;
        
    }
}
