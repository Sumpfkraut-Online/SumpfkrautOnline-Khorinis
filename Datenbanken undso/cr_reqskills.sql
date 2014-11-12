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
-- Tabellenstruktur für Tabelle `cr_reqskills`
--

CREATE TABLE IF NOT EXISTS `cr_reqskills` (
  `crafting_id` int(11) NOT NULL DEFAULT '0',
  `skill_id` int(11) NOT NULL DEFAULT '0',
  `amount` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`crafting_id`,`skill_id`),
  KEY `skill_id` (`skill_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `cr_reqskills`
--

INSERT INTO `cr_reqskills` (`crafting_id`, `skill_id`, `amount`) VALUES
(8, 9, 1),
(9, 9, 1),
(10, 9, 1),
(11, 9, 1),
(12, 9, 1),
(13, 9, 1),
(14, 9, 5),
(15, 9, 1),
(16, 2, 20),
(17, 2, 30),
(18, 2, 30),
(19, 9, 1),
(21, 9, 5),
(22, 9, 20),
(23, 9, 17),
(24, 9, 10),
(25, 9, 10),
(25, 13, 1),
(26, 9, 20),
(26, 31, 1),
(27, 9, 20);

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `cr_reqskills`
--
ALTER TABLE `cr_reqskills`
  ADD CONSTRAINT `cr_reqskills_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_reqskills_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skills` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
