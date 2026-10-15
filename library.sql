-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: localhost
-- Létrehozás ideje: 2026. Okt 15. 21:20
-- Kiszolgáló verziója: 10.4.28-MariaDB
-- PHP verzió: 8.2.4

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `library`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `Books`
--

CREATE TABLE `Books` (
  `ISBN` varchar(255) NOT NULL,
  `Title` longtext NOT NULL,
  `Author` longtext NOT NULL,
  `Language` longtext NOT NULL,
  `Pages` int(11) NOT NULL,
  `PublisherID` int(11) NOT NULL,
  `Description` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- TÁBLA KAPCSOLATAI `Books`:
--   `PublisherID`
--       `Publishers` -> `ID`
--

--
-- A tábla adatainak kiíratása `Books`
--

INSERT INTO `Books` (`ISBN`, `Title`, `Author`, `Language`, `Pages`, `PublisherID`, `Description`) VALUES
('0000000000', 'És mégis mozog a Föld', 'Jókai Mór', 'magyar', 300, 1, 'Szépirodalmi regény'),
('0000000001', 'Toldi trilógia', 'Arany János', 'magyar', 1300, 2, 'Ballada, trilógia kötet'),
('0000000002', 'Micimackó', 'A. A. Milne', 'magyar', 125, 2, 'Klasszikus mese regény');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `Publishers`
--

CREATE TABLE `Publishers` (
  `ID` int(11) NOT NULL,
  `Name` longtext NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- TÁBLA KAPCSOLATAI `Publishers`:
--

--
-- A tábla adatainak kiíratása `Publishers`
--

INSERT INTO `Publishers` (`ID`, `Name`) VALUES
(1, 'Európa'),
(2, 'Alexandra');

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `__EFMigrationsHistory`
--

CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- TÁBLA KAPCSOLATAI `__EFMigrationsHistory`:
--

--
-- A tábla adatainak kiíratása `__EFMigrationsHistory`
--

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`) VALUES
('20231123183224_InitialMigration', '7.0.14'),
('20231123184424_AddDescriptionToBookMigration', '7.0.14');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `Books`
--
ALTER TABLE `Books`
  ADD PRIMARY KEY (`ISBN`),
  ADD KEY `IX_Books_PublisherID` (`PublisherID`);

--
-- A tábla indexei `Publishers`
--
ALTER TABLE `Publishers`
  ADD PRIMARY KEY (`ID`);

--
-- A tábla indexei `__EFMigrationsHistory`
--
ALTER TABLE `__EFMigrationsHistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `Publishers`
--
ALTER TABLE `Publishers`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `Books`
--
ALTER TABLE `Books`
  ADD CONSTRAINT `FK_Books_Publishers_PublisherID` FOREIGN KEY (`PublisherID`) REFERENCES `Publishers` (`ID`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
