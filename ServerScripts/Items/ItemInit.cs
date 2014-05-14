using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripts.Items.Potions;
using GUC.Server.Scripts.Items.Potions.Health;
using GUC.Server.Scripts.Items.Potions.Mana;

namespace GUC.Server.Scripts.Items
{
    public static class ItemInit
    {
        /// <summary>
        /// Die .get-Methode muss von den Items aufgerufen werden, damit die Instanz erstellt und
        /// über ItemInstace.getInstance abgefragt werden kann.
        /// Dieses aufrufen erledigen wir hier.
        /// 
        /// </summary>
        public static void Init()
        {
            //Potions:
            //Health:
            ITPO_HEALTH_01.get();
            ITPO_HEALTH_02.get();
            ITPO_HEALTH_03.get();
            ITPO_HEALTH_ADDON_04.get();
            //Mana:
            ITPO_MANA_01.get();
            ITPO_MANA_02.get();
            ITPO_MANA_03.get();
            ITPO_MANA_ADDON_04.get();
            //Perm:
            ITPO_PERM_DEX.get();
            ITPO_PERM_STR.get();
            ITPO_PERM_HEALTH.get();
            ITPO_PERM_MANA.get();
            ITPO_MEGADRINK.get();

            ITPO_SPEED.get();

            //Scrolls:
            ITSC_SHRINK.get();
            ITSC_TRFSHEEP.get();
        }
    }
}
