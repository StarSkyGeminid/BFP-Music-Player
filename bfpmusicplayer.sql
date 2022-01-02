-- phpMyAdmin SQL Dump
-- version 5.1.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Dec 22, 2021 at 04:55 AM
-- Server version: 10.4.22-MariaDB
-- PHP Version: 8.0.13

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `bfpmusicplayer`
--

-- --------------------------------------------------------

--
-- Table structure for table `file`
--

CREATE TABLE `file` (
  `UID` varchar(200) NOT NULL,
  `location` text DEFAULT NULL,
  `filename` varchar(200) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;


-- --------------------------------------------------------

--
-- Table structure for table `history`
--

CREATE TABLE `history` (
  `UID` varchar(200) DEFAULT NULL,
  `waktu` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `musicinfo`
--

CREATE TABLE `musicinfo` (
  `UID` varchar(200) DEFAULT NULL,
  `title` varchar(200) DEFAULT NULL,
  `artist` varchar(200) DEFAULT NULL,
  `album` varchar(200) DEFAULT NULL,
  `waktu` int(11) DEFAULT NULL,
  `playlist` int(1) DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `file`
--
ALTER TABLE `file`
  ADD PRIMARY KEY (`UID`);

--
-- Indexes for table `history`
--
ALTER TABLE `history`
  ADD KEY `FileName` (`UID`);

--
-- Indexes for table `musicinfo`
--
ALTER TABLE `musicinfo`
  ADD KEY `FileName` (`UID`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `history`
--
ALTER TABLE `history`
  ADD CONSTRAINT `history_ibfk_1` FOREIGN KEY (`UID`) REFERENCES `file` (`UID`);

--
-- Constraints for table `musicinfo`
--
ALTER TABLE `musicinfo`
  ADD CONSTRAINT `musicinfo_ibfk_1` FOREIGN KEY (`UID`) REFERENCES `file` (`UID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
