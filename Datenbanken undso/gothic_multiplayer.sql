-- phpMyAdmin SQL Dump
-- version 4.1.6
-- http://www.phpmyadmin.net
--
-- Host: 127.0.0.1
-- Erstellungszeit: 17. Aug 2014 um 20:57
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
-- Tabellenstruktur für Tabelle `account`
--

CREATE TABLE IF NOT EXISTS `account` (
  `accountID` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(45) DEFAULT NULL,
  `password` varchar(64) DEFAULT NULL,
  `banned` bit(1) DEFAULT NULL,
  `lastLogin` varchar(45) DEFAULT NULL,
  `rights` varchar(3) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  PRIMARY KEY (`accountID`),
  UNIQUE KEY `username_UNIQUE` (`username`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=38 ;

--
-- Daten für Tabelle `account`
--

INSERT INTO `account` (`accountID`, `username`, `password`, `banned`, `lastLogin`, `rights`, `email`) VALUES
(1, 'Anti-Cheat System', '22c9d49cdc49a44a216ab219e6c53940411337a7ad9af791a8021a7fa2d05f3e', b'1', '1407702836', 'std', ''),
(2, 'zarata', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'1', '1408055463', 'std', ''),
(3, 'basti', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'1', '1407858042', 'std', ''),
(4, 'penis', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'0', '1407798122', 'std', ''),
(5, 'duadoistdoof', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'0', '1407799346', 'std', ''),
(6, 'arsch', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'0', '1407799713', 'std', ''),
(7, 'anana', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'1', '1407799929', 'std', ''),
(8, 'nase', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'0', '1407801163', 'std', ''),
(9, 'nassee', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'1', '1407801561', 'std', ''),
(10, 'bastian', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'1', '1407866975', 'std', ''),
(11, 'Xavo', '34f9b0e57dbac4746eeca954b630885e75e364dffd52cb740e02d756d9f845fe', b'1', '1408056197', 'std', ''),
(12, 'trader', 'a665a45920422f9d417e4867efdc4fb8a04a1f3fff1fa07e998e86f7f7a27ae3', b'1', '1407870237', 'adm', ''),
(13, 'hhhhhhh', '1df44bfda708d83ccaf1529b4952f382b623bc68d7f5ddaeb8349a7c2f0598bb', b'1', '1408042483', 'std', ''),
(14, 'Awiro', 'e4010593d2e934d21b7cad2594ab0a3827e183d707435de19912df33902a8b14', b'1', '1408299236', 'std', ''),
(15, '5656456ss', '1df44bfda708d83ccaf1529b4952f382b623bc68d7f5ddaeb8349a7c2f0598bb', b'1', '1408112065', 'std', ''),
(16, '888888', '', b'1', '1407976420', 'std', ''),
(17, 'Flyvix', 'f8393de3fde7b43f007b8a1c7dfa6550745749568ee0b7111e45e12cc183c447', b'1', '1408199578', 'adm', ''),
(18, 'Solidus', 'a3174f0080d6142c230007337f22f2b5550e7232a5d9021a0eea341453894b74', b'1', '1408122587', 'std', ''),
(19, 'Angar', '4b738d0ecdd245db3e4ec47114b1552f3f2969eb2b61c83d7434cd80ecd2a076', b'1', '1408041893', 'std', ''),
(20, 'Andre', '4b738d0ecdd245db3e4ec47114b1552f3f2969eb2b61c83d7434cd80ecd2a076', b'1', '1408042017', 'std', ''),
(21, 'tMb', '440e2a6f813964500f340a31b0f406232f07b9baf72e0c1d5228c02483200b76', b'1', '1408043947', 'std', ''),
(22, 'Leon', '390668109950b39c80e6214cf04af2d8ff77d131ae7a81c0c132ed2ae3ab6aab', b'1', '1408201752', 'std', ''),
(23, '2435456678d', '3645cbcbde950e80e6accc02419c18821ef3c6495ef7e5d69f5b19afb92712b6', b'1', '1408113341', 'std', ''),
(24, 'hallo', 'd3751d33f9cd5049c4af2b462735457e4d3baf130bcbb87f389e349fbaeb20b9', b'1', '1408121711', 'std', ''),
(25, 'Anti-Cheat', '293134169bccde4accbb6b92652f4ed37e071fec49b61651b454f4ad999c21e1', b'1', '1408188265', 'std', ''),
(26, '2344356456', '3645cbcbde950e80e6accc02419c18821ef3c6495ef7e5d69f5b19afb92712b6', b'1', '1408199219', 'std', ''),
(27, '342345', '3645cbcbde950e80e6accc02419c18821ef3c6495ef7e5d69f5b19afb92712b6', b'1', '1408199537', 'std', ''),
(28, '345567', 'cf790490415abff6c7cb777a6f9680c3e19eaec0bba3ae8b3c49d7c0f0c42cf9', b'1', '1408199543', 'std', ''),
(29, 'Esteban', '3645cbcbde950e80e6accc02419c18821ef3c6495ef7e5d69f5b19afb92712b6', b'1', '1408200834', 'std', ''),
(30, 'Cailan', 'cf790490415abff6c7cb777a6f9680c3e19eaec0bba3ae8b3c49d7c0f0c42cf9', b'1', '1408200116', 'std', ''),
(31, 'Davo', '09513d26faa09fcea750899bc762ea5da96b8d02eca94bbd66f12e9738d1b734', b'1', '1408201315', 'std', ''),
(32, 'Lord', '0a654b3032812f103184dcd0fa0e60d5db9655bcd5ce8058725230312c30724c', b'1', '1408200783', 'std', ''),
(33, 'Assassael', '8dca5e336b8b3f58057f70626b2c1fa2807318aa27fd909966890055fda7c12b', b'1', '1408299303', 'std', ''),
(34, 'Lazarus', '23e78862efd451fc4cfc68dc2471af6d5180d5309eba53879dd60f07ab6c9f3c', b'1', '1408227268', 'std', ''),
(35, 'Jan', 'd29f478f0dcceb851073efe816282630e198ad2ff7a1d523a826bd10970e7f88', b'1', '1408232408', 'std', ''),
(36, 'Leviathan', '9401d5ed1c1edc2e7a5bdaaf33c55ca6936b5017e2021e6e23a528b5459b1286', b'1', '1408280789', 'std', ''),
(37, 'Lucifer', '9401d5ed1c1edc2e7a5bdaaf33c55ca6936b5017e2021e6e23a528b5459b1286', b'1', '1408280838', 'std', '');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `animations`
--

CREATE TABLE IF NOT EXISTS `animations` (
  `anim_name` varchar(45) NOT NULL DEFAULT '',
  `anim_command` varchar(45) DEFAULT NULL,
  `duration` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`anim_name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `animations`
--

INSERT INTO `animations` (`anim_name`, `anim_command`, `duration`) VALUES
('HUM_DIE_M02', 'tot', 5000),
('HUM_FEGEN_A01', 'fegen', 5000),
('HUM_FORGETIT_M01', 'bitchslap', 5000),
('HUM_FREEFIGHT_M01', 'train', 5000),
('HUM_GREETCOOL_M01', 'gruss', 5000),
('HUM_GREETNOV_M01', 'novizengruss', 5000),
('HUM_GREETRIGHT_M01', 'winken', 5000),
('HUM_GUARD_AMBIENT_A_JUE01', 'guard2', 5000),
('HUM_GUARD_AMBIENT_A_NOENTRY_JUE01', 'halt', 5000),
('HUM_GUARD_AMBIENT_B_JUE01', 'guard1', 5000),
('HUM_GUARD_AMBIENT_B_SALUT_JUE01', 'salutieren', 5000),
('HUM_IGETYOU_M01', 'kriegdich', 5000),
('HUM_INNOSPRAYFP_A01', 'beten2', 5000),
('HUM_INNOS_A01', 'beten1', 5000),
('HUM_PEE_M01', 'pissen', 5000),
('HUM_PRAY_M01', 'anbeten', 5000),
('HUM_SEARCH_M02', 'umschauen', 5000),
('HUM_SHOCKED_M01', 'anfall', 5000),
('HUM_SITGROUND_M01', 'sitzen1', 5000),
('HUM_WASHSELF_M02', 'waschen', 5000),
('HUM_WATCHFIGHTNEW_A01', 'anfeuern', 5000),
('R_SCRATCHHEAD', 'kopfkratzen', 5000),
('S_SIT', 'sit', 5000),
('T_COMEOVERHERE', 'herbeiwinken', 5000),
('T_DANCE_01', 'tanz1', 5000),
('T_DANCE_02', 'tanz2', 5000),
('T_DANCE_03', 'tanz3', 5000),
('T_DANCE_04', 'tanz4', 5000),
('T_DANCE_05', 'tanz5', 5000),
('T_DANCE_06', 'tanz6', 5000),
('T_DANCE_07', 'tanz7', 5000),
('T_DANCE_08', 'tanz8', 5000),
('T_DANCE_09', 'tanz9', 5000),
('T_GREETGRD', 'gruss', 5000),
('T_HGUARD_LOOKAROUND', 'umsehen', 5000),
('T_[Stand]_COMEIN', 'durchgang', 5000),
('T_[Stand]_NOENTRY', 'verweigern', 5000);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ce_class`
--

CREATE TABLE IF NOT EXISTS `ce_class` (
  `idce_class` int(11) NOT NULL,
  `description` varchar(100) NOT NULL,
  `health` int(11) NOT NULL,
  `health_max` int(11) NOT NULL,
  `strength` int(11) NOT NULL,
  `dexterity` int(11) NOT NULL,
  `skill_1handed` int(11) NOT NULL,
  `skill_2handed` int(11) NOT NULL,
  `skill_bow` int(11) NOT NULL,
  `skill_xbow` int(11) NOT NULL,
  `mana_max` int(11) NOT NULL,
  PRIMARY KEY (`idce_class`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `ce_class`
--

INSERT INTO `ce_class` (`idce_class`, `description`, `health`, `health_max`, `strength`, `dexterity`, `skill_1handed`, `skill_2handed`, `skill_bow`, `skill_xbow`, `mana_max`) VALUES
(1, 'Einhandkaempfer: +2 Staerke + 5 Prozent Einhandkampf', 100, 100, 7, 5, 4, 0, 0, 0, 0),
(2, 'Zweihandkaempfer: +2 Staerke + 4 Prozent Zweihandkampf', 100, 100, 7, 5, 0, 4, 0, 0, 0),
(3, 'Bogenschuetze: +2 Geschick + 4 Prozent Bogenkampf', 100, 100, 5, 7, 0, 0, 4, 0, 0),
(4, 'Armbrustschütze: +2 Geschick + 4 Armbrust', 100, 100, 7, 5, 0, 0, 0, 4, 0),
(5, 'Allrounder : +1 auf alles!', 100, 100, 6, 6, 1, 1, 1, 1, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `character`
--

CREATE TABLE IF NOT EXISTS `character` (
  `char_id` int(11) NOT NULL AUTO_INCREMENT,
  `PlayerName` varchar(45) NOT NULL DEFAULT '',
  `PlayerAngle` smallint(6) DEFAULT NULL,
  `PlayerPosX` bigint(20) DEFAULT NULL,
  `PlayerPosY` bigint(20) DEFAULT NULL,
  `PlayerPosZ` bigint(20) DEFAULT NULL,
  `BodyModel` varchar(45) DEFAULT NULL,
  `BodyTextureID` smallint(6) DEFAULT NULL,
  `HeadModel` varchar(45) DEFAULT NULL,
  `HeadTextureID` smallint(6) DEFAULT NULL,
  `PlayerAnimationID` smallint(6) DEFAULT NULL,
  `PlayerFatness` tinyint(4) DEFAULT NULL,
  `PlayerColorRed` tinyint(4) DEFAULT NULL,
  `PlayerColorGreen` tinyint(4) DEFAULT NULL,
  `PlayerColorBlue` tinyint(4) DEFAULT NULL,
  `EquippedArmor` varchar(45) DEFAULT NULL,
  `EquippedMeleeWeapon` varchar(45) DEFAULT NULL,
  `EquippedRangeWeapon` varchar(45) DEFAULT NULL,
  `PlayerExperience` int(11) DEFAULT NULL,
  `PlayerLevel` tinyint(4) DEFAULT NULL,
  `PlayerExperienceNextLevel` int(11) DEFAULT NULL,
  `PlayerLearnPoints` smallint(6) DEFAULT NULL,
  `PlayerHealth` smallint(6) DEFAULT NULL,
  `PlayerMana` smallint(6) DEFAULT NULL,
  `PlayerMagicLevel` tinyint(4) DEFAULT NULL,
  `PlayerAcrobatic` tinyint(4) DEFAULT NULL,
  `LeftHand` varchar(45) DEFAULT NULL,
  `RightHand` varchar(45) DEFAULT NULL,
  `PlayerWalk` varchar(45) DEFAULT NULL,
  `PlayerWeaponMode` smallint(6) DEFAULT NULL,
  `PlayerWorld` varchar(45) DEFAULT NULL,
  `IsNPC` tinyint(4) DEFAULT NULL,
  `PlayerIP` varchar(45) DEFAULT NULL,
  `MacAddress` varchar(45) DEFAULT NULL,
  `accountID` int(11) NOT NULL,
  `hungerLevel` tinyint(4) DEFAULT NULL,
  `lastPlunderTime` bigint(20) DEFAULT NULL,
  `warnings` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`PlayerName`),
  UNIQUE KEY `char_id_UNIQUE` (`char_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=216 ;

--
-- Daten für Tabelle `character`
--

INSERT INTO `character` (`char_id`, `PlayerName`, `PlayerAngle`, `PlayerPosX`, `PlayerPosY`, `PlayerPosZ`, `BodyModel`, `BodyTextureID`, `HeadModel`, `HeadTextureID`, `PlayerAnimationID`, `PlayerFatness`, `PlayerColorRed`, `PlayerColorGreen`, `PlayerColorBlue`, `EquippedArmor`, `EquippedMeleeWeapon`, `EquippedRangeWeapon`, `PlayerExperience`, `PlayerLevel`, `PlayerExperienceNextLevel`, `PlayerLearnPoints`, `PlayerHealth`, `PlayerMana`, `PlayerMagicLevel`, `PlayerAcrobatic`, `LeftHand`, `RightHand`, `PlayerWalk`, `PlayerWeaponMode`, `PlayerWorld`, `IsNPC`, `PlayerIP`, `MacAddress`, `accountID`, `hungerLevel`, `lastPlunderTime`, `warnings`) VALUES
(50, 'anana', 133, 5100, 362, -9445, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 25, 196, 0, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 0, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '127.0.0.1', '3E-E0-72-3C-0F-7F', 7, 48, 0, 0),
(120, 'Andre', 300, 31244, 4255, -1480, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 285, 0, 127, 23, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 0, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '91.56.246.49', '00-26-22-44-68-B8', 20, 46, 0, 0),
(116, 'Angar', 293, -8850, 169, -14678, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 16, 285, 0, 56, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '91.56.246.49', '00-26-22-44-68-B8', 19, 49, 0, 0),
(167, 'Anti-Cheat', 227, 65957, 6806, 46280, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 0, 0, 127, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 0, 0, 9999, 0, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 1, '0', '0', 25, 49, 0, 0),
(191, 'Assassael', 30, 72543, 3257, -10895, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 265, 0, 56, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '2.173.52.73', '08-86-3B-0F-B6-17', 33, 43, 0, 0),
(76, 'Awiro', 299, 65747, 5679, -25140, 'Hum_Body_Naked0', 2, 'Hum_Head_Thief', 20, 196, 1, 127, 23, 127, 'ITAR_DJG_CRAWLER', 'ITMW_NAGELKEULE', 'ITRW_BOW_L_04', 30, 0, 500, 0, 120, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '212.197.177.247', '08-86-3B-E0-CD-C3', 14, 35, 1407872279, 0),
(36, 'basti', 133, -133, -80, -968, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 196, 0, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '127.0.0.1', '3E-E0-72-3C-0F-7F', 3, 49, 0, 0),
(61, 'bastian', 258, 10839, 1359, -16379, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 15, 265, 0, 32, 127, 127, 'ITAR_KDW_H', 'NULL', 'NULL', 0, 0, 500, 0, 100, 0, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '127.0.0.1', '3E-E0-72-3C-0F-7F', 10, 81, 0, 0),
(1, 'bot2', 227, 65957, 6806, 46280, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 0, 0, 127, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 0, 0, 9999, 0, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 1, '0', '0', 1, 49, 0, 0),
(171, 'Cailan', 48, 5821, 368, -2059, 'Hum_Body_Naked0', 0, 'Hum_Head_Pony', 26, 174, 1, 127, 23, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 65, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '188.110.248.235', '44-8A-5B-21-3D-3A', 28, 46, 0, 0),
(177, 'Davo', 350, 6676, 513, -2613, 'Hum_Body_Naked0', 9, 'Hum_Head_Psionic', 13, 196, -1, 127, 23, 127, 'ITAR_LEATHER_L', 'NULL', 'NULL', 0, 0, 500, 0, 97, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '77.9.13.3', '94-DE-80-78-38-7D', 31, 58, 0, 0),
(168, 'Esteban', 270, 12210, 98, -12733, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 196, 0, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 40, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '79.242.163.148', '00-FF-89-61-9C-E2', 26, 49, 0, 0),
(109, 'Flyvix', 311, -4187, 1312, -13934, 'Hum_Body_Naked0', 8, 'Hum_Head_Pony', 25, 196, 2, 127, 23, 127, 'ITAR_PAL_H', 'NULL', 'NULL', 90, 30, 0, 0, 0, 25, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '80.226.24.11', 'F8-D1-11-B3-98-91', 17, 98, 0, 0),
(163, 'hallo', 287, -5321, -120, -10220, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 19, 265, 2, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 0, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '88.152.186.126', '90-E6-BA-72-26-A2', 24, 49, 0, 0),
(206, 'Jan', 4, 13222, 1736, -17113, 'Hum_Body_Naked0', 3, 'Hum_Head_Pony', 60, 196, 1, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '84.134.230.63', 'AC-22-0B-50-20-CE', 35, 48, 0, 0),
(203, 'Lazarus', 334, 6186, 368, -2498, 'Hum_Body_Naked0', 9, 'Hum_Head_Fighter', 27, 206, 0, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', '', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '77.183.244.112', '62-34-A6-1F-54-47', 34, 46, 0, 0),
(86, 'Leon', 83, 7940, 368, -5083, 'Hum_Body_Naked0', 8, 'Hum_Head_Bald', 14, 196, 1, 127, 23, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 10, 5, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '62.143.26.200', '00-26-C6-36-E8-D7', 16, 45, 1408056360, 0),
(141, 'Leon1', 252, -6235, -142, -4977, 'Hum_Body_Naked0', 8, 'Hum_Head_Bald', 14, 196, 1, 127, 23, 127, 'ITAR_KDW_H', 'NULL', 'NULL', 0, 0, 500, 0, 10, 5, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '62.143.0.160', '00-26-C6-36-E8-D7', 22, 46, 1408056360, 0),
(208, 'Leviathan', 119, 370, -82, -1077, 'Hum_Body_Naked0', 10, 'Hum_Head_Psionic', 19, 285, 2, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '87.143.81.151', '74-DE-2B-D4-7A-27', 36, 45, 0, 0),
(181, 'Lord', 174, 5791, 366, -1741, 'Hum_Body_Naked0', 9, 'Hum_Head_Thief', 19, 196, 2, 127, 127, 0, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 85, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '80.226.24.11', 'F8-D1-11-B3-98-91', 32, 46, 0, 0),
(211, 'Lucifer', 323, 30852, 4293, -21460, 'Hum_Body_Naked0', 10, 'Hum_Head_Bald', 9, 206, 2, 32, 127, 127, 'NULL', 'NULL', 'NULL', 50, 0, 500, 0, 100, 0, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '87.143.81.151', '74-DE-2B-D4-7A-27', 37, 41, 0, 0),
(52, 'nase', 90, -8451, 37, -14690, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 23, 196, 0, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '127.0.0.1', '3E-E0-72-3C-0F-7F', 8, 49, 0, 0),
(54, 'nassee', 6, -7579, -135, -12763, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 21, 196, 0, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '127.0.0.1', '3E-E0-72-3C-0F-7F', 9, 49, 0, 0),
(69, 'Nikonius', 290, 22898, 1346, 3818, 'Hum_Body_Naked0', 8, 'Hum_Head_Pony', 85, 265, 1, 127, 23, 127, 'ITAR_KDW_H', 'NULL', 'NULL', 10, 0, 500, 0, 0, 50, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '188.22.245.216', 'E0-CA-94-AF-D3-FB', 13, 41, 0, 0),
(79, 'Nixonius', 66, 50938, 5061, 18195, 'Hum_Body_Naked0', 1, 'Hum_Head_Bald', 9, 265, 0, 32, 127, 127, 'ITAR_KDW_H', 'ITMW_2H_BAU_AXE', 'NULL', 10, 0, 500, 0, 75, 20, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '178.191.248.212', 'E0-CA-94-AF-D3-FB', 15, 41, 0, 0),
(145, 'Rohal', 269, -9960, 97, -14624, 'Hum_Body_Naked0', 9, 'Hum_Head_Bald', 61, 196, 1, 127, 23, 127, 'ITAR_VLK_H', 'NULL', 'NULL', 50, 0, 500, 0, 100, 10, 0, 0, 'ITLSTORCHBURNING', 'NULL', 'NULL', 0, 'OLDWORLD/OLDWORLD.ZEN', 0, '79.242.163.148', '00-FF-89-61-9C-E2', 23, 38, 0, 0),
(110, 'Solidus', 107, 2791, 1758, 7599, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 196, 0, 32, 127, 127, 'ITAR_VLK_L', 'NULL', 'NULL', 0, 0, 500, 0, 100, 0, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '88.152.186.126', '90-E6-BA-72-26-A2', 18, 68, 0, 0),
(133, 'tMb', 114, 32834, 3611, 799, 'Hum_Body_Naked0', 1, 'Hum_Head_Pony', 26, 285, 0, 56, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 78, 0, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '91.56.246.49', '00-26-22-44-68-B8', 21, 44, 0, 0),
(64, 'trader', 2, -7053, -124, -10257, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 18, 265, 0, 32, 127, 127, 'NULL', 'NULL', 'NULL', 0, 0, 500, 0, 100, 10, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '80.187.96.23', '3E-E0-72-3C-0F-7F', 12, 47, 0, 0),
(62, 'Xavo', 191, -7632, -58, -6258, 'Hum_Body_Naked0', 2, 'Hum_Head_Bald', 21, 265, 0, 32, 127, 127, 'ITAR_SLD_H', 'ITMW_SCHWERT4', 'ITRW_CROSSBOW_M_01', 50, 0, 500, 0, 150, 50, 0, 0, 'NULL', 'NULL', 'NULL', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '80.187.96.23', '3E-E0-72-3C-0F-7F', 11, 97, 0, 0),
(2, 'zarata', 139, 49571, 7879, 38672, 'Hum_Body_Naked0', 9, 'Hum_Head_Pony', 19, 196, 0, 32, 127, 127, 'ITAR_SLD_H', 'NULL', 'NULL', 130, 1, 600, 0, 0, 30, 0, 0, 'NULL', 'NULL', '', 0, 'NEWWORLD/NEWWORLD.ZEN', 0, '80.187.96.23', '3E-E0-72-3C-0F-7F', 2, 29, 0, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `char_friends`
--

CREATE TABLE IF NOT EXISTS `char_friends` (
  `account_id_p1` int(11) NOT NULL DEFAULT '0',
  `account_id_p2` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`account_id_p1`,`account_id_p2`),
  KEY `account_id_p2` (`account_id_p2`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `char_friends`
--

INSERT INTO `char_friends` (`account_id_p1`, `account_id_p2`) VALUES
(13, 11),
(14, 11),
(15, 11),
(16, 11),
(22, 11),
(11, 13),
(11, 14),
(15, 14),
(11, 15),
(14, 15),
(11, 16),
(18, 17),
(17, 18),
(11, 22),
(29, 22),
(30, 22),
(31, 22),
(32, 22),
(22, 29),
(30, 29),
(31, 29),
(22, 30),
(29, 30),
(31, 30),
(32, 30),
(22, 31),
(30, 31),
(32, 31),
(22, 32),
(29, 32),
(31, 32);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `char_onlinetime`
--

CREATE TABLE IF NOT EXISTS `char_onlinetime` (
  `account_id` int(11) NOT NULL,
  `day` tinyint(4) DEFAULT NULL,
  `time` smallint(6) DEFAULT NULL,
  `hadTeach` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`account_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `char_onlinetime`
--

INSERT INTO `char_onlinetime` (`account_id`, `day`, `time`, `hadTeach`) VALUES
(1, 15, 600, 0),
(2, 15, 60, 0),
(3, 12, 0, 0),
(4, 12, 180, 0),
(5, 12, 120, 0),
(6, 12, 60, 0),
(7, 12, 180, 0),
(10, 12, 600, 0),
(11, 15, 600, 0),
(12, 12, 420, 0),
(13, 14, 60, 0),
(14, 17, 300, 0),
(15, 15, 600, 0),
(16, 14, 600, 0),
(17, 16, 0, 0),
(18, 15, 600, 0),
(19, 14, 60, 0),
(20, 14, 600, 0),
(21, 14, 600, 0),
(22, 16, 600, 0),
(23, 15, 600, 0),
(24, 15, 60, 0),
(25, 17, 600, 0),
(28, 16, 60, 0),
(29, 16, 600, 0),
(30, 16, 600, 0),
(31, 16, 600, 0),
(32, 16, 420, 0),
(33, 17, 120, 0),
(34, 17, 420, 0),
(35, 17, 300, 0),
(36, 17, 600, 0),
(37, 17, 600, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `char_readingability`
--

CREATE TABLE IF NOT EXISTS `char_readingability` (
  `account_id` int(11) NOT NULL DEFAULT '0',
  `teach_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`account_id`,`teach_id`),
  KEY `teach_id` (`teach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `char_skill`
--

CREATE TABLE IF NOT EXISTS `char_skill` (
  `account_id` int(11) NOT NULL DEFAULT '0',
  `skill_id` int(11) NOT NULL DEFAULT '0',
  `skill_value` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`account_id`,`skill_id`),
  KEY `skill_id` (`skill_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `char_skill`
--

INSERT INTO `char_skill` (`account_id`, `skill_id`, `skill_value`) VALUES
(2, 1, 150),
(2, 2, 100),
(2, 3, 100),
(2, 4, 100),
(2, 5, 100),
(2, 6, 100),
(2, 7, 100),
(2, 8, 100),
(2, 9, 100),
(2, 10, 100),
(2, 11, 100),
(2, 12, 100),
(2, 13, 100),
(2, 14, 100),
(2, 15, 100),
(2, 16, 100),
(2, 17, 100),
(2, 18, 100),
(2, 19, 100),
(2, 20, 100),
(2, 21, 100),
(2, 22, 100),
(2, 23, 100),
(2, 24, 100),
(2, 25, 100),
(2, 26, 100),
(2, 27, 100),
(2, 28, 100),
(2, 29, 100),
(2, 30, 100),
(2, 31, 100),
(2, 32, 100),
(2, 33, 100),
(2, 34, 100),
(2, 35, 100),
(2, 36, 100),
(2, 37, 100),
(2, 38, 100),
(2, 39, 100),
(2, 40, 100),
(2, 41, 100),
(2, 42, 100),
(2, 43, 100),
(2, 44, 100),
(2, 45, 100),
(2, 46, 100),
(2, 47, 100),
(3, 1, 100),
(3, 3, 2),
(3, 4, 0),
(3, 5, 4),
(3, 6, 0),
(3, 7, 0),
(3, 8, 0),
(4, 1, 100),
(4, 3, 6),
(4, 4, 6),
(4, 5, 1),
(4, 6, 1),
(4, 7, 1),
(4, 8, 1),
(5, 1, 100),
(5, 3, 7),
(5, 4, 5),
(5, 5, 4),
(5, 6, 0),
(5, 7, 0),
(5, 8, 0),
(6, 1, 100),
(6, 3, 7),
(6, 4, 5),
(6, 5, 4),
(6, 6, 0),
(6, 7, 0),
(6, 8, 0),
(7, 1, 100),
(7, 3, 7),
(7, 4, 5),
(7, 5, 4),
(7, 6, 0),
(7, 7, 0),
(7, 8, 0),
(8, 1, 100),
(8, 3, 5),
(8, 4, 7),
(8, 5, 0),
(8, 6, 0),
(8, 7, 4),
(8, 8, 0),
(9, 1, 100),
(9, 3, 7),
(9, 4, 5),
(9, 5, 4),
(9, 6, 0),
(9, 7, 0),
(9, 8, 0),
(10, 1, 100),
(10, 3, 7),
(10, 4, 5),
(10, 5, 4),
(10, 6, 0),
(10, 7, 0),
(10, 8, 0),
(10, 9, 0),
(11, 1, 150),
(11, 2, 50),
(11, 3, 75),
(11, 4, 30),
(11, 5, 60),
(11, 6, 60),
(11, 7, 30),
(11, 8, 60),
(11, 9, 100),
(11, 10, 60),
(11, 11, 60),
(11, 12, 60),
(11, 13, 60),
(11, 14, 60),
(11, 15, 60),
(11, 16, 60),
(11, 17, 60),
(11, 18, 60),
(11, 19, 60),
(11, 20, 60),
(11, 21, 60),
(11, 22, 60),
(11, 23, 60),
(11, 24, 60),
(11, 25, 60),
(11, 26, 60),
(11, 27, 60),
(11, 28, 60),
(11, 29, 60),
(11, 30, 60),
(11, 31, 60),
(11, 32, 60),
(11, 33, 60),
(11, 34, 60),
(11, 35, 60),
(11, 36, 60),
(11, 37, 60),
(11, 38, 60),
(11, 39, 60),
(11, 40, 60),
(11, 41, 60),
(11, 42, 60),
(11, 43, 60),
(11, 44, 60),
(11, 45, 60),
(11, 46, 60),
(11, 47, 60),
(12, 1, 100),
(12, 3, 7),
(12, 4, 5),
(12, 5, 4),
(12, 6, 0),
(12, 7, 0),
(12, 8, 0),
(13, 1, 125),
(13, 2, 50),
(13, 3, 50),
(13, 4, 5),
(13, 5, 0),
(13, 6, 30),
(13, 7, 0),
(13, 8, 0),
(14, 1, 130),
(14, 3, 30),
(14, 4, 60),
(14, 5, 60),
(14, 6, 0),
(14, 7, 60),
(14, 8, 30),
(14, 9, 0),
(15, 1, 125),
(15, 2, 50),
(15, 3, 50),
(15, 4, 5),
(15, 5, 0),
(15, 6, 30),
(15, 7, 0),
(15, 8, 0),
(15, 45, 0),
(16, 1, 130),
(16, 3, 30),
(16, 4, 60),
(16, 5, 60),
(16, 6, 0),
(16, 7, 60),
(16, 8, 30),
(17, 1, 100),
(17, 2, 100),
(17, 3, 100),
(17, 4, 100),
(17, 5, 100),
(17, 6, 100),
(17, 7, 100),
(17, 8, 100),
(17, 9, 100),
(17, 10, 100),
(17, 11, 100),
(17, 12, 100),
(17, 13, 100),
(17, 14, 100),
(17, 15, 100),
(17, 16, 100),
(17, 17, 100),
(17, 18, 100),
(17, 19, 100),
(17, 20, 100),
(17, 21, 100),
(17, 22, 100),
(17, 23, 100),
(17, 24, 100),
(17, 25, 100),
(17, 26, 100),
(17, 27, 100),
(17, 28, 100),
(17, 29, 100),
(17, 30, 100),
(17, 31, 100),
(17, 32, 100),
(17, 33, 100),
(17, 34, 100),
(17, 35, 100),
(17, 36, 100),
(17, 37, 100),
(17, 38, 100),
(17, 39, 100),
(17, 40, 100),
(17, 41, 100),
(17, 42, 100),
(17, 43, 100),
(17, 44, 100),
(17, 45, 100),
(17, 46, 100),
(17, 47, 100),
(18, 1, 100),
(18, 3, 7),
(18, 4, 5),
(18, 5, 4),
(18, 6, 0),
(18, 7, 0),
(18, 8, 0),
(19, 1, 100),
(19, 3, 7),
(19, 4, 5),
(19, 5, 4),
(19, 6, 0),
(19, 7, 0),
(19, 8, 0),
(20, 1, 100),
(20, 3, 6),
(20, 4, 6),
(20, 5, 1),
(20, 6, 1),
(20, 7, 1),
(20, 8, 1),
(21, 1, 100),
(21, 3, 7),
(21, 4, 5),
(21, 5, 4),
(21, 6, 0),
(21, 7, 0),
(21, 8, 0),
(22, 1, 100),
(22, 3, 7),
(22, 4, 5),
(22, 5, 4),
(22, 6, 0),
(22, 7, 0),
(22, 8, 0),
(23, 1, 100),
(23, 3, 7),
(23, 4, 5),
(23, 5, 4),
(23, 6, 0),
(23, 7, 0),
(23, 8, 0),
(24, 1, 100),
(24, 3, 7),
(24, 4, 5),
(24, 5, 0),
(24, 6, 4),
(24, 7, 0),
(24, 8, 0),
(29, 1, 100),
(29, 3, 7),
(29, 4, 5),
(29, 5, 4),
(29, 6, 0),
(29, 7, 0),
(29, 8, 0),
(30, 1, 100),
(30, 3, 7),
(30, 4, 5),
(30, 5, 4),
(30, 6, 0),
(30, 7, 0),
(30, 8, 0),
(31, 1, 100),
(31, 3, 7),
(31, 4, 5),
(31, 5, 4),
(31, 6, 0),
(31, 7, 0),
(31, 8, 0),
(32, 1, 100),
(32, 3, 7),
(32, 4, 5),
(32, 5, 4),
(32, 6, 0),
(32, 7, 0),
(32, 8, 0),
(33, 1, 100),
(33, 3, 7),
(33, 4, 5),
(33, 5, 4),
(33, 6, 0),
(33, 7, 0),
(33, 8, 0),
(34, 1, 100),
(34, 3, 7),
(34, 4, 5),
(34, 5, 4),
(34, 6, 0),
(34, 7, 0),
(34, 8, 0),
(35, 1, 100),
(35, 3, 7),
(35, 4, 5),
(35, 5, 4),
(35, 6, 0),
(35, 7, 0),
(35, 8, 0),
(36, 1, 100),
(36, 3, 6),
(36, 4, 6),
(36, 5, 1),
(36, 6, 1),
(36, 7, 1),
(36, 8, 1),
(37, 1, 100),
(37, 3, 6),
(37, 4, 6),
(37, 5, 1),
(37, 6, 1),
(37, 7, 1),
(37, 8, 1),
(37, 9, 0),
(37, 14, 0),
(37, 22, 0);

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
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=49 ;

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
(34, 'Apfelkompott herstellen', 10000, 80),
(35, 'Beten / Spenden ( 100 Gold )', 0, 100),
(36, 'Ledergürtel herstellen', 10000, 50),
(37, 'Kriegerguertel herstellen', 10000, 50),
(38, 'Milizguertel', 10000, 50),
(39, 'Druidenguertel herstellen', 10000, 50),
(40, 'Fackel herstellen', 10000, 50),
(41, 'Arbeiterkleidung herstellen', 10000, 50),
(42, 'Lederrüstung herstellen', 10000, 50),
(43, 'Kriegerruestung herstellen', 10000, 50),
(44, 'Schwere Kriegerruestung herstellen', 10000, 50),
(45, 'Milizruestung herstellen', 60000, 50),
(46, 'Schwere Milizruestung herstellen', 60000, 50),
(47, 'Rote Buergerkleidung', 60000, 50),
(48, 'Bunte Buergerkleidung herstellen', 60000, 50);

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
(34, 123),
(35, 639),
(36, 115),
(37, 115),
(38, 115),
(39, 115),
(40, 90),
(41, 115),
(42, 115),
(43, 115),
(44, 115),
(45, 115),
(46, 115),
(47, 115),
(48, 115);

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
(35, 4, 100),
(36, 172, 5),
(37, 159, 1),
(37, 175, 1),
(38, 159, 1),
(38, 175, 1),
(39, 172, 5),
(40, 75, 1),
(41, 166, 3),
(41, 172, 5),
(42, 172, 10),
(43, 159, 5),
(43, 172, 10),
(44, 159, 10),
(44, 175, 1),
(45, 159, 5),
(45, 172, 10),
(46, 159, 10),
(46, 175, 1),
(47, 172, 6),
(48, 172, 7);

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
(27, 9, 20),
(36, 9, 10),
(37, 9, 15),
(38, 9, 15),
(39, 9, 10),
(41, 9, 30),
(42, 9, 35),
(43, 9, 40),
(44, 9, 50),
(45, 9, 35),
(46, 9, 40),
(47, 9, 30),
(48, 9, 30);

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
(36, 124),
(37, 124),
(38, 124),
(41, 124),
(42, 124),
(43, 124),
(44, 124),
(45, 124),
(46, 124),
(47, 124),
(48, 124),
(20, 128),
(40, 128),
(39, 129),
(40, 129),
(41, 129),
(42, 129),
(43, 129),
(44, 129),
(45, 129),
(46, 129),
(47, 129),
(48, 129),
(5, 132),
(6, 132),
(28, 201),
(29, 201),
(30, 201),
(31, 201),
(33, 201),
(34, 201);

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
(35, 195, 1),
(36, 243, 1),
(37, 245, 1),
(38, 246, 1),
(39, 247, 1),
(40, 256, 1),
(41, 133, 1),
(42, 145, 1),
(43, 147, 1),
(44, 149, 1),
(45, 146, 1),
(46, 148, 1),
(47, 137, 1),
(48, 138, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fg_actions`
--

CREATE TABLE IF NOT EXISTS `fg_actions` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `cat_id` int(11) DEFAULT NULL,
  `caption` text,
  `duration` int(11) DEFAULT NULL,
  `radius` int(11) DEFAULT NULL,
  `specialAction` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `cat_id` (`cat_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=5 ;

--
-- Daten für Tabelle `fg_actions`
--

INSERT INTO `fg_actions` (`id`, `cat_id`, `caption`, `duration`, `radius`, `specialAction`) VALUES
(1, 1, 'Reis', 2000, 200, 0),
(2, 1, 'Weizen', 2000, 200, 0),
(3, 2, 'Giessen', 5000, 1000, 1),
(4, 2, 'Duengen', 5000, 1000, 2);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fg_categories`
--

CREATE TABLE IF NOT EXISTS `fg_categories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caption` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Daten für Tabelle `fg_categories`
--

INSERT INTO `fg_categories` (`id`, `caption`) VALUES
(1, 'Samen einpflanzen'),
(2, 'Sonstiges');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fg_positions`
--

CREATE TABLE IF NOT EXISTS `fg_positions` (
  `x` int(11) NOT NULL DEFAULT '0',
  `y` int(11) NOT NULL DEFAULT '0',
  `z` int(11) NOT NULL DEFAULT '0',
  `world` text,
  `instance_name` text,
  `water` tinyint(4) DEFAULT NULL,
  `fertilizer` tinyint(4) DEFAULT NULL,
  `reported` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`x`,`y`,`z`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `fg_positions`
--

INSERT INTO `fg_positions` (`x`, `y`, `z`, `world`, `instance_name`, `water`, `fertilizer`, `reported`) VALUES
(8142, 268, -5243, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_BEET', 0, 0, 0),
(10825, 1292, -13601, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_MANA_HERB_02', 1, 1, 1),
(10925, 1242, -13341, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_BEET', 1, 1, 1),
(13591, 1695, -17268, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_MANA_HERB_02', 1, 1, 1),
(13656, 1715, -16964, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_MANA_HERB_02', 1, 1, 1),
(13820, 1741, -17114, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_MANA_HERB_02', 1, 1, 1),
(28614, 3109, 7768, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_BEET', 0, 0, 0),
(28840, 3143, 7811, 'NEWWORLDNEWWORLD.ZEN', 'ITPL_MANA_HERB_02', 0, 0, 0);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fg_reqitems`
--

CREATE TABLE IF NOT EXISTS `fg_reqitems` (
  `action_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  `cnt` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`action_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `fg_reqitems`
--

INSERT INTO `fg_reqitems` (`action_id`, `item_id`, `cnt`) VALUES
(1, 71, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fg_reqskills`
--

CREATE TABLE IF NOT EXISTS `fg_reqskills` (
  `action_id` int(11) NOT NULL DEFAULT '0',
  `skill_id` int(11) NOT NULL DEFAULT '0',
  `amount` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`action_id`,`skill_id`),
  KEY `skill_id` (`skill_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fg_reqtools`
--

CREATE TABLE IF NOT EXISTS `fg_reqtools` (
  `action_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`action_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `fg_reqtools`
--

INSERT INTO `fg_reqtools` (`action_id`, `item_id`) VALUES
(1, 132),
(2, 132);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `fg_resitems`
--

CREATE TABLE IF NOT EXISTS `fg_resitems` (
  `action_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  `cnt` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`action_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `fg_resitems`
--

INSERT INTO `fg_resitems` (`action_id`, `item_id`, `cnt`) VALUES
(1, 65, 1),
(2, 66, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ht_hunt`
--

CREATE TABLE IF NOT EXISTS `ht_hunt` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caption` text,
  `duration` int(11) DEFAULT NULL,
  `propability` tinyint(4) DEFAULT NULL,
  `dead` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Daten für Tabelle `ht_hunt`
--

INSERT INTO `ht_hunt` (`id`, `caption`, `duration`, `propability`, `dead`) VALUES
(1, 'Scheeren', 500, 100, 0),
(2, 'Fell abziehen', 1000, 100, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ht_instance`
--

CREATE TABLE IF NOT EXISTS `ht_instance` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` text,
  `caption` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=2 ;

--
-- Daten für Tabelle `ht_instance`
--

INSERT INTO `ht_instance` (`id`, `name`, `caption`) VALUES
(1, 'SHEEP', 'Schaf');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ht_reqskills`
--

CREATE TABLE IF NOT EXISTS `ht_reqskills` (
  `hunt_id` int(11) NOT NULL DEFAULT '0',
  `skill_id` int(11) NOT NULL DEFAULT '0',
  `value` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`hunt_id`,`skill_id`),
  KEY `skill_id` (`skill_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ht_reqtools`
--

CREATE TABLE IF NOT EXISTS `ht_reqtools` (
  `hunt_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`hunt_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ht_resitems`
--

CREATE TABLE IF NOT EXISTS `ht_resitems` (
  `hunt_id` int(11) NOT NULL DEFAULT '0',
  `item_id` int(11) NOT NULL DEFAULT '0',
  `amount` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`hunt_id`,`item_id`),
  KEY `item_id` (`item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `ht_resitems`
--

INSERT INTO `ht_resitems` (`hunt_id`, `item_id`, `amount`) VALUES
(1, 166, 1),
(2, 172, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `ht_targetinst`
--

CREATE TABLE IF NOT EXISTS `ht_targetinst` (
  `hunt_id` int(11) NOT NULL DEFAULT '0',
  `inst_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`hunt_id`,`inst_id`),
  KEY `inst_id` (`inst_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `ht_targetinst`
--

INSERT INTO `ht_targetinst` (`hunt_id`, `inst_id`) VALUES
(1, 1),
(2, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `inv_categories`
--

CREATE TABLE IF NOT EXISTS `inv_categories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caption` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=14 ;

--
-- Daten für Tabelle `inv_categories`
--

INSERT INTO `inv_categories` (`id`, `caption`) VALUES
(1, 'Spezielles'),
(2, 'Lebensmittel'),
(3, 'Pflanzen'),
(4, 'Einhandwaffen'),
(5, 'Zweihandwaffen'),
(6, 'Fernkampfwaffen'),
(7, 'Werkzeug'),
(8, 'Ruestungen'),
(9, 'Herstellung'),
(10, 'Jagdbeute'),
(11, 'Traenke und Heilung'),
(12, 'Guertel'),
(13, 'Spruchrollen/Runen');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `inv_entries`
--

CREATE TABLE IF NOT EXISTS `inv_entries` (
  `account_id` int(11) NOT NULL DEFAULT '0',
  `inventory_item_id` int(11) NOT NULL DEFAULT '0',
  `amount` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`account_id`,`inventory_item_id`),
  KEY `inventory_item_id` (`inventory_item_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `inv_entries`
--

INSERT INTO `inv_entries` (`account_id`, `inventory_item_id`, `amount`) VALUES
(2, 2, 29),
(2, 3, 29),
(2, 4, 66),
(2, 5, 29),
(2, 6, 29),
(2, 7, 29),
(2, 8, 29),
(2, 9, 29),
(2, 10, 29),
(2, 11, 29),
(2, 12, 29),
(2, 13, 29),
(2, 14, 29),
(2, 15, 29),
(2, 16, 29),
(2, 17, 29),
(2, 18, 29),
(2, 19, 29),
(2, 20, 29),
(2, 22, 29),
(2, 23, 29),
(2, 24, 29),
(2, 25, 29),
(2, 26, 29),
(2, 27, 29),
(2, 28, 29),
(2, 29, 29),
(2, 30, 29),
(2, 31, 29),
(2, 32, 29),
(2, 33, 29),
(2, 34, 29),
(2, 35, 29),
(2, 36, 29),
(2, 37, 29),
(2, 38, 29),
(2, 39, 29),
(2, 40, 29),
(2, 41, 29),
(2, 42, 29),
(2, 43, 29),
(2, 44, 29),
(2, 45, 29),
(2, 46, 29),
(2, 47, 29),
(2, 48, 26),
(2, 49, 28),
(2, 50, 29),
(2, 51, 29),
(2, 52, 29),
(2, 53, 28),
(2, 54, 29),
(2, 55, 29),
(2, 56, 29),
(2, 57, 29),
(2, 58, 29),
(2, 59, 29),
(2, 60, 30),
(2, 61, 29),
(2, 62, 29),
(2, 63, 29),
(2, 64, 29),
(2, 65, 27),
(2, 66, 29),
(2, 67, 29),
(2, 68, 29),
(2, 69, 29),
(2, 70, 29),
(2, 71, 26),
(2, 72, 29),
(2, 73, 29),
(2, 74, 29),
(2, 75, 29),
(2, 76, 29),
(2, 77, 29),
(2, 78, 29),
(2, 79, 29),
(2, 80, 29),
(2, 81, 29),
(2, 82, 31),
(2, 83, 29),
(2, 84, 29),
(2, 85, 29),
(2, 86, 29),
(2, 87, 29),
(2, 88, 29),
(2, 89, 29),
(2, 90, 29),
(2, 91, 29),
(2, 92, 29),
(2, 93, 29),
(2, 94, 29),
(2, 95, 29),
(2, 96, 29),
(2, 97, 29),
(2, 98, 29),
(2, 99, 29),
(2, 100, 29),
(2, 101, 29),
(2, 102, 29),
(2, 103, 29),
(2, 104, 31),
(2, 105, 29),
(2, 106, 29),
(2, 107, 29),
(2, 108, 29),
(2, 109, 29),
(2, 110, 29),
(2, 111, 29),
(2, 112, 29),
(2, 113, 29),
(2, 114, 495),
(2, 115, 29),
(2, 116, 29),
(2, 117, 29),
(2, 118, 31),
(2, 119, 29),
(2, 120, 29),
(2, 121, 29),
(2, 122, 29),
(2, 123, 29),
(2, 124, 29),
(2, 125, 29),
(2, 126, 29),
(2, 127, 29),
(2, 128, 29),
(2, 129, 29),
(2, 130, 29),
(2, 131, 29),
(2, 132, 29),
(2, 133, 30),
(2, 134, 29),
(2, 135, 29),
(2, 136, 34),
(2, 137, 32),
(2, 138, 30),
(2, 139, 29),
(2, 140, 29),
(2, 141, 29),
(2, 142, 29),
(2, 143, 29),
(2, 144, 29),
(2, 145, 29),
(2, 146, 29),
(2, 147, 29),
(2, 148, 29),
(2, 149, 4),
(2, 150, 29),
(2, 151, 29),
(2, 152, 29),
(2, 154, 29),
(2, 155, 29),
(2, 156, 29),
(2, 157, 29),
(2, 158, 29),
(2, 159, 9),
(2, 160, 29),
(2, 161, 29),
(2, 162, 29),
(2, 163, 29),
(2, 164, 29),
(2, 165, 29),
(2, 166, 26),
(2, 167, 29),
(2, 168, 29),
(2, 169, 29),
(2, 170, 29),
(2, 171, 29),
(2, 172, 24),
(2, 173, 29),
(2, 174, 29),
(2, 175, 27),
(2, 176, 29),
(2, 177, 29),
(2, 178, 29),
(2, 179, 29),
(2, 180, 29),
(2, 181, 29),
(2, 182, 29),
(2, 183, 29),
(2, 184, 29),
(2, 185, 29),
(2, 186, 29),
(2, 187, 29),
(2, 188, 29),
(2, 189, 29),
(2, 190, 27),
(2, 191, 26),
(2, 192, 26),
(2, 193, 23),
(2, 194, 22),
(2, 195, 15),
(2, 196, 29),
(2, 197, 29),
(2, 198, 29),
(2, 199, 29),
(2, 200, 29),
(2, 201, 29),
(2, 202, 29),
(2, 203, 29),
(2, 204, 29),
(2, 205, 29),
(2, 206, 29),
(2, 207, 29),
(2, 209, 29),
(2, 210, 29),
(2, 211, 29),
(2, 212, 29),
(2, 213, 29),
(2, 215, 29),
(2, 216, 29),
(2, 217, 29),
(2, 218, 29),
(2, 219, 29),
(2, 220, 29),
(2, 221, 29),
(2, 222, 29),
(2, 223, 29),
(2, 224, 29),
(2, 225, 29),
(2, 226, 29),
(2, 227, 29),
(2, 228, 29),
(2, 229, 29),
(2, 230, 29),
(2, 231, 29),
(2, 232, 29),
(2, 233, 29),
(2, 234, 29),
(2, 235, 29),
(2, 236, 29),
(2, 237, 29),
(2, 238, 29),
(2, 239, 29),
(2, 240, 29),
(2, 241, 29),
(2, 242, 29),
(2, 243, 16),
(2, 245, 16),
(2, 246, 16),
(2, 247, 16),
(2, 248, 14),
(2, 249, 14),
(2, 250, 14),
(2, 251, 14),
(2, 252, 14),
(2, 253, 14),
(2, 254, 22),
(2, 255, 12),
(2, 256, 14),
(2, 257, 14),
(2, 258, 14),
(2, 259, 11),
(2, 260, 13),
(10, 2, 8),
(10, 3, 8),
(10, 4, 8),
(10, 5, 8),
(10, 6, 8),
(10, 7, 8),
(10, 8, 8),
(10, 9, 8),
(10, 10, 8),
(10, 11, 8),
(10, 12, 8),
(10, 13, 8),
(10, 14, 8),
(10, 15, 8),
(10, 16, 8),
(10, 17, 8),
(10, 18, 8),
(10, 19, 8),
(10, 20, 8),
(10, 22, 8),
(10, 23, 8),
(10, 24, 5),
(10, 25, 8),
(10, 26, 8),
(10, 27, 8),
(10, 28, 8),
(10, 29, 8),
(10, 30, 8),
(10, 31, 8),
(10, 32, 8),
(10, 33, 8),
(10, 34, 8),
(10, 35, 8),
(10, 36, 8),
(10, 37, 8),
(10, 38, 8),
(10, 39, 8),
(10, 40, 8),
(10, 41, 8),
(10, 42, 8),
(10, 43, 8),
(10, 44, 8),
(10, 45, 8),
(10, 46, 8),
(10, 47, 8),
(10, 48, 7),
(10, 49, 8),
(10, 50, 8),
(10, 51, 8),
(10, 52, 8),
(10, 53, 8),
(10, 54, 8),
(10, 55, 8),
(10, 56, 8),
(10, 57, 8),
(10, 58, 8),
(10, 59, 8),
(10, 60, 8),
(10, 61, 8),
(10, 62, 8),
(10, 63, 8),
(10, 64, 7),
(10, 65, 8),
(10, 66, 8),
(10, 67, 8),
(10, 68, 8),
(10, 69, 8),
(10, 70, 8),
(10, 71, 8),
(10, 72, 8),
(10, 73, 8),
(10, 74, 8),
(10, 75, 8),
(10, 76, 8),
(10, 77, 8),
(10, 78, 8),
(10, 79, 8),
(10, 80, 8),
(10, 81, 8),
(10, 82, 8),
(10, 83, 8),
(10, 84, 8),
(10, 85, 8),
(10, 86, 8),
(10, 87, 8),
(10, 88, 8),
(10, 89, 8),
(10, 90, 8),
(10, 91, 8),
(10, 92, 8),
(10, 93, 8),
(10, 94, 8),
(10, 95, 8),
(10, 96, 8),
(10, 97, 8),
(10, 98, 8),
(10, 99, 8),
(10, 100, 8),
(10, 101, 8),
(10, 102, 8),
(10, 103, 8),
(10, 104, 8),
(10, 105, 8),
(10, 106, 8),
(10, 107, 8),
(10, 108, 8),
(10, 109, 8),
(10, 110, 8),
(10, 111, 8),
(10, 112, 8),
(10, 113, 8),
(10, 114, 8),
(10, 115, 8),
(10, 116, 8),
(10, 117, 8),
(10, 118, 8),
(10, 119, 8),
(10, 120, 8),
(10, 121, 8),
(10, 122, 8),
(10, 123, 8),
(10, 124, 8),
(10, 125, 8),
(10, 126, 8),
(10, 127, 8),
(10, 128, 8),
(10, 129, 8),
(10, 130, 8),
(10, 131, 8),
(10, 132, 8),
(10, 133, 8),
(10, 134, 8),
(10, 135, 8),
(10, 136, 8),
(10, 137, 8),
(10, 138, 8),
(10, 139, 8),
(10, 140, 8),
(10, 141, 8),
(10, 142, 8),
(10, 143, 8),
(10, 144, 4),
(10, 145, 8),
(10, 146, 8),
(10, 147, 8),
(10, 148, 8),
(10, 149, 8),
(10, 150, 8),
(10, 151, 8),
(10, 152, 8),
(10, 154, 8),
(10, 155, 8),
(10, 156, 8),
(10, 157, 8),
(10, 158, 8),
(10, 159, 8),
(10, 160, 8),
(10, 161, 8),
(10, 162, 8),
(10, 163, 8),
(10, 164, 8),
(10, 165, 8),
(10, 166, 8),
(10, 167, 8),
(10, 168, 8),
(10, 169, 8),
(10, 170, 8),
(10, 171, 8),
(10, 172, 8),
(10, 173, 8),
(10, 174, 8),
(10, 175, 8),
(10, 176, 8),
(10, 177, 8),
(10, 178, 8),
(10, 179, 8),
(10, 180, 8),
(10, 181, 8),
(10, 182, 8),
(10, 183, 8),
(10, 184, 8),
(10, 185, 8),
(10, 186, 8),
(10, 187, 8),
(10, 188, 8),
(10, 189, 8),
(10, 190, 8),
(10, 191, 8),
(10, 192, 8),
(10, 193, 8),
(10, 194, 7),
(10, 195, 8),
(10, 196, 8),
(10, 197, 8),
(10, 198, 8),
(10, 199, 8),
(10, 200, 8),
(10, 201, 8),
(10, 202, 8),
(10, 203, 8),
(10, 204, 8),
(10, 205, 8),
(10, 206, 8),
(10, 207, 8),
(10, 209, 8),
(10, 210, 8),
(10, 211, 8),
(10, 212, 8),
(10, 213, 8),
(10, 215, 8),
(10, 216, 8),
(10, 217, 8),
(10, 218, 8),
(10, 219, 8),
(10, 220, 8),
(10, 221, 8),
(10, 222, 8),
(10, 223, 8),
(10, 224, 8),
(10, 225, 8),
(10, 226, 8),
(10, 227, 8),
(10, 228, 8),
(10, 229, 8),
(10, 230, 8),
(10, 231, 8),
(10, 232, 8),
(10, 233, 8),
(10, 234, 8),
(10, 235, 8),
(10, 236, 8),
(10, 237, 8),
(10, 238, 8),
(10, 239, 8),
(10, 240, 8),
(10, 241, 8),
(10, 242, 8),
(10, 243, 8),
(10, 245, 8),
(10, 246, 8),
(10, 247, 8),
(10, 248, 8),
(10, 249, 8),
(10, 250, 8),
(10, 251, 8),
(10, 252, 8),
(10, 253, 8),
(10, 254, 8),
(10, 255, 8),
(10, 256, 8),
(10, 257, 8),
(10, 258, 7),
(10, 259, 8),
(10, 260, 7),
(10, 261, 6),
(10, 262, 5),
(10, 263, 5),
(10, 264, 5),
(10, 266, 5),
(10, 267, 5),
(10, 268, 4),
(10, 269, 8),
(10, 270, 4),
(10, 271, 2),
(11, 2, 8),
(11, 3, 7),
(11, 4, 8),
(11, 5, 7),
(11, 6, 7),
(11, 7, 7),
(11, 8, 7),
(11, 9, 7),
(11, 10, 2),
(11, 11, 7),
(11, 12, 7),
(11, 13, 7),
(11, 14, 7),
(11, 15, 7),
(11, 16, 7),
(11, 17, 7),
(11, 18, 2),
(11, 19, 2),
(11, 20, 7),
(11, 22, 3),
(11, 23, 7),
(11, 24, 2),
(11, 25, 7),
(11, 26, 7),
(11, 27, 7),
(11, 28, 7),
(11, 29, 7),
(11, 30, 7),
(11, 31, 7),
(11, 32, 2),
(11, 33, 7),
(11, 34, 7),
(11, 35, 2),
(11, 36, 7),
(11, 37, 7),
(11, 38, 7),
(11, 39, 7),
(11, 40, 7),
(11, 41, 7),
(11, 42, 6),
(11, 43, 7),
(11, 44, 7),
(11, 45, 7),
(11, 46, 7),
(11, 47, 7),
(11, 48, 13),
(11, 49, 6),
(11, 50, 7),
(11, 51, 7),
(11, 52, 7),
(11, 53, 2),
(11, 54, 7),
(11, 55, 2),
(11, 56, 7),
(11, 57, 6),
(11, 58, 7),
(11, 59, 7),
(11, 60, 2),
(11, 61, 7),
(11, 62, 2),
(11, 63, 2),
(11, 64, 6),
(11, 65, 7),
(11, 66, 6),
(11, 67, 6),
(11, 68, 2),
(11, 69, 2),
(11, 70, 7),
(11, 71, 6),
(11, 72, 7),
(11, 73, 2),
(11, 74, 7),
(11, 75, 7),
(11, 76, 7),
(11, 77, 7),
(11, 78, 7),
(11, 79, 7),
(11, 80, 7),
(11, 81, 7),
(11, 82, 7),
(11, 83, 6),
(11, 84, 7),
(11, 85, 7),
(11, 86, 7),
(11, 87, 7),
(11, 88, 7),
(11, 89, 7),
(11, 90, 7),
(11, 91, 7),
(11, 92, 7),
(11, 93, 7),
(11, 94, 7),
(11, 95, 3),
(11, 96, 7),
(11, 97, 7),
(11, 98, 7),
(11, 99, 7),
(11, 100, 7),
(11, 101, 7),
(11, 102, 7),
(11, 103, 7),
(11, 104, 7),
(11, 105, 7),
(11, 106, 7),
(11, 107, 7),
(11, 108, 7),
(11, 109, 7),
(11, 110, 7),
(11, 111, 7),
(11, 112, 7),
(11, 113, 7),
(11, 114, 2),
(11, 115, 7),
(11, 116, 7),
(11, 117, 2),
(11, 118, 7),
(11, 119, 7),
(11, 120, 7),
(11, 121, 7),
(11, 122, 7),
(11, 123, 7),
(11, 124, 7),
(11, 125, 7),
(11, 126, 7),
(11, 127, 2),
(11, 128, 7),
(11, 129, 7),
(11, 130, 7),
(11, 131, 7),
(11, 132, 7),
(11, 133, 7),
(11, 134, 7),
(11, 135, 7),
(11, 136, 7),
(11, 137, 7),
(11, 138, 7),
(11, 139, 7),
(11, 140, 7),
(11, 141, 7),
(11, 142, 7),
(11, 143, 7),
(11, 145, 7),
(11, 146, 7),
(11, 147, 8),
(11, 148, 7),
(11, 149, 8),
(11, 150, 7),
(11, 151, 7),
(11, 152, 7),
(11, 154, 7),
(11, 155, 7),
(11, 156, 7),
(11, 157, 7),
(11, 158, 7),
(11, 159, 8),
(11, 160, 8),
(11, 161, 7),
(11, 162, 8),
(11, 163, 7),
(11, 164, 7),
(11, 165, 7),
(11, 166, 7),
(11, 167, 7),
(11, 168, 7),
(11, 169, 7),
(11, 170, 7),
(11, 171, 7),
(11, 172, 7),
(11, 173, 7),
(11, 174, 7),
(11, 175, 8),
(11, 176, 7),
(11, 177, 7),
(11, 178, 7),
(11, 179, 7),
(11, 180, 7),
(11, 181, 7),
(11, 182, 7),
(11, 183, 7),
(11, 184, 7),
(11, 185, 7),
(11, 186, 7),
(11, 187, 7),
(11, 188, 7),
(11, 189, 7),
(11, 191, 3),
(11, 192, 4),
(11, 193, 2),
(11, 194, 2),
(11, 195, 6),
(11, 196, 7),
(11, 197, 7),
(11, 198, 7),
(11, 199, 7),
(11, 200, 7),
(11, 201, 7),
(11, 202, 8),
(11, 203, 7),
(11, 204, 7),
(11, 205, 7),
(11, 206, 7),
(11, 207, 7),
(11, 209, 7),
(11, 210, 7),
(11, 211, 7),
(11, 212, 7),
(11, 213, 7),
(11, 215, 4),
(11, 216, 7),
(11, 217, 2),
(11, 218, 7),
(11, 219, 7),
(11, 220, 7),
(11, 221, 7),
(11, 222, 2),
(11, 223, 7),
(11, 224, 7),
(11, 225, 7),
(11, 226, 7),
(11, 227, 7),
(11, 228, 7),
(11, 229, 7),
(11, 230, 7),
(11, 231, 7),
(11, 232, 7),
(11, 233, 7),
(11, 234, 7),
(11, 235, 7),
(11, 236, 7),
(11, 237, 7),
(11, 238, 7),
(11, 239, 7),
(11, 240, 7),
(11, 241, 7),
(11, 242, 7),
(11, 243, 7),
(11, 245, 8),
(11, 246, 7),
(11, 247, 1),
(11, 248, 7),
(11, 249, 7),
(11, 250, 7),
(11, 251, 8),
(11, 252, 7),
(11, 253, 7),
(11, 254, 80),
(11, 255, 2),
(11, 256, 8),
(11, 257, 1),
(11, 258, 2),
(11, 259, 2),
(11, 260, 2),
(11, 261, 2),
(11, 262, 5),
(11, 263, 5),
(11, 264, 8),
(11, 266, 7),
(11, 267, 2),
(11, 268, 5),
(11, 269, 14),
(11, 270, 2),
(11, 271, 7),
(11, 272, 3),
(11, 273, 2),
(11, 274, 2),
(11, 275, 3),
(12, 3, 1),
(12, 4, 1),
(12, 5, 1),
(12, 6, 1),
(12, 7, 1),
(12, 8, 1),
(12, 9, 1),
(12, 10, 1),
(12, 11, 1),
(12, 12, 1),
(12, 13, 1),
(12, 14, 1),
(12, 15, 1),
(12, 16, 1),
(12, 17, 1),
(12, 18, 1),
(12, 19, 1),
(12, 20, 1),
(12, 22, 1),
(12, 23, 1),
(12, 24, 1),
(12, 25, 1),
(12, 26, 1),
(12, 27, 1),
(12, 28, 1),
(12, 29, 1),
(12, 30, 1),
(12, 31, 1),
(12, 32, 1),
(12, 33, 1),
(12, 34, 1),
(12, 35, 1),
(12, 36, 1),
(12, 37, 1),
(12, 38, 1),
(12, 39, 1),
(12, 40, 1),
(12, 41, 1),
(12, 42, 1),
(12, 43, 1),
(12, 44, 1),
(12, 45, 1),
(12, 46, 1),
(12, 47, 1),
(12, 48, 1),
(12, 49, 1),
(12, 50, 1),
(12, 51, 1),
(12, 52, 1),
(12, 53, 1),
(12, 54, 1),
(12, 55, 1),
(12, 56, 1),
(12, 57, 1),
(12, 58, 1),
(12, 59, 1),
(12, 60, 1),
(12, 61, 1),
(12, 62, 1),
(12, 63, 1),
(12, 64, 1),
(12, 65, 1),
(12, 66, 1),
(12, 67, 1),
(12, 68, 1),
(12, 69, 1),
(12, 70, 1),
(12, 71, 1),
(12, 72, 1),
(12, 73, 1),
(12, 74, 1),
(12, 75, 1),
(12, 76, 1),
(12, 77, 1),
(12, 78, 1),
(12, 79, 1),
(12, 80, 1),
(12, 81, 1),
(12, 82, 1),
(12, 84, 1),
(12, 85, 1),
(12, 86, 1),
(12, 87, 1),
(12, 88, 1),
(12, 89, 1),
(12, 90, 1),
(12, 91, 1),
(12, 92, 1),
(12, 93, 1),
(12, 94, 1),
(12, 95, 1),
(12, 96, 1),
(12, 97, 1),
(12, 98, 1),
(12, 99, 1),
(12, 100, 1),
(12, 101, 1),
(12, 102, 1),
(12, 103, 1),
(12, 104, 1),
(12, 105, 1),
(12, 106, 1),
(12, 107, 1),
(12, 108, 1),
(12, 109, 1),
(12, 110, 1),
(12, 111, 1),
(12, 112, 1),
(12, 113, 1),
(12, 114, 1),
(12, 115, 1),
(12, 116, 1),
(12, 117, 1),
(12, 118, 1),
(12, 119, 1),
(12, 120, 1),
(12, 121, 1),
(12, 122, 1),
(12, 123, 1),
(12, 124, 1),
(12, 125, 1),
(12, 126, 1),
(12, 128, 1),
(12, 129, 1),
(12, 130, 1),
(12, 132, 1),
(12, 133, 1),
(12, 134, 1),
(12, 135, 1),
(12, 136, 1),
(12, 137, 1),
(12, 138, 1),
(12, 139, 1),
(12, 140, 1),
(12, 141, 1),
(12, 142, 1),
(12, 143, 1),
(12, 145, 1),
(12, 146, 1),
(12, 148, 1),
(12, 150, 1),
(12, 151, 1),
(12, 152, 1),
(12, 154, 1),
(12, 155, 1),
(12, 156, 1),
(12, 157, 1),
(12, 158, 1),
(12, 161, 1),
(12, 163, 1),
(12, 164, 1),
(12, 165, 1),
(12, 166, 1),
(12, 167, 1),
(12, 168, 1),
(12, 169, 1),
(12, 170, 1),
(12, 171, 1),
(12, 172, 1),
(12, 173, 1),
(12, 174, 1),
(12, 176, 1),
(12, 177, 1),
(12, 178, 1),
(12, 179, 1),
(12, 180, 1),
(12, 181, 1),
(12, 182, 1),
(12, 183, 1),
(12, 184, 1),
(12, 185, 1),
(12, 186, 1),
(12, 187, 1),
(12, 188, 1),
(12, 189, 1),
(12, 196, 1),
(12, 197, 1),
(12, 198, 1),
(12, 199, 1),
(12, 200, 1),
(12, 201, 1),
(12, 203, 1),
(12, 204, 1),
(12, 205, 1),
(12, 206, 1),
(12, 207, 1),
(12, 209, 1),
(12, 210, 1),
(12, 211, 1),
(12, 212, 1),
(12, 213, 1),
(12, 215, 1),
(12, 216, 1),
(12, 217, 1),
(12, 218, 1),
(12, 219, 1),
(12, 220, 1),
(12, 221, 1),
(12, 222, 1),
(12, 223, 1),
(12, 224, 1),
(12, 225, 1),
(12, 226, 1),
(12, 227, 1),
(12, 228, 1),
(12, 229, 1),
(12, 230, 1),
(12, 231, 1),
(12, 232, 1),
(12, 233, 1),
(12, 234, 1),
(12, 235, 1),
(12, 236, 1),
(12, 237, 1),
(12, 238, 1),
(12, 239, 1),
(12, 240, 1),
(12, 241, 1),
(12, 242, 1),
(12, 243, 1),
(12, 246, 1),
(12, 247, 1),
(12, 248, 1),
(12, 249, 1),
(12, 250, 1),
(12, 252, 1),
(12, 253, 1),
(12, 255, 1),
(12, 256, 1),
(12, 261, 1),
(12, 262, 1),
(12, 263, 1),
(12, 266, 1),
(12, 267, 1),
(12, 268, 1),
(12, 269, 2),
(12, 271, 1),
(13, 48, 1),
(13, 127, 1),
(13, 131, 1),
(13, 191, 1),
(13, 192, 1),
(13, 258, 1),
(13, 259, 1),
(13, 270, 1),
(14, 4, 18),
(14, 83, 2),
(14, 95, 4),
(14, 114, 16),
(14, 117, 1),
(14, 215, 3),
(14, 222, 2),
(14, 254, 16),
(14, 262, 2),
(14, 263, 2),
(14, 268, 1),
(15, 4, 321),
(15, 22, 1),
(15, 48, 3),
(15, 114, 22),
(15, 127, 4),
(15, 190, 3),
(15, 191, 11),
(15, 192, 5),
(15, 247, 3),
(15, 254, 29),
(15, 260, 3),
(15, 268, 2),
(15, 270, 3),
(15, 274, 1),
(16, 4, 174),
(16, 10, 1),
(16, 22, 5),
(16, 35, 1),
(16, 48, 1),
(16, 60, 1),
(16, 114, 204),
(16, 117, 4),
(16, 190, 12),
(16, 191, 5),
(16, 192, 3),
(16, 194, 1),
(16, 256, 15),
(16, 261, 5),
(16, 267, 5),
(16, 273, 1),
(16, 274, 1),
(17, 2, 23),
(17, 3, 23),
(17, 4, 24),
(17, 5, 23),
(17, 6, 23),
(17, 7, 23),
(17, 8, 23),
(17, 9, 23),
(17, 10, 20),
(17, 11, 23),
(17, 12, 23),
(17, 13, 23),
(17, 14, 23),
(17, 15, 23),
(17, 16, 23),
(17, 17, 23),
(17, 18, 20),
(17, 19, 20),
(17, 20, 23),
(17, 22, 20),
(17, 23, 23),
(17, 24, 20),
(17, 25, 23),
(17, 26, 23),
(17, 27, 23),
(17, 28, 23),
(17, 29, 23),
(17, 30, 23),
(17, 31, 23),
(17, 32, 20),
(17, 33, 23),
(17, 34, 23),
(17, 35, 20),
(17, 36, 23),
(17, 37, 23),
(17, 38, 23),
(17, 39, 23),
(17, 40, 23),
(17, 41, 23),
(17, 42, 20),
(17, 43, 23),
(17, 44, 23),
(17, 45, 23),
(17, 46, 23),
(17, 47, 23),
(17, 48, 20),
(17, 49, 20),
(17, 50, 20),
(17, 51, 23),
(17, 52, 23),
(17, 53, 20),
(17, 54, 23),
(17, 55, 20),
(17, 56, 23),
(17, 57, 20),
(17, 58, 23),
(17, 59, 23),
(17, 60, 20),
(17, 61, 23),
(17, 62, 20),
(17, 63, 20),
(17, 64, 20),
(17, 65, 20),
(17, 66, 20),
(17, 67, 20),
(17, 68, 20),
(17, 69, 20),
(17, 70, 23),
(17, 71, 23),
(17, 72, 23),
(17, 73, 20),
(17, 74, 23),
(17, 75, 22),
(17, 76, 23),
(17, 77, 23),
(17, 78, 23),
(17, 79, 23),
(17, 80, 23),
(17, 81, 23),
(17, 82, 23),
(17, 83, 23),
(17, 84, 23),
(17, 85, 23),
(17, 86, 23),
(17, 87, 23),
(17, 88, 23),
(17, 89, 23),
(17, 90, 22),
(17, 91, 23),
(17, 92, 23),
(17, 93, 23),
(17, 94, 23),
(17, 95, 23),
(17, 96, 23),
(17, 97, 23),
(17, 98, 23),
(17, 99, 23),
(17, 100, 23),
(17, 101, 23),
(17, 102, 23),
(17, 103, 23),
(17, 104, 23),
(17, 105, 23),
(17, 106, 23),
(17, 107, 23),
(17, 108, 23),
(17, 109, 23),
(17, 110, 23),
(17, 111, 23),
(17, 112, 23),
(17, 113, 23),
(17, 114, 28),
(17, 115, 23),
(17, 116, 23),
(17, 117, 23),
(17, 118, 23),
(17, 119, 23),
(17, 120, 23),
(17, 121, 23),
(17, 122, 23),
(17, 123, 23),
(17, 124, 23),
(17, 125, 23),
(17, 126, 23),
(17, 127, 23),
(17, 128, 23),
(17, 129, 23),
(17, 130, 23),
(17, 131, 23),
(17, 132, 23),
(17, 133, 23),
(17, 134, 23),
(17, 135, 23),
(17, 136, 22),
(17, 137, 23),
(17, 138, 23),
(17, 139, 23),
(17, 140, 23),
(17, 141, 23),
(17, 142, 23),
(17, 143, 23),
(17, 145, 23),
(17, 146, 23),
(17, 147, 23),
(17, 148, 23),
(17, 149, 23),
(17, 150, 23),
(17, 151, 23),
(17, 152, 23),
(17, 154, 23),
(17, 155, 23),
(17, 156, 23),
(17, 157, 23),
(17, 158, 23),
(17, 159, 23),
(17, 160, 23),
(17, 161, 23),
(17, 162, 23),
(17, 163, 23),
(17, 164, 23),
(17, 165, 23),
(17, 166, 23),
(17, 167, 23),
(17, 168, 23),
(17, 169, 23),
(17, 170, 23),
(17, 171, 23),
(17, 172, 23),
(17, 173, 23),
(17, 174, 23),
(17, 175, 23),
(17, 176, 23),
(17, 177, 23),
(17, 178, 23),
(17, 179, 23),
(17, 180, 23),
(17, 181, 23),
(17, 182, 23),
(17, 183, 23),
(17, 184, 23),
(17, 185, 23),
(17, 186, 23),
(17, 187, 23),
(17, 188, 23),
(17, 189, 23),
(17, 190, 21),
(17, 191, 21),
(17, 192, 21),
(17, 193, 20),
(17, 194, 20),
(17, 195, 19),
(17, 196, 23),
(17, 197, 23),
(17, 198, 23),
(17, 199, 23),
(17, 200, 23),
(17, 201, 23),
(17, 202, 23),
(17, 203, 23),
(17, 204, 23),
(17, 205, 23),
(17, 206, 23),
(17, 207, 23),
(17, 209, 23),
(17, 210, 23),
(17, 211, 23),
(17, 212, 23),
(17, 213, 23),
(17, 215, 23),
(17, 216, 23),
(17, 217, 20),
(17, 218, 23),
(17, 219, 23),
(17, 220, 23),
(17, 221, 23),
(17, 222, 20),
(17, 223, 23),
(17, 224, 23),
(17, 225, 23),
(17, 226, 23),
(17, 227, 23),
(17, 228, 23),
(17, 229, 23),
(17, 230, 23),
(17, 231, 23),
(17, 232, 23),
(17, 233, 23),
(17, 234, 23),
(17, 235, 23),
(17, 236, 23),
(17, 237, 23),
(17, 238, 23),
(17, 239, 23),
(17, 240, 23),
(17, 241, 23),
(17, 242, 23),
(17, 243, 23),
(17, 245, 22),
(17, 246, 23),
(17, 247, 23),
(17, 248, 23),
(17, 249, 23),
(17, 250, 23),
(17, 251, 23),
(17, 252, 23),
(17, 253, 23),
(17, 254, 46),
(17, 255, 20),
(17, 256, 23),
(17, 257, 21),
(17, 258, 21),
(17, 259, 21),
(17, 260, 23),
(17, 261, 23),
(17, 262, 23),
(17, 263, 23),
(17, 264, 23),
(17, 266, 23),
(17, 267, 23),
(17, 268, 23),
(17, 269, 46),
(17, 270, 23),
(17, 271, 23),
(17, 272, 23),
(17, 273, 23),
(17, 274, 23),
(17, 275, 23),
(17, 276, 21),
(18, 4, 26),
(18, 10, 1),
(18, 18, 1),
(18, 42, 1),
(18, 48, 6),
(18, 49, 1),
(18, 50, 2),
(18, 53, 1),
(18, 55, 1),
(18, 57, 1),
(18, 60, 1),
(18, 75, 1),
(18, 90, 1),
(18, 136, 1),
(18, 245, 1),
(18, 254, 11),
(22, 4, 16),
(22, 10, 1),
(22, 22, 3),
(22, 60, 23),
(22, 114, 7),
(22, 127, 2),
(22, 136, 2),
(22, 145, 2),
(22, 190, 1),
(22, 193, 1),
(22, 247, 3),
(22, 254, 13),
(22, 256, 15),
(22, 258, 1),
(22, 259, 1),
(22, 260, 2),
(23, 4, 1063),
(23, 22, 8),
(23, 60, 15),
(23, 114, 61),
(23, 136, 5),
(23, 137, 3),
(23, 138, 1),
(23, 145, 1),
(23, 190, 4),
(23, 191, 1),
(23, 193, 10),
(23, 194, 2),
(23, 254, 13),
(23, 256, 15),
(23, 260, 1),
(29, 4, 709),
(29, 22, 3),
(29, 114, 58),
(29, 190, 3),
(29, 191, 2),
(29, 193, 4),
(29, 194, 2),
(29, 254, 26),
(29, 260, 1),
(31, 4, 191),
(31, 22, 1),
(31, 60, 15),
(31, 136, 5),
(31, 137, 3),
(31, 138, 1),
(31, 145, 1),
(31, 193, 5),
(31, 256, 15),
(32, 270, 3),
(33, 4, 18),
(33, 18, 3),
(33, 22, 2),
(33, 35, 3),
(33, 42, 3),
(33, 48, 2),
(33, 60, 2),
(33, 159, 5),
(34, 4, 31),
(34, 22, 1),
(34, 193, 1),
(34, 254, 13),
(37, 4, 98),
(37, 22, 1),
(37, 114, 2),
(37, 115, 1),
(37, 133, 1),
(37, 138, 1),
(37, 193, 1),
(37, 254, 14),
(37, 256, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `inv_items`
--

CREATE TABLE IF NOT EXISTS `inv_items` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `instance_name` text,
  `caption` text,
  `saturation` tinyint(4) NOT NULL DEFAULT '0',
  `cat_id` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `cat_id` (`cat_id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=280 ;

--
-- Daten für Tabelle `inv_items`
--

INSERT INTO `inv_items` (`id`, `instance_name`, `caption`, `saturation`, `cat_id`) VALUES
(2, 'ItMi_Nugget', 'Erzbrocken', 0, 1),
(3, 'ItMi_Coal', 'Kohle', 0, 1),
(4, 'ItMi_Gold', 'Gold', 0, 1),
(5, 'ItMi_GoldCup', 'Goldener Kelch', 0, 1),
(6, 'ItMi_Magischespulver', 'Magisches Pulver', 0, 1),
(7, 'ItMi_Pulver', 'Pulver', 0, 1),
(8, 'ItMi_Salz', 'Salz', 0, 1),
(9, 'ItMi_Schwachespulver', 'Schwaches Pulver', 0, 1),
(10, 'ItFo_Apple', 'Apfel', 2, 2),
(11, 'ItFo_Apfelkompott', 'Apfelkompott', 5, 2),
(12, 'ItFo_Apfelsaft', 'Apfelsaft', 5, 2),
(13, 'ItFo_Apfelwein', 'Apfelwein', 7, 2),
(14, 'ItFo_Auflauf', 'Auflauf', 5, 2),
(15, 'ItFo_Beerenkompott', 'Beerenkompott', 10, 2),
(16, 'ItFo_Beerensaft', 'Beerensaft', 5, 2),
(17, 'ItFo_Besseresbeerenkompott', 'Besseres Beerenkompott', 5, 2),
(18, 'ItFo_Beer', 'Bier', 10, 2),
(19, 'ItFo_Bread', 'Brot', 4, 2),
(20, 'ItFo_Bruehwurst', 'Bruehwurst', 8, 2),
(22, 'ItFo_Fish', 'Fisch', 5, 2),
(23, 'ItFo_Fischbrot', 'Fischbrot', 12, 2),
(24, 'ItFo_FishSoup', 'Fischsuppe', 10, 2),
(25, 'ItFo_Fliederwein', 'Fliederwein', 7, 2),
(26, 'ItFo_Fruchtwein', 'Fruchtwein', 12, 2),
(27, 'ItFo_Gebratenerfisch', 'Gebratener Fisch', 5, 2),
(28, 'ItFo_Gebratenerschinken', 'Gebratener Schinken', 8, 2),
(29, 'ItFo_Getreidebrand', 'Getreidebrand', 15, 2),
(30, 'ItMi_Grosserfisch', 'Grosser Fisch', 0, 2),
(31, 'ItFo_Hochwertigerbeerensaft', 'Hochwertiger Beerensaft', 10, 2),
(32, 'ItFo_Honey', 'Honig', 2, 2),
(33, 'ItFo_Honigbrot', 'Honigbrot', 10, 2),
(34, 'ItFo_Honigwasser', 'Honigwasser', 10, 2),
(35, 'ItFo_Cheese', 'Kaese', 4, 2),
(36, 'ItFo_Kaesebrot', 'Kaesebrot', 12, 2),
(37, 'ItMi_Kleinerfisch', 'Kleiner Fisch', 0, 2),
(38, 'ItFo_Kochschinken', 'Kochschinken', 8, 2),
(39, 'ItFo_Kochwurst', 'Kochwurst', 8, 2),
(40, 'ItFo_Marmelade', 'Marmelade', 5, 2),
(41, 'ItFo_Marmeladenbrot', 'Marmeladenbrot', 10, 2),
(42, 'ItFo_Milk', 'Milch', 2, 2),
(43, 'ItFo_Obstbrand', 'Obstbrand', 0, 2),
(44, 'ItFo_Pikanterohwurst', 'Pikante Rohwurst', 10, 2),
(45, 'ItFo_Pilzpfanne', 'Pilzpfanne', 25, 2),
(46, 'ItFo_Pilzsuppe', 'Pilzsuppe', 10, 2),
(47, 'ItMi_Roherfisch', 'Roher Fisch', 0, 2),
(48, 'ItFoMuttonRaw', 'Rohes Fleisch', 0, 2),
(49, 'ItFo_Wine', 'Rotwein', 12, 2),
(50, 'ItFo_Addon_Rum', 'Rum', 20, 2),
(51, 'ItFo_Sandwich', 'Sandwich', 25, 2),
(52, 'ItFo_Schantalentalbe', 'Schantal Entalbe', 25, 2),
(53, 'ItFo_Bacon', 'Schinken', 6, 2),
(54, 'ItFo_Schinkenbrot', 'Schinkenbrot', 18, 2),
(55, 'ItFo_CoragonsBeer', 'Starkbier', 15, 2),
(56, 'ItFo_Traubensaft', 'Traubensaft', 5, 2),
(57, 'ItFo_Water', 'Wasser', 1, 2),
(58, 'ItFo_Weinschorle', 'Weinschorle', 10, 2),
(59, 'ItFo_Weisswein', 'Weisswein', 8, 2),
(60, 'ItFo_Sausage', 'Wurst', 6, 2),
(61, 'ItFo_Wurstbrot', 'Wurstbrot', 20, 2),
(62, 'ItPl_Mushroom_02', 'Buddlerfleisch', 0, 3),
(63, 'ItPl_Mushroom_01', 'Dunkelpilz', 0, 3),
(64, 'ItPl_Temp_Herb', 'Feldknoeterich', 5, 3),
(65, 'ItPl_Beet', 'Feldruebe', 5, 3),
(66, 'ItPl_Mana_Herb_02', 'Feuerkraut', 5, 3),
(67, 'ItPl_Mana_Herb_01', 'Feuernessel', 5, 3),
(68, 'ItPl_Health_Herb_02', 'Heilkraut', 5, 3),
(69, 'ItPl_Health_Herb_01', 'Heilpflanze', 5, 3),
(70, 'ItPl_Hopfen', 'Hopfen', 5, 3),
(71, 'ItMi_Reissaat', 'Reissaat', 5, 3),
(72, 'ItMi_Ruebensaat', 'Ruebensaat', 5, 3),
(73, 'ItPl_Planeberry', 'Weidenbeere', 2, 3),
(74, 'ItPl_Weinbeere', 'Weinbeere', 5, 3),
(75, 'ItMw_1h_Bau_Mace', 'Schwerer Ast', 0, 4),
(76, 'ItMw_Jagdmesser', 'Jagdmesser', 0, 4),
(77, 'ItMw_1h_MISC_Sword', 'Rostiges Schwert', 0, 4),
(78, 'ItMw_ShortSword2', 'Grobes Kurzschwert', 0, 4),
(79, 'ItMw_ShortSword3', 'Kurzschwert', 0, 4),
(80, 'ItMw_ShortSword5', 'Edles Kurzschwert', 0, 4),
(81, 'ItMw_1h_Sld_Sword', 'Grobes Schwert', 0, 4),
(82, 'ItMw_1h_Pal_Sword', 'Schwert', 0, 4),
(83, 'ItMw_Schwert1', 'Edles Schwert', 0, 4),
(84, 'ItMw_Schwert', 'Grobes Langschwert', 0, 4),
(85, 'ItMw_Schwert2', 'Langschwert', 0, 4),
(86, 'ItMw_Schwert4', 'Edles Langschwert', 0, 4),
(87, 'ItMw_Schwert3', 'Grobes Bastardschwert', 0, 4),
(88, 'ItMw_1H_Special_02', 'Bastardschwert', 0, 4),
(89, 'ItMw_ElBastardo', 'El Bastardo', 0, 4),
(90, 'ItMw_1h_Vlk_Axe', 'Beil', 0, 4),
(91, 'ItMw_1h_Sld_Axe', 'Kriegsbeil', 0, 4),
(92, 'ItMw_Doppelaxt', 'Doppelaxt', 0, 4),
(93, 'ItMw_Bartaxt', 'Bartaxt', 0, 4),
(94, 'ItMw_Nagelknueppel', 'Nagelknueppel', 0, 4),
(95, 'ItMw_Nagelkeule', 'Nagelkeule', 0, 4),
(96, 'ItMw_Morgenstern', 'Morgenstern', 0, 4),
(97, 'ItMw_Kriegskeule', 'Kriegskeule', 0, 4),
(98, 'ItMw_Steinbrecher', 'Steinbrecher', 0, 4),
(99, 'ItMw_Streitkolben', 'Streitkolben', 0, 4),
(100, 'ItMw_Inquisitor', 'Inquisitor', 0, 4),
(101, 'ItMw_Gardeaxt', 'Gardeaxt', 0, 4),
(102, 'ItMw_Fleischerbeil', 'Fleischerbeil', 0, 4),
(103, 'ItMw_Zweihaender1', 'Leichter Zweihaender', 0, 5),
(104, 'ItMw_2h_Pal_Sword', 'Zweihaender', 0, 5),
(105, 'ItMw_Zweihaender2', 'Edler Zweihaender', 0, 5),
(106, 'ItMw_Zweihaender4', 'Schwerer Zweihaender', 0, 5),
(107, 'ItMw_Sturmbringer', 'Sturmbringer', 0, 5),
(108, 'ItMw_2H_Special_03', 'Schlachtklinge', 0, 5),
(109, 'ItMw_Streitaxt1', 'Leichte Streitaxt', 0, 5),
(110, 'ItMw_Streitaxt2', 'Streitaxt', 0, 5),
(111, 'ItMw_Schlachtaxt', 'Schlachtaxt', 0, 5),
(112, 'ItMw_Barbarenstreitaxt', 'Barbenstreitaxt', 0, 5),
(113, 'ItMw_Berserkeraxt', 'Berserkeraxt', 0, 5),
(114, 'ItRw_Arrow', 'Pfeil', 0, 6),
(115, 'ItRw_Bow_L_01', 'Kurzbogen', 0, 6),
(116, 'ItRw_Bow_L_02', 'Weidenbogen', 0, 6),
(117, 'ItRw_Bow_L_04', 'Ulmenbogen', 0, 6),
(118, 'ItRw_Bow_M_02', 'Eschenbogen', 0, 6),
(119, 'ItRw_Bow_M_04', 'Buchenbogen', 0, 6),
(120, 'ItRw_Bow_H_02', 'Eichenbogen', 0, 6),
(121, 'ItRw_Bow_H_01', 'Knochenbogen', 0, 6),
(122, 'ItRw_Bow_H_03', 'Kriegsbogen', 0, 6),
(123, 'ItMw_Zange', 'Zange', 0, 7),
(124, 'ItMw_1H_Mace_L_04', 'Schmiedehammer', 0, 7),
(125, 'ItMw_Ruebenstecher', 'Ruebenstecher', 0, 4),
(126, 'ItMi_Angelrute', 'Angelrute', 0, 7),
(127, 'ItMw_2h_Bau_Axe', 'Holzfaelleraxt', 0, 7),
(128, 'ItMi_Handsaege', 'Handsaege', 0, 7),
(129, 'ItMw_Schnitzmesser', 'Schnitzmesser', 0, 7),
(130, 'ItMw_Pfanne', 'Pfanne', 0, 7),
(131, 'ItMi_Broom', 'Besen', 0, 7),
(132, 'ItMw_2H_Axe_L_01', 'Spitzhacke', 0, 7),
(133, 'ITAR_BAU_L', 'Arbeiterkleidung', 0, 8),
(134, 'ITAR_BAU_M', 'Gruene Bauernkleidung', 0, 8),
(135, 'ITAR_BauBabe_L', 'Baeuerinkleidung', 0, 8),
(136, 'ITAR_VLK_L', 'Braune Buergerkleidung', 0, 8),
(137, 'ITAR_VLK_M', 'Rote Buergerkleidung', 0, 8),
(138, 'ITAR_VLK_H', 'Bunte Buergerkleidung', 0, 8),
(139, 'ITAR_VlkBabe_H', 'Schwarze Buergerinkleidung', 0, 8),
(140, 'ITAR_BARKEEPER', 'Wirtskleidung', 0, 8),
(141, 'ITAR_SMITH', 'Schmiedekleidung', 0, 8),
(142, 'ITAR_NOV_L', 'Novizenrobe', 0, 8),
(143, 'ITAR_KDF_L', 'Feuermagierrobe', 0, 8),
(144, 'ITAR_KDF_H', 'Hohe Feuermagierrobe', 0, 8),
(145, 'ITAR_Leather_L', 'Lederruestung', 0, 8),
(146, 'ITAR_MIL_L', 'Milizruestung', 0, 8),
(147, 'ITAR_SLD_M', 'Kriegerrüstung', 0, 8),
(148, 'ITAR_MIL_M', 'Schwere Milizruestung', 0, 8),
(149, 'ITAR_SLD_H', 'Schwere Kriegerrüstung', 0, 8),
(150, 'ITAR_PAL_M', 'Ritterruestung', 0, 8),
(151, 'ITAR_Governor', 'Wams des Statthalters', 0, 8),
(152, 'ITAR_JUDGE', 'Richterrobe', 0, 8),
(154, 'ItMi_Sehne', 'Sehne', 0, 9),
(155, 'ItMi_Kleinerast', 'Kleiner Ast', 0, 9),
(156, 'ItMi_Grosserast', 'Großer Ast', 0, 9),
(157, 'ItMi_Buchenstock', 'Buchenstock', 0, 9),
(158, 'ItMi_Hainbuchenstock', 'Hainbuchenstock', 0, 9),
(159, 'ItMiSwordraw', 'Rohstahl', 0, 9),
(160, 'ItMiSwordrawhot', 'Gluehender Rohstahl', 0, 9),
(161, 'ItMi_KurzklingeHot', 'Gluehende Kurzklinge', 0, 9),
(162, 'ItMiSwordbladehot', 'Gluehende Klinge', 0, 9),
(163, 'ItMi_Kurzklinge', 'Kurzklinge', 0, 9),
(164, 'ItMi_Axtblatt02', 'Axtblatt', 0, 9),
(165, 'ItMi_Stahlkugel02', 'Stahlkugel', 0, 9),
(166, 'ItMi_Wolle', 'Wolle', 0, 9),
(167, 'ItMi_Seide', 'Seide', 0, 9),
(168, 'ItMi_Blutfliegenkokon', 'Blutfliegenkokon', 0, 9),
(169, 'ItMi_Holzbrett', 'Holzbrett', 0, 9),
(170, 'ItMi_Fleischkadaver', 'Fleischkadaver', 0, 10),
(171, 'ItAt_Claw', 'Kralle', 0, 10),
(172, 'ItAt_WolfFur', 'Wolfsfell', 0, 10),
(173, 'ItAt_Addon_KeilerFur', 'Keilerfell', 0, 10),
(174, 'ItAt_WargFur', 'Wargfell', 0, 10),
(175, 'ItAt_ShadowFur', 'Fell eines Schattenlaeufers', 0, 10),
(176, 'ItAt_TrollFur', 'Trollfell', 0, 10),
(177, 'ItAt_TrollBlackFur', 'Fell eines schwarzen Trolls', 0, 10),
(178, 'ItAt_Sting', 'Stachel (Blutfliegenstachel)', 0, 10),
(179, 'ItAt_BugMandibles', 'Feldraeuberzange', 0, 10),
(180, 'ItAt_CrawlerMandibles', 'Minecrawlerzange', 0, 10),
(181, 'ItAt_ShadowHorn', 'Horn eines Schattenlaeufers', 0, 10),
(182, 'ItAt_WaranFiretongue', 'Flammenzunge', 0, 10),
(183, 'ItAt_TrollTooth', 'Trollhauer', 0, 10),
(184, 'ItPo_KleinerNovizentrank', 'Kleiner Novizentrank', 0, 11),
(185, 'ItPo_Novizentrank', 'Novizentrank', 0, 11),
(186, 'ItPo_GrosserNovizentrank', 'Grosser Novizentrank', 0, 11),
(187, 'ItPo_SchwacherPaladintrank', 'Schwacher Paladintrank', 0, 11),
(188, 'ItPo_Paladintrank', 'Paladintrank', 0, 11),
(189, 'ItPo_StarkerPaladintrank', 'Starker Paladintrank', 0, 11),
(190, 'ItPo_Mana_01', 'Schwacher Manatrank', 0, 11),
(191, 'ItPo_Mana_02', 'Manatrank', 0, 11),
(192, 'ItPo_Mana_03', 'Starker Manatrank', 0, 11),
(193, 'ItPo_Health_01', 'Schwacher Heiltrank', 0, 11),
(194, 'ItPo_Health_02', 'Heiltrank', 0, 11),
(195, 'ItPo_Health_03', 'Starker Heiltrank', 0, 11),
(196, 'ItMi_Heilsalbe', 'Heilsalbe', 0, 11),
(197, 'ItMi_Wundsalbe', 'Wundsalbe', 0, 11),
(198, 'ItPo_EssenzDesLebens', 'Essenz des Lebens', 0, 11),
(199, 'ItMi_Einfacherverband', 'Einfacher Verband', 0, 11),
(200, 'ItMi_Verband', 'Verband', 0, 11),
(201, 'ItMw_Kochloeffel', 'Kochloeffel', 0, 7),
(202, 'ItMiSwordblade', 'Klinge', 0, 9),
(203, 'ItMi_Edelklinge', 'Edle Klinge', 0, 9),
(204, 'ItMi_Edelklingehot', 'Gluehende edle Klinge', 0, 9),
(205, 'ItMi_Stahlkugel01', 'Gehaertete Stahlkugel', 0, 9),
(206, 'ItMi_EingeschmolzenerStahl', 'Eingeschmolzener Stahl', 0, 9),
(207, 'ItMi_Stein', 'Stein', 0, 9),
(209, 'ITAR_Leather_H', 'Schwere Lederruestung', 0, 8),
(210, 'ITAR_VlkBabe_M', 'Weisse Buergerinkleidung', 0, 8),
(211, 'ItAr_FireArmor_Addon', 'Administratorruestung', 0, 8),
(212, 'ITAR_Dementor', 'Supporterruestung', 0, 8),
(213, 'ITAR_PAL_H', 'Paladinruestung', 0, 8),
(215, 'ITAR_DJG_Crawler', 'Ruestung aus Crawlerplatten', 0, 8),
(216, 'ItMw_2H_Sword_Sleeper_02', 'Uriziel (geladen)', 0, 5),
(217, 'ItPl_Forestberry', 'Waldbeere', 1, 3),
(218, 'ItMi_OldCoin', 'Alte Muenze', 0, 1),
(219, 'ItMi_Addon_Shell_01', 'Klappmuschel', 0, 1),
(220, 'ItMi_Addon_Shell_02', 'Hornmuschel', 0, 1),
(221, 'ItMi_Addon_Joint_01', 'Gruener Novize', 0, 1),
(222, 'ItFo_Addon_Shellflesh', 'Muschelfleisch', 1, 2),
(223, 'ItMiSwordbladewarm', 'Heisse Klinge', 0, 9),
(224, 'ItMi_KurzklingeWarm', 'Heisse Kurzklinge', 0, 9),
(225, 'ItMi_Edelklingewarm', 'Heisse edle Klinge', 0, 9),
(226, 'ItMiSwordrawWarm', 'Heisser Rohstahl', 0, 9),
(227, 'ItMi_HotPan', 'Heisse Pfanne', 0, 9),
(228, 'ItMi_HotHammer', 'Heisser Schmiedehammer', 0, 9),
(229, 'ItMi_HotZange', 'Heisse Zange', 0, 9),
(230, 'ItMi_HotRuebenstecher', 'Heisser Ruebenstecher', 0, 9),
(231, 'ItMi_HotRoughKurzschwert', 'Heisses grobes Kurzschwert', 0, 9),
(232, 'ItMi_UncouthRoughKurzschwert', 'Ungeschliffenes grobes Kurzschwert', 0, 9),
(233, 'ItFo_Fruechtekompott', 'Fruechtekompott', 15, 2),
(234, 'ItFo_Fleischgericht', 'Fleischgericht', 30, 2),
(235, 'ItMi_Behandelterbuchenstock', 'Behandelter Buchenstock', 0, 9),
(236, 'ItMi_Behandelterhainbuchenstock', 'Behandelter Hainbuchenstock', 0, 9),
(237, 'ItMi_Crawlersekret', 'Crawlersekret', 0, 10),
(238, 'ItMi_Roherreis', 'Roher Reis', 0, 2),
(239, 'ItMi_Salzmineral', 'Salzmineral', 0, 1),
(240, 'ItMi_Sauerteig', 'Sauerteig', 0, 9),
(241, 'ItMi_Traumruf', 'Traumruf', 0, 1),
(242, 'ItPl_Beere', 'Beere', 5, 3),
(243, 'ItBE_Addon_Leather_01', 'Leaderguertel', 0, 12),
(245, 'ItBE_Addon_SLD_01', 'Soeldnerguertel', 0, 12),
(246, 'ItBE_Addon_MIL_01', 'Milizguertel', 0, 12),
(247, 'ItBE_Addon_NOV_01', 'Schaerpe der Bereitschaft', 0, 12),
(248, 'ItRw_Bow_M_03', 'Langbogen', 0, 6),
(249, 'ItRw_Crossbow_L_01', 'Jagdarmbrust', 0, 6),
(250, 'ItRw_Crossbow_L_02', 'Leichte Armbrust', 0, 6),
(251, 'ItRw_Crossbow_M_01', 'Armbrust', 0, 6),
(252, 'ItRw_Crossbow_M_02', 'Kriegsarmbrust', 0, 6),
(253, 'ItRw_Crossbow_H_01', 'Schwere Armbrust', 0, 6),
(254, 'ItRw_Bolt', 'Bolzen', 0, 6),
(255, 'ItFo_Addon_Meatsoup', 'Fleischsuppe', 0, 2),
(256, 'ItLsTorch', 'Fackel', 0, 1),
(257, 'ItSc_PalLightHeal', 'Kleine Wundheilung', 0, 13),
(258, 'ItSc_PalMediumHeal', 'Mittlere Wundheilung', 0, 13),
(259, 'ItSc_PalFullHeal', 'Grosse Heilung', 0, 13),
(260, 'ItSc_Light', 'Licht', 0, 13),
(261, 'ITAR_Bloodwyn_Addon', 'Jägerrüstung', 0, 8),
(262, 'ITAR_BDT_M', 'Schurkenrüstung', 0, 8),
(263, 'ITAR_BDT_H', 'Hohe Schurkenrüstung', 0, 8),
(264, 'ITAR_SLD_L', 'Leichte Kriegerrüstung', 0, 8),
(265, 'ITAR_KDW_L_Addon', 'Leichte Rüstung', 0, NULL),
(266, 'ITAR_Fake_RANGER', 'Rangerrüstung', 0, 8),
(267, 'ITAR_RANGER_Addon', 'Ranger Rüstung', 0, 8),
(268, 'ItLsTorchburning', 'Fackel brennend', 0, 1),
(269, 'ITAR_KDF_H', 'Schwere Druidenrobe', 0, 6),
(270, 'ITAR_KDW_H', 'KDW/Druidenrobe', 0, 6),
(271, 'ITAR_GRD_ARMOR_I', 'Garderüstung', 0, 6),
(272, 'ItMw_1H_Sword_M_01', 'Degen', 0, 4),
(273, 'ItMw_Rapier', 'Rapier', 0, 4),
(274, 'ItMw_1h_Vlk_Sword', 'Degen', 0, 4),
(275, 'Holy_Hammer_Mis', 'Heiliger Hammer', 0, 6),
(276, 'ItSc_TrfLurker', 'Lurkerverwandlung ', 0, 13),
(277, 'ItSc_TrfSnapper', 'Snapper verwandlung', 0, NULL),
(278, 'ItSc_TrfWaran', 'Waran Verwandlung', 0, NULL),
(279, 'ItSc_TrfScavenger', 'Scavanger Verwandlung', 0, NULL);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `launch_logintoken`
--

CREATE TABLE IF NOT EXISTS `launch_logintoken` (
  `idlaunch_logintoken` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `token_hash` varchar(20) NOT NULL,
  `accountID` int(11) NOT NULL,
  `timestamp` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`idlaunch_logintoken`),
  UNIQUE KEY `idlaunch_logintoken_UNIQUE` (`idlaunch_logintoken`),
  UNIQUE KEY `token_hash_UNIQUE` (`token_hash`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COMMENT='Contains Logintokens used to check if a client is logged in when conecting to the gmp-server' AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `obm_npcs`
--

CREATE TABLE IF NOT EXISTS `obm_npcs` (
  `id` smallint(6) NOT NULL DEFAULT '0',
  `GMPAid` smallint(6) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `instance` varchar(45) DEFAULT NULL,
  `isActive` tinyint(4) DEFAULT NULL,
  `cType` tinyint(4) DEFAULT '1',
  `world` varchar(100) DEFAULT NULL,
  `x` float(10,1) DEFAULT NULL,
  `y` float(10,1) DEFAULT NULL,
  `z` float(10,1) DEFAULT NULL,
  `rot_h` float(5,2) DEFAULT NULL,
  `rot_v` float(5,2) DEFAULT NULL,
  `bodyModel` varchar(45) DEFAULT NULL,
  `bodyTexture` varchar(45) DEFAULT NULL,
  `headModel` varchar(45) DEFAULT NULL,
  `headTexture` varchar(45) DEFAULT NULL,
  `fatness` tinyint(4) DEFAULT NULL,
  `animation` varchar(45) DEFAULT NULL,
  `ownerType` tinyint(4) DEFAULT '0',
  `owner` smallint(6) DEFAULT NULL,
  `creationDate` char(19) DEFAULT NULL,
  `changeDate` char(19) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `obm_objectmanager`
--

CREATE TABLE IF NOT EXISTS `obm_objectmanager` (
  `id` smallint(6) NOT NULL DEFAULT '0',
  `name` varchar(45) DEFAULT NULL,
  `running` tinyint(4) DEFAULT NULL,
  `iterTime` float(10,3) DEFAULT NULL,
  `iterTime_toDB` float(10,3) DEFAULT NULL,
  `mode` varchar(45) DEFAULT NULL,
  `directory` varchar(255) DEFAULT NULL,
  `changeDate` char(19) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `obm_players`
--

CREATE TABLE IF NOT EXISTS `obm_players` (
  `id` smallint(6) NOT NULL DEFAULT '0',
  `GMPAid` smallint(6) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `instance` varchar(45) DEFAULT NULL,
  `isActive` tinyint(4) DEFAULT NULL,
  `cType` tinyint(4) DEFAULT '1',
  `world` varchar(100) DEFAULT NULL,
  `x` float(10,1) DEFAULT NULL,
  `y` float(10,1) DEFAULT NULL,
  `z` float(10,1) DEFAULT NULL,
  `rot_h` float(5,2) DEFAULT NULL,
  `rot_v` float(5,2) DEFAULT NULL,
  `bodyModel` varchar(45) DEFAULT NULL,
  `bodyTexture` varchar(45) DEFAULT NULL,
  `headModel` varchar(45) DEFAULT NULL,
  `headTexture` varchar(45) DEFAULT NULL,
  `fatness` tinyint(4) DEFAULT NULL,
  `animation` varchar(45) DEFAULT NULL,
  `ownerType` tinyint(4) DEFAULT '0',
  `owner` smallint(6) DEFAULT NULL,
  `creationDate` char(19) DEFAULT NULL,
  `changeDate` char(19) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `obm_vobs`
--

CREATE TABLE IF NOT EXISTS `obm_vobs` (
  `id` smallint(6) NOT NULL DEFAULT '0',
  `GMPAid` smallint(6) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `instance` varchar(45) DEFAULT NULL,
  `isActive` tinyint(4) DEFAULT NULL,
  `cType` tinyint(4) DEFAULT '1',
  `world` varchar(100) DEFAULT NULL,
  `x` float(10,1) DEFAULT NULL,
  `y` float(10,1) DEFAULT NULL,
  `z` float(10,1) DEFAULT NULL,
  `rot_x` float(5,2) DEFAULT NULL,
  `rot_y` float(5,2) DEFAULT NULL,
  `rot_z` float(5,2) DEFAULT NULL,
  `ownerType` tinyint(4) DEFAULT '0',
  `owner` smallint(6) DEFAULT NULL,
  `creationDate` char(19) DEFAULT NULL,
  `changeDate` char(19) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `obm_worlditems`
--

CREATE TABLE IF NOT EXISTS `obm_worlditems` (
  `id` smallint(6) NOT NULL DEFAULT '0',
  `GMPAid` smallint(6) DEFAULT NULL,
  `name` varchar(50) DEFAULT NULL,
  `instance` varchar(45) DEFAULT NULL,
  `isActive` tinyint(4) DEFAULT NULL,
  `cType` tinyint(4) DEFAULT '1',
  `world` varchar(100) DEFAULT NULL,
  `x` float(10,1) DEFAULT NULL,
  `y` float(10,1) DEFAULT NULL,
  `z` float(10,1) DEFAULT NULL,
  `amount` smallint(6) DEFAULT NULL,
  `ownerType` tinyint(4) DEFAULT '0',
  `owner` smallint(6) DEFAULT NULL,
  `creationDate` char(19) DEFAULT NULL,
  `changeDate` char(19) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `skills`
--

CREATE TABLE IF NOT EXISTS `skills` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caption` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=48 ;

--
-- Daten für Tabelle `skills`
--

INSERT INTO `skills` (`id`, `caption`) VALUES
(1, 'Lebensenergie'),
(2, 'Mana'),
(3, 'Staerke'),
(4, 'Geschick'),
(5, 'Einhandwaffen'),
(6, 'Zweihandwaffen'),
(7, 'Bogen'),
(8, 'Armbrust'),
(9, 'Schmiedekunst'),
(10, 'Kurzschwert'),
(11, 'Grobes Kurzschwert'),
(12, 'Edles Kurzschwert'),
(13, 'Grobes Schwert'),
(14, 'Schwert'),
(15, 'Edles Schwert'),
(16, 'Grobes Langschwert'),
(17, 'Langschwert'),
(18, 'Edles Langschwert'),
(19, 'Grobes Bastardschwert'),
(20, 'Bastardschwert'),
(21, 'El Bastardo'),
(22, 'Beil'),
(23, 'Kriegsbeil'),
(24, 'Doppelaxt'),
(25, 'Bartaxt'),
(26, 'Gardeaxt'),
(27, 'Nagelknüppel'),
(28, 'Nagelkeule'),
(29, 'Morgenstern'),
(30, 'Kriegskeule'),
(31, 'Steinbrecher'),
(32, 'Streitkolben'),
(33, 'Inquisitor'),
(34, 'Leichter Zweihänder'),
(35, 'Zweihänder'),
(36, 'Edler Zweihänder'),
(37, 'Schwerer Zweihänder'),
(38, 'Sturmbringer'),
(39, 'Schlachtklinge'),
(40, 'Leichte Streitaxt'),
(41, 'Streitaxt'),
(42, 'Schlachtaxt'),
(43, 'Barbarenstreitaxt'),
(44, 'Berserkeraxt'),
(45, 'Pfanne'),
(46, 'Fleischerbeil'),
(47, 'Dolch');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `spwn_spawner`
--

CREATE TABLE IF NOT EXISTS `spwn_spawner` (
  `id` smallint(6) NOT NULL DEFAULT '0',
  `name` varchar(45) DEFAULT NULL,
  `running` tinyint(4) DEFAULT NULL,
  `iterTime` float(10,3) DEFAULT NULL,
  `iterTime_toDB` float(10,3) DEFAULT NULL,
  `mode` varchar(45) DEFAULT NULL,
  `maxSpawns_item` smallint(6) DEFAULT NULL,
  `maxSpawns_npc` smallint(6) DEFAULT NULL,
  `maxSpawns_vob` smallint(6) DEFAULT NULL,
  `maxHistory` smallint(6) DEFAULT NULL,
  `world` varchar(100) DEFAULT NULL,
  `spatials` varchar(255) DEFAULT NULL,
  `spawns` varchar(255) DEFAULT NULL,
  `spawned_item` smallint(6) DEFAULT NULL,
  `spawned_npc` smallint(6) DEFAULT NULL,
  `spawned_vob` smallint(6) DEFAULT NULL,
  `history` varchar(255) DEFAULT NULL,
  `changeDate` char(19) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `spwn_spawner`
--

INSERT INTO `spwn_spawner` (`id`, `name`, `running`, `iterTime`, `iterTime_toDB`, `mode`, `maxSpawns_item`, `maxSpawns_npc`, `maxSpawns_vob`, `maxHistory`, `world`, `spatials`, `spawns`, `spawned_item`, `spawned_npc`, `spawned_vob`, `history`, `changeDate`) VALUES
(1, 'SPAWNER_PLANTS_001', NULL, 1500.000, NULL, 'normal', 1000, 0, 0, 1000, 'OLDWORLD//COLONY.ZEN', 'filterscripts/DATA/spawner/Spawner_TEST/SPATIALS', 'filterscripts/DATA/spawner/Spawner_TEST/SPAWNS', 0, 0, 0, 'filterscripts/DATA/spawner/Spawner_TEST/HISTORY', '0001-01-01 00:00:01'),
(2, 'SPAWNER_FARMS_001', NULL, 5000.000, NULL, 'farming', 1000, 0, 0, 1000, 'OLDWORLD//COLONY.ZEN', 'filterscripts/DATA/spawner/SPAWNER_FARMS_001/SPATIALS', 'filterscripts/DATA/spawner/SPAWNER_FARMS_001/SPAWNS', 0, 0, 0, 'filterscripts/DATA/spawner/SPAWNER_FARMS_001/HISTORY', '0001-01-01 00:00:01');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tc_borders`
--

CREATE TABLE IF NOT EXISTS `tc_borders` (
  `teach_id` int(11) DEFAULT NULL,
  `lower` int(11) DEFAULT NULL,
  `upper` int(11) DEFAULT NULL,
  `cost` smallint(6) DEFAULT NULL,
  KEY `teach_id` (`teach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tc_categories`
--

CREATE TABLE IF NOT EXISTS `tc_categories` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caption` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=3 ;

--
-- Daten für Tabelle `tc_categories`
--

INSERT INTO `tc_categories` (`id`, `caption`) VALUES
(1, 'Basis'),
(2, 'Schmieden');

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tc_level`
--

CREATE TABLE IF NOT EXISTS `tc_level` (
  `account_id` int(11) NOT NULL DEFAULT '0',
  `teach_id` int(11) NOT NULL DEFAULT '0',
  `level` smallint(6) DEFAULT NULL,
  PRIMARY KEY (`account_id`,`teach_id`),
  KEY `teach_id` (`teach_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tc_ranks`
--

CREATE TABLE IF NOT EXISTS `tc_ranks` (
  `account_id` int(11) NOT NULL DEFAULT '0',
  `rank` smallint(6) DEFAULT NULL,
  `cat_id` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`account_id`,`cat_id`),
  KEY `cat_id` (`cat_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `tc_ranks`
--

INSERT INTO `tc_ranks` (`account_id`, `rank`, `cat_id`) VALUES
(11, 1, 1),
(11, 1, 2),
(13, 0, 1),
(13, 0, 2),
(14, 0, 1),
(14, 0, 2),
(15, 0, 1),
(15, 0, 2),
(16, 0, 1),
(16, 0, 2),
(17, 0, 1),
(17, 0, 2),
(18, 0, 1),
(18, 0, 2),
(22, 0, 1),
(22, 0, 2),
(29, 0, 1),
(29, 0, 2),
(30, 0, 1),
(30, 0, 2),
(31, 0, 1),
(31, 0, 2),
(32, 0, 1),
(32, 0, 2);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tc_teach`
--

CREATE TABLE IF NOT EXISTS `tc_teach` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `caption` text,
  `cat_id` int(11) DEFAULT NULL,
  `mastering_tc` int(11) DEFAULT NULL,
  `basic` tinyint(4) DEFAULT NULL,
  `duration` int(11) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `cat_id` (`cat_id`),
  KEY `mastering_tc` (`mastering_tc`)
) ENGINE=InnoDB  DEFAULT CHARSET=latin1 AUTO_INCREMENT=85 ;

--
-- Daten für Tabelle `tc_teach`
--

INSERT INTO `tc_teach` (`id`, `caption`, `cat_id`, `mastering_tc`, `basic`, `duration`) VALUES
(1, 'Stärke + Einhand', 1, NULL, 1, 1000),
(3, 'Stärke + Zweihand', 1, NULL, 1, 1000),
(5, 'Geschicklichkeit + Bogen', 1, NULL, 1, 1000),
(7, 'Schmiedekunst', 2, NULL, 1, 1000),
(8, 'Lesen: Schmiedekunst', 2, 7, 0, 1000),
(9, 'Kurzschwert schmieden', 2, NULL, 1, 1000),
(11, 'Lesen: Kurzschwert schmieden', 2, 9, 0, 1000),
(12, 'Grobes Kurzschwert schmieden', 2, NULL, 1, 1000),
(13, 'Edles Kurzschwert schmieden', 2, NULL, 0, 1000),
(14, 'Schwert schmieden', 2, NULL, 1, 1000),
(15, 'Grobes Schwert schmieden', 2, NULL, 1, 1000),
(16, 'Edles Schwert schmieden', 2, NULL, 0, 1000),
(17, 'Grobes Langschwert schmieden', 2, NULL, 1, 1000),
(18, 'Langschwert schmieden', 2, NULL, 1, 1000),
(19, 'Edles Langschwert schmieden', 2, NULL, 0, 1000),
(20, 'Grobes Bastardschwert schmieden', 2, NULL, 1, 1000),
(21, 'Lesen: Grobes Kurzschwert schmieden', 2, 12, 0, 1000),
(22, 'Lesen: Schwert schmieden', 2, 14, 0, 1000),
(23, 'Lesen: Grobes Schwert schmieden', 2, 15, 0, 1000),
(24, 'Lesen: Grobes Langschwert schmieden', 2, 17, 0, 1000),
(25, 'Lesen: Langschwert schmieden', 2, 18, 0, 1000),
(26, 'Lesen: Grobes Bastardschwert schmieden', 2, 20, 0, 1000),
(27, 'Bastardschwert schmieden', 2, NULL, 1, 1000),
(28, 'El Bastardo schmieden', 2, NULL, 0, 1000),
(29, 'Lesen: Bastardschwert schmieden', 2, 27, 0, 1000),
(30, 'Beil schmieden', 2, NULL, 1, 1000),
(31, 'Kriegsbeil schmieden', 2, NULL, 1, 1000),
(32, 'Doppelaxt schmieden', 2, NULL, 0, 1000),
(33, 'Bartaxt schmieden', 2, NULL, 1, 1000),
(34, 'Gardeaxt schmieden', 2, NULL, 0, 1000),
(35, 'Lesen: Beil schmieden', 2, 30, 0, 1000),
(36, 'Lesen: Kriegsbeil schmieden', 2, 31, 0, 1000),
(37, 'Lesen: Bartaxt schmieden', 2, 33, 0, 1000),
(38, 'Nagelknüppel schmieden', 2, NULL, 1, 1000),
(39, 'Nagelkeule schmieden', 2, NULL, 1, 1000),
(40, 'Morgenstern schmieden', 2, NULL, 1, 1000),
(41, 'Kriegskeule schmieden', 2, NULL, 1, 1000),
(42, 'Steinbrecher schmieden', 2, NULL, 0, 1000),
(43, 'Streitkolben schmieden', 2, NULL, 1, 1000),
(44, 'Inquisitor schmieden', 2, NULL, 0, 1000),
(45, 'Leichter Zweihänder schmieden', 2, NULL, 1, 1000),
(46, 'Zweihänder schmieden', 2, NULL, 1, 1000),
(47, 'Edler Zweihänder schmieden', 2, NULL, 0, 1000),
(48, 'Schwerer Zweihänder schmieden', 2, NULL, 1, 1000),
(49, 'Sturmbringer schmieden', 2, NULL, 1, 1000),
(50, 'Schlachtklinge schmieden', 2, NULL, 0, 1000),
(51, 'Leichte Streitaxt schmieden', 2, NULL, 1, 1000),
(52, 'Streitaxt schmieden', 2, NULL, 1, 1000),
(53, 'Schlachtaxt schmieden', 2, NULL, 0, 1000),
(54, 'Barbarenstreitaxt schmieden', 2, NULL, 1, 1000),
(55, 'Berserkeraxt schmieden', 2, NULL, 0, 1000),
(56, 'Pfanne schmieden', 2, NULL, 1, 1000),
(57, 'Lesen: Nagelknüppel schmieden', 2, 38, 0, 1000),
(60, 'Lesen: Kriegskeule schmieden', 2, 41, 0, 1000),
(61, 'Lesen: Morgenstern schmieden', 2, 40, 0, 1000),
(62, 'Lesen: Nagelkeule schmieden', 2, 39, 0, 1000),
(64, 'Lesen: Streitkolben schmieden', 2, 43, 0, 1000),
(65, 'Lesen: Leichter Zweihänder schmieden', 2, 45, 0, 1000),
(66, 'Lesen: Zweihänder schmieden', 2, 46, 0, 1000),
(68, 'Lesen: Sturmbringer schmieden', 2, 49, 0, 1000),
(69, 'Lesen: Schwerer Zweihänder schmieden', 2, 48, 0, 1000),
(71, 'Lesen: Streitaxt schmieden', 2, 52, 0, 1000),
(72, 'Lesen: Barbarenstreitaxt schmieden', 2, 54, 0, 1000),
(73, 'Lesen: Leichte Streitaxt schmieden', 2, 51, 0, 1000),
(74, 'Lesen: Sturmbringer schmieden', 2, 49, 0, 1000),
(76, 'Fleischerbeil schmieden', 2, NULL, 1, 1000),
(77, 'Dolch schmieden', 2, NULL, 1, 1000),
(78, 'Lesen: Fleischerbeil schmieden', 2, 76, 0, 1000),
(80, 'Lesen: Pfanne schmieden', 2, 56, 0, 1000),
(81, 'Lesen: Dolch schmieden', 2, 77, 0, 1000),
(82, 'Lesen: Stärke + Einhand', 1, 1, 0, 1000),
(83, 'Lesen: Stärke + Zweihand', 1, 3, 0, 1000),
(84, 'Lesen: Geschicklichkeit + Bogen', 1, 5, 0, 1000);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tc_teachprecond`
--

CREATE TABLE IF NOT EXISTS `tc_teachprecond` (
  `teach_id` int(11) NOT NULL DEFAULT '0',
  `skill_id` int(11) NOT NULL DEFAULT '0',
  `skill_value` int(11) DEFAULT NULL,
  PRIMARY KEY (`teach_id`,`skill_id`),
  KEY `skill_id` (`skill_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `tc_teachprecond`
--

INSERT INTO `tc_teachprecond` (`teach_id`, `skill_id`, `skill_value`) VALUES
(9, 9, 14),
(12, 9, 10),
(13, 9, 16),
(14, 9, 22),
(15, 9, 18),
(16, 9, 26),
(17, 9, 30),
(18, 9, 34),
(19, 9, 38),
(20, 9, 42),
(27, 9, 46),
(28, 9, 50),
(30, 9, 10),
(31, 9, 20),
(32, 9, 30),
(33, 9, 40),
(34, 9, 50),
(38, 9, 10),
(39, 9, 17),
(40, 9, 24),
(41, 9, 31),
(42, 9, 38),
(43, 9, 45),
(44, 9, 50),
(45, 9, 18),
(46, 9, 24),
(47, 9, 30),
(48, 9, 36),
(49, 9, 44),
(50, 9, 50),
(51, 9, 20),
(52, 9, 28),
(53, 9, 36),
(54, 9, 42),
(55, 9, 50),
(56, 9, 5),
(76, 9, 13),
(77, 9, 8);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `tc_teachskills`
--

CREATE TABLE IF NOT EXISTS `tc_teachskills` (
  `teach_id` int(11) NOT NULL DEFAULT '0',
  `skill_id` int(11) NOT NULL DEFAULT '0',
  `skill_value` int(11) DEFAULT NULL,
  PRIMARY KEY (`teach_id`,`skill_id`),
  KEY `skill_id` (`skill_id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `tc_teachskills`
--

INSERT INTO `tc_teachskills` (`teach_id`, `skill_id`, `skill_value`) VALUES
(1, 3, 1),
(1, 5, 2),
(3, 3, 1),
(3, 6, 2),
(5, 4, 1),
(5, 7, 2),
(7, 9, 1),
(9, 10, 1),
(12, 11, 1),
(13, 12, 1),
(14, 14, 1),
(15, 13, 1),
(16, 15, 1),
(17, 16, 1),
(18, 17, 1),
(19, 18, 1),
(20, 19, 1),
(27, 20, 1),
(28, 21, 1),
(30, 22, 1),
(31, 23, 1),
(32, 24, 1),
(33, 25, 1),
(34, 26, 1),
(38, 27, 1),
(39, 28, 1),
(40, 29, 1),
(41, 30, 1),
(42, 31, 1),
(43, 32, 1),
(44, 33, 1),
(45, 34, 1),
(46, 35, 1),
(47, 36, 1),
(48, 37, 1),
(49, 38, 1),
(50, 39, 1),
(51, 40, 1),
(52, 41, 1),
(53, 42, 1),
(54, 43, 1),
(55, 44, 1),
(56, 45, 1),
(76, 46, 1),
(77, 47, 1);

-- --------------------------------------------------------

--
-- Tabellenstruktur für Tabelle `wp_waypoints`
--

CREATE TABLE IF NOT EXISTS `wp_waypoints` (
  `name` varchar(45) NOT NULL,
  `posX` double NOT NULL,
  `posY` double NOT NULL,
  `posZ` double NOT NULL,
  `angle` int(11) NOT NULL,
  `world` varchar(45) NOT NULL,
  PRIMARY KEY (`name`),
  UNIQUE KEY `idwp_waypoints_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Daten für Tabelle `wp_waypoints`
--

INSERT INTO `wp_waypoints` (`name`, `posX`, `posY`, `posZ`, `angle`, `world`) VALUES
('adanos', 8846.1591796875, 226.44023132324, -230.70767211914, 112, 'NEWWORLDNEWWORLD.ZEN'),
('alchemist_mittel', 6868.267578125, 222.11596679688, -483.88153076172, 331, 'NEWWORLDNEWWORLD.ZEN'),
('banditen', 30484.482421875, 4299.4858398438, -844.03509521484, 150, 'NEWWORLDNEWWORLD.ZEN'),
('banditen16', 49833.27734375, 1287.0845947266, 6147.5131835938, 114, 'NEWWORLDNEWWORLD.ZEN'),
('banditen3', 16.261499404907, 2281.3420410156, 15313.524414063, 115, 'NEWWORLDNEWWORLD.ZEN'),
('banditen9', 58465.30078125, 1933.2889404297, -28379.751953125, 350, 'NEWWORLDNEWWORLD.ZEN'),
('bloodfly1', 18780.11328125, 2448.7678222656, 13216.024414063, 63, 'NEWWORLDNEWWORLD.ZEN'),
('blutfliegen4', 38756.38671875, 7002.375, 30638.7265625, 145, 'NEWWORLDNEWWORLD.ZEN'),
('blutfliegen5', 35153.9140625, 6504.4086914063, 30685.37109375, 88, 'NEWWORLDNEWWORLD.ZEN'),
('bogner', 7693.0615234375, 368.10244750977, -3426.2766113281, 63, 'NEWWORLDNEWWORLD.ZEN'),
('bordell', 595.93548583984, -88.940467834473, -3096.3356933594, 91, 'NEWWORLDNEWWORLD.ZEN'),
('charedit1_helper', 11818.7890625, 98.190887451172, -13310.577148438, 92, 'NEWWORLDNEWWORLD.ZEN'),
('charedit1_player', 12166.776367188, 98.17716217041, -13331.317382813, 271, 'NEWWORLDNEWWORLD.ZEN'),
('charedit2_helper', 11818.094726563, 93.769989013672, -12736.424804688, 89, 'NEWWORLDNEWWORLD.ZEN'),
('charedit2_player', 12210.192382813, 98.176361083984, -12733.612304688, 271, 'NEWWORLDNEWWORLD.ZEN'),
('charedit3_helper', 11818.986328125, 98.200088500977, -12134.163085938, 92, 'NEWWORLDNEWWORLD.ZEN'),
('charedit3_player', 12191.026367188, 98.200622558594, -12140.202148438, 270, 'NEWWORLDNEWWORLD.ZEN'),
('charedit4_helper', 11829.010742188, 98.170402526855, -11558.078125, 88, 'NEWWORLDNEWWORLD.ZEN'),
('charedit4_player', 12149.1796875, 98.198455810547, -11540.47265625, 270, 'NEWWORLDNEWWORLD.ZEN'),
('charedit5_helper', 11818.970703125, 98.167419433594, -10955.296875, 0, 'NEWWORLDNEWWORLD.ZEN'),
('diegos_haus', 11387.568359375, 998.19097900391, -4626.5551757813, 136, 'NEWWORLDNEWWORLD.ZEN'),
('drachensnapper3', 53434.46875, 7924.6962890625, 36033.66015625, 238, 'NEWWORLDNEWWORLD.ZEN'),
('feldrauber2', 28665.3359375, 2191.2883300781, 13804.7265625, 265, 'NEWWORLDNEWWORLD.ZEN'),
('feldrauber9', 57590.4609375, 2045.1693115234, -13875.495117188, 23, 'NEWWORLDNEWWORLD.ZEN'),
('feldraueber', 59319.0390625, 2113.3955078125, -15965.603515625, 4, 'NEWWORLDNEWWORLD.ZEN'),
('feldraueber10', 64028.23828125, 2574.19921875, -14660.651367188, 352, 'NEWWORLDNEWWORLD.ZEN'),
('feldraueber11', 64830.390625, 2646.6298828125, -8047.0703125, 231, 'NEWWORLDNEWWORLD.ZEN'),
('feldraueber4', 40035.00390625, 3364.4831542969, -11204.403320313, 179, 'NEWWORLDNEWWORLD.ZEN'),
('goblins2', 71641.3515625, 5223.4541015625, 18965.79296875, 93, 'NEWWORLDNEWWORLD.ZEN'),
('goblins3', 69197.8203125, 4598.6713867188, 25708.98828125, 197, 'NEWWORLDNEWWORLD.ZEN'),
('goblins4', 33182.33203125, 4641.3896484375, 12606.551757813, 145, 'NEWWORLDNEWWORLD.ZEN'),
('hof_akil', 31648.39453125, 3772.0139160156, 6465.4829101563, 0, 'NEWWORLDNEWWORLD.ZEN'),
('hof_lobart', 15723.954101563, 2014.2351074219, -14251.065429688, 254, 'NEWWORLDNEWWORLD.ZEN'),
('innos', 38758.50390625, 4460.3251953125, 9787.158203125, 259, 'NEWWORLDNEWWORLD.ZEN'),
('knast', 3746.3610839844, 848.19415283203, 5543.3315429688, 64, 'NEWWORLDNEWWORLD.ZEN'),
('landstreicher4', 68399.109375, 4370.3637695313, 14837.8515625, 77, 'NEWWORLDNEWWORLD.ZEN'),
('landstreicher5', 50444.83203125, 8015.8696289063, 29403.255859375, 46, 'NEWWORLDNEWWORLD.ZEN'),
('login', 65957.5, 6806.6059570313, 46280.4921875, 227, 'NEWWORLDNEWWORLD.ZEN'),
('lurker1', 65183.30859375, 3276.8083496094, 18680.076171875, 199, 'NEWWORLDNEWWORLD.ZEN'),
('lurker4', 44393.3515625, 2674.4157714844, -14950.583007813, 132, 'NEWWORLDNEWWORLD.ZEN'),
('lurker5', 45018.23046875, 2603.9685058594, -16200.607421875, 160, 'NEWWORLDNEWWORLD.ZEN'),
('lurker8', 40464.1484375, 2953.595703125, -24484.6640625, 110, 'NEWWORLDNEWWORLD.ZEN'),
('mine', 4073.3010253906, -2490.8276367188, -16485.404296875, 260, 'NEWWORLDNEWWORLD.ZEN'),
('mine_erz', 3100.7502441406, -442.22213745117, -9667.4150390625, 6, 'NEWWORLDNEWWORLD.ZEN'),
('molerat', 31462.080078125, 3353.5600585938, 624.62493896484, 23, 'NEWWORLDNEWWORLD.ZEN'),
('molerat1', 27349.939453125, 2653.7998046875, 10815.360351563, 329, 'NEWWORLDNEWWORLD.ZEN'),
('molerat15', 55414.53515625, 1625.3101806641, 5401.6557617188, 183, 'NEWWORLDNEWWORLD.ZEN'),
('molerat3', 52399.3203125, 3206.7824707031, 9547.478515625, 339, 'NEWWORLDNEWWORLD.ZEN'),
('nordtor', 11377.6328125, 363.35260009766, 6358.8125, 239, 'NEWWORLDNEWWORLD.ZEN'),
('oldworld', 8096.5444335938, 5625.86328125, 37036.2421875, 232, 'OLDWORLDOLDWORLD.ZEN'),
('ork2', 18242.939453125, 1216.2355957031, 2956.6010742188, 190, 'NEWWORLDNEWWORLD.ZEN'),
('orkkrieger1', 17761.228515625, 929.88885498047, 5235.7880859375, 280, 'NEWWORLDNEWWORLD.ZEN'),
('orkspaeher1', 3150.4328613281, 241.19668579102, 12639.971679688, 145, 'NEWWORLDNEWWORLD.ZEN'),
('orkspaeher3', 36560.28125, 5312.7260742188, 29297.884765625, 188, 'NEWWORLDNEWWORLD.ZEN'),
('penis', 37732.4453125, 3914.3405761719, -3855.7231445313, 269, 'NEWWORLDNEWWORLD.ZEN'),
('ratte2', 4277.23046875, 2855.8395996094, 21960.998046875, 133, 'NEWWORLDNEWWORLD.ZEN'),
('ratten3', 47596.8515625, 3553.9526367188, 7768.4248046875, 266, 'NEWWORLDNEWWORLD.ZEN'),
('ratten5', 39811.2421875, 6958.3872070313, 31512.447265625, 197, 'NEWWORLDNEWWORLD.ZEN'),
('riesenratten', 29555.59765625, 4372.541015625, -3421.17578125, 120, 'NEWWORLDNEWWORLD.ZEN'),
('saegewerk', 27595.79296875, 711.56048583984, -14365.491210938, 166, 'NEWWORLDNEWWORLD.ZEN'),
('scavanger1', 5829.4609375, 1846.873046875, 13059.708984375, 90, 'NEWWORLDNEWWORLD.ZEN'),
('scavanger10', 32501.669921875, 4838.986328125, 23322.455078125, 181, 'NEWWORLDNEWWORLD.ZEN'),
('scavanger12', 45534.76171875, 2984.5634765625, -27050.029296875, 80, 'NEWWORLDNEWWORLD.ZEN'),
('scavanger3', 22934.4765625, 1456.8682861328, 4025.7897949219, 249, 'NEWWORLDNEWWORLD.ZEN'),
('scavanger4', 43446.78125, 3304.7858886719, -896.17810058594, 236, 'NEWWORLDNEWWORLD.ZEN'),
('scavanger6', 63353.6171875, 6712.8090820313, 33326.05078125, 157, 'NEWWORLDNEWWORLD.ZEN'),
('scavanger9', 58938.4140625, 6928.5415039063, 40882.890625, 232, 'NEWWORLDNEWWORLD.ZEN'),
('schattenlaufer1', 1483.0631103516, 2948.7250976563, 18714.775390625, 352, 'NEWWORLDNEWWORLD.ZEN'),
('schmied', 5893.4169921875, 368.25305175781, -1726.5281982422, 332, 'NEWWORLDNEWWORLD.ZEN'),
('schwarzertroll1', 48797.41015625, 7988.7001953125, 39063.5703125, 130, 'NEWWORLDNEWWORLD.ZEN'),
('snapper', 35486.19921875, 6944.0810546875, 34702.25, 90, 'NEWWORLDNEWWORLD.ZEN'),
('snapper1', 37878.65234375, 3371.5734863281, 2744.2998046875, 185, 'NEWWORLDNEWWORLD.ZEN'),
('snapper4', 52473.828125, 8090.8227539063, 34341.640625, 308, 'NEWWORLDNEWWORLD.ZEN'),
('snapper5', 45846.65234375, 2611.1584472656, -31774.638671875, 188, 'NEWWORLDNEWWORLD.ZEN'),
('spawn_hafen', -879.09906005859, -571.80072021484, 359.84783935547, 150, 'NEWWORLDNEWWORLD.ZEN'),
('suedtor', 7918.3100585938, 368.22741699219, -7244.5991210938, 4, 'NEWWORLDNEWWORLD.ZEN'),
('taverne', 38252.43359375, 3921.3937988281, -2232.4951171875, 268, 'NEWWORLDNEWWORLD.ZEN'),
('viertel_hafen', -705.06970214844, -84.263328552246, -839.90496826172, 91, 'NEWWORLDNEWWORLD.ZEN'),
('wolf1', 20438.666015625, 2108.5778808594, 13532.9765625, 123, 'NEWWORLDNEWWORLD.ZEN'),
('wolf11', 62543.6875, 2284.0222167969, 5498.0336914063, 188, 'NEWWORLDNEWWORLD.ZEN'),
('wolf12', 59582.51171875, 2108.5314941406, 6359.7182617188, 205, 'NEWWORLDNEWWORLD.ZEN'),
('wolf3', 62887.703125, 4505.517578125, 14770.682617188, 99, 'NEWWORLDNEWWORLD.ZEN'),
('wolf5', 69062.8515625, 6687.568359375, 34259.3203125, 96, 'NEWWORLDNEWWORLD.ZEN'),
('xardasturm', 31680.65234375, 4399.197265625, -16023.420898438, 292, 'NEWWORLDNEWWORLD.ZEN'),
('zombie1', 14071.939453125, 2750.1154785156, 14521.5078125, 77, 'NEWWORLDNEWWORLD.ZEN'),
('zombie2', 12933.44140625, 3049.7307128906, 15361.494140625, 298, 'NEWWORLDNEWWORLD.ZEN');

--
-- Constraints der exportierten Tabellen
--

--
-- Constraints der Tabelle `char_friends`
--
ALTER TABLE `char_friends`
  ADD CONSTRAINT `char_friends_ibfk_1` FOREIGN KEY (`account_id_p1`) REFERENCES `account` (`accountID`),
  ADD CONSTRAINT `char_friends_ibfk_2` FOREIGN KEY (`account_id_p2`) REFERENCES `account` (`accountID`);

--
-- Constraints der Tabelle `char_onlinetime`
--
ALTER TABLE `char_onlinetime`
  ADD CONSTRAINT `char_onlinetime_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`accountID`);

--
-- Constraints der Tabelle `char_readingability`
--
ALTER TABLE `char_readingability`
  ADD CONSTRAINT `char_readingability_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`accountID`),
  ADD CONSTRAINT `char_readingability_ibfk_2` FOREIGN KEY (`teach_id`) REFERENCES `tc_teach` (`id`);

--
-- Constraints der Tabelle `char_skill`
--
ALTER TABLE `char_skill`
  ADD CONSTRAINT `char_skill_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`accountID`),
  ADD CONSTRAINT `char_skill_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skills` (`id`);

--
-- Constraints der Tabelle `cr_playeranim`
--
ALTER TABLE `cr_playeranim`
  ADD CONSTRAINT `cr_playeranim_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`);

--
-- Constraints der Tabelle `cr_reqitems`
--
ALTER TABLE `cr_reqitems`
  ADD CONSTRAINT `cr_reqitems_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_reqitems_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `cr_reqskills`
--
ALTER TABLE `cr_reqskills`
  ADD CONSTRAINT `cr_reqskills_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_reqskills_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skills` (`id`);

--
-- Constraints der Tabelle `cr_reqtools`
--
ALTER TABLE `cr_reqtools`
  ADD CONSTRAINT `cr_reqtools_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_reqtools_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `cr_resitems`
--
ALTER TABLE `cr_resitems`
  ADD CONSTRAINT `cr_resitems_ibfk_1` FOREIGN KEY (`crafting_id`) REFERENCES `cr_crafting` (`id`),
  ADD CONSTRAINT `cr_resitems_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `fg_actions`
--
ALTER TABLE `fg_actions`
  ADD CONSTRAINT `fg_actions_ibfk_1` FOREIGN KEY (`cat_id`) REFERENCES `fg_categories` (`id`);

--
-- Constraints der Tabelle `fg_reqitems`
--
ALTER TABLE `fg_reqitems`
  ADD CONSTRAINT `fg_reqitems_ibfk_1` FOREIGN KEY (`action_id`) REFERENCES `fg_actions` (`id`),
  ADD CONSTRAINT `fg_reqitems_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `fg_reqskills`
--
ALTER TABLE `fg_reqskills`
  ADD CONSTRAINT `fg_reqskills_ibfk_1` FOREIGN KEY (`action_id`) REFERENCES `fg_actions` (`id`),
  ADD CONSTRAINT `fg_reqskills_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skills` (`id`);

--
-- Constraints der Tabelle `fg_reqtools`
--
ALTER TABLE `fg_reqtools`
  ADD CONSTRAINT `fg_reqtools_ibfk_1` FOREIGN KEY (`action_id`) REFERENCES `fg_actions` (`id`),
  ADD CONSTRAINT `fg_reqtools_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `fg_resitems`
--
ALTER TABLE `fg_resitems`
  ADD CONSTRAINT `fg_resitems_ibfk_1` FOREIGN KEY (`action_id`) REFERENCES `fg_actions` (`id`),
  ADD CONSTRAINT `fg_resitems_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `ht_reqskills`
--
ALTER TABLE `ht_reqskills`
  ADD CONSTRAINT `ht_reqskills_ibfk_1` FOREIGN KEY (`hunt_id`) REFERENCES `ht_hunt` (`id`),
  ADD CONSTRAINT `ht_reqskills_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skills` (`id`);

--
-- Constraints der Tabelle `ht_reqtools`
--
ALTER TABLE `ht_reqtools`
  ADD CONSTRAINT `ht_reqtools_ibfk_1` FOREIGN KEY (`hunt_id`) REFERENCES `ht_hunt` (`id`),
  ADD CONSTRAINT `ht_reqtools_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `ht_resitems`
--
ALTER TABLE `ht_resitems`
  ADD CONSTRAINT `ht_resitems_ibfk_1` FOREIGN KEY (`hunt_id`) REFERENCES `ht_hunt` (`id`),
  ADD CONSTRAINT `ht_resitems_ibfk_2` FOREIGN KEY (`item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `ht_targetinst`
--
ALTER TABLE `ht_targetinst`
  ADD CONSTRAINT `ht_targetinst_ibfk_1` FOREIGN KEY (`hunt_id`) REFERENCES `ht_hunt` (`id`),
  ADD CONSTRAINT `ht_targetinst_ibfk_2` FOREIGN KEY (`inst_id`) REFERENCES `ht_instance` (`id`);

--
-- Constraints der Tabelle `inv_entries`
--
ALTER TABLE `inv_entries`
  ADD CONSTRAINT `inv_entries_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`accountID`),
  ADD CONSTRAINT `inv_entries_ibfk_2` FOREIGN KEY (`inventory_item_id`) REFERENCES `inv_items` (`id`);

--
-- Constraints der Tabelle `inv_items`
--
ALTER TABLE `inv_items`
  ADD CONSTRAINT `inv_items_ibfk_1` FOREIGN KEY (`cat_id`) REFERENCES `inv_categories` (`id`);

--
-- Constraints der Tabelle `tc_borders`
--
ALTER TABLE `tc_borders`
  ADD CONSTRAINT `tc_borders_ibfk_1` FOREIGN KEY (`teach_id`) REFERENCES `tc_teach` (`id`);

--
-- Constraints der Tabelle `tc_level`
--
ALTER TABLE `tc_level`
  ADD CONSTRAINT `tc_level_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`accountID`),
  ADD CONSTRAINT `tc_level_ibfk_2` FOREIGN KEY (`teach_id`) REFERENCES `tc_teach` (`id`);

--
-- Constraints der Tabelle `tc_ranks`
--
ALTER TABLE `tc_ranks`
  ADD CONSTRAINT `tc_ranks_ibfk_1` FOREIGN KEY (`account_id`) REFERENCES `account` (`accountID`),
  ADD CONSTRAINT `tc_ranks_ibfk_2` FOREIGN KEY (`cat_id`) REFERENCES `tc_categories` (`id`);

--
-- Constraints der Tabelle `tc_teach`
--
ALTER TABLE `tc_teach`
  ADD CONSTRAINT `tc_teach_ibfk_1` FOREIGN KEY (`cat_id`) REFERENCES `tc_categories` (`id`),
  ADD CONSTRAINT `tc_teach_ibfk_2` FOREIGN KEY (`mastering_tc`) REFERENCES `tc_teach` (`id`);

--
-- Constraints der Tabelle `tc_teachprecond`
--
ALTER TABLE `tc_teachprecond`
  ADD CONSTRAINT `tc_teachprecond_ibfk_1` FOREIGN KEY (`teach_id`) REFERENCES `tc_teach` (`id`),
  ADD CONSTRAINT `tc_teachprecond_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skills` (`id`);

--
-- Constraints der Tabelle `tc_teachskills`
--
ALTER TABLE `tc_teachskills`
  ADD CONSTRAINT `tc_teachskills_ibfk_1` FOREIGN KEY (`teach_id`) REFERENCES `tc_teach` (`id`),
  ADD CONSTRAINT `tc_teachskills_ibfk_2` FOREIGN KEY (`skill_id`) REFERENCES `skills` (`id`);

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
