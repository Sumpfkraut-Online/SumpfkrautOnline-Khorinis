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
-- Tabellenstruktur für Tabelle `cr_crafting`
--

CREATE TABLE IF NOT EXISTS `cr_crafting` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caption` text,
  `duration` int(11) DEFAULT NULL,
  `propability` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=35 ;

--
-- Daten für Tabelle `cr_crafting`
--

INSERT INTO `cr_crafting` (`id`, `caption`, `duration`, `propability`) VALUES
(5, 'Kohle abbauen', 10000, 10),
(6, 'Erz schürfen', 30000, 30),
(8, 'Rohlinge herstellen', 5000, 80),
(9, 'Rohlinge herstellen', 5000, 80),
(10, 'Gluehender Stahl herstellen', 5000, 70),
(11, 'Gluehende Klinge herstellen', 5000, 70),
(12, 'Klinge herstellen', 5000, 70),
(13, 'Spitzhacke herstellen', 10000, 70),
(14, 'Kurzschwert herstellen', 10000, 70),
(15, 'Pfanne herstellen', 10000, 70),
(16, 'Heiltrank herstellen', 10000, 70),
(17, 'Starken Heiltrank herstellen', 10000, 70),
(18, 'Manatrank herstellen', 10000, 70),
(19, 'Handsäge herstellen', 10000, 70),
(20, 'Äste absägen', 10000, 70),
(21, 'Beil herstellen', 10000, 70),
(22, 'Edles Langschwert herstellen', 10000, 50),
(23, 'Langschwert herstellen', 10000, 70),
(24, 'Edles Kurzschwert herstellen', 10000, 70),
(25, 'Grobes Schwert', 10000, 70),
(26, 'Steinbrecher herstellen', 10000, 70),
(27, 'Steinbrecher herstellen', 10000, 50),
(28, 'Pilzpfanne herstellen', 10000, 80),
(29, 'Marmelade herstellen', 10000, 80),
(30, 'Kochwurst herstellen', 10000, 80),
(31, 'Pilzsuppe herstellen', 10000, 80),
(32, 'Gebratenes Fleisch', 10000, 80),
(33, 'Fleischsuppe herstellen', 10000, 80),
(34, 'Apfelkompott herstellen', 10000, 80);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
