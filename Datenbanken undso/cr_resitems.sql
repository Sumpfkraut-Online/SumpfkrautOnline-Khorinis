-- phpMyAdmin SQL Dump
-- version 4.1.6
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Erstellungszeit: 10. Aug 2014 um 23:53
-- Server Version: 5.6.16
-- PHP-Version: 5.5.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;

--
-- Datenbank: `gothic_multiplayer`
--

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `cr_resitems`
--

CREATE TABLE IF NOT EXISTS `cr_resitems` (
  `crafting_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  `cnt` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`crafting_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `cr_resitems`
--

INSERT INTO `cr_resitems` (`crafting_id`, `item_id`, `cnt`) VALUES
(5, 3, 1),
(6, 2, 5),
(8, 159, 1),
(9, 159, 1),
(10, 160, 1),
(11, 162, 1),
(12, 202, 1),
(13, 132, 1),
(14, 79, 1),
(15, 342, 1),
(16, 194, 1),
(17, 195, 1),
(18, 191, 1),
(19, 128, 1),
(20, 75, 1),
(21, 90, 1),
(22, 86, 1),
(23, 85, 1),
(24, 80, 1),
(25, 81, 1),
(26, 98, 1),
(27, 98, 1),
(28, 45, 1),
(29, 40, 1),
(30, 39, 1),
(31, 46, 1),
(32, 359, 1),
(33, 21, 1),
(34, 11, 1);

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `cr_resitems`
--
ALTER TABLE `cr_resitems`
  ADD CONSTRAINT `cr_resitems_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_resitems_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
