using System;
using System.Collections.Generic;
using System.Text;
using Gothic.mClasses;
using Injection;
using Network;
using RakNet;

namespace GMP.Network.Messages
{
    public class PlayerKeyMessage : InputReceiver
    {
        class Key{
            public int key;
            public byte times;
            public bool pressed;
        }
        private int changedSteps = 0;
        private Dictionary<int, Key> keyList = new Dictionary<int, Key>();

        public PlayerKeyMessage()
        {
            InputHooked.receivers.Add(this);
        }

        public void KeyReleased(int key)
        {
            if (keyList.ContainsKey(key))
            {
                keyList[key].pressed = false;
                keyList[key].times += 1;
            }
            else
            {
                Key k = new Key();
                k.key = key;
                k.times = 1;
                k.pressed = false;

                keyList.Add(key, k);
            }
        }

        public void KeyPressed(int key)
        {
            if (keyList.ContainsKey(key))
            {
                keyList[key].pressed = true;
            }
            else
            {
                Key k = new Key();
                k.key = key;
                k.times = 0;
                k.pressed = true;

                keyList.Add(key, k);
            }
        }

        public void wheelChanged(int steps) {
            changedSteps += steps;
        }

        private long lastUpdateTime = 0;

        public void update()
        {
            long now = DateTime.Now.Ticks;

            if (lastUpdateTime + 10000 * 1000 > now)
                return;

            if (this.keyList.Count == 0)
            {
                lastUpdateTime = now;
                return;
            }

            RakNet.BitStream stream = Program.client.sentBitStream;
            stream.Reset();
            stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
            stream.Write((byte)NetWorkIDS.PlayerKeyMessage);

            stream.Write(Program.Player.id);
            stream.Write((short)this.keyList.Count);
            foreach (KeyValuePair<int, Key> pair in this.keyList)
            {
                stream.Write(pair.Value.key);
                stream.Write(pair.Value.pressed);
                stream.Write(pair.Value.times);
            }
            this.keyList.Clear();

            lastUpdateTime = now;
            Program.client.client.Send(stream, PacketPriority.IMMEDIATE_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);
        }


        //public void Write(RakNet.BitStream stream, Client client, byte type, Player player, int id, int value, float value2)
        //{
        //    stream.Reset();
        //    stream.Write((byte)DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
        //    stream.Write((byte)NetWorkIDS.AnimationMessage);

        //}
    }
}
