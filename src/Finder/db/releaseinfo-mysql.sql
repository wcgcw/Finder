/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:38:24
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for releaseinfo
-- ----------------------------
DROP TABLE IF EXISTS `releaseinfo`;
CREATE TABLE `releaseinfo` (
  `uid` bigint(20) NOT NULL AUTO_INCREMENT,
  `Title` mediumtext COLLATE utf8mb4_bin NOT NULL,
  `Contexts` longtext COLLATE utf8mb4_bin NOT NULL,
  `ReleaseDate` varchar(20) COLLATE utf8mb4_bin NOT NULL,
  `InfoSource` mediumtext COLLATE utf8mb4_bin NOT NULL,
  `KeyWords` varchar(200) COLLATE utf8mb4_bin NOT NULL,
  `ReleaseName` varchar(100) COLLATE utf8mb4_bin NOT NULL,
  `CollectDate` varchar(20) COLLATE utf8mb4_bin NOT NULL,
  `Snapshot` mediumtext COLLATE utf8mb4_bin,
  `webName` varchar(200) COLLATE utf8mb4_bin NOT NULL,
  `pid` varchar(20) COLLATE utf8mb4_bin NOT NULL,
  `part` varchar(20) COLLATE utf8mb4_bin NOT NULL,
  `reposts` varchar(20) COLLATE utf8mb4_bin DEFAULT NULL,
  `comments` varchar(20) COLLATE utf8mb4_bin DEFAULT NULL,
  `kid` int(11) DEFAULT NULL,
  `sheng` varchar(50) COLLATE utf8mb4_bin DEFAULT NULL,
  `shi` varchar(50) COLLATE utf8mb4_bin DEFAULT NULL,
  `xian` varchar(600) COLLATE utf8mb4_bin DEFAULT NULL,
  `Deleted` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`uid`),
  KEY `FilterReleaseInfo_index1` (`CollectDate`)
) ENGINE=InnoDB AUTO_INCREMENT=449432 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_bin;
