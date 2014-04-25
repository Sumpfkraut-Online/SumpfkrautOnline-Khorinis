using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.WorldObjects.Character
{
    internal partial class Player
    {
        #region Members:
        protected ulong guid;
        #endregion

        #region Propertys:
        public override ulong Guid { get { return guid; } }
        public override RakNet.RakNetGUID GUID { get { return new RakNet.RakNetGUID(guid); } }

        public String DriveString = "";
        public String MacString = "";
        #endregion


        public Player(RakNet.RakNetGUID guid, String name)
            : this()
        {
            this.guid = guid.g;
            this.Name = name;

            
        }
    }
}
