using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinApi.User.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    public struct KeyBind
    {
        public readonly static KeyBind MoveForward = new KeyBind(VirtualKeys.W, VirtualKeys.Up);
        public readonly static KeyBind MoveLeft = new KeyBind(VirtualKeys.A);
        public readonly static KeyBind MoveRight = new KeyBind(VirtualKeys.D);
        public readonly static KeyBind MoveBack = new KeyBind(VirtualKeys.S, VirtualKeys.Down);
        public readonly static KeyBind Action = new KeyBind(VirtualKeys.Control, VirtualKeys.LeftButton);
        public readonly static KeyBind Jump = new KeyBind(VirtualKeys.Menu);
        public readonly static KeyBind Inventory = new KeyBind(VirtualKeys.Tab, VirtualKeys.I);
        public readonly static KeyBind TurnLeft = new KeyBind(VirtualKeys.Q, VirtualKeys.Left);
        public readonly static KeyBind TurnRight = new KeyBind(VirtualKeys.E, VirtualKeys.Right);
        public readonly static KeyBind DrawWeapon = new KeyBind(VirtualKeys.Space);
        public readonly static KeyBind DrawFists = new KeyBind(VirtualKeys.OEM5);

        //[JsonIgnore]
        VirtualKeys defaultKey1;
        public VirtualKeys DefaultKey1 { get { return this.defaultKey1; } }
        //[JsonIgnore]
        VirtualKeys defaultKey2;
        public VirtualKeys DefaultKey2 { get { return this.defaultKey2; } }
        VirtualKeys key1;
        public VirtualKeys Key1 { get { return this.Key1; } }
        VirtualKeys key2;
        public VirtualKeys Key2 { get { return this.Key2; } }

        public KeyBind(VirtualKeys key) : this(key, VirtualKeys.None)
        {
        }

        public KeyBind(VirtualKeys defaultKey1, VirtualKeys defaultKey2)
        {
            if (!Enum.IsDefined(typeof(VirtualKeys), defaultKey1))
                throw new ArgumentException("DefaultKey1 value is not defined in VirtualKeys: " + defaultKey1);
            if (!Enum.IsDefined(typeof(VirtualKeys), defaultKey2))
                throw new ArgumentException("DefaultKey2 value is not defined in VirtualKeys: " + defaultKey2);

            this.defaultKey1 = defaultKey1;
            this.defaultKey2 = defaultKey2;

            this.key1 = defaultKey1;
            this.key2 = defaultKey2;
        }

        public void Set(VirtualKeys key)
        {
            if (!Enum.IsDefined(typeof(VirtualKeys), key))
                throw new ArgumentException("Key value is not defined in VirtualKeys: " + key);

            if (key1 == key)
                return;

            key2 = key1;
            key1 = key;
        }

        public bool Contains(VirtualKeys key)
        {
            return key1 == key || key2 == key;
        }

        public bool IsPressed()
        {
            return InputHandler.IsPressed(key1) || InputHandler.IsPressed(key2);
        }

        public void Reset()
        {
            this.key1 = this.defaultKey1;
            this.key2 = this.defaultKey2;
        }
    }
}
