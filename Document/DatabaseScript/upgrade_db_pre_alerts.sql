-- ============================================================
-- 预报警功能数据库升级脚本（在目标库的 yishaadmin_hyh 上执行）
-- 日期：2026-11-20
-- ============================================================

-- 1. 新建预报警表
CREATE TABLE IF NOT EXISTS `param_pre_alerts` (
  `Id` bigint NOT NULL,
  `BaseIsDelete` int NOT NULL,
  `BaseCreateTime` datetime NOT NULL,
  `BaseModifyTime` datetime NOT NULL,
  `BaseCreatorId` bigint NOT NULL,
  `BaseModifierId` bigint NOT NULL,
  `BaseVersion` int NOT NULL,
  `Name` varchar(255) DEFAULT NULL,
  `Param_Key` varchar(255) DEFAULT NULL COMMENT '参数',
  `Param_Value` varchar(255) DEFAULT NULL COMMENT '值(3K)',
  `Source_SYS` varchar(5000) DEFAULT NULL COMMENT '所属系统',
  `Remark` varchar(255) DEFAULT NULL COMMENT '备注',
  `AlarmLayer` int DEFAULT NULL COMMENT '报警层（1:一层；2:二层；3:三层）',
  `Param_Value1` varchar(255) DEFAULT NULL COMMENT '值(KNS)',
  `Status` int NOT NULL DEFAULT 0 COMMENT '状态（0:未确认；1:已确认报警；2:已萃取）',
  PRIMARY KEY (`Id` DESC) USING BTREE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci COMMENT='预报警事件记录表';

-- 2. 萃取表增加报警层字段
-- 注意：MySQL 8.0 不支持 ADD COLUMN IF NOT EXISTS
-- 如果提示 Duplicate column name 'AlarmLayer'，说明字段已存在，忽略即可
ALTER TABLE `param_alert_extraction`
  ADD COLUMN `AlarmLayer` int DEFAULT NULL COMMENT '报警层';

-- 3. 菜单：预报警列表（挂在三维界面菜单下，ParentId 与"报警列表"相同）
INSERT INTO sysmenu (Id, BaseIsDelete, BaseCreateTime, BaseModifyTime, BaseCreatorId, BaseModifierId, BaseVersion, ParentId, MenuName, MenuIcon, MenuUrl, MenuTarget, MenuSort, MenuType, MenuStatus, Authorize, Remark)
SELECT 1030000000000000001, 0, NOW(), NOW(), 16508640061130151, 16508640061130151, 0, 972518583609331712, '预报警列表', '', 'ThreeDimensions‌Manage/PreAlerts/PreAlertsIndex', '', 115, 2, 1, '', ''
FROM DUAL
WHERE NOT EXISTS (SELECT 1 FROM sysmenu WHERE Id = 1030000000000000001);
