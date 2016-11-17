/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:38:58
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for soundalert
-- ----------------------------
DROP TABLE IF EXISTS `soundalert`;
CREATE TABLE `soundalert` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `sendTime` tinytext,
  `keyword` text,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
