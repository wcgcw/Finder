/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:38:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for queue
-- ----------------------------
DROP TABLE IF EXISTS `queue`;
CREATE TABLE `queue` (
  `Url` varchar(100) NOT NULL,
  `Title` varchar(50) DEFAULT NULL,
  `Pid` int(11) DEFAULT NULL,
  `Sheng` varchar(20) DEFAULT NULL,
  `Shi` varchar(20) DEFAULT NULL,
  `Xian` varchar(20) DEFAULT NULL,
  `WebName` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`Url`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
