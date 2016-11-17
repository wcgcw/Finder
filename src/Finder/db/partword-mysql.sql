/*
Navicat MySQL Data Transfer

Source Server         : local
Source Server Version : 50716
Source Host           : localhost:3306
Source Database       : finder

Target Server Type    : MYSQL
Target Server Version : 50716
File Encoding         : 65001

Date: 2016-11-14 10:37:47
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for partword
-- ----------------------------
DROP TABLE IF EXISTS `partword`;
CREATE TABLE `partword` (
  `uid` int(11) NOT NULL AUTO_INCREMENT,
  `word` varchar(20) DEFAULT NULL,
  `part` varchar(30) DEFAULT NULL,
  PRIMARY KEY (`uid`)
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of partword
-- ----------------------------
INSERT INTO `partword` VALUES ('1', '强拆', '0');
INSERT INTO `partword` VALUES ('5', '执法', '1');
INSERT INTO `partword` VALUES ('6', '内讧', '0');
INSERT INTO `partword` VALUES ('7', '内斗', '0');
INSERT INTO `partword` VALUES ('8', '戒严', '0');
INSERT INTO `partword` VALUES ('9', '军车', '0');
INSERT INTO `partword` VALUES ('10', '军队', '0');
INSERT INTO `partword` VALUES ('11', '枪响', '0');
INSERT INTO `partword` VALUES ('12', '军警', '0');
INSERT INTO `partword` VALUES ('13', '武警', '0');
INSERT INTO `partword` VALUES ('14', '自焚', '0');
INSERT INTO `partword` VALUES ('15', '斗争', '0');
INSERT INTO `partword` VALUES ('16', '信访办', '0');
INSERT INTO `partword` VALUES ('17', '尸体', '0');
INSERT INTO `partword` VALUES ('18', '失踪', '0');
INSERT INTO `partword` VALUES ('19', '限制出境', '0');
INSERT INTO `partword` VALUES ('20', '接受调查', '0');
INSERT INTO `partword` VALUES ('21', '爆炸', '0');
INSERT INTO `partword` VALUES ('22', '警车', '0');
INSERT INTO `partword` VALUES ('23', '骚乱', '0');
INSERT INTO `partword` VALUES ('24', '聚集', '0');
INSERT INTO `partword` VALUES ('25', '示威', '0');
INSERT INTO `partword` VALUES ('26', '上街', '0');
INSERT INTO `partword` VALUES ('27', '围堵', '0');
INSERT INTO `partword` VALUES ('28', '辞职', '0');
INSERT INTO `partword` VALUES ('29', '下台', '0');
INSERT INTO `partword` VALUES ('30', '下课', '0');
INSERT INTO `partword` VALUES ('31', '死亡', '0');
INSERT INTO `partword` VALUES ('32', '问责', '0');
INSERT INTO `partword` VALUES ('33', '查封', '0');
INSERT INTO `partword` VALUES ('34', '车祸', '0');
INSERT INTO `partword` VALUES ('35', '分裂', '0');
INSERT INTO `partword` VALUES ('36', '民愤', '0');
INSERT INTO `partword` VALUES ('37', '追悼会', '0');
INSERT INTO `partword` VALUES ('38', '打架', '0');
INSERT INTO `partword` VALUES ('39', '家族', '0');
INSERT INTO `partword` VALUES ('40', '禁令', '0');
INSERT INTO `partword` VALUES ('41', '性骚扰', '0');
INSERT INTO `partword` VALUES ('42', '冲突', '0');
INSERT INTO `partword` VALUES ('44', '集会', '0');
INSERT INTO `partword` VALUES ('45', '抗议', '0');
