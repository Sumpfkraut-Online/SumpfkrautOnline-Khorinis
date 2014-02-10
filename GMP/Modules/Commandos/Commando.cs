using System;
using System.Collections.Generic;
using System.Text;
using Network;
using Injection;
using GMP.Network.Messages;

namespace Modules
{
    /// <summary>
    /// Die Commando-Klasse soll die restlichen GMP-Funktionen zugänglicher machen, um einfache Funktionen zur Synchronisierung zu bieten.
    /// </summary>
    public static class Commando
    {
        /// <summary>
        /// Gibt einem bestimmten ein Item ins Inventar.
        /// </summary>
        /// <param name="pl">Der Spieler, dem ein Item hinzugefügt werden soll</param>
        /// <param name="item">Item-Instance</param>
        /// <param name="amount">Anzahl der Items</param>
        public static void GiveItem(Player pl, String item, int amount)
        {
            CommandoMessage.GiveItem(pl.id, item, amount);
        }

        /// <summary>
        /// Setzt die unter pl.itemList eingetragene Item-Liste als Spielerinventar und synchronisiert diesen mit anderen Spielern.
        /// </summary>
        /// <param name="pl">Spieler-Klasse</param>
        public static void SetInventory(Player pl)
        {

        }

        /// <summary>
        /// Achtung: Nur anzuwenden, wenn der Spieler noch nicht tatsächlich auf dem Server ist, also unter StartModules.
        /// </summary>
        /// <param name="world"> Die Welt die gesetzt werden muss.</param>
        public static void SetStartMap( String world)
        {

        }

        public static void SetTalent(Player pl, int talendid, int talentvalue, int talentskill)
        {
            CommandoMessage.SetTalents(pl.id, talendid, talentvalue, talentskill);
        }


    }
}
