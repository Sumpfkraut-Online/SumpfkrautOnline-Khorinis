-- phpMyAdmin SQL Dump
-- version 4.1.6
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Erstellungszeit: 10. Aug 2014 um 23:52
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
-- Tabellenstruktur für Tabelle `cr_reqitems`
--

CREATE TABLE IF NOT EXISTS `cr_reqitems` (
  `crafting_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  `cnt` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`crafting_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `cr_reqitems`
--

INSERT INTO `cr_reqitems` (`crafting_id`, `item_id`, `cnt`) VALUES
(8, 2, 5),
(8, 3, 2),
(9, 2, 5),
(9, 3, 2),
(10, 159, 1),
(11, 160, 1),
(12, 162, 1),
(13, 202, 1),
(14, 202, 1),
(15, 159, 1),
(16, 68, 4),
(17, 69, 4),
(18, 66, 4),
(19, 202, 1),
(21, 75, 1),
(21, 202, 1),
(22, 2, 1),
(22, 202, 1),
(23, 202, 1),
(24, 202, 1),
(25, 202, 1),
(26, 159, 1),
(26, 202, 1),
(27, 159, 1),
(27, 202, 1),
(28, 62, 2),
(28, 63, 2),
(29, 73, 3),
(30, 48, 3),
(31, 62, 2),
(31, 63, 2),
(32, 48, 1),
(33, 359, 3),
(34, 10, 3);

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `cr_reqitems`
--
ALTER TABLE `cr_reqitems`
  ADD CONSTRAINT `cr_reqitems_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_reqitems_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
