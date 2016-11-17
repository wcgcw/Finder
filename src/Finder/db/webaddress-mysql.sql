/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:40:17
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for webaddress
-- ----------------------------
DROP TABLE IF EXISTS `webaddress`;
CREATE TABLE `webaddress` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `url` text NOT NULL,
  `name` varchar(50) NOT NULL,
  `likeurl` text NOT NULL,
  `pid` int(11) NOT NULL,
  `sheng` varchar(29) DEFAULT NULL,
  `shi` varchar(29) DEFAULT NULL,
  `xian` varchar(29) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
