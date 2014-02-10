using System;
using System.Collections.Generic;
using System.Text;
using GMP.Net;
using Network;

namespace GMP.Network.Messages
{
    public class AutoPlayerMessage : Message
    {
        public Dictionary<int, Dictionary<byte, List<List<object>>>> SenderList = new Dictionary<int, Dictionary<byte, List<List<object>>>>();

        public interface AutoMessageReadWrite
        {
            void Write(RakNet.BitStream stream, List<object> data);
        }

        public void addMessageReadWrite(byte type, AutoMessageReadWrite amrw)
        {

        }

        public void addMessage(int playerid, byte MessageType, List<object> Data)
        {

        }


        public void Write(RakNet.BitStream stream, Client client)
        {

            foreach (KeyValuePair<int, Dictionary<byte, List<List<object>>>> pair in SenderList)
            {
                AutoPlayerMessageTypes types = AutoPlayerMessageTypes.None;
                //Alle Types erfassen
                foreach (KeyValuePair<byte, List<List<object>>> p2 in pair.Value)
                {
                    types |= (AutoPlayerMessageTypes)p2.Key;
                }
                stream.Write(pair.Key);//Spieler-ID
                stream.Write((byte)types);//Welche Funktionen überträgt der Spieler alles?

                
            }
        }
    }
}
