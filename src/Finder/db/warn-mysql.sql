/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:40:00
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for warn
-- ----------------------------
DROP TABLE IF EXISTS `warn`;
CREATE TABLE `warn` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `IntervalHours` int(11) NOT NULL,
  `IntervalHoursTotalInfo` int(11) NOT NULL,
  `Keyword` text NOT NULL,
  `WarnWay` int(11) NOT NULL,
  `WarnContent` text,
  `Mobile` tinytext,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8;
