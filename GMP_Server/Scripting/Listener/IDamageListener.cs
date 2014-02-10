using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface IDamageListener
    {
        int OnDamageReceived(ref int attackID, ref int victimID, ref byte DamageMode, ref byte WeaponMode, byte itemType, String weapon );
    }
}
