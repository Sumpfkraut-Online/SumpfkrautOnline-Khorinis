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
        }

        public void Toggle(ArenaClient client, bool open)
        {
            if (open)
            {
                if (!clients.ContainsKey(client))
                {
                    GUCTimer timer = new GUCTimer(UpdateInterval, () => SendUpdate(client));
                    clients.Add(client, timer);
                    timer.Start();

                    if (clients.Count == 1)
                    {
                        packetTimer.Start();
                        WriteUpdate();
                    }

                    SendUpdate(client);
                }
            }
            else
            {
                Remove(client);
            }
        }

        public void Remove(ArenaClient client)
        {
            if (clients.TryGetValue(client, out GUCTimer timer))
            {
                timer.Stop();
                clients.Remove(client);

                if (clients.Count == 0)
                    packetTimer.Stop();
            }
        }

        public void RemoveAll()
        {
            foreach (GUCTimer timer in clients.Values)
                timer.Stop();
            clients.Clear();
            packetTimer.Stop();
        }

        void SendUpdate(ArenaClient client)
        {
            if (client.IsConnected)
                client.SendScriptMessage(packet, packet.Length, NetPriority.Low, NetReliability.Unreliable);
        }

        void WriteUpdate()
        {
            var stream = ArenaClient.GetStream(msgID);
            WriteBoard(stream);
            this.packet = stream.CopyData();
        }

        protected abstract void WriteBoard(PacketWriter stream);
    }
}
