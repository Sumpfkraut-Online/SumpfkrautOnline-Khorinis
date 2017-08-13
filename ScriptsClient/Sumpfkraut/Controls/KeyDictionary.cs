using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using WinApi.User.Enumeration;

namespace GUC.Scripts.Sumpfkraut.Controls
{
    class KeyDictionary : IEnumerable
    {
        public delegate void KeyPressHandler(bool down);

        Dictionary<VirtualKeys, KeyPressHandler> keys = new Dictionary<VirtualKeys, KeyPressHandler>();
        Dictionary<KeyBind, KeyPressHandler> keyBinds = new Dictionary<KeyBind, KeyPressHandler>();

        public void Add(VirtualKeys key, KeyPressHandler action)
        {
            if (key == VirtualKeys.None || !Enum.IsDefined(typeof(VirtualKeys), key))
                return;

            keys.Add(key, action);
        }
        
        public void Add(VirtualKeys key1, VirtualKeys key2, KeyPressHandler action)
        {
            Add(key1, action);
            Add(key2, action);
        }

        public void Add(KeyBind bind, KeyPressHandler action)
        {
            keyBinds.Add(bind, action);
            bind.OnKeyChanged += Bind_OnKeyChanged;
            foreach (var key in bind.Keys)
                Add(key, action);

        }

        void Bind_OnKeyChanged(KeyBind keyBind, VirtualKeys key, bool added)
        {
            KeyPressHandler action;
            if (!keyBinds.TryGetValue(keyBind, out action))
                return;

            if (added) Add(key, action);
            else Remove(key);
        }

        public bool Remove(VirtualKeys key)
        {
            return keys.Remove(key);
        }

        public bool Remove(KeyBind bind)
        {
            if (!keyBinds.Remove(bind))
                return false;

            bind.OnKeyChanged -= Bind_OnKeyChanged;
            foreach (var key in bind.Keys)
                Remove(key);

            return true;
        }
        
        public bool TryCall(VirtualKeys key, bool down)
        {
            KeyPressHandler action;
            if (!keys.TryGetValue(key, out action))
                return false;
            action(down);
            return true;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
