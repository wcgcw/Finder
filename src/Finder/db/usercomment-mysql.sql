/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:39:47
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for usercomment
-- ----------------------------
DROP TABLE IF EXISTS `usercomment`;
CREATE TABLE `usercomment` (
  `CommentID` int(11) NOT NULL AUTO_INCREMENT,
  `uid` int(11) NOT NULL,
  `Comment` text NOT NULL,
  `SubmitDate` tinytext NOT NULL,
  `UserName` text,
  `Deleted` int(11) NOT NULL DEFAULT '0',
  PRIMARY KEY (`CommentID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
