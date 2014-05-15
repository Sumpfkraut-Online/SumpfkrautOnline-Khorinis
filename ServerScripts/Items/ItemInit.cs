using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripts.Items.Potions;
using GUC.Server.Scripts.Items.Potions.Health;
using GUC.Server.Scripts.Items.Potions.Mana;
using GUC.Server.Scripts.Items.Plants;

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
            ITPO_ADDON_GEIST_01.get();
            ITPO_ADDON_GEIST_02.get();

            //Scrolls:
            ITSC_SHRINK.get();
            ITSC_TRFSHEEP.get();

            //Plants:
            ITPL_BEET.get();
            ITPL_BLUEPLANT.get();
            ITPL_FORESTBERRY.get();
            ITPL_HEALTH_HERB_01.get();
            ITPL_HEALTH_HERB_02.get();
            ITPL_HEALTH_HERB_03.get();
            ITPL_MANA_HERB_01.get();
            ITPL_MANA_HERB_02.get();
            ITPL_MANA_HERB_03.get();
            ITPL_MUSHROOM_01.get();
            ITPL_MUSHROOM_02.get();
            ITPL_PERM_HERB.get();
            ITPL_PLANEBERRY.get();
            ITPL_SPEED_HERB_01.get();
            ITPL_SWAMPHERB.get();
            ITPL_TERMP_HERB.get();
            ITPL_WEED.get();
        }
    }
}
