using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Enumeration;
using GUC.Types;
using GUC.Server.Network;
using RakNet;
using GUC.Network;

namespace GUC.Server.WorldObjects
{
    public class Vob : AbstractDropVob
    {
        public Vob(string visual)
            : this(visual, true, true)
        {
        }

        public Vob(string visual, bool cddyn, bool cdstatic)
        {
            this.visual = visual;
            this.cddyn = cddyn;
            this.cdstatic = cdstatic;
        }

        internal override void WriteSpawn(IEnumerable<Client> list)
        {
            BitStream stream = Program.server.SetupStream(NetworkID.WorldVobSpawnMessage);
            stream.mWrite(ID);
            stream.mWrite(visual);
            stream.mWrite(pos);
            stream.mWrite(dir);
            stream.mWrite(cddyn);
            stream.mWrite(cdstatic);
            stream.mWrite(physicsEnabled);

            foreach (Client client in list)
                Program.server.ServerInterface.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, client.guid, false);
        }
    }
}
