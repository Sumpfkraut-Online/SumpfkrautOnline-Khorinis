using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;
using GUC.Log;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    public class KeyBind
    {
        #region Static Dictionary

        static readonly Dictionary<VirtualKeys, KeyBind> allKeys = new Dictionary<VirtualKeys, KeyBind>();

        public static bool TryGetKeyBind(VirtualKeys key, out KeyBind bind)
        {
            return allKeys.TryGetValue(key, out bind);
        }

        public static bool ContainsKey(VirtualKeys key)
        {
            return allKeys.ContainsKey(key);
        }

        #endregion

        #region Static Keys
        
        public readonly static KeyBind MoveForward = new KeyBind("MoveForward", VirtualKeys.W, VirtualKeys.Up);
        public readonly static KeyBind MoveLeft = new KeyBind("MoveLeft", VirtualKeys.A);
        public readonly static KeyBind MoveRight = new KeyBind("MoveRight", VirtualKeys.D);
        public readonly static KeyBind MoveBack = new KeyBind("MoveBack", VirtualKeys.S, VirtualKeys.Down);
        public readonly static KeyBind Action = new KeyBind("Action", VirtualKeys.Control, VirtualKeys.LeftButton);
        public readonly static KeyBind Jump = new KeyBind("Jump", VirtualKeys.Menu);
        public readonly static KeyBind Inventory = new KeyBind("Inventory", VirtualKeys.Tab, VirtualKeys.I);
        public readonly static KeyBind TurnLeft = new KeyBind("TurnLeft", VirtualKeys.Q, VirtualKeys.Left);
        public readonly static KeyBind TurnRight = new KeyBind("TurnRight", VirtualKeys.E, VirtualKeys.Right);
        public readonly static KeyBind DrawWeapon = new KeyBind("DrawWeapon", VirtualKeys.Space);
        public readonly static KeyBind DrawFists = new KeyBind("DrawFists", VirtualKeys.OEM5);
<<<<<<< HEAD
        public readonly static KeyBind OpenAllChat = new KeyBind("OpenAllChat", VirtualKeys.T, VirtualKeys.Return);
        public readonly static KeyBind OpenTeamChat = new KeyBind("OpenTeamChat", VirtualKeys.Z);
        public readonly static KeyBind OpenScoreBoard = new KeyBind("OpenScoreBoard", VirtualKeys.F1);
        public readonly static KeyBind RequestTrade = new KeyBind("RequestTrade", VirtualKeys.Y);
=======
        public readonly static KeyBind ChatAll = new KeyBind("ChatAll", VirtualKeys.T, VirtualKeys.Return);
        public readonly static KeyBind ChatTeam = new KeyBind("ChatTeam", VirtualKeys.Z);
        public readonly static KeyBind ScoreBoard = new KeyBind("ScoreBoard", VirtualKeys.F1);
        public readonly static KeyBind StatusMenu = new KeyBind("StatusMenu", VirtualKeys.B, VirtualKeys.C);
>>>>>>> ShodenTestBranch

        #endregion

        #region Properties

        string name;
        public string Name { get { return this.name; } }
        List<VirtualKeys> defaultKeys = new List<VirtualKeys>();
        public IEnumerable<VirtualKeys> DefaultKeys { get { return defaultKeys; } }
        List<VirtualKeys> keys = new List<VirtualKeys>();
        public IEnumerable<VirtualKeys> Keys { get { return keys; } }

        /// <summary> Returns VirtualKeys.None if index is out of range. </summary>
        public VirtualKeys GetKey(int index)
        {
            return (index >= 0 && index < keys.Count) ? keys[index] : VirtualKeys.None;
        }

        /// <summary> Returns VirtualKeys.None if index is out of range. </summary>
        public VirtualKeys GetDefaultKey(int index)
        {
            return (index >= 0 && index < defaultKeys.Count) ? defaultKeys[index] : VirtualKeys.None;
        }

        public delegate void KeyChangedHandler(KeyBind keyBind, VirtualKeys key, bool added);
        public event KeyChangedHandler OnKeyChanged = null;

        #endregion

        #region Constructors

        private KeyBind(string name, params VirtualKeys[] defaultKeys)
        {
            this.name = string.IsNullOrWhiteSpace(name) ? "unnamed" : name;

            for (int i = 0; i < defaultKeys.Length; i++)
                Add(defaultKeys[i], true);
        }

        #endregion

        #region Add & Remove

        public void Add(VirtualKeys key)
        {
            Add(key, false);
        }

        void Add(VirtualKeys key, bool defaultKey)
        {
            try
            {
                if (!Enum.IsDefined(typeof(VirtualKeys), key))
                {
                    Logger.Log("KeyBind '{0}': Tried to add undefined key bind {1}.", name, key);
                    return;
                }

                if (key == VirtualKeys.None)
                {
                    Logger.Log("KeyBind '{0}': Tried to add key bind 'None'", name);
                    return;
                }

                KeyBind otherBind;
                if (allKeys.TryGetValue(key, out otherBind))
                {
                    if (otherBind == this) // key is already set
                        return;

                    // there is already a KeyBind with this key, replace it
                    otherBind.Remove(key);
                    Logger.Log("Key '{0}' of KeyBind '{1}' was replaced by KeyBind '{2}'", key, otherBind.Name, this.Name);
                }

                // add
                allKeys.Add(key, this);
                keys.Add(key);
                if (defaultKey)
                    defaultKeys.Add(key);
                else if (OnKeyChanged != null)
                    OnKeyChanged(this, key, true);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
            }
        }

        public bool Remove(VirtualKeys key)
        {
            KeyBind otherBind;
            if (!allKeys.TryGetValue(key, out otherBind) || otherBind != this)
                    return false;

            allKeys.Remove(key);
            keys.Remove(key);

            if (OnKeyChanged != null)
                OnKeyChanged(this, key, false);

            return true;
        }

        #endregion

        public void Reset()
        {
            keys.ForEach(key => Remove(key));
            defaultKeys.ForEach(key => Add(key));
        }

        public bool Contains(VirtualKeys key)
        {
            return keys.Contains(key);
        }

        public bool IsPressed()
        {
            return !keys.TrueForAll(key => !InputHandler.IsPressed(key));
        }
    }
}
