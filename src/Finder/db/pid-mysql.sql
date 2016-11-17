/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:38:00
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for pid
-- ----------------------------
DROP TABLE IF EXISTS `pid`;
CREATE TABLE `pid` (
  `pid` int(11) NOT NULL,
  `name` varchar(20) DEFAULT NULL,
  PRIMARY KEY (`pid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of pid
-- ----------------------------
INSERT INTO `pid` VALUES ('0', '全网');
INSERT INTO `pid` VALUES ('1', '博客');
INSERT INTO `pid` VALUES ('2', '论坛');
INSERT INTO `pid` VALUES ('3', '微博');
INSERT INTO `pid` VALUES ('4', '主流媒体');
INSERT INTO `pid` VALUES ('5', '贴吧');
INSERT INTO `pid` VALUES ('6', '微信');
INSERT INTO `pid` VALUES ('7', '自定义');
