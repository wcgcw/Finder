/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:36:24
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for filterreleaseinfo
-- ----------------------------
DROP TABLE IF EXISTS `filterreleaseinfo`;
CREATE TABLE `filterreleaseinfo` (
  `FocusLevel` int(11) NOT NULL,
  `uid` bigint(20) NOT NULL,
  `ActionDate` varchar(20) DEFAULT NULL,
  `Deleted` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
