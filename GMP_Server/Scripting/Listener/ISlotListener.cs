using System;
using System.Collections.Generic;
using System.Text;

namespace GMP_Server.Scripting.Listener
{
    public interface ISlotListener
    {
        void OnRightHandSlotChanged(Player pl, String oldItem, String newItem);
        void OnLeftHandSlotChanged(Player pl, String oldItem, String newItem);
        void OnSwordSlotChanged(Player pl, String oldItem, String newItem);
        void OnLongSwordSlotChanged(Player pl, String oldItem, String newItem);
        void OnBowSlotChanged(Player pl, String oldItem, String newItem);
        void OnCrossBowSlotChanged(Player pl, String oldItem, String newItem);
        void OnTorsoSlotChanged(Player pl, String oldItem, String newItem);
        void OnHelmetSlotChanged(Player pl, String oldItem, String newItem);
        void OnShieldSlotChanged(Player pl, String oldItem, String newItem);
    }
}
