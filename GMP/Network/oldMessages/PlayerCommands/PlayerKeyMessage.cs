using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gothic.mClasses;
using GUC.WorldObjects.Character;
using RakNet;
using GUC.Enumeration;
using Gothic.zClasses;
using WinApi;

namespace GUC.Network.Messages.PlayerCommands
{
    class PlayerKeyMessage : InputReceiver
    {
        Dictionary<byte, byte> keys = new Dictionary<byte, byte>();

        private PlayerKeyMessage()
        {

        }

        private static PlayerKeyMessage _self = null;
        public static PlayerKeyMessage getPlayerKeyMessage()
        {
            if (_self == null)
                _self = new PlayerKeyMessage();
            return _self;
        }

        public void Init()
        {
            InputHooked.receivers.Add(this);
        }
        private long last = 0;
        public void update()
        {

            if (last == 0)
            {
                last = DateTime.Now.Ticks;
                return;
            }

            long now = DateTime.Now.Ticks;
            if (last + 10000 * 500 < now)
            {
                if (keys.Count == 0)
                {
                    keys.Clear();
                    last = now;
                    return;
                }

                BitStream stream = Program.client.sentBitStream;
                stream.Reset();
                stream.Write((byte)RakNet.DefaultMessageIDTypes.ID_USER_PACKET_ENUM);
                stream.Write((byte)NetworkID.PlayerKeyMessage);
                stream.Write(Player.Hero.ID);
                stream.Write((byte)keys.Count);
                foreach (KeyValuePair<byte, byte> pair in keys)
                {
                    stream.Write(pair.Key);
                    stream.Write(pair.Value);
                }
                Program.client.client.Send(stream, PacketPriority.HIGH_PRIORITY, PacketReliability.RELIABLE_ORDERED, (char)0, RakNet.RakNet.UNASSIGNED_SYSTEM_ADDRESS, true);

                

                keys.Clear();
                last = now;
            }
        }

        public void KeyReleased(int key)
        {
            if (Cursor.noHandle)
            {
                return;
            }
            
            //if (!Player.sSendAllKeys && !Player.sSendKeys.Contains((byte)key))
            //    return;
            if (keys.ContainsKey((byte)key))
            {
                keys[(byte)key]++;
            }
            else
            {
                keys.Add((byte)key, 0);
            }
        }

        public void KeyPressed(int key)
        {
            if (Cursor.noHandle)
            {
                return;
            }


            //if (!Player.sSendAllKeys && !Player.sSendKeys.Contains((byte)key))
            //    return;
            if (keys.ContainsKey((byte)key))
            {
                keys[(byte)key]++;
            }
            else
            {
                keys.Add((byte)key, 1);
            }
        }

        public void wheelChanged(int steps)
        {
            
        }
    }
}
