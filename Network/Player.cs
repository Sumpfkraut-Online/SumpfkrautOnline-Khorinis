using System;
using System.Collections.Generic;
using System.Text;
using RakNet;
using System.Collections;

namespace Network
{
    public class item
    {
        public String code;
        public int Amount;
        public bool inInv;
    }

    public class NPC
    {
        public Player npcPlayer;
        public Player controller;
        public float[] spawn;
        public String wp;
        public bool isStatic;
        public bool isSummond;
        public bool isSpawned;
        public bool isPlayerSpawned;

        public long deadTimer;
    }

    public class MobInteract
    {
        public String name;
        public String objectName;
        public String modelName;
        public String onStateFunc;
        public float[] pos = new float[3];
    }

    public class Player
    {
        #region Compare
        public class PlayerComparer : IComparer<Player>
        {
            public int Compare(Player x, Player y)
            {
                if (x == null)
                {
                    if (y == null)
                        return 0;
                    else
                        return -1;
                }
                else
                {
                    if (y == null)
                        return 1;
                    else
                    {
                        if (x.id == y.id)
                            return 0;
                        if (y.id > x.id)
                            return -1;
                        else
                            return 1;
                    }
                }
            }
        }
        public class PlayerAddressComparer : IComparer<Player>
        {
            public int Compare(Player x, Player y)
            {
                if (x == null)
                {
                    if (y == null)
                        return 0;
                    else
                        return -1;
                }
                else
                {
                    if (y == null)
                        return 1;
                    else
                    {
                        if (x.NPCAddress == y.NPCAddress)
                            return 0;
                        if (y.NPCAddress > x.NPCAddress)
                            return -1;
                        else
                            return 1;
                    }
                }
            }
        }
        #endregion


        public enum SendTime
        {
            HP,
            HP_MAX,
            STR,
            Dex,
            itemList,
            
            Max
        }
        public Player(RakNetGUID guid, SystemAddress address)
        {
            this.mGuid = guid.g;
            this.mSystemAddress = address.ToString();
        }

        public Player(String guid)
        {
            guidStr = guid;
        }

        public RakNetGUID guid
        {
            get { return new RakNetGUID(mGuid); }
        }

        public SystemAddress systemAddress
        {
            get { return new SystemAddress(mSystemAddress); }
        }

        public void InsertItem(item item)
        {
            item Item = GetItem(item.code);
            if (Item == null)
                itemList.Add(item);
            else
                Item.Amount += item.Amount;
        }

        public void RemoveItem(item item)
        {
            item Item = GetItem(item.code);
            if (Item == null)
                return;
            if (Item.Amount - item.Amount <= 0)
                itemList.Remove(Item);
            else
                Item.Amount = Item.Amount - item.Amount;
        }

        public item GetItem(String name)
        {
            foreach (item Item in itemList)
            {
                if (Item.code.Trim().ToLower() == name.Trim().ToLower())
                    return Item;
            }
            return null;
        }

        public Dictionary<String, Object> variableDataList = new Dictionary<string,object>();

        #region Variables
        public String guidStr;
        public String mSystemAddress;
        public ulong mGuid;

        public String a;
        public String b;


        public String name;
        public String actualMap;
        public String instance = "PC_HERO";

        public String equippedArmor = "";
        public String equippedWeapon = "";
        public String equippedRangeWeapon = "";
        public String[] slots = new String[] { "", "", "", "", "", "", "", "", "" };
        public String activeSpell = "";
        
        public String[] spells;//Not used!

        public int bodyTex, headTex, voice;
        public String BodyVisual = "", HeadVisual = "";

        public float[] scale = new float[3];
        public float fatness = 1.0f;
        public MobInteract mobInteract = null;

        public int id;
        public int NPCAddress;
        public float[] pos = new float[3];
        public float[] dir = new float[3];
        public bool isPlayer;
        public bool isSpawned;
        public bool isNPC;

        public int lastHP = -1;
        public int lastHP_Max = -1;
        public int lastMP = -1;
        public int lastMP_Max = -1;
        public int lastStr = -1;
        public int lastDex = -1;
        public int[] lastHitChances = new int[4];
        public int[] lastTalentSkills = new int[22];
        public int[] lastTalentValues = new int[22];
        public bool knowName;

        public int lastAniID = -1;
        public int lastAniValue = -1;
        public String lastWeaponMode = "";
        public byte lastWeaponModeType;

        public short lastAnimation = 30000;

        public long[] lastSendet = new long[(int)SendTime.Max];

        public List<item> itemList  = new List<item>();
        public List<Object> UserData = new List<Object>();
        public List<NPC> NPCList = new List<NPC>();
        public NPC NPC = null;

        public byte isFriend;
        public bool isImmortal;
        public bool isInvisible;
        public bool isFreeze;
        public bool isMuted;

        public bool newPlayer = true;

        public Dictionary<int, int> SetableUserData = new Dictionary<int,int>();
        #endregion

        #region StaticFuncs
        public static Player getPlayer(int id, List<Player> playerList)
        {
            if (id == -1)
                return null;
            foreach (Player player in playerList)
            {
                if (player.id == id)
                    return player;
            }
            return null;
        }

        public static Player getPlayerSort(int id, List<Player> playerList)
        {
            if (id == -1)
                return null;

            Player pl = new Player("");
            pl.id = id;

            int index = playerList.BinarySearch(pl, new PlayerComparer());
            if (index < 0)
                return null;

            return playerList[index];

            //return getPlayerSort(id, playerList, false, playerList.Count);
        }

        public static Player getPlayerSortByAddress(int id, List<Player> playerList)
        {
            if (id == -1)
                return null;

            Player pl = new Player("");
            pl.NPCAddress = id;

            int index = playerList.BinarySearch(pl, new PlayerAddressComparer());
            if (index <= -1 || index >= playerList.Count)
                return null;
            return playerList[index];

            //return getPlayerSort(id, playerList, false, playerList.Count);
        }

        public static Player getPlayerByName(string id, List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                if (player.name == id)
                    return player;
            }
            return null;
        }

        public static Player getPlayerByAddress(int npcaddress, List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                if (player.NPCAddress == npcaddress)
                    return player;
            }
            return null;
        }

        public static Player getPlayerByGuid(ulong guid, List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                if (player.mGuid == guid)
                    return player;
            }
            return null;
        }

        public static Player getPlayerByGuid(String guid, List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                if (player.guidStr == guid)
                    return player;
            }
            return null;
        }

        public static Player getPlayerByGuid(RakNetGUID guid, List<Player> playerList)
        {
            foreach (Player player in playerList)
            {
                if (player.guid == guid)
                    return player;
            }
            return null;
        }

        public override string ToString()
        {
            return name;
        }

        public static bool isSameMap(String map1, String map2)
        {
            String _map1 = getMap(map1);
            String _map2 = getMap(map2);

            if (_map1 == _map2)
                return true;
            else
                return false;
        }

        public static String getMap(String map)
        {
            map = map.ToUpper().Trim();
            map = map.Replace('/', '\\');

            return map;
        }
        #endregion
    }
}
