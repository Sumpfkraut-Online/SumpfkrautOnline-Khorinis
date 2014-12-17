using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GUC.Server.Scripts.Items.Potions;
using GUC.Server.Scripts.Items.Potions.Health;
using GUC.Server.Scripts.Items.Potions.Mana;
using GUC.Server.Scripts.Items.Plants;
using GUC.Server.Scripts.Items.Amulet;
using GUC.Server.Scripts.Items.Belts;
using GUC.Server.Scripts.Items.Rings;
using GUC.Server.Scripts.Items.Armors;
using GUC.Server.Scripts.Items.Food;
using GUC.Server.Scripts.Items.Keys;
using GUC.Server.Scripts.Items.Misc;
using GUC.Server.Scripts.Items.Weapons.Melee;
using GUC.Server.Scripts.Items.Scrolls;

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
            
            //Scrolls:
            ITSC_SHRINK.get();
            ITSC_TRFSHEEP.get();
            ITSC_FIREBOLT.get();
            ITSC_WINDFIST.get();

            ITMW_1H_BAU_MACE.get();

            #region Amulet
            ITAM_ADDON_FRANCO.get();
            ITAM_ADDON_HEALTH.get();
            ITAM_ADDON_MANA.get();
            ITAM_ADDON_STR.get();

            ITAM_DEX_01.get();
            ITAM_DEX_STRG_01.get();
            ITAM_HP_01.get();
            ITAM_HP_MANA_01.get();
            ITAM_MANA_01.get();
            ITAM_PROT_EDGE_01.get();
            ITAM_PROT_FIRE_01.get();
            ITAM_PROT_MAGIC_01.get();
            ITAM_PROT_POINT_01.get();
            ITAM_PROT_TOTAL_01.get();

            ITAM_STRG_01.get();
            #endregion

            #region Armors
            ITAR_BARKEEPER.get();
            ITAR_BAU_L.get();
            ITAR_BAU_M.get();
            ITAR_BAUBABE_L.get();
            ITAR_BAUBABE_M.get();
            ITAR_BDT_H.get();
            ITAR_BDT_M.get();
            ITAR_BLOODWYN_ADDON.get();
            ITAR_CORANGAR.get();
            ITAR_DEMENTOR.get();
            ITAR_DIEGO.get();
            ITAR_DJG_BABE.get();
            ITAR_DJG_CRAWLER.get();
            ITAR_DJG_H.get();
            ITAR_DJG_L.get();
            ITAR_DJG_M.get();
            ITAR_FAKE_RANGER.get();
            ITAR_FIREARMOR_ADDON.get();
            ITAR_GOVERNOR.get();
            ITAR_JUDGE.get();
            ITAR_KDF_H.get();
            ITAR_KDF_L.get();
            ITAR_KDW_H.get();
            ITAR_KDW_L_ADDON.get();
            ITAR_LEATHER_L.get();
            ITAR_LESTER.get();
            ITAR_MAYAZOMBIE_ADDON.get();
            ITAR_MIL_L.get();
            ITAR_MIL_M.get();
            ITAR_NOV_L.get();
            ITAR_OREBARON_ADDON.get();
            ITAR_PAL_H.get();
            ITAR_PAL_M.get();
            ITAR_PIR_H_ADDON.get();
            ITAR_PIR_L_ADDON.get();
            ITAR_PIR_M_ADDON.get();
            ITAR_PRISONER.get();
            ITAR_RANGER_ADDON.get();
            ITAR_RAVEN_ADDON.get();
            ITAR_SLD_H.get();
            ITAR_SLD_L.get();
            ITAR_SLD_M.get();
            ITAR_SMITH.get();
            ITAR_THROUS_ADDON.get();
            ITAR_VLK_H.get();
            ITAR_VLK_L.get();
            ITAR_VLK_M.get();
            ITAR_VLKBABE_H.get();
            ITAR_VLKBABE_L.get();
            ITAR_VLKBABE_M.get();
            ITAR_XARDAS.get();
            #endregion

            #region Belts
            ITBE_ADDON_DEX_10.get();
            ITBE_ADDON_DEX_5.get();
            ITBE_ADDON_KDF_01.get();
            ITBE_ADDON_LEATHER_01.get();
            ITBE_ADDON_MC.get();
            ITBE_ADDON_MIL_01.get();
            ITBE_ADDON_NOV_01.get();
            ITBE_ADDON_PROT_EDGE.get();
            ITBE_ADDON_PROT_EDGPOI.get();
            ITBE_ADDON_PROT_FIRE.get();
            ITBE_ADDON_PROT_MAGIC.get();
            ITBE_ADDON_PROT_POINT.get();
            ITBE_ADDON_PROT_TOTAL.get();
            ITBE_ADDON_STR_10.get();
            ITBE_ADDON_STR_5.get();
            #endregion

            #region Food
            ITFO_ADDON_FIRESTEW.get();
            ITFO_ADDON_GROG.get();
            ITFO_ADDON_LOUSHAMMER.get();
            ITFO_ADDON_MEATSOUP.get();
            ITFO_ADDON_PFEFFER_01.get();
            ITFO_ADDON_RUM.get();
            ITFO_ADDON_SCHLAFHAMMER.get();
            ITFO_ADDON_SCHNELLERHERING.get();
            ITFO_ADDON_SHELLFLESH.get();
            ITFO_APPLE.get();
            ITFO_BACON.get();
            ITFO_BEER.get();
            ITFO_BOOZE.get();
            ITFO_BREAD.get();
            ITFO_CHEESE.get();
            ITFO_CORAGONSBEER.get();
            ITFO_FISH.get();
            ITFO_FISHSOUP.get();
            ITFO_HONEY.get();
            ITFO_MILK.get();
            ITFO_SAUSAGE.get();
            ITFO_STEW.get();
            ITFO_WATER.get();
            ITFO_WINE.get();
            ITFO_XPSTEW.get();
            ITFOMUTTON.get();
            ITFOMUTTONRAW.get();
            
            #endregion

            #region Keys
            ITKE_ADDON_BUDDLER_01.get();
            ITKE_ADDON_ESTEBAN_01.get();
            ITKE_ADDON_SKINNER.get();
            ITKE_ADDON_TAVERN_01.get();
            ITKE_ADDON_THROUS.get();
            ITKE_CANYONLIBRARY_HIERARCHY_BOOKS_ADDON.get();
            ITKE_CITY_TOWER_01.get();
            ITKE_CITY_TOWER_02.get();
            ITKE_CITY_TOWER_03.get();
            ITKE_CITY_TOWER_04.get();
            ITKE_CITY_TOWER_05.get();
            ITKE_CITY_TOWER_06.get();
            ITKE_GREG_ADDON_MIS.get();
            ITKE_KEY_01.get();
            ITKE_KEY_02.get();
            ITKE_KEY_03.get();
            ITKE_LOCKPICK.get();
            ITKE_ORLAN_TELEPORTSTATION.get();
            ITKE_PORTALTEMPELWALKTHROUGH_ADDON.get();
            

            #endregion

            #region Misc
            ITMI_APFELTABAK.get();
            ITMI_AQUAMARINE.get();
            ITMI_BLOODCUP_MIS.get();
            ITMI_BROOM.get();
            ITMI_BRUSH.get();
            ITMI_COAL.get();
            ITMI_DARKPEARL.get();
            ITMI_DOPPELTABAK.get();
            ITMI_FLASK.get();
            ITMI_GOLD.get();
            ITMI_GOLDCANDLEHOLDER.get();
            ITMI_GOLDCHALICE.get();
            ITMI_GOLDCHEST.get();
            ITMI_GOLDCUP.get();
            ITMI_GOLDNECKLACE.get();
            ITMI_GOLDPLATE.get();
            ITMI_GOLDRING.get();
            ITMI_HAMMER.get();
            ITMI_HOLYWATER.get();
            ITMI_HONIGTABAK.get();
            ITMI_INNOSSTATUE.get();
            ITMI_JEWELERYCHEST.get();
            ITMI_JOINT.get();
            ITMI_LUTE.get();
            ITMI_NUGGET.get();
            ITMI_OLDCOIN.get();
            ITMI_PACKET.get();
            ITMI_PAN.get();
            ITMI_PANFULL.get();
            ITMI_PILZTABAK.get();
            ITMI_PITCH.get();
            ITMI_PLIERS.get();
            ITMI_POCKET.get();
            ITMI_QUARTZ.get();
            ITMI_ROCKCRYSTAL.get();
            ITMI_RUNEBLANK.get();
            ITMI_SAW.get();
            ITMI_SCOOP.get();
            ITMI_SEXTANT.get();
            ITMI_SILVERCANDLEHOLDER.get();
            ITMI_SILVERCHALICE.get();
            ITMI_SILVERCUP.get();
            ITMI_SILVERNECKLACE.get();
            ITMI_SILVERPLATE.get();
            ITMI_SILVERRING.get();
            ITMI_STOMPER.get();
            ITMI_SULFUR.get();
            ITMI_SUMPFTABAK.get();
            ITMISWORDBLADE.get();
            ITMISWORDBLADEHOT.get();
            ITMISWORDRAW.get();
            ITMISWORDRAWHOT.get();
            #endregion

            #region Plants
            //Plants:
            ITPL_BEET.get();
            ITPL_BLUEPLANT.get();
            ITPL_DEX_HERB_01.get();
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
            ITPL_STRENGTH_HERB_01.get();
            ITPL_SWAMPHERB.get();
            ITPL_TEMP_HERB.get();
            ITPL_WEED.get();
            #endregion

            #region Potions
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
            #endregion

            #region Rings
            ITRI_ADDON_HEALTH_01.get();
            ITRI_ADDON_HEALTH_02.get();
            ITRI_ADDON_MANA_01.get();
            ITRI_ADDON_MANA_02.get();
            ITRI_ADDON_STR_01.get();
            ITRI_ADDON_STR_02.get();
            ITRI_DEX_01.get();
            ITRI_DEX_02.get();
            ITRI_DEX_STRG_01.get();
            ITRI_HP_01.get();
            ITRI_HP_02.get();
            ITRI_HP_MANA_01.get();
            ITRI_MANA_01.get();
            ITRI_MANA_02.get();
            ITRI_PROT_EDGE_01.get();
            ITRI_PROT_EDGE_02.get();
            ITRI_PROT_FIRE_01.get();
            ITRI_PROT_FIRE_02.get();
            ITRI_PROT_MAGE_01.get();
            ITRI_PROT_MAGE_02.get();
            ITRI_PROT_POINT_01.get();
            ITRI_PROT_POINT_02.get();
            ITRI_PROT_TOTAL_01.get();
            ITRI_PROT_TOTAL_02.get();
            ITRI_STR_01.get();
            ITRI_STR_02.get();
            
            #endregion
        }
    }
}
