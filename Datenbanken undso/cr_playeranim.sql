-- phpMyAdmin SQL Dump
-- version 4.1.6
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Erstellungszeit: 10. Aug 2014 um 23:54
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
-- Tabellenstruktur für Tabelle `cr_playeranim`
--

CREATE TABLE IF NOT EXISTS `cr_playeranim` (
  `crafting_id` int(11) NOT NULL DEFAULT '0',
  `anim_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`crafting_id`,`anim_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `cr_playeranim`
--

INSERT INTO `cr_playeranim` (`crafting_id`, `anim_id`) VALUES
(5, 248),
(6, 248),
(8, 119),
(9, 119),
(10, 119),
(11, 117),
(12, 115),
(13, 121),
(14, 121),
(15, 115),
(16, 217),
(17, 217),
(18, 217),
(19, 121),
(20, 90),
(21, 121),
(22, 115),
(23, 121),
(24, 115),
(25, 115),
(26, 115),
(27, 115),
(28, 250),
(28, 282),
(29, 123),
(30, 123),
(31, 123),
(32, 250),
(32, 282),
(33, 123),
(34, 123);

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `cr_playeranim`
--
ALTER TABLE `cr_playeranim`
  ADD CONSTRAINT `cr_playeranim_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
