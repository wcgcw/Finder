/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:39:16
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for systemset
-- ----------------------------
DROP TABLE IF EXISTS `systemset`;
CREATE TABLE `systemset` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `EvidenceImgSavePath` text,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of systemset
-- ----------------------------
INSERT INTO `systemset` VALUES ('1', 'E:\\test');
