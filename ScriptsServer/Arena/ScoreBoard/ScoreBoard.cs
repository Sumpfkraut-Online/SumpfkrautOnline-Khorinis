using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;
using GUC.Network;

namespace GUC.Scripts.Arena
{
    abstract class ScoreBoard
    {
        const long UpdateInterval = 1000 * TimeSpan.TicksPerMillisecond;
        
        Dictionary<ArenaClient, GUCTimer> clients;
        GUCTimer packetTimer;
        byte[] packet;

        ScriptMessages msgID;

        public ScoreBoard(ScriptMessages messageID)
        {
            msgID = messageID;
            clients = new Dictionary<ArenaClient, GUCTimer>(20);
            packetTimer = new GUCTimer(UpdateInterval, WriteUpdate);
            packetTimer.Start();

            WriteUpdate();
        }

        public void Toggle(ArenaClient client)
        {
            if (!clients.TryGetValue(client, out GUCTimer timer))
            {
                SendUpdate(client);

                timer = new GUCTimer(UpdateInterval, () => SendUpdate(client));
                clients.Add(client, timer);
                timer.Start();
            }
            else
            {
                timer.Stop();
                clients.Remove(client);
            }
        }

        public void Remove(ArenaClient client)
        {
            if (clients.TryGetValue(client, out GUCTimer timer))
            {
                timer.Stop();
                clients.Remove(client);
            }
        }

        void SendUpdate(ArenaClient client)
        {
            client.SendScriptMessage(packet, packet.Length, NetPriority.Low, NetReliability.Unreliable);
        }

        void WriteUpdate()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)msgID);
            WriteBoard(stream);
            this.packet = stream.CopyData();
        }

        protected abstract void WriteBoard(PacketWriter stream);
    }
}
