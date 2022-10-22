USE [FFB_Dbase]
GO

/****** Object:  Trigger [dbo].[TRIGGER_t_Incentive]    Script Date: 6/29/2022 1:55:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE TRIGGER [dbo].[TRIGGER_t_Incentive]
ON [dbo].[t_Incentive]
AFTER UPDATE, INSERT, DELETE
AS
DECLARE
	@ActionAudit varchar(50);
	IF EXISTS (SELECT 0 FROM inserted)
	BEGIN
		IF EXISTS (SELECT 0 FROM deleted)
		BEGIN
			SELECT @ActionAudit = 'UPDATE'
			INSERT into t_Incentive_Audit
				([IncentiveID],[Periode],[GroupSuppID],[PMKSID],[SupplierID]
				,[Netto],[Incentive],[PPH22],[Remarks],[UploadToSAP]
				,[FCApproveBy],[FCApproveDT],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[Approval]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[IncentiveID],[Periode],[GroupSuppID],[PMKSID],[SupplierID]
				,[Netto],[Incentive],[PPH22],[Remarks],[UploadToSAP]
				,[FCApproveBy],[FCApproveDT],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[Approval]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	   ELSE
	   BEGIN
			SELECT @ActionAudit = 'INSERT'
			INSERT into t_Incentive_Audit
				([IncentiveID],[Periode],[GroupSuppID],[PMKSID],[SupplierID]
				,[Netto],[Incentive],[PPH22],[Remarks],[UploadToSAP]
				,[FCApproveBy],[FCApproveDT],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[Approval]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[IncentiveID],[Periode],[GroupSuppID],[PMKSID],[SupplierID]
				,[Netto],[Incentive],[PPH22],[Remarks],[UploadToSAP]
				,[FCApproveBy],[FCApproveDT],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[Approval]
				,@ActionAudit,GETDATE()
			FROM inserted i
	   END
	END
	ELSE 
	BEGIN
		SELECT @ActionAudit = 'DELETE'
			INSERT into t_Incentive_Audit
				([IncentiveID],[Periode],[GroupSuppID],[PMKSID],[SupplierID]
				,[Netto],[Incentive],[PPH22],[Remarks],[UploadToSAP]
				,[FCApproveBy],[FCApproveDT],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[Approval]
				,[ActionAudit],[TimeAudit])
			SELECT 
				[IncentiveID],[Periode],[GroupSuppID],[PMKSID],[SupplierID]
				,[Netto],[Incentive],[PPH22],[Remarks],[UploadToSAP]
				,[FCApproveBy],[FCApproveDT],[FCCreatedBy],[FCCreatedDT]
				,[FCUpdatedBy],[FCUpdatedDT],[FCDefunctInd],[Approval]
				,@ActionAudit,GETDATE()
		FROM deleted i
	END 
GO

ALTER TABLE [dbo].[t_Incentive] ENABLE TRIGGER [TRIGGER_t_Incentive]
GO


