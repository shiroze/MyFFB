USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_t_GroupIncentive]    Script Date: 6/29/2022 1:42:59 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE TRIGGER [dbo].[TRIGGER_t_GroupIncentive]
ON [dbo].[t_GroupIncentive]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into t_GroupIncentive_Audit
				([NoId],[GroupSuppID],[Approval],[Incentive],[IncentiveQty1],[IncentivePrice1]
				,[IncentiveQty2],[IncentivePrice2],[IncentiveQty3],[IncentivePrice3]
				,[IncentiveQty4],[IncentivePrice4],[IncentiveQty5],[IncentivePrice5]
				,[IncentiveQty6],[IncentivePrice6],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[FCApproveBy],[FCApproveDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[NoId],[GroupSuppID],[Approval],[Incentive],[IncentiveQty1],[IncentivePrice1]
				,[IncentiveQty2],[IncentivePrice2],[IncentiveQty3],[IncentivePrice3]
				,[IncentiveQty4],[IncentivePrice4],[IncentiveQty5],[IncentivePrice5]
				,[IncentiveQty6],[IncentivePrice6],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[FCApproveBy],[FCApproveDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into t_GroupIncentive_Audit
				([NoId],[GroupSuppID],[Approval],[Incentive],[IncentiveQty1],[IncentivePrice1]
				,[IncentiveQty2],[IncentivePrice2],[IncentiveQty3],[IncentivePrice3]
				,[IncentiveQty4],[IncentivePrice4],[IncentiveQty5],[IncentivePrice5]
				,[IncentiveQty6],[IncentivePrice6],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[FCApproveBy],[FCApproveDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[NoId],[GroupSuppID],[Approval],[Incentive],[IncentiveQty1],[IncentivePrice1]
				,[IncentiveQty2],[IncentivePrice2],[IncentiveQty3],[IncentivePrice3]
				,[IncentiveQty4],[IncentivePrice4],[IncentiveQty5],[IncentivePrice5]
				,[IncentiveQty6],[IncentivePrice6],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[FCApproveBy],[FCApproveDT]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
			INSERT into t_GroupIncentive_Audit
				([NoId],[GroupSuppID],[Approval],[Incentive],[IncentiveQty1],[IncentivePrice1]
				,[IncentiveQty2],[IncentivePrice2],[IncentiveQty3],[IncentivePrice3]
				,[IncentiveQty4],[IncentivePrice4],[IncentiveQty5],[IncentivePrice5]
				,[IncentiveQty6],[IncentivePrice6],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[FCApproveBy],[FCApproveDT]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[NoId],[GroupSuppID],[Approval],[Incentive],[IncentiveQty1],[IncentivePrice1]
				,[IncentiveQty2],[IncentivePrice2],[IncentiveQty3],[IncentivePrice3]
				,[IncentiveQty4],[IncentivePrice4],[IncentiveQty5],[IncentivePrice5]
				,[IncentiveQty6],[IncentivePrice6],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[FCApproveBy],[FCApproveDT]
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[t_GroupIncentive] ENABLE TRIGGER [TRIGGER_t_GroupIncentive]
GO


