using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface IEquipmentListener
    {
        void OnWeaponChanged(Player pl, String oldWeapon, String newWeapon);
        void OnRangeWeaponChanged(Player pl, String oldRangeWeapon, String newRangeWeapon);
        void OnArmorChanged(Player pl, String oldArmor, String newArmor);
    }
}
