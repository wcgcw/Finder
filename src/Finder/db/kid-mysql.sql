/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:37:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for kid
-- ----------------------------
DROP TABLE IF EXISTS `kid`;
CREATE TABLE `kid` (
  `kid` int(11) NOT NULL,
  `name` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`kid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of kid
-- ----------------------------
INSERT INTO `kid` VALUES ('0', '常规舆情');
INSERT INTO `kid` VALUES ('1', '敏感舆情');
INSERT INTO `kid` VALUES ('2', '重点舆情');
INSERT INTO `kid` VALUES ('3', '突发舆情');
