using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Scripting;

namespace GUC.Scripts.Arena
{
    class ScoreBoard
    {
        const long UpdateInterval = 1000 * TimeSpan.TicksPerMillisecond;

        ScriptMessages msgID;
        Dictionary<ArenaClient, GUCTimer> clients;
        GUCTimer packetTimer;
        byte[] packet;

        Func<ArenaClient, bool> selector;

        public ScoreBoard(ScriptMessages messageID, Func<ArenaClient, bool> selector = null)
        {
            msgID = messageID;
            this.selector = selector ?? new Func<ArenaClient, bool>(c => true);
            clients = new Dictionary<ArenaClient, GUCTimer>(20);
            packetTimer = new GUCTimer(UpdateInterval, WriteUpdate);
            packetTimer.Start();
            
            WriteUpdate();
        }

        public void Toggle(ArenaClient client)
        {
            GUCTimer timer;
            if (!clients.TryGetValue(client, out timer))
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

        void SendUpdate(ArenaClient client)
        {
            client.SendScriptMessage(packet, packet.Length, NetPriority.Low, NetReliability.Unreliable);
        }

        void WriteUpdate()
        {
            var stream = ArenaClient.GetScriptMessageStream();
            stream.Write((byte)msgID);
            stream.Write((byte)ArenaClient.GetCount());
            ScoreBoardItem info = new ScoreBoardItem();
            ArenaClient.ForEach(c =>
            {
                var client = (ArenaClient)c;
                if (selector(client))
                {
                    info.Fill(client);
                    info.Write(stream);
                }
            });

            this.packet = stream.CopyData();
        }
    }
}
