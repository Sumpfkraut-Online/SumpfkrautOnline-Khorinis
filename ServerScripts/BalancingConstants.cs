using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GUC.Server.Scripts
{
    /**
     * Every constant property regarding the game balancing belongs here.
     * @see CRespawn
     */
    public class BalancingConstants
    {
        public const int RESPAWNINTERVALLMILLISECONDS = 30000; //will effect many things like plants, monsters and ore
        public enum PlantList
        {
            ItPl_Blueplant,
            ItPl_Mushroom_02,
            ItPl_Strength_Herb_01,
            ItPl_Mushroom_01,
            ItPl_Temp_Herb,
            ItPl_Beet,
            ItPl_Mana_Herb_01,
            ItPl_Mana_Herb_02,
            ItPl_Mana_Herb_03,
            ItPl_Dex_Herb_01,
            ItPl_Health_Herb_01,
            ItPl_Health_Herb_02,
            ItPl_Health_Herb_03,
            ItPl_Perm_Herb,
            ItPl_Speed_Herb_01,
            ItPl_Sagitta_Herb_Mis,
            ItPl_SwampHerb,
            ItPl_Weed,
            ItPl_Forestberry,
            ItPl_Planeberry
        }



    }
}
