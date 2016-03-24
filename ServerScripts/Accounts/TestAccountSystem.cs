using GUC.Network;
using GUC.Server.Network.Messages;
using GUC.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts.Accounts
{
    class TestAccountSystem : ExtendedObject
    {

        public TestAccountSystem ()
        {
            AccountMessage.OnCreateAccount = CreateAccount;
            AccountMessage.OnCreateCharacter = CreateCharacter;
            AccountMessage.OnLoginAccount = LoginAccount;
            AccountMessage.OnGetCharacters = GetCharacters;
        }

        public int CreateAccount (string name, string pw)
        {
            return 0;
        }

        private bool CreateCharacter (int accID, AccCharInfo ci)
        {
            return true;
        }

        private AccCharInfo[] GetCharacters(int accID)
        {
            AccCharInfo[] chars = new AccCharInfo[1];
            chars[0] = new AccCharInfo();
            chars[0].Name = "TESTIFIX";
            chars[0].BodyMesh = 0;
            chars[0].BodyTex = 2;
            chars[0].HeadMesh = 4;
            chars[0].HeadTex = 121;
            chars[0].Fatness = 1.0f;
            chars[0].BodyHeight = 0.9f;
            chars[0].BodyWidth = 1.1f;
            //chars[i].Voice = Convert.ToInt32(res[0][i][10]);
            chars[0].FormerClass = 0;
            //ci.posx = Convert.ToSingle(res[0][i][12]);
            //ci.posy = Convert.ToSingle(res[0][i][13]);
            //ci.posz = Convert.ToSingle(res[0][i][14]);
            chars[0].SlotNum = 0;
            return chars;             
        }

        private int LoginAccount (string name, string pw)
        {
            return 0;
        }

    }
}
