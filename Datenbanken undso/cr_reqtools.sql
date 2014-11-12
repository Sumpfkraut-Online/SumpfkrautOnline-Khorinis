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
-- Tabellenstruktur für Tabelle `cr_reqtools`
--

CREATE TABLE IF NOT EXISTS `cr_reqtools` (
  `crafting_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`crafting_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `cr_reqtools`
--

INSERT INTO `cr_reqtools` (`crafting_id`, `item_id`) VALUES
(12, 124),
(15, 124),
(22, 124),
(25, 124),
(26, 124),
(20, 128),
(5, 132),
(6, 132),
(28, 201),
(29, 201),
(30, 201),
(31, 201),
(33, 201),
(34, 201);

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `cr_reqtools`
--
ALTER TABLE `cr_reqtools`
  ADD CONSTRAINT `cr_reqtools_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_reqtools_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
